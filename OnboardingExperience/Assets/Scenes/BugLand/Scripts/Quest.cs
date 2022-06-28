using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest", fileName = "New Quest")]
public class Quest : ScriptableObject
{
    [HideInInspector] public enum QuestType {Interaction, Kill, Gather}
    public QuestType type;
    public string title;
    [TextArea(25,25)]
    public string details;
    public int count;
    public int xp;
    public GameObject target;
    public bool isComplete;
}
