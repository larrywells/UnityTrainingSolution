using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;

[CreateAssetMenu(menuName = "Pop-up Template", fileName = "New Popup Template")]
public class PopUpTemplate : ScriptableObject
{
    [HideInInspector] public enum Modules { Interface, Profiler, Auditor, CodeCoverage}
    public Modules module;

    [HideInInspector] public enum ValidationTypes { Addition, Subtraction, OnChange, ValueMatch }
    public ValidationTypes validationType;

    //[SerializeField] private Dictionary<ScriptableObject, PopUp> Windows = new Dictionary<ScriptableObject, PopUp>();
    [SerializeField] List <PopUpTemplate> templateList = new List <PopUpTemplate>();
    [SerializeField] PopUpTemplate[] templateArray;
    public string title;

    [TextArea(15, 20)] //Text box in the editor can now go to 20 lines and willd default to 15.
    public string mainText;
    public Texture helpImage;
    public Rect location;

    public string thisPopUpType;
    public string nextPopUpDataPath;
    public string NextPopUpType;

    public bool isSkippable = true; //Controls whether the continue or close options appear on the pop-up. Set this to false when you want the user to perform an action before continuing the tutorial. Defaults to true, turn off via inspector.
    
    
}
