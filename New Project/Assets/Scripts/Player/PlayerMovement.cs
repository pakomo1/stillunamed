using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Cinemachine;
public class PlayerMovement : NetworkBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private CinemachineVirtualCamera cinamchineVirtualCamera;
    [SerializeField] private AudioListener audioListener;
    //References
    private Rigidbody2D rb;
    private AnimationController animCtrl;
    private BoxCollider2D boxCollider;

    public Vector2 movementDirection; 

    //Animation States
    private const string WALKFRONT = "WalkFront";
    private const string WALKBACK = "WalkBack";
    private const string WALKRIGHT = "RightWalk";
    private const string WALKLEFT = "LeftWalk";

    private int idleAnimStateNum;
    private string[] idleAnimStates = new string[3] { "idleFront", "idleLeft", "idleRight" };

    public bool IsInteractingWithUI { get; set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animCtrl = GetComponent<AnimationController>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Get the size of the sprite
        Vector3 spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size;

        // Adjust the size of the BoxCollider2D to account for the game object's scale
        boxCollider.size = new Vector2(spriteSize.x / transform.localScale.x, spriteSize.y / transform.localScale.y);

        // Set the position of the BoxCollider2D to be on top of the GameObject
        boxCollider.offset = new Vector2(0, boxCollider.size.y / 2);
    
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            audioListener.enabled = true;
            cinamchineVirtualCamera.Priority = 1;
        }
        else
        {
            cinamchineVirtualCamera.Priority = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner || IsInteractingWithUI) { return; }
        //Moves the player
        rb.AddForce(movementDirection * moveSpeed);
    }

    private void Update()
    {
        if (!IsOwner) { return; }
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
        else if (movementDirection.x < 0)
        {
            animCtrl.ChangeAnimationState(WALKLEFT);
            idleAnimStateNum = 1;
        }
        else if (movementDirection.x > 0)
        {
            animCtrl.ChangeAnimationState(WALKRIGHT);
            idleAnimStateNum = 2;
        }
    }

    private void OnMove(InputValue _value)
    {
        if (IsInteractingWithUI) { return; }
        movementDirection = _value.Get<Vector2>() * moveSpeed;
    }
}
