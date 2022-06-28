using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCController : Interactables
{
    [SerializeField] private Quest quest;
    [SerializeField] private GameObject questAvailableIcon;
    private PlayerController playerC;
    private bool hasQuest;

     void Start()
    {
        
        if (quest != null && PlayerController.GetQuest() != quest)
        {
            hasQuest = true;
            
        }

    }
    public override void Interact()
    {
        if (PlayerController.GetQuest() != null)
        {
            base.Interact();
        }
        
    }

    public override void Update()
    {
        base.Update();
        //If the NPC has a quest, the player is too far to interact and doesn't already have that quest, show a prompt for the player to come get a quest.
        questAvailableIcon.SetActive(hasQuest && (isInRange == false) && PlayerController.GetQuest() != quest);
    }
}
