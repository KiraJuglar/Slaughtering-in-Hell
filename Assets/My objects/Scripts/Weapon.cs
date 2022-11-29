using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    pistol,
    shotgun,
    machineGun,
    assaultRifle,
    chainsaw,
}

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponType type = WeaponType.pistol;
    float rate;
    float damage;
    float range;
    int ammoCapacity;
    [SerializeField] int ammo = 0;

    [SerializeField] GameObject bullet;
    bool canShoot = true;

    private void Start()
    {
        setType(WeaponType.pistol);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool shoot(Vector2 facingDirection)
    {
        if (ammo > 0)
        {
            if (canShoot)
            {
                canShoot = false;
                StartCoroutine(shootTime());
                float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
                Quaternion rotationTarget = Quaternion.AngleAxis(angle, Vector3.forward);
                GameObject newBullet = Instantiate(bullet, transform.position, rotationTarget);
                newBullet.GetComponent<Bullet>().Damage = damage;
                newBullet.GetComponent<Bullet>().DestroyTime = range;
                ammo--;
            }
            return false;
        }
        else
            return true;
    }

    IEnumerator shootTime()
    {
        yield return new WaitForSeconds(rate);
        canShoot = true;

    }

    public void setType(WeaponType weapon)
    {
        switch(weapon)
        {
            case WeaponType.pistol:
                range = 0.5f;
                rate = 0.5f;
                damage = 25f;
                ammoCapacity = 10;
                break;
            case WeaponType.shotgun:
                range = 0.5f;
                rate = 0.5f;
                damage = 100f;
                ammoCapacity = 5;
                break;
            case WeaponType.assaultRifle:
                range = 1f;
                rate = 0.1f;
                damage = 30f;
                ammoCapacity = 30;
                break;
            case WeaponType.machineGun:
                range = 0.7f;
                rate = 0.05f;
                damage = 25f;
                ammoCapacity = 20;
                break;
            case WeaponType.chainsaw:

                break;
        }
    }

    public void Reload(int ammo)
    {
        if (ammo > ammoCapacity)
            this.ammo = ammoCapacity;
        else
            this.ammo += ammo;
    }

    public int AmmoCapacity
    {
        get { return ammoCapacity; }
    }

    public int Ammo
    {
        get { return ammo; }
    }
}