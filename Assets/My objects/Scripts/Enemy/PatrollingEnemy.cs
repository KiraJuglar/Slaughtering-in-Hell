using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : Enemy
{
    [SerializeField] Transform groundSensor;
    [SerializeField] Transform vision;
    [SerializeField] float visionDistance = 3f;
    bool playerDetected = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {

        EnemyDirection();
        
        if (!playerDetected)
            playerDetected = PlayerDetection();
        if (playerDetected)
        {
            if (Vector3.Distance(player.position, transform.position) < visionDistance)
                playerDetected = false;

            if (canAttack)
            {
                Attack();
                anim.SetBool("run", false);
                anim.SetTrigger("attack");
            }
            else if (canShoot)
            {
                StartCoroutine(Shoot());
                anim.SetBool("run", false);
                anim.SetTrigger("shoot");
            }
            else
                this.transform.position = Vector2.MoveTowards(this.transform.position, player.position + diference, speed * Time.deltaTime);
        }
        else if(!hasBeenAtacked)
        {
            #region Patrullar
            
            if (!EndOfPlatform() || rigidBody.velocity.x == 0)
                facingRight = !facingRight;
            PlatformPatrolling();
            #endregion
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
        }
    }

    private bool PlayerDetection()
    {
        Vector2 directionVector;
        if (facingRight)
            directionVector = Vector2.right;
        else
            directionVector = Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(vision.position, directionVector, visionDistance);
        if (hit && hit.transform.name == "Player")
        {
            player = hit.transform;
            return true;
        }
        return false;
    }


    private bool EndOfPlatform()
    {
        //Detecta si la plataforma se ha terminado
        return Physics2D.Raycast(groundSensor.position, Vector2.down, 1.5f);
    }


    public void PlatformPatrolling()
    {
        float currentSpeed = speed;
        if (!facingRight)
        {
            currentSpeed = -speed;
        }

        rigidBody.velocity = new Vector2(currentSpeed, rigidBody.velocity.y);
        anim.SetBool("run", true);

    }



}