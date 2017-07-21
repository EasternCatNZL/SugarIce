using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerMovement : MonoBehaviour
{

    public PlayerIndex PlayerID;

    private Vector3 Direction;
    private Vector3 PrevDirection;

    private bool PlayerConnected = false;
    private GamePadState state;
    private GamePadState prevState;

    // Use this for initialization
    void Start()
    {
        Direction = new Vector3(0.0f, 0.0f, 0.0f);
        state = GamePad.GetState(PlayerID);
        print(state.IsConnected);
        if (!state.IsConnected)
        {
            Debug.LogError("Player " + PlayerID.ToString() + " is not connected");
        }
        else
        {
            PlayerConnected = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerConnected)
        {
            state = GamePad.GetState(PlayerID);
            prevState = state;

            Direction.x += state.ThumbSticks.Left.X;
            Direction.z += state.ThumbSticks.Left.Y;

            Direction.Normalize();

            Debug.DrawRay(this.transform.position, Direction * 5, Color.red);

            if (Vector3.Dot(transform.right, Direction) < 0.0f)
            {
                transform.Rotate(0.0f, -Vector3.Angle(transform.forward, Direction), 0.0f);
            }
            if (Vector3.Dot(transform.right, Direction) > 0.0f)
            {
                transform.Rotate(0.0f, Vector3.Angle(transform.forward, Direction), 0.0f);
            }
        }
    }
}
