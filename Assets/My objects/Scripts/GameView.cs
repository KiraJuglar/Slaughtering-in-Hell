using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameView : MonoBehaviour
{
    [SerializeField] GameObject healthTMP, armorTMP, ammoTMP;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        int healthTXT = player.GetComponent<PlayerController>().Health;
        int armorTXT = player.GetComponent<PlayerController>().Armor;
        int ammoTXT = player.GetComponent<PlayerController>().GetWeapon().Ammo;
        int maxAmmo = player.GetComponent<PlayerController>().GetWeapon().AmmoCapacity;

        healthTMP.GetComponent<TextMeshProUGUI>().text = healthTXT.ToString() + "%";
        armorTMP.GetComponent<TextMeshProUGUI>().text = armorTXT.ToString() + "%";
        ammoTMP.GetComponent<TextMeshProUGUI>().text = ammoTXT.ToString() + "/" + maxAmmo.ToString();
    }
}
