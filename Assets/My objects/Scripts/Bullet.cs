using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 4;
    [SerializeField] float damage = 25;
    [SerializeField] float destroyTime = 5;
    [SerializeField] int impacts = 1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    void Update()
    {
        transform.position += transform.right.normalized * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage();
        }
    }

    public float Damage
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