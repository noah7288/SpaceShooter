using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    private float fireRate;// = 0.3f;
    private float fireRateMax;// = 0.2f;
    public float shootCooldown;// = 10;
    public float shootCooldownMax;// = 10;
    public float reloadTime;
    public float reloadTimeF;//for HUD
    public float reloadTimeMax;

    public GameObject projectile;
    public GameObject projectile1;
    public GameObject projectile2;
    public GameObject projectile3;
    public Transform gunEnd;
    public float projectileSpeed;// = 50f;
    List<Quaternion> shot;
    public float spreadAngle;

    private int gunCount;
    [Header("game objects")]
    public GameObject gunSlot;
    public GameObject[] gun;
    public GameObject gun1Pickup;
    public GameObject gun2Pickup;
    public GameObject gun3Pickup;
    public Transform droppoint;
    public GameObject SecWep;
    public Transform SGunend;
    private bool UsingSecond;

    public GunAmmo GA;

    public Camera cam;

    public enum Weapon 
    {
        gun1, 
        gun2,
        gun3,

    };

    public Weapon wep;
    private void Start()
    {
        gunCount = gunSlot.transform.childCount;
        gun = new GameObject[gunCount];
        wep = Weapon.gun2;
        CurrentWeapon();
        UsingSecond = false;
        SecWep.SetActive(false);
    }

    public void CurrentWeapon()
    {
        switch (wep)
        {
            case Weapon.gun1:

                fireRate = 0.1f;
                fireRateMax = 0.1f;
                shootCooldown = 18;//
                shootCooldownMax = 18;
                projectileSpeed = 90f;
                reloadTime = 1.0f;
                reloadTimeMax = 1.0f;
                GameMan.instance.ammoMax = 125;//
                GameMan.instance.ammoCap = 125;
                GameMan.instance.ammoPickUp = 65;
                spreadAngle = 0.02f;

                shot = new List<Quaternion>(6);
                shot.Add(Quaternion.Euler(Vector3.zero));
                for (int i = 0; i < gunCount; i++)
                {
                    gun[i] = gunSlot.transform.GetChild(i).gameObject;
                    gun[i].SetActive(false);
                }
                gun[0].SetActive(true);
                GA = gun[0].GetComponent<GunAmmo>();
                shootCooldown = GA.clip;
                GameMan.instance.ammoMax = GA.reserve;
                gunEnd = GameObject.FindWithTag("gunEnd1").transform;
                UsingSecond = false;
                SecWep.SetActive(false);
                break;
            case Weapon.gun2:
                fireRate = 0.3f;
                fireRateMax = 0.2f;
                shootCooldown = 10;
                shootCooldownMax = 10;
                projectileSpeed = 80f;
                reloadTime = 1.5f;
                reloadTimeMax = 1.5f;
                GameMan.instance.ammoMax = 70;
                GameMan.instance.ammoCap = 70;
                GameMan.instance.ammoPickUp = 35;

                for (int i = 0; i < gunCount; i++)
                {
                    gun[i] = gunSlot.transform.GetChild(i).gameObject;
                    gun[i].SetActive(false);
                }
                gun[1].SetActive(true);
                GA = gun[1].GetComponent<GunAmmo>();
                shootCooldown = GA.clip;
                GameMan.instance.ammoMax = GA.reserve;
                gunEnd = GameObject.FindWithTag("gunEnd2").transform;
                UsingSecond = false;
                SecWep.SetActive(false);
                break;
            case Weapon.gun3:
                fireRate = 0.9f;
                fireRateMax = 0.9f;
                shootCooldown = 3;
                shootCooldownMax = 3;
                projectileSpeed = 60f;
                reloadTime = 2.5f;
                reloadTimeMax = 2.5f;
                GameMan.instance.ammoMax = 15;
                GameMan.instance.ammoCap = 15;
                GameMan.instance.ammoPickUp = 8;
                spreadAngle = 3f;

                shot = new List<Quaternion>(6);
                for (int i = 0; i < 6; i++)
                {
                    shot.Add(Quaternion.Euler(Vector3.zero));
                }
                

                for (int i = 0; i < gunCount; i++)
                {
                    gun[i] = gunSlot.transform.GetChild(i).gameObject;
                    gun[i].SetActive(false);
                }
                gun[2].SetActive(true);
                GA = gun[2].GetComponent<GunAmmo>();
                shootCooldown = GA.clip;
                GameMan.instance.ammoMax = GA.reserve;
                gunEnd = GameObject.FindWithTag("gunEnd3").transform;
                UsingSecond = false;
                SecWep.SetActive(false);
                break;

            default:
                break;
        }
    }

    private enum ShootType
    {
        
    }
    void Shoot()
    {
        GameObject bullet;
        Rigidbody rb2;
        switch (wep)
        {
            case Weapon.gun1:

                for (int i = 0; i < 1; i++)
                {
                    shot[i] = Random.rotation;
                    bullet = Instantiate(projectile3, gunEnd.position, gunEnd.rotation);
                    bullet.transform.rotation = Quaternion.RotateTowards(bullet.transform.rotation, shot[i], spreadAngle);
                    if (spreadAngle <= 1.5f)
                    {
                        spreadAngle = spreadAngle + 0.5f;
                    }
                    rb2 = bullet.GetComponent<Rigidbody>();
                    rb2.AddForce(bullet.transform.forward * projectileSpeed, ForceMode.Impulse);
                    shootCooldown = shootCooldown - 1;
                    fireRate = 0;
                }
                break;
            case Weapon.gun2:
                for (int i = 0; i < 1; i++)
                {
                    bullet = Instantiate(projectile1, gunEnd.position, gunEnd.rotation);
                rb2 = bullet.GetComponent<Rigidbody>();
                rb2.AddForce(bullet.transform.forward * projectileSpeed, ForceMode.Impulse);
                shootCooldown = shootCooldown - 1;
                fireRate = 0;
        }
                break;
            case Weapon.gun3:
                for (int i = 0; i < 6; i++)
                {
                    shot[i] = Random.rotation;
                    bullet = Instantiate(projectile2, gunEnd.position, gunEnd.rotation);
                    bullet.transform.rotation = Quaternion.RotateTowards(bullet.transform.rotation, shot[i], spreadAngle);
                    rb2 = bullet.GetComponent<Rigidbody>();
                    rb2.AddForce(bullet.transform.forward * projectileSpeed, ForceMode.Impulse);
                    
                    fireRate = 0;
                }
                shootCooldown = shootCooldown - 1;
                break;
            default:
                break;
        }
    }

    public void DropWeapon()
    {
        GameObject wepPre;
        Rigidbody rb;
        switch (wep)
        {
            case Weapon.gun1:
                wepPre = Instantiate(gun1Pickup, droppoint.position, droppoint.rotation);
                GA = wepPre.GetComponent<GunAmmo>();
                Ammo();
                rb = wepPre.GetComponent<Rigidbody>();
                rb.AddForce(wepPre.transform.right * -3, ForceMode.Impulse);
                break;
            case Weapon.gun2:
                wepPre = Instantiate(gun2Pickup, droppoint.position, droppoint.rotation);
                GA = wepPre.GetComponent<GunAmmo>();
                Ammo();
                rb = wepPre.GetComponent<Rigidbody>();
                rb.AddForce(wepPre.transform.right * -3, ForceMode.Impulse);
                break;
            case Weapon.gun3:
                wepPre = Instantiate(gun3Pickup, droppoint.position, droppoint.rotation);
                GA = wepPre.GetComponent<GunAmmo>();
                Ammo();
                rb = wepPre.GetComponent<Rigidbody>();
                rb.AddForce(wepPre.transform.right * -3, ForceMode.Impulse);
                break;
        }
    }

    void Ammo()
    {
        GA.clip = shootCooldown;
        GA.reserve = GameMan.instance.ammoMax;
    }


    private void Update()
    {
        Aim();
        if (Input.GetKeyDown(KeyCode.E))//for testing, remove this
        {

            if (!UsingSecond)
            {
                if (wep == Weapon.gun1)
                {
                    GA = gun[0].GetComponent<GunAmmo>();
                    GA.clip = shootCooldown;
                    GA.reserve = GameMan.instance.ammoMax;
                    gun[0].SetActive(false);
                }
                else if (wep == Weapon.gun2)
                {
                    
                    GA = gun[1].GetComponent<GunAmmo>();
                    GA.clip = shootCooldown;
                    GA.reserve = GameMan.instance.ammoMax;
                    gun[1].SetActive(false);
                }
                else if (wep == Weapon.gun3)
                {
                    GA = gun[2].GetComponent<GunAmmo>();
                    GA.clip = shootCooldown;
                    GA.reserve = GameMan.instance.ammoMax;
                    gun[2].SetActive(false);
                }
                SecWep.SetActive(true);
                UsingSecond = true;
                Secondary();
            }
            else if (UsingSecond)
            {
                if (wep == Weapon.gun1)
                {
                    CurrentWeapon();
                    shootCooldownMax = 18;
                    GA = gun[0].GetComponent<GunAmmo>();
                    shootCooldown = GA.clip;
                    GameMan.instance.ammoMax = GA.reserve;
                    
                    gun[0].SetActive(true);
                    
                }
                if (wep == Weapon.gun2)
                {
                    CurrentWeapon();
                    shootCooldownMax = 10;
                    GA = gun[1].GetComponent<GunAmmo>();
                    shootCooldown = GA.clip;
                    GameMan.instance.ammoMax = GA.reserve;
                    
                    gun[1].SetActive(true);
                }
                if (wep == Weapon.gun3)
                {
                    CurrentWeapon();
                    shootCooldownMax = 3;
                    GA = gun[2].GetComponent<GunAmmo>();
                    shootCooldown = GA.clip;
                    GameMan.instance.ammoMax = GA.reserve;
                    
                    gun[2].SetActive(true);
                    
                }
                SecWep.SetActive(false);
                UsingSecond = false;
            }
             //for HUD elements

            //if (wep == Weapon.gun1)
            //{
            //    wep = Weapon.gun2;
            //    CurrentWeapon();
            //}
            //else if (wep == Weapon.gun2)
            //{
            //    wep = Weapon.gun3;
            //    CurrentWeapon();
            //}
            //else if (wep == Weapon.gun3)
            //{
            //    wep = Weapon.gun1;
            //    CurrentWeapon();
            //}
            //else
            //{
            //    wep = Weapon.gun1;
            //}
        }
        if (Input.GetButton("Fire1") & fireRate >= fireRateMax & GameMan.instance.IsReloading == false)
        {
            if (!UsingSecond) 
            { 
                if (shootCooldown > 0)
                    {
                        Shoot();
                    }
                else
                    {
                        GameMan.instance.IsReloading = true;//for HUD
                        reloadTimeF = 0;
                        Invoke(nameof(Reload), reloadTime);
                    }
            }
            if (UsingSecond)
            {
                if (shootCooldown <= 1f)
                {
                    Overheat();
                }
                else
                {
                    SecondShoot();

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R) & GameMan.instance.IsReloading == false)//Getbutton("reload") at some point
        {
            if (shootCooldown < shootCooldownMax && !UsingSecond) {
                
                GameMan.instance.IsReloading = true;//for HUD
                reloadTimeF = 0;
                Invoke(nameof(Reload), reloadTime);
            }
        }

        GameMan.instance.ammo = ((int)shootCooldown);
        //reloadTimeF = reloadTime;
        GameMan.instance.relT = reloadTimeF;
        GameMan.instance.relTM = reloadTimeMax;
        
    }

    private bool canShoot = true;
    void Secondary()
    {
        fireRate = 0.5f;
        fireRateMax = 0.5f;
        GameMan.instance.ammoMax = 8;
        shootCooldownMax = 8;
        shootCooldown = 8;
        //change HUD
    }

    void Overheat()
    {
        if (canShoot) { 
            canShoot = false;
            Invoke(nameof(Overheat), 3f);
        }
        else if(!canShoot)
        {
            canShoot = true;
        }
        
    }

    void SecondShoot()
    {

        if (shootCooldown > 0 && canShoot)
        {
            GameObject bullet;
            Rigidbody rb2;
            bullet = Instantiate(projectile1, SGunend.position, SGunend.rotation);
            rb2 = bullet.GetComponent<Rigidbody>();
            rb2.AddForce(bullet.transform.forward * projectileSpeed, ForceMode.Impulse);
            shootCooldown = shootCooldown - 1;
            fireRate = 0;
        }
    }


    void Reload()
    {
        
        if (GameMan.instance.ammoMax >= shootCooldownMax)
        {
            GameMan.instance.ammoMax = GameMan.instance.ammoMax - (shootCooldownMax - shootCooldown);
            shootCooldown = shootCooldownMax;
        }
        else if(GameMan.instance.ammoMax < shootCooldownMax)
        {
            for (int i = -((int)GameMan.instance.ammoMax); i < GameMan.instance.ammoMax; i++){
                if (shootCooldown < shootCooldownMax)
                {
                    GameMan.instance.ammoMax--;
                    shootCooldown++;
                }
                
            }
            
        }

        GameMan.instance.IsReloading = false;


    }

    void Aim()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 20, Color.black );
        Debug.DrawRay(gunEnd.position, gunEnd.forward * 20, Color.blue);
        //Vector3 dir = (gameObject.transform.position - playerOb.position);
        //var ray = (gunEnd.position, gunEnd.up);
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            //Quaternion rotate = Quaternion.LookRotation(gunEnd.forward, gunEnd.up);
            gunEnd.LookAt(hit.point);
            
        }
    }

    void SMGRecoil()
    {
        switch (wep)
        {
            case Weapon.gun1:
                if (spreadAngle >= 0.1)
                {
                    spreadAngle = spreadAngle - (Time.deltaTime);

                }

                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        //if (shootCooldown <= shootCooldownMax)
        //{
        //    shootCooldown = shootCooldown + Time.deltaTime * 2;
        //}
        if(reloadTimeF < reloadTimeMax)
        {
            reloadTimeF = reloadTimeF + Time.deltaTime;
        }
        else if( reloadTimeF > reloadTimeMax)
        {
            reloadTimeF = reloadTimeMax;
        }
        
        if (fireRate <= fireRateMax)
        {
            fireRate = fireRate + Time.deltaTime;
        }
        SMGRecoil();
        if (UsingSecond && shootCooldown <= shootCooldownMax)
        {
            shootCooldown = shootCooldown + Time.deltaTime;
        }
    }

    
}
