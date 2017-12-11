using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LineUp
{
    [ExecuteInEditMode]
    public class Viewer : EditorWindow
    {
        WWW editorWWW = null;
        public static bool isWaiting = false;

        int count = 10;
        int start = 0, end = 100;
        string date = "";
        string startDate = "", endDate = "";
        string sessionTag = "";
        FilterTypes filterTypes;

        [MenuItem("Line Up/Viewer")]
        public static void Init()
        {
            Viewer window = (Viewer)EditorWindow.GetWindow(typeof(Viewer));
            window.Show();
        }

        void OnFocus()    // Window has been selected
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI; // Remove delegate listener if it has previously been assigned.
            SceneView.onSceneGUIDelegate += OnSceneGUI; // Add (or re-add) the delegate.

            DisplayMethods.onFinishedMovmentData -= RunRepaint;
            DisplayMethods.onFinishedMovmentData += RunRepaint;
        }

        private void OnDestroy()
        {
            DisplayMethods.onFinishedMovmentData -= RunRepaint;
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        }

        private void OnGUI()
        {
            DrawTopBar();

            switch (filterTypes)
            {
                case FilterTypes.limit:
                    DrawGetByCount();
                    break;

                case FilterTypes.date:
                    DrawGetByDate();
                    break;
                case FilterTypes.sessionTag:
                    DrawGetByTag();
                    break;
            }

            GUILayout.Space(10);

            DrawMain();
        }

        void DrawTopBar()
        {
            EditorGUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Get Data By Count"))
            {
                filterTypes = FilterTypes.limit;
                DisplayMethods.sessionId.Clear();
                DisplayMethods.sessionsMovmentData.Clear();
            }

            if (GUILayout.Button("Get Data By Date"))
            {
                filterTypes = FilterTypes.date;
                DisplayMethods.sessionId.Clear();
                DisplayMethods.sessionsMovmentData.Clear();
            }

            if (GUILayout.Button("Get Data By Tag"))
            {
                filterTypes = FilterTypes.sessionTag;
                DisplayMethods.sessionId.Clear();
                DisplayMethods.sessionsMovmentData.Clear();
            }

            EditorGUILayout.EndHorizontal();
        }

        void DrawGetByCount()
        {
            EditorGUILayout.BeginVertical();

            //Start load count
            EditorGUILayout.BeginHorizontal("box");
            GUILayout.Label("Count");
            count = EditorGUILayout.IntField(count);

            if (GUILayout.Button("Get First[" + count + "] Sessions"))
                GetData(count.ToString(), "", "limit", LineUpSqlSettings.getDataWithFilter); //Grab the first x amount of row session numbers
            EditorGUILayout.EndHorizontal();
            //End load count ####

            //Start load range
            EditorGUILayout.BeginHorizontal("box");
            GUILayout.Label("Range");
            start = EditorGUILayout.IntField(start);
            GUILayout.Label("-");
            end = EditorGUILayout.IntField(end);

            if (GUILayout.Button("Get Sessions Between [" + start + "]and[" + end + "]"))
                GetData(start.ToString(), end.ToString(), FilterTypes.sessionRange.ToString(), LineUpSqlSettings.getDataWithFilter); //Grab sessions between two id's
            EditorGUILayout.EndHorizontal();
            //End load range

            EditorGUILayout.EndVertical();
        }

        void DrawGetByDate()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Change Date"))
                DatePopUp.Init(this, "Date");

                if (GUILayout.Button("Get Sessions From [" + date +"]"))
                GetData(date, "", FilterTypes.date.ToString(), LineUpSqlSettings.getDataWithFilter); //Grab sessions from the defined date
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Change Start Date"))
                DatePopUp.Init(this, "StartDate");

            if (GUILayout.Button("Change End Date"))
                DatePopUp.Init(this, "EndDate");

            if (GUILayout.Button("Get Sessions Between [" + startDate + "] And [" + endDate + "]"))
                GetData(startDate, endDate, FilterTypes.dateRange.ToString(), LineUpSqlSettings.getDataWithFilter); //Grab sessions between two date's
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        void DrawGetByTag()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal("box");
            GUILayout.Label("Tag");
            sessionTag = GUILayout.TextField(sessionTag);
            if(GUILayout.Button("Load By Tag"))
                GetData(sessionTag, "", FilterTypes.sessionTag.ToString(), LineUpSqlSettings.getDataWithFilter); //Grab sessions with a tag
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        void DrawMain()
        {
            if (isWaiting)
            {
                GUILayout.Label("Loading");
            }
            else
            {
                DrawAvailableSessions();
            }
        }

        void DrawAvailableSessions()
        {
            EditorGUILayout.BeginVertical("box");
            if (DisplayMethods.sessionId.Count > 0 && GUILayout.Button("Load All Sessions"))
            {
                LoadAllSessions();
            }

            foreach (string id in DisplayMethods.sessionId)
            {
                if (GUILayout.Button("Load Session: [" + id + "]")) //Create a button for each session displayed
                    LoadSession(id);
            }

            EditorGUILayout.EndVertical();
        }

        void OnSceneGUI(SceneView sceneView)
        {
            for (int session = 0; session < DisplayMethods.sessionsMovmentData.Count; session++) //Go through each session collected from the filter
            {
                Random.InitState(session); //Set the seed using the session number so the colour is the same every time
                Handles.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1); //Create a random colour

                for (int i = 0; i < DisplayMethods.sessionsMovmentData[session].Count; i++) //Go through each Vector3 recorded and draw the lines to display the movment
                {
                    Vector3 a = DisplayMethods.sessionsMovmentData[session][i];
                    Vector3 b = (i + 1 >= DisplayMethods.sessionsMovmentData[session].Count) ? a : DisplayMethods.sessionsMovmentData[session][i + 1]; //Get the next point unless the array is over in that case just use point a

                    Handles.DrawLine(a, b);
                }
            }
        }

        public void GetData(string filter1, string filter2, string filterType, string phpScript)
        {
            WWWForm form = new WWWForm(); //Create an empty form to post to the php script
            form.AddField("filterType", filterType); //Set the value and form field name
            form.AddField("filter1", filter1); //Set the value and form field name
            form.AddField("filter2", filter2); //Set the value and form field name

            editorWWW = new WWW(phpScript, form); //Post the form to the php script

            isWaiting = true; //Set this so we now know to check if the result has downloaded in Update
        }

        void Update()
        {
            if (editorWWW != null && editorWWW.isDone && isWaiting) //Using this instead of a coroutine as they can't be run in editor
            {
                isWaiting = false; //Now the method has run we can stop checking if the download is complete
                DisplayMethods.SeperateAndStoreResults(editorWWW.text);
                Repaint(); //Repaint to get rid of the loading sign
            }
        }

        void LoadSession(string id)
        {
            GetData(id, "", "sessionId", LineUpSqlSettings.getDataWithFilter);
        }

        void LoadAllSessions()
        {
            GetData(DisplayMethods.sessionId[0], DisplayMethods.sessionId[DisplayMethods.sessionId.Count - 1], "range", LineUpSqlSettings.getDataWithFilter);
        }

        void RunRepaint()
        {
            SceneView.RepaintAll();
        }

        public void ReciveDate(string callType, string newDate)
        {
            switch (callType)
            {
                case "Date":
                    date = newDate;
                    break;
                case "StartDate":
                    startDate = newDate;
                    break;
                case "EndDate":
                    endDate = newDate;
                    break;
            }
        }
    }
}