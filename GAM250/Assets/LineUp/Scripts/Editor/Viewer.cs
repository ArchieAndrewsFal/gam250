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

        [MenuItem("Line Up/Viewer")]
        public static void Init()
        {
            Viewer window = (Viewer)EditorWindow.GetWindow(typeof(Viewer));
            window.Show();
        }

        void OnFocus()    // Window has been selected
        {
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI; // Remove delegate listener if it has previously been assigned.
            SceneView.onSceneGUIDelegate += this.OnSceneGUI; // Add (or re-add) the delegate.
        }

        private void OnDestroy()
        {
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        }

        private void OnGUI()
        {
            DrawTopBar();
            DrawMain();
        }

        void DrawTopBar()
        {
            EditorGUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Show All Data (First 100)"))
                GetData(100, "limit", LineUpSqlSettings.getDataByCountPhp); //Grab the first 100 rows session numbers

            EditorGUILayout.EndHorizontal();
        }

        void DrawMain()
        {
            EditorGUILayout.BeginVertical("box");
            DrawAvailableSessions();
            EditorGUILayout.EndVertical();
        }

        void DrawAvailableSessions()
        {
            foreach (string id in DisplayMethods.sessionId)
            {
                if (GUILayout.Button("Load Session: [" + id + "]")) //Create a button for each session displayed
                    LoadSession(id);
            }
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

        public void GetData(int value, string formField, string phpScript)
        {
            WWWForm form = new WWWForm(); //Create an empty form to post to the php script
            form.AddField(formField, value); //Set the value and form field name

            editorWWW = new WWW(phpScript, form); //Post the form to the php script

            isWaiting = true; //Set this so we now know to check if the result has downloaded in Update
        }

        void Update()
        {
            if (editorWWW != null && editorWWW.isDone && isWaiting) //Using this instead of a coroutine as they can't be run in editor
            {
                isWaiting = false; //Now the method has run we can stop checking if the download is complete
                DisplayMethods.SeperateAndStoreResults(editorWWW.text);
            }
        }

        void LoadSession(string id)
        {
            int idAsInt = 0;
            int.TryParse(id, out idAsInt);

            if (idAsInt != 0)
                GetData(idAsInt, "sessionId", LineUpSqlSettings.getDataBySessionId);
        }
    }
}