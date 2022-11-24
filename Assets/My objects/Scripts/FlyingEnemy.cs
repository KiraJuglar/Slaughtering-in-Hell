using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void FixedUpdate()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
    }
}
