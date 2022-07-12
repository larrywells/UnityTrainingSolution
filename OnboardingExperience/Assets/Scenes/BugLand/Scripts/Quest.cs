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

    public int goalCount;
    private int currentCount;
    public int xp;

    public GameObject target;
    public bool isComplete = false;

    public void ProgressQuest()
    {
        currentCount++;
        if (currentCount >= goalCount)
        {
            isComplete = true;
        }
    }
}
