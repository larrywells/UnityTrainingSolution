using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SphereCollider))]
public class NPCController : MonoBehaviour
{
    [SerializeField] private GameObject interactPanel;
    private bool isInteractable = false;
    private bool spawning = false;
    List<GameObject> balls  = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteractable && CompareTag("NPC"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Hello there! Everytime I use the foutain to the left there, everything around me starts to freeze and go crazy, can you please check out the issue?");
            }
        }
        else if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
               spawning = true;
            }
        }

        if (spawning)
        {
            for (int i = 0; i < 100; i++)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.SetParent(this.transform);
                go.transform.position = this.transform.position + new Vector3(0, 3, 0);
                go.AddComponent<Rigidbody>();
                go.GetComponent<Rigidbody>().AddForce(new Vector3(0, 3, 0), ForceMode.Impulse);
                Physics.IgnoreCollision(go.GetComponent<Collider>(), GameObject.Find("Player").GetComponent<Collider>());
                balls.Add(go);
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                spawning = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPanel.SetActive(true);
            isInteractable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPanel.SetActive(false);
            isInteractable = false;
        }
    }
}
