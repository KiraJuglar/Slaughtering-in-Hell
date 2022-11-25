using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : Enemy
{
    [SerializeField] Transform groundSensor;
    [SerializeField] Transform vision;
    [SerializeField] float visionDistance = 3f;
    bool playerDetected = false;

    Rigidbody2D enemyRigidbody;

    
    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        EnemyDirection();

        if(!playerDetected)
            playerDetected = PlayerDetection();
        if (playerDetected)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, player.position + diference, speed * Time.deltaTime);
        }
        else
        {
            #region Patrullar
            if (!EndOfPlatform() || enemyRigidbody.velocity.x == 0)
                facingRight = !facingRight;
            PlatformPatrolling();
            #endregion
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

        enemyRigidbody.velocity = new Vector2(currentSpeed, enemyRigidbody.velocity.y);
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(vision.position, vision.position + Vector3.left);
    }
}