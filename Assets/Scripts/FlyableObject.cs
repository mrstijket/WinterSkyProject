using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyableObject : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    CircleCollider2D circleCol;
    [SerializeField] float Force = 2500f;
    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        circleCol = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (circleCol.IsTouching(collision.collider))
        {
            myRigidbody.velocity = new Vector2(0f, Force);
        }
    }
}
