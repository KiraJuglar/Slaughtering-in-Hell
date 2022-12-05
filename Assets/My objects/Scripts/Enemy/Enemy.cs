using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

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
    Rigidbody2D rigidBody;

    public EnemieCounterLevel enemiesCounter;

    // Start is called before the first frame update
    private void Awake()
    {
        // Grab references for rigidBody and animator from objetc
        anim = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        enemiesCounter = FindObjectOfType<EnemieCounterLevel>();
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
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            rigidBody.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | 
            RigidbodyConstraints2D.FreezePositionY;
            anim.SetBool("death", true);
            Destroy(gameObject, 1f);
            enemiesCounter.countEnemies--;
        }
        else
        {
            StartCoroutine(WaitAttacked());
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
        Vector3 auxV = new Vector3(-0.5f,0.5f,0);
        canShoot = false;
        Vector3 direction = player.position - transform.position;
        if (player && Vector3.Distance(this.transform.position, player.position) < 10)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotationTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            GameObject newBullet = Instantiate(bullet, transform.position + auxV, rotationTarget);
            newBullet.GetComponent<Bullet>().Damage = damage;
            newBullet.GetComponent<Bullet>().DestroyTime = 5;
                
        }
        yield return new WaitForSeconds(timeAttack);
        canShoot = true;
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

    private IEnumerator WaitAttacked()
    {
        rigidBody.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | 
        RigidbodyConstraints2D.FreezePositionY;
        anim.SetTrigger("damaged");
        yield return new WaitForSeconds(2);
        rigidBody.GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionX | 
        ~RigidbodyConstraints2D.FreezePositionY;
    }

}
