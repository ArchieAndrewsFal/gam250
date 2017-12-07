using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LineUp
{
#if UNITY_EDITOR

    [CustomEditor(typeof(LineUpInterface))]
    public class LineUpInterfaceEditor : Editor
    {
        WWW editorWWW = null;

        public override void OnInspectorGUI()
        {
            LineUpInterface targetInstance = (LineUpInterface)target;

            if (!EditorApplication.isPlaying)
                EditorApplication.update += Callback;

            if (GUILayout.Button("GO"))
                GetDataFromCount(100);
        }

        public void GetDataFromCount(int count)
        {
            Debug.Log("Started the Routine wghghogegefgdfdfooppp");
            WWWForm form = new WWWForm(); //Create an empty form to post to the php script
            form.AddField("limit", count); //Set the limit of the data we want to recive

            editorWWW = new WWW(LineUpSqlSettings.getDataByCountPhp, form); //Post the form to the php script
        }

        void Callback()
        {
            if (editorWWW != null && editorWWW.isDone)
            {
                Debug.Log(editorWWW.text);
                EditorApplication.update -= Callback;
                Repaint();
            }
        }
    }
#endif
}
