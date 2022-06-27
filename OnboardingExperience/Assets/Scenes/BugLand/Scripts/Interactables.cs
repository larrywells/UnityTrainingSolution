using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactables : MonoBehaviour
{
    [SerializeField] private GameObject interactButton;
    private float interactRange = 5.0f;
    GameObject player;
    //private bool isInRange = false;

    // Start is called before the first frame update
    void Start()
    {
         player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //If the player is within interact range, show the interact button to the user and check for input.
        if (CheckDistanceToPlayer())
        {
            interactButton.SetActive(true);
            if (Input.GetButtonDown("Submit"))
            {
                Interact();
            }
        } else
        {
            interactButton.SetActive(false);
        }
        
    }

    //return true is player is in interact range and false if they are too far away
    private bool CheckDistanceToPlayer()
    {
        if (player == null)
        {
            Debug.LogError("No gameobject with Player tag was found!");
            return false;
        }
        return (Vector3.Distance(this.transform.position, player.transform.position) < interactRange);
    }

    //Implement a custom interact for children to execute when the interact button is pressed.
    public abstract void Interact();
}
