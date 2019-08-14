using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5.0f;
    [Range(0, 1)]
    public float turnRate = 0.2f;
    public float deadZone = 0.1f;

    [Header("Input Axis")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    public enum PlayerState
    {
        Normal,
        Holding,
        Working,
        Down
    }
    [Header("Player state")]
    public PlayerState playerState = PlayerState.Normal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    //Move the player using horizontal and vertical axis input from all sources
    void MovePlayer()
    {
        Vector3 newPos = Vector3.zero;
        if(Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis) > deadZone)
        {
            newPos += Vector3.right * movementSpeed * Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis);
        }
        if(Luminosity.IO.InputManager.GetAxisRaw(verticalAxis) > deadZone)
        {
            newPos += Vector3.forward * movementSpeed * Luminosity.IO.InputManager.GetAxisRaw(verticalAxis);
        }

        transform.position += newPos * movementSpeed * Time.deltaTime;
    }

    void RotatePlayer()
    {
        Vector3 direction = new Vector3(Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis), 0.0f, Luminosity.IO.InputManager.GetAxisRaw(verticalAxis));
        float angle = Mathf.Atan2(Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis), Luminosity.IO.InputManager.GetAxisRaw(verticalAxis));
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }


}
