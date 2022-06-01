using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class PopUp : EditorWindow
{
    protected PopUpTemplate template;
    protected string templatePath;
    protected string newTitle;
    protected string mainText;
    public string nextWindow;
    protected bool isSkippable = true;
    protected Rect location;
    Texture helpImage;
    Vector2 scrollPosition;
    
    public virtual void InitializePopup() //Sets the data from the template scriptable object or returns an error if no template was provided
    {
        if (templatePath != null)
        {
            template = Resources.Load<PopUpTemplate>(templatePath) as PopUpTemplate;

            newTitle = template.title;
            mainText = template.mainText;
            helpImage = template.helpImage;
            nextWindow = template.NextPopUpType;
            position = template.location;
            isSkippable = template.isSkippable;
            location = template.location;
           
        } else
        {
            Debug.LogError("No template path was provided for PopUp > InitializePopup");
        }
        
    }

    public virtual void InitializePopup(PopUpTemplate template) //Override used for creating windows with a specific template
    {
        newTitle = template.title;
        mainText = template.mainText;
        helpImage = template.helpImage;
        nextWindow = template.NextPopUpType;
        position = template.location;
        isSkippable = template.isSkippable;
        location = template.location;
        templatePath = template.nextPopUpDataPath;
    }

    private void OnInspectorUpdate()
    {
        GUI.color = Color.white;
       this.position = location;
    }
    private void OnGUI()
    {
        SetUpLayout();
        GUILayout.Box($"Position \n X: {this.position.x} \n Y: {this.position.y} \n Scale \n W: {this.position.width} \n H: {this.position.height}");
    }

    protected virtual void SetUpLayout()
    {
        GUI.color = Color.white;
        GUILayout.Label(newTitle, EditorStyles.boldLabel); //Display the title of the box
        GUILayout.Box(mainText); //Display text to welcome user
        if (helpImage)
        {
            
            GUILayout.Box(helpImage,GUILayout.Height(100),GUILayout.Width(250));
        }
        GUILayout.BeginHorizontal(); //Create a horizontal row to store continue and skip button
        if (isSkippable)
        {
            if (GUILayout.Button("Continue"))
            {
                NextWindow();
            }


            if (GUILayout.Button("Close"))
            {
                this.Close();
            }
            GUILayout.EndHorizontal();
        }
    }

    public void SetPath(string path)
    {
        templatePath = path;
    }

    public string GetPath()
    {
        return templatePath;
    }

    public virtual void NextWindow() {

        PopUpManager.NextPopUp();
    }

    public void ValidationCheck<T>(T newValue)
    {
        T initialValue; 
       switch (template.validationType)
        {
            case PopUpTemplate.ValidationTypes.Addition:
                break;
            case PopUpTemplate.ValidationTypes.Subtraction:
                break;
            case PopUpTemplate.ValidationTypes.OnChange:
                break;
            case PopUpTemplate.ValidationTypes.ValueMatch:
                break;
        }

    }
}
