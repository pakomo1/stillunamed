using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OfLinePlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    //References
    private Rigidbody2D rb;
    private AnimationController animCtrl;

    public Vector2 movementDirection;

    //Animation States
    private const string WALKFRONT = "WalkFront";
    private const string WALKBACK = "WalkBack";
    private const string WALKRIGHT= "RightWalk";
    private const string WALKLEFT = "LeftWalk";

    private int idleAnimStateNum;
    private string[] idleAnimStates = new string[3] { "idleFront", "idleLeft","idleRight" };

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animCtrl = GetComponent<AnimationController>();
    }

    private void FixedUpdate()
    {
        //Moves the player
        rb.AddForce(movementDirection * moveSpeed);
    }

    private void Update()
    {
        //Change the rotation of the Player
       /* if (movementDirection.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Clamp(movementDirection.x, -1, 1), 1, 1);
        }*/

        //Change the Animation states depending on the player input
        if (movementDirection.x == 0 && movementDirection.y == 0)
        {
            animCtrl.ChangeAnimationState(idleAnimStates[idleAnimStateNum]);
        }
        else if (movementDirection.y > 0)
        {
            animCtrl.ChangeAnimationState(WALKBACK);
            idleAnimStateNum = 0;
        }
        else if (movementDirection.y < 0)
        {
            animCtrl.ChangeAnimationState(WALKFRONT);
            idleAnimStateNum = 0;
        }
        else if (movementDirection.x > 0)
        {
            animCtrl.ChangeAnimationState(WALKRIGHT);
            idleAnimStateNum = 2;
        }
        else if (movementDirection.x < 0)
        {
            animCtrl.ChangeAnimationState(WALKLEFT);
            idleAnimStateNum = 1;
        }
    }

    public void OnMove(InputValue _value)
    {
        movementDirection = _value.Get<Vector2>() * moveSpeed;
    }
}
