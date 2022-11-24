using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed = 5;
    [SerializeField] int damage = 10;
    [SerializeField] int health = 5;

    protected Transform player;
    protected bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {

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
