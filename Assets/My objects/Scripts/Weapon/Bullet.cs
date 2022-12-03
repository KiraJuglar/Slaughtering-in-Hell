using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 4;
    [SerializeField] float destroyTime = 5;
    [SerializeField] int damage = 25;
    [SerializeField] int impacts = 1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    void Update()
    {
        transform.position += transform.right.normalized * Time.deltaTime * speed;
        if (impacts == 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            impacts--;
        }
        else if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
            impacts--;
        }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public float DestroyTime
    {
        get { return destroyTime; }
        set { destroyTime = value; }
    }
}