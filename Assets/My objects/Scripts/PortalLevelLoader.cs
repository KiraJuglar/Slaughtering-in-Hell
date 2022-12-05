using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalLevelLoader : MonoBehaviour
{
    public int sceneBuildIndex;
    private GameObject playerObj;
    private Animator anim;
    Rigidbody2D rigidBody;
    private int enemiesCounter;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player");
        rigidBody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemiesCounter = gameObject.GetComponent<EnemieCounterLevel>().countEnemies;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemiesCounter = gameObject.GetComponent<EnemieCounterLevel>().countEnemies;
        if (collision.tag == "Player" && enemiesCounter <= 0)
        {
            StartCoroutine(WaitAndClosePortal());
        }
    }

    IEnumerator WaitAndClosePortal()
    {
        playerObj.GetComponent<SpriteRenderer>().enabled = false;
        rigidBody.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        anim.SetBool("Closing", true);
        anim.SetBool("Opened", false);
        yield return new WaitForSeconds(1f);
        anim.SetBool("Closing", false);
        anim.SetBool("returnCycle", true);
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }



}
