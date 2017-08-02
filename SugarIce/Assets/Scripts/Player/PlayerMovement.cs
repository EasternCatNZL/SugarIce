using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Speed = 5.0f;
    public float CurrentSpeed = 0.0f;
    [Range(0,1)]
    public float TurnRate = 0.2f; //Must be set between 1 - 0

    [Header("Boost Settings")]
    public ParticleSystem VFXBoost = null;
    public float BoostAmount = 2.0f;
    public float BoostLength = 5.0f;
    public float SlowAmount = 2.0f;
    public float SlowLength = 3.0f;
    private float BoostStartTime = 0.0f;
    private float SlowStartTime = 0.0f;
    private bool Boosting = false;
    private bool Slowing = false;

    [Range(0,1)]
    public float BoostDecay = 0.1f;
    private float BoostMultipler = 1.0f;

    [Header("Controller Settings")]
    public PlayerIndex PlayerID;
    [Range(0,1)]
    public float Deadzone = 0.1f;

    private PlayerStateControl playerState;

    private Vector3 Direction;
    private Vector3 PrevDirection;

    private Rigidbody Rigid;
    private bool PlayerConnected = false;
    private GamePadState state;
    private GamePadState prevState;

    // Use this for initialization
    void Start()
    {
        Direction = new Vector3(0.0f, 0.0f, 0.0f);
        //Check a controller is hooked up
        state = GamePad.GetState(PlayerID);
        if (!state.IsConnected)
        {
            Debug.LogWarning("Player " + PlayerID.ToString() + " is not connected");
        }
        else
        {
            PlayerConnected = true;
        }

        Rigid = GetComponent<Rigidbody>();
        Rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        playerState = GetComponent<PlayerStateControl>();
    }

    // Update is called once per frame
    void Update()
    {
        prevState = state;
        state = GamePad.GetState(PlayerID);

        if(Boosting && Time.time - BoostStartTime > BoostLength)
        {
            Boosting = false;
            Slowing = true;
            SlowStartTime = Time.time;
        }
        else if(Slowing && Time.time - SlowStartTime > SlowLength)
        {
            Slowing = false;
            if (VFXBoost)
            {
                if (VFXBoost.isPlaying)
                {
                    VFXBoost.Stop();
                }
            }
        }
        if (PlayerConnected && playerState.isPlaying)
        {
            //Activate the boost
            if (!Boosting && !Slowing && GetComponent<PlayerStateControl>().isHolding && prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed)
            {
                if(VFXBoost)
                {
                    if(!VFXBoost.isPlaying)
                    {
                        VFXBoost.Play();
                    }
                }
                Boosting = true;
                BoostStartTime = Time.time;
                BoostMultipler -= BoostDecay;
                GetComponent<PlayerPickUpBehaviour>().DestroyHeldItem();
            }
            if(Boosting) //Speed if boosting
            {
                CurrentSpeed = Speed + (BoostAmount * BoostMultipler);
            }
            else if (Slowing)//Speed if slowed
            {
                CurrentSpeed = Speed - SlowAmount;
            }
            else//Normal speed
            {
                CurrentSpeed = Speed;
            }

            state = GamePad.GetState(PlayerID);
            prevState = state;

            //Accumlate Direction so the model doesn't snap back
            Direction.x += state.ThumbSticks.Left.X;
            Direction.z += state.ThumbSticks.Left.Y;

            Direction.Normalize();

            Debug.DrawRay(this.transform.position, Direction * 5, Color.red);

            //Player Rotations
            if (Vector3.Dot(transform.right, Direction) < 0.0f)
            {
                transform.Rotate(0.0f, -Vector3.Angle(transform.forward, Direction) * TurnRate, 0.0f);
            }
            if (Vector3.Dot(transform.right, Direction) > 0.0f)
            {
                transform.Rotate(0.0f, Vector3.Angle(transform.forward, Direction) * TurnRate, 0.0f);
            }

            //Move the player forward
            if(state.ThumbSticks.Left.X + state.ThumbSticks.Left.Y > Deadzone || state.ThumbSticks.Left.X + state.ThumbSticks.Left.Y < -Deadzone)
            {
                Rigid.MovePosition(transform.position + transform.forward * CurrentSpeed * Time.deltaTime);
            }

        }
    }
}
