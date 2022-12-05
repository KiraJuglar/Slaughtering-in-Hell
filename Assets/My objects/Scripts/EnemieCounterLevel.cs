using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieCounterLevel : MonoBehaviour
{

    [SerializeField] public int countEnemies;
    private Animator anim;
    private GameObject enemiesObj;

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemiesObj = GameObject.Find("Enemies");
        countEnemies = getChildren(enemiesObj);
        Debug.Log(countEnemies);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesObj == null)
        {
            Debug.Log("No se encontró el objeto con los enemigos");
        }
        if (countEnemies == 0)
        {
            anim.SetTrigger("AllDead");
            StartCoroutine(OpeningPortal());
        }
    }

    private int getChildren(GameObject obj)
    {
        int count = obj.transform.childCount;
        return count;
    }

    IEnumerator OpeningPortal()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Opened", true);
    }

    #region Metodo para iniciar en una posicion especifica cada nivel del portal
    private void OnLevelWasLoaded(int level)
    {
        FindStartPos();
    }


    void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("PortalStartPos").transform.position;
    }
    #endregion
}
