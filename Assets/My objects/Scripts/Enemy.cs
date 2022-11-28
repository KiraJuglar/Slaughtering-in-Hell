using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed = 5;
    [SerializeField] int damage = 10;
    [SerializeField] int health = 5;
    [SerializeField] int timeAttack = 1;

    bool attack = true;
    [SerializeField] GameObject bullet;

    protected Transform player;
    protected bool facingRight = true;
    protected Vector3 diference = new Vector3(1, 0, 0);

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {


    }



    protected void EnemyDirection()
    {
        isFacingRight();
        if (facingRight)
        {
            diference.x = -1;
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            diference.x = 1;
            this.transform.eulerAngles = Vector3.zero;
        }
    }



    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void isFacingRight()
    {
        if (!player)
            return;
        if (this.transform.position.x > player.position.x)
            facingRight = false;
        else
            facingRight = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (attack)
            {
                collision.GetComponent<PlayerController>().TakeDamage(damage);
                attack = false;
                StartCoroutine(reloadAttack());
            }
        }
    }

    protected void Shoot()
    {
        Vector3 direction = player.position - transform.position;
        if (player && Vector3.Distance(this.transform.position, player.position) < 5)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotationTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            GameObject newBullet = Instantiate(bullet, transform.position, rotationTarget);
        }
    }


    IEnumerator reloadAttack()
    {
        yield return new WaitForSeconds(timeAttack);
        attack = true;
    }
}
