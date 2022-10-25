using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float currentSpeed = 0f;
    [SerializeField] float minSpeedValue = 1f;
    [SerializeField] float maxSpeedValue = 4f;
    [SerializeField] float jumpForce = 4f;
    [SerializeField] float pushForce = 8f;
    [SerializeField] float deathTime = 3f;
    [SerializeField] float pusherColliderEnableTime = 1f;

    [Header("Check Ground")]
    public Transform groundPoint;
    public LayerMask whatIsGround;
    public bool isGrounded;
    public bool isAlive = true;

    [Header("Check Wall")]
    public LayerMask whatIsWall;
    public bool isWalled;
    bool isAbleToPush = true;

    private float inputX;
    public Rigidbody2D myRigidbody;
    public Animator myAnimator;
    public CapsuleCollider2D capsuleCollider2D;
    GameSession gameSession;
    [SerializeField] GameObject deathVFX;
    PassageController passageController;
    JumpOnWalls[] JumpOnWalls;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        currentSpeed = Time.time;
        gameSession = FindObjectOfType<GameSession>();
        passageController = FindObjectOfType<PassageController>();
        JumpOnWalls = FindObjectsOfType<JumpOnWalls>();
    }

    void Update()
    {
        if (!isAlive) 
        {
            return; 
        }
        if (inputX != 0)
        {
            myAnimator.SetBool("isRunning", true);
            currentSpeed += Time.deltaTime;
            if (currentSpeed < minSpeedValue)
            {
                currentSpeed = minSpeedValue;
            }
            if (currentSpeed > maxSpeedValue)
            {
                currentSpeed = maxSpeedValue;
            }
        }
        else
        {
            currentSpeed = 0;
            myAnimator.SetBool("isRunning", false);
        }
        myRigidbody.velocity = new Vector2(inputX * currentSpeed, myRigidbody.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);
        isWalled = Physics2D.OverlapCapsule(transform.position, new Vector2(2.1f, .75f), CapsuleDirection2D.Vertical, 0f, whatIsWall);
        deathVFX.transform.position = new Vector3((gameObject.transform.position.x - 0.5f), (gameObject.transform.position.y+0.1f));

        FlipSprite();
    }
    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!isAlive) { return; }
        if (isGrounded && context.performed)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
            myAnimator.SetTrigger("Jump");
        }
    }
    public void UseWand(InputAction.CallbackContext context)
    {
        if (!isAlive) { return; }
        if (context.performed)
        {
            myAnimator.Play("StabWand");
        }
    }
    public void PlayAsOtherObject(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            passageController.ChangeCameraFollow();
        }
    }
    public void PushThePlayer(InputAction.CallbackContext context)
    {
        if (isWalled && context.performed && isAbleToPush)
        {
            myRigidbody.velocity = Vector2.up * pushForce; //myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, pushForce);
            myRigidbody.AddForce(Vector2.right * pushForce);
            myAnimator.SetTrigger("Jump");
            for(int i = 0; i < JumpOnWalls.Length; i++)
            {
                JumpOnWalls[i].relativeJoint2D.enabled = false;
                JumpOnWalls[i].stunCollider.enabled = false;
            }
            StartCoroutine(pushingProcess());
        }
        IEnumerator pushingProcess()
        {
            isAbleToPush = false;
            yield return new WaitForSeconds(pusherColliderEnableTime);
            isAbleToPush = true;
            for (int i = 0; i < JumpOnWalls.Length; i++)
            {
                JumpOnWalls[i].stunCollider.enabled = true;
            }
        }
    }
    public void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Water"))
        {
            StartCoroutine(PlayerDeathProcess());
        }
    }
    IEnumerator PlayerDeathProcess()
    {
        deathVFX.SetActive(true);
        isAlive = false;
        myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
        yield return new WaitForSeconds(deathTime);
        deathVFX.SetActive(false);
        gameSession.PlayerDie();
    }
}