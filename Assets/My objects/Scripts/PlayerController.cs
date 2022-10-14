using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Variables del movimiento del jugador
        [SerializeField] float jumpForce = 6f;
        [SerializeField] float runningSpeed = 2f;
        Rigidbody2D rigidBody;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    #region Métodos de movimiento del jugador
    void Jump()
    {
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Move()
    {
        rigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * runningSpeed, rigidBody.velocity.y);
        if (Input.GetAxis("Horizontal") > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    #endregion


}
