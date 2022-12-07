using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieCounterLevel : MonoBehaviour
{

    [SerializeField] public int countEnemies; //Contador de enemigos por nivel
    private Animator anim; //Animator
    private GameObject enemiesObj; // Objeto de los enemigos
    private GameObject playerObj;
    [SerializeField] Transform portalEnd; //Posicion del portal al final
    [SerializeField] Transform portalPlayerBegin; //Posicion del player

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerObj = GameObject.Find("Player");
        enemiesObj = GameObject.Find("Enemies");
        countEnemies = getChildren(enemiesObj);
        FindStartPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesObj == null)
        {
            Debug.Log("No se encontró el objeto con los enemigos");
        }
        if (countEnemies <= 0)
        {
            FindEndPos();
            //Debug.Log(countEnemies);

            StartCoroutine(OpeningPortal());
        }
    }

    private int getChildren(GameObject obj)
    {
        int count = obj.transform.childCount;
        Debug.Log(count);
        return count;
    }

    IEnumerator OpeningPortal()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Opened", true);
        anim.SetBool("Opening", false);
    }

    IEnumerator ClosingPortal()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Opened", false);
        anim.SetBool("Closing", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("Opened", false);
        anim.SetBool("Closing", false);
        anim.SetBool("returnCycle", true);
    }

    IEnumerator DisablePlayerSpriteTemporary()
    {
        playerObj.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        playerObj.GetComponent<SpriteRenderer>().enabled = true;
    }

    #region Metodos para cambiar una posicion especifica cada nivel del portal
    //Cambiar posicion del portal a una distinta al matar los enemigos
    void FindEndPos()
    {
        anim.SetBool("Opening", true);
        anim.SetBool("returnCycle", false);
        //Portal End
        gameObject.transform.position = portalEnd.position;
        gameObject.transform.rotation = Quaternion.Euler(Vector3.forward * 0);
    }

    //Cambiar posicion del portal cuando inicia el nivel a la del jugador
    void FindStartPos()
    {
        //Portal Start
        gameObject.transform.position = portalPlayerBegin.position;
        gameObject.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
        anim.SetBool("Opening", true);
        StartCoroutine(DisablePlayerSpriteTemporary());
        StartCoroutine(OpeningPortal());
        StartCoroutine(ClosingPortal());
    }
    #endregion
}
