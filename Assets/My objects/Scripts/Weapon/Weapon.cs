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
    float range;
    int damage;
    int ammoCapacity;
    int[] ammo = { 0, 0, 0, 0, 0 };

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
        int shoots = 1;
        if (type == WeaponType.shotgun)
            shoots = 3;

        if (ammo[(int)type] > 0)
        {
            if (canShoot)
            {
                Vector3 auxV = new Vector3(0,0.5f,0);
                if (facingDirection.x > 0)
                    auxV.x = 0.68f;
                else
                    auxV.x = -0.68f;

                canShoot = false;
                StartCoroutine(shootTime());
                for(int i = 0; i < shoots; i++)
                {
                    facingDirection.y += i/2;
                    facingDirection.x += i/2;
                    float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
                    Quaternion rotationTarget = Quaternion.AngleAxis(angle, Vector3.forward);
                    GameObject newBullet = Instantiate(bullet, transform.position + auxV, rotationTarget);
                    newBullet.GetComponent<Bullet>().Damage = damage;
                    newBullet.GetComponent<Bullet>().DestroyTime = range;
                    ammo[(int)type]--;
                }
                
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
        type = weapon;
        switch(weapon)
        {
            case WeaponType.pistol:
                range = 2f;
                rate = 0.5f;
                damage = 25;
                ammoCapacity = 100;
                break;
            case WeaponType.shotgun:
                range = 1f;
                rate = 0.8f;
                damage = 100;
                ammoCapacity = 200;
                break;
            case WeaponType.assaultRifle:
                range = 2f;
                rate = 0.1f;
                damage = 30;
                ammoCapacity = 100;
                break;
            case WeaponType.machineGun:
                range = 0.7f;
                rate = 0.05f;
                damage = 25;
                ammoCapacity = 200;
                break;
            case WeaponType.chainsaw:

                break;
        }
    }

    /*public void Reload(int ammo)
    {
        if (ammo > ammoCapacity)
            this.ammo = ammoCapacity;
        else
            this.ammo += ammo;
    }*/

    public int AmmoCapacity
    {
        get { return ammoCapacity; }
    }

    public int Ammo
    {
        get { return ammo[(int)type]; }
    }

    public int getAmmoIn(int i)
    {
        return ammo[i];
    }

    public void CollectAmmo(int ammo, WeaponType wType)
    {
        this.ammo[(int)wType] += ammo;
    }

    public WeaponType Type
    {
        get { return type; }
    }
}