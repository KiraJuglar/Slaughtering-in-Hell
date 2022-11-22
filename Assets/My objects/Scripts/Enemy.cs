using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] int damage = 10;
    [SerializeField] int health = 5;
    bool facingRight = true;
    Rigidbody2D enemyRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (enemyRigidbody.velocity.x == 0)
            facingRight = !facingRight;
        PlatformPatrolling();
    }


    public void PlatformPatrolling()
    {
        float currentSpeed = speed;
        if (facingRight)
        {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            currentSpeed = -speed;
            this.transform.eulerAngles = Vector3.zero;
        }

        enemyRigidbody.velocity = new Vector2(currentSpeed, enemyRigidbody.velocity.y);
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
        }
        
    }
}
