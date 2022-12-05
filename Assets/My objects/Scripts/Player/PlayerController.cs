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
    [SerializeField] int punchDamage = 20;
    Vector2 facingDirection;
    Weapon weapon;
    bool[] weaponsUnlocked = { true, false, false, false, false };
    bool needreload = false;
    #endregion

    private Animator anim; // Animacion
    [SerializeField]private LayerMask groundLayer; // Mascara del suelo
    private BoxCollider2D boxCollider; // Box Collider del player
    private bool shooting; // Validamos si el jugador esta disparando
    private GameObject[] players;

    #region Brinco
    [SerializeField] private bool jumpRequest = false;
    [SerializeField] private int maxJumps = 2, availableJumps = 0;
    #endregion

    #region Campos para el dash del player
    private float _dashingTime = 0.1f;
    private float _dashForce = 15f;
    private float _dashingCooldown = 2f;
    private int _nDashes = 2;
    private int _maxDashes = 2;
    private bool _isDashing;
    private bool _canDash = true;
    [SerializeField] private TrailRenderer tr;
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
        if (_isDashing)
        {
            return;
        }
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
        if (_isDashing)
        {
            return;
        }

        #region Movimiento
        if (Input.GetKeyDown(KeyCode.Space) && availableJumps > 0)
        {
            Jump();
        }
        #endregion

        #region Dash
        if ((Input.GetKeyDown(KeyCode.Q) && _canDash) && _nDashes > 0 )
        {
            StartCoroutine(Dash());
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
            Punch();
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

    void Punch()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1f);
        if(hit && hit.transform.tag == "Enemy")
        {
            hit.transform.GetComponent<Enemy>().TakeDamage(punchDamage);
        }
    }

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
        if (healthPoints + points < 0)
            healthPoints = 0;
        else if (healthPoints + points <= MAX_HEALTH)
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
            rigidBody.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | 
            RigidbodyConstraints2D.FreezePositionY;
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

    #region Metodo para realizar el dash
    private IEnumerator Dash()
    {
        _isDashing = true;
        _canDash = false;
        float originalGravity = rigidBody.gravityScale;;
        rigidBody.gravityScale = 0f;
        rigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * _dashForce, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(_dashingTime);
        tr.emitting = false;
        rigidBody.gravityScale = originalGravity;
        _isDashing = false;
        _nDashes--;
        if (_nDashes == 0)
        {
            yield return new WaitForSeconds(_dashingCooldown);
            _nDashes = _maxDashes;
        }
        _canDash = true;
    }
    #endregion

    #region Metodo para iniciar en una posicion especifica cada nivel
    private void OnLevelWasLoaded(int level)
    {
        FindStartPos();
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 1)
        {
            Destroy(players[1]);
        }
    }


    void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
    }
    #endregion

}
