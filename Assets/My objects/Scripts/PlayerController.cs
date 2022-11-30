using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Constantes
    [SerializeField] const int INITIAL_HEALTH = 100;
    [SerializeField] const int MAX_HEALTH = 100;
    [SerializeField] const int MAX_ARMOR = 100;
    [SerializeField] const int INITIAL_ARMOR = 0;
    #endregion

    #region Variables del movimiento del jugador
    [SerializeField] float jumpForce = 6f;
    [SerializeField] float runningSpeed = 0.2f;
    Rigidbody2D rigidBody;
    #endregion

    #region Variables de stats del jugador
    [SerializeField] int healthPoints = INITIAL_HEALTH;
    [SerializeField] int armorPoints = INITIAL_ARMOR;
    #endregion

    #region Variables para disparo de jugador
    [SerializeField] Camera playerCamera; //Camara que nos otorgara la posici�n en la que apunta y dispara el usuario
    [SerializeField] Transform aim; //Mira del jugador
    Vector2 facingDirection;
    Weapon weapon;
    bool[] weaponsUnlocked = { true, false, false, false, false };
    bool needreload = false;
    #endregion

    private Animator anim; // Animacion
    [SerializeField]private LayerMask groundLayer; // Mascara del suelo
    private BoxCollider2D boxCollider; // Box Collider del player
    private bool shooting; // Validamos si el jugador esta disparando

    # region Brinco
    [SerializeField] private bool jumpRequest = false;
    [SerializeField] private int maxJumps = 2, availableJumps = 0;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Grab references for rigidBody and animator from objetc
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        weapon = GetComponent<Weapon>();
    }

    private void FixedUpdate()
    {
        Move();
        if (jumpRequest)
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            availableJumps--;
            jumpRequest = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        #region Movimiento
        if (Input.GetKeyDown(KeyCode.Space) && availableJumps > 0)
        {
            Jump();
        }
        #endregion

        #region Disparo
        facingDirection = playerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        aim.position = (Vector3)facingDirection.normalized + transform.position;

        if (Input.GetMouseButton(0))
        {
            needreload =  weapon.shoot(facingDirection);//Se intenta disparar, si no hay munición se recargará el arma
            /*if (needreload && ammo > 0)
            {
                ReloadWeapon();
            }*/
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //    ReloadWeapon();

        /*if (Input.GetMouseButtonDown(0) && isGrounded())
        {
            anim.SetTrigger("shoot");
            float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
            Quaternion rotationTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            GameObject newBullet = Instantiate(bullet, transform.position, rotationTarget);
        }
        if (isShooting_Jumping())
        {
            anim.SetTrigger("shoot_jumping");
            float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
            Quaternion rotationTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            GameObject newBullet = Instantiate(bullet, transform.position, rotationTarget);
        
            if (!isShooting_Jumping())
            {
                anim.SetBool("isShootingJumping", false);
            }
        }*/
        #endregion

        #region Golpe
        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetTrigger("punch");
        }
        #endregion

        #region Cambiar arma
        if (Input.GetKeyDown(KeyCode.Alpha1) && weaponsUnlocked[0])
            weapon.setType(WeaponType.pistol);
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponsUnlocked[1])
            weapon.setType(WeaponType.shotgun);
        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponsUnlocked[2])
            weapon.setType(WeaponType.assaultRifle);
        if (Input.GetKeyDown(KeyCode.Alpha4) && weaponsUnlocked[3])
            weapon.setType(WeaponType.machineGun);
        #endregion

        anim.SetBool("grounded", isGrounded());
    }

    /*void ReloadWeapon()
    {
        int ammoRequired = weapon.AmmoCapacity - weapon.Ammo;
        weapon.Reload(ammo);
        
        ammo -= ammoRequired;
        if (ammo < 0)
            ammo = 0;
    }*/


    #region M�todos de movimiento del jugador
    void Jump()
    {
        jumpRequest = true;
        anim.SetTrigger("jump");
    }

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(horizontalInput * runningSpeed, rigidBody.velocity.y);
        if (Input.GetAxis("Horizontal") > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        anim.SetBool("run", horizontalInput != 0);
    }

    #endregion

    #region M�todos de estadisticas del jugador
    public void CollectHealth(int points)
    {
        if (healthPoints + points <= 0)
            //Die();
            if (healthPoints + points <= MAX_HEALTH)
            {
                healthPoints += points;
            }
            else
            {
                healthPoints = MAX_HEALTH;
            }
    }

    public void CollectArmor(int points)
    {
        if (armorPoints + points <= MAX_ARMOR)
        {
            armorPoints += points;
        }
        else
        {
            armorPoints = MAX_ARMOR;
        }
    }

    
    #endregion

    #region Métodos para la colisión con el suelo del jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            availableJumps = maxJumps;
        }
    }
    #endregion

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Debug.Log("Game Over");
            anim.SetBool("death", true);
        }
        else
        {
            anim.SetTrigger("damaged");
        }
    }

    #region Metodo para validar si el player esta tocando el suelo
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    #endregion

    #region Metodo para validar si el player esta disparando
    private bool isShooting_Jumping()
    {
        if(Input.GetMouseButtonDown(0) && !isGrounded())
        {
            return true;
        }else
            return false;
        
    }
    #endregion

    public Weapon GetWeapon()
    {
        return weapon;
    }

    public int Health
    {
        get
        {
            return healthPoints;
        }
    }

    public int Armor
    {
        get
        {
            return armorPoints;
        }
    }

}
