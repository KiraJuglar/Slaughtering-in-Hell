using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 4;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }
    void Update()
    {
        transform.position += transform.right.normalized * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Que haga da�o al enemigo
    }
}
