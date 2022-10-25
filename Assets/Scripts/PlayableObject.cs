using UnityEngine;
using UnityEngine.InputSystem;

public class PlayableObject : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    private float inputX;
    private float inputY;
    [SerializeField] float treeControlSpeed = 5f;
    public bool isPlayerControlling = false;
    public RelativeJoint2D relativeJoint2D;
    PlayerController player;
    public BoxCollider2D stunCollider;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        relativeJoint2D.enabled = false;
        player = FindObjectOfType<PlayerController>();
        myRigidbody.velocity = new Vector2(0f, 0f);
    }
    
    void Update()
    {
        if(!isPlayerControlling) { return; }
        if(inputX == 0f && inputY == 0f)
        {
            //myRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        myRigidbody.velocity = new Vector2(inputX * treeControlSpeed, inputY * treeControlSpeed);
    }
    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x;
        inputY = context.ReadValue<Vector2>().y;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (stunCollider.IsTouching(collision.collider))
        {
            relativeJoint2D.enabled = true;
            relativeJoint2D.connectedBody = player.myRigidbody;
        }
    }
}
