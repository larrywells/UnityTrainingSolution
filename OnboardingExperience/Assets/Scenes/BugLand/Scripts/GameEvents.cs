using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private static GameObject player;
    private static GameObject currentTarget;
    private static Quest quest;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log($"Player has been found: {player.gameObject.name}");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject FindPlayer()
    {
        return player;
    }

    //If the player makes an interaction while holding an interaction quest, check if the target matches the quest target and if so, progress the quest.
    public static void PlayerInteraction(GameObject _target)
    {
        currentTarget = _target;
        Debug.Log($"Player interacted with an object: {_target.name}");

        if (quest != null)
        {
            if (PlayerController.GetQuest().type == Quest.QuestType.Interaction && PlayerController.GetQuest().target.name == _target.name)
            {
                quest.ProgressQuest();
            }
        }
        
    }

    public static void PlayerPickedUpQuest()
    {
        if (currentTarget.GetComponent<NPCController>())
        {
            quest = currentTarget.GetComponent<NPCController>().GetQuest();
        }
        
        PlayerController.SetQuest(quest);
        Debug.Log($"Player picked up a quest: {PlayerController.GetQuest().title}");
    }
}
