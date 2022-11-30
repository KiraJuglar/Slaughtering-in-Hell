using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    PatrollingEnemy patrollingEnemy;

    #region Caracteristicas del enemigo
    [SerializeField] protected float speed = 5;
    [SerializeField] int damage = 10;
    [SerializeField] int health = 5;
    #endregion

    #region Variables de ataque
    [SerializeField] protected bool canShoot = false;
    bool attack = true;
    protected bool canAttack = false;
    [SerializeField] int timeAttack = 1;
    [SerializeField] GameObject bullet;
    protected Transform player;
    #endregion

    #region Variables de movimiento
    protected bool facingRight = true;
    protected Vector3 diference = new Vector3(1, 0, 0);
    #endregion

    protected Animator anim; // Animacion

    // Start is called before the first frame update
    void Start()
    {
        // Grab references for rigidBody and animator from objetc
        anim = GetComponent<Animator>();
    }

    #region M�todos de movimiento
    void isFacingRight()
    {
        if (!player)
            return;
        if (this.transform.position.x > player.position.x)
            facingRight = false;
        else
            facingRight = true;
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

    #endregion


    #region M�todos de ataque
    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            anim.SetTrigger("damaged");
        }
    }

    

    protected void Attack()
    {
        if (attack)
        {
            player.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            attack = false;
            StartCoroutine(reloadAttack());
        }
    }

    protected IEnumerator Shoot()
    {
        Vector3 direction = player.position - transform.position;
        if (player && Vector3.Distance(this.transform.position, player.position) < 5)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotationTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            GameObject newBullet = Instantiate(bullet, transform.position, rotationTarget);
            newBullet.GetComponent<Bullet>().Damage = damage;
            newBullet.GetComponent<Bullet>().DestroyTime = 5;
                
        }
        yield return new WaitForSeconds(timeAttack);
    }

    IEnumerator reloadAttack()
    {
        yield return new WaitForSeconds(timeAttack);
        attack = true;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canAttack = false;
        }
    }


}
