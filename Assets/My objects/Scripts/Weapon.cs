using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    float shotSpeed;
    float damage;
    int ammoCapacity;
    int bulletsPerShot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class Shotgun : Weapon
    {
        private new void Start()
        {
            shotSpeed = 3f;
            damage = 10;
            ammoCapacity = 7;
            bulletsPerShot = 1;
        }
    }
}
