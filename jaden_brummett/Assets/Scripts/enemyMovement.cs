using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    // Variables
    public float detectionRadius = 3;
    public float movementSpeed = 5;
    public bool canMove = false;
    public bool movementDirection = false; // false = down | true = up
    public bool isFollowing = false;

    public Transform playerTarget;
    private Animator myAnimator;
    private Rigidbody2D myRB;
    private CircleCollider2D detectionZone;
    private Vector2 up;
    private Vector2 down;
    private Vector2 zero;

    // Start is called before the first frame update
    void Start()
    {
        up = new Vector2(0, movementSpeed);
        down = new Vector2(0, -movementSpeed);
        zero = new Vector2(0, 0);

        playerTarget = GameObject.Find("playerSprite").transform;

        // Assign our Rigidbody Component to our Rigidbody variable in our code.
        myRB = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        detectionZone = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        detectionZone.radius = detectionRadius;

        if (isFollowing == false)
        {
            myAnimator.SetBool("iswalking", false);
            myRB.velocity = zero;
        }

        else if (isFollowing == true)
        {
            Vector3 lookPos = playerTarget.position - transform.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            myRB.rotation = angle;
            lookPos.Normalize();

            myAnimator.SetBool("iswalking", true);

            myRB.MovePosition(transform.position + (lookPos * movementSpeed * Time.deltaTime));
        }

        // Oscillating movement between two triggers
        if (canMove == true)
        {
            if (movementDirection == true)
                myRB.velocity = up;

            else if (movementDirection == false)
                myRB.velocity = down;
        }
    }

    // Runs when our enemy PHYSICALLY colliders with something.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name.Contains("bullet"))
        {
            Destroy(collision.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    // Runs when our enemy collider is collided with OR when our enemy collides with another trigger.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("player"))
            isFollowing = true;

        if (collision.gameObject.name == "trigger1")
            movementDirection = false;

        else if (collision.gameObject.name == "trigger2")
            movementDirection = true;
    }

    // Runs when an object leaves our enemy's trigger volume OR when our enemy leaves another trigger volume.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("player"))
            isFollowing = false;
    }
}
