using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    healthPotion,
    pistolAmmo,
    fusilAmmo,
    shotgunAmmo,
    armor,
}

public class Collectables : MonoBehaviour
{

    [SerializeField] CollectableType type = CollectableType.healthPotion;
    [SerializeField] int value = 1;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }


    void Collect()
    {

        switch (this.type)
        {
            case CollectableType.healthPotion:
                player.GetComponent<PlayerController>().CollectHealth(this.value);
                break;
            case CollectableType.fusilAmmo:
                player.GetComponent<PlayerController>().GetWeapon().CollectAmmo(this.value, WeaponType.assaultRifle);
                break;
            case CollectableType.shotgunAmmo:
                player.GetComponent<PlayerController>().GetWeapon().CollectAmmo(this.value, WeaponType.shotgun);
                break;
            case CollectableType.pistolAmmo:
                player.GetComponent<PlayerController>().GetWeapon().CollectAmmo(this.value, WeaponType.pistol);
                break;
            case CollectableType.armor:
                player.GetComponent<PlayerController>().CollectArmor(this.value);
                break;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
            Collect();
    }
}
