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

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player");
        rigidBody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(WaitAndClosePortal());
        }
    }

    IEnumerator WaitAndClosePortal()
    {
        playerObj.GetComponent<SpriteRenderer>().enabled = false;
        rigidBody.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        anim.SetBool("Closing", true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }



}
