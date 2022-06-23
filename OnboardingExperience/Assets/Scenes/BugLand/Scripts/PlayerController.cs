using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float mSpeed;
    private float x, z;
    Vector3 moveDirection;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerPosition();
    }

    private void SetMoveDirection()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        moveDirection = new Vector3(x, 0, z);
    }

    private void UpdatePlayerPosition()
    {
        SetMoveDirection();
        this.transform.Translate(moveDirection * mSpeed * Time.deltaTime);
    }
}