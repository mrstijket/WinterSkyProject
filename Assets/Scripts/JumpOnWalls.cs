using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOnWalls : MonoBehaviour
{
    public BoxCollider2D stunCollider;
    public BoxCollider2D wallCollider;
    Transform stopPoint;
    public RelativeJoint2D relativeJoint2D;
    [SerializeField] GameObject afterJump;
    [SerializeField] GameObject afterJumps;
    PlayerController player;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        relativeJoint2D.enabled = false;
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (stunCollider.IsTouching(collision.collider))
        {
            relativeJoint2D.enabled = true;
            relativeJoint2D.connectedBody = player.myRigidbody;
            stopPoint = collision.gameObject.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(!stunCollider.IsTouching(player.capsuleCollider2D))
        {
            Debug.Log("deneme");
            Instantiate(afterJump, afterJumps.transform, instantiateInWorldSpace: true);
            afterJump.transform.position = player.transform.position - new Vector3(0f, 2f, 0f);
        }
    }
}
