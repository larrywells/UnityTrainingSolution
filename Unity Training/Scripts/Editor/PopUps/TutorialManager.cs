using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TutorialManager : EditorWindow
{
    Rect size;
    [SerializeField] private Dictionary<ScriptableObject, PopUp> Windows = new Dictionary<ScriptableObject, PopUp>();
    [MenuItem("Window/Tutorial Manager")]
    static void Init()
    {
        EditorWindow window = EditorWindow.CreateInstance<TutorialManager>();
        PopUpManager.StartTutorial();
        window.Show();
        
    }

    private void Update()
    {
        Debug.Log("Why tho");
    }

    private void OnGUI()
    {
        size = new Rect(500, 250, 250, 100);
        GetWindow<TutorialManager>().maxSize = new Vector2(250,100);    

        PopUp nextWindow = EditorWindow.CreateInstance<GenericPopUp>();
        GUILayout.Label("Select a section to start");

        if (GUILayout.Button("Hierarchy"))
        {
            nextWindow.SetPath("Scripts/Editor/PopUps/ScriptableObjects/TutorialData");

        }
        else if (GUILayout.Button("Inspector"))
        {
            nextWindow.SetPath("Scripts/Editor/PopUps/ScriptableObjects/Inspector/InspectorIntroData");
            
        }
        else if (GUILayout.Button("Scene View"))
        {
            nextWindow.SetPath("Scripts/Editor/PopUps/ScriptableObjects/Scene/SceneIntroData");
        }
        else if (GUILayout.Button("Game View"))
        {
            nextWindow.SetPath("Scripts/Editor/PopUps/ScriptableObjects/Game/GameTransitionData");
        }

        if (nextWindow.GetPath() != null)
        {
            nextWindow.InitializePopup();
            nextWindow.Repaint();
            nextWindow.Show();
        }
    }
}
