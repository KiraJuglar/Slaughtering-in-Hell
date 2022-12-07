using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameView : MonoBehaviour
{
    [SerializeField] Image ammo1, ammo2, health1, health2, health3, healthP, frag, armor1, armor2, armor3, armorP;
    [SerializeField] Image bull1, bull2, bull3, shel1, shel2, shel3;
    [SerializeField] Image lives;
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
        int bullAmmo = player.GetComponent<PlayerController>().GetWeapon().getAmmoIn(1);
        int shelAmmo = player.GetComponent<PlayerController>().GetWeapon().getAmmoIn(3);
        int live = player.GetComponent<PlayerController>().Lives;

        lives.sprite = Resources.Load<Sprite>("Sprites/" + live.ToString());


        ammo1.sprite = Resources.Load<Sprite>("Sprites/" + ((int)(ammo/10)).ToString());
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

        bull1.sprite = Resources.Load<Sprite>("Sprites/Yellow Numbers/" + (bullAmmo / 100).ToString());
        bull2.sprite = Resources.Load<Sprite>("Sprites/Yellow Numbers/" + (bullAmmo / 10).ToString());
        bull3.sprite = Resources.Load<Sprite>("Sprites/Yellow Numbers/" + (bullAmmo % 10).ToString());

        shel1.sprite = Resources.Load<Sprite>("Sprites/Yellow Numbers/" + (shelAmmo / 100).ToString());
        shel2.sprite = Resources.Load<Sprite>("Sprites/Yellow Numbers/" + (shelAmmo / 10).ToString());
        shel3.sprite = Resources.Load<Sprite>("Sprites/Yellow Numbers/" + (shelAmmo % 10).ToString());
    }
}
