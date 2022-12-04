using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameView : MonoBehaviour
{
    [SerializeField] Image ammo1, ammo2, health1, health2, health3, healthP, frag, armor1, armor2, armor3, armorP;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        healthP.sprite = Resources.Load<Sprite>("Sprites/%");
        armorP.sprite = Resources.Load<Sprite>("Sprites/%");
    }

    // Update is called once per frame
    void Update()
    {
        int health = player.GetComponent<PlayerController>().Health;
        int armor = player.GetComponent<PlayerController>().Armor;
        int ammo = player.GetComponent<PlayerController>().GetWeapon().Ammo;


        ammo1.sprite = Resources.Load<Sprite>("Sprites/" + (ammo/10).ToString());
        ammo2.sprite = Resources.Load<Sprite>("Sprites/" + (ammo%10).ToString());

        if(health > 99)
        {
            health1.sprite = Resources.Load<Sprite>("Sprites/1");
            health2.sprite = Resources.Load<Sprite>("Sprites/0");
            health3.sprite = Resources.Load<Sprite>("Sprites/0");
        }
        else
        {
            health1.sprite = Resources.Load<Sprite>("Sprites/0");
            health2.sprite = Resources.Load<Sprite>("Sprites/" + ((int)(health / 10)).ToString());
            health3.sprite = Resources.Load<Sprite>("Sprites/" + (health % 10).ToString());
        }

        frag.sprite = Resources.Load<Sprite>("Sprites/0");

        if(armor > 99)
        {
            armor1.sprite = Resources.Load<Sprite>("Sprites/1");
            armor2.sprite = Resources.Load<Sprite>("Sprites/0");
            armor3.sprite = Resources.Load<Sprite>("Sprites/0");
        }
        else
        {
            armor1.sprite = Resources.Load<Sprite>("Sprites/0");
            armor2.sprite = Resources.Load<Sprite>("Sprites/" + (armor / 10).ToString());
            armor3.sprite = Resources.Load<Sprite>("Sprites/" + (armor % 10).ToString());
        }

    }
}
