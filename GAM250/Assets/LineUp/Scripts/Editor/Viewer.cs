using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Viewer : EditorWindow
{
    WWW editorWWW = null;
    bool isWaiting = false;

    [MenuItem("Line Up/Viewer")]
    public static void Init()
    {
        Viewer window = (Viewer)EditorWindow.GetWindow(typeof(Viewer));
        window.Show();
    }

    private void OnGUI()
    {
        DrawTopBar();
    }

    void DrawTopBar()
    {
        EditorGUILayout.BeginHorizontal("box");

        if (GUILayout.Button("Show All Data (First 100)"))
            GetDataFromCount(100);

        EditorGUILayout.EndHorizontal();
    }

    void DrawMain()
    {
        EditorGUILayout.BeginHorizontal("box");
        EditorGUILayout.EndVertical();
    }

    public void GetDataFromCount(int count)
    {
        WWWForm form = new WWWForm(); //Create an empty form to post to the php script
        form.AddField("limit", count); //Set the limit of the data we want to recive

        editorWWW = new WWW(LineUpSqlSettings.getDataByCountPhp, form); //Post the form to the php script

        isWaiting = true;
    }

    void Update()
    {
        if (editorWWW != null && editorWWW.isDone && isWaiting)
        {
            Debug.Log(editorWWW.text);
            isWaiting = false;
        }
    }
}
