using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickupSelector : MonoBehaviour
{
    private Transform selection;
    


    public Transform Player;
    public GameObject weaponslot;
    public Weapons wepScript;
    public newFPSControl newFPS;

    public GunAmmo GA;

    float dist;

    //public bool pickup;//temp

    private void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        weaponslot = GameObject.FindWithTag("gunslot");
        wepScript = weaponslot.GetComponent<Weapons>();
        newFPS = Player.GetComponent<newFPSControl>();
        GameMan.Instance.pickup = false;
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            selection = hit.transform;
            
            var _selection = selection.gameObject;
            dist = Vector3.Distance(Player.transform.position, _selection.transform.position);
            if (dist <= 5 & (_selection.tag == "pickupWeapon" || _selection.tag == "pickupWeapon2" || _selection.tag == "pickupWeapon3" || _selection.tag == "MedPack" || _selection.tag == "Ammo"))
            {
                //pickup.gameObject.SetActive(true);
                GameMan.instance.pickup = true;
            }
            else
            {
                //pickup.gameObject.SetActive(false);
                GameMan.instance.pickup = false;
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_selection.tag == "pickupWeapon" & dist <= 5)
                {
                    wepScript.DropWeapon();
                    wepScript.wep = Weapons.Weapon.gun1;
                    wepScript.CurrentWeapon();
                    GA = _selection.GetComponent<GunAmmo>();
                    wepScript.shootCooldown = GA.clip;
                    GameMan.instance.ammoMax = GA.reserve;
                    //_selection.SetActive(false);
                    Destroy(_selection);
                }
                else if (_selection.tag == "pickupWeapon2" & dist <= 5)
                {
                    wepScript.DropWeapon();
                    wepScript.wep = Weapons.Weapon.gun2;
                    wepScript.CurrentWeapon();
                    GA = _selection.GetComponent<GunAmmo>();
                    wepScript.shootCooldown = GA.clip;
                    GameMan.instance.ammoMax = GA.reserve;
                    //_selection.SetActive(false);
                    Destroy(_selection);
                }
                else if (_selection.tag == "pickupWeapon3" & dist <= 5)
                {
                    wepScript.DropWeapon();
                    wepScript.wep = Weapons.Weapon.gun3;
                    wepScript.CurrentWeapon();
                    GA = _selection.GetComponent<GunAmmo>();
                    wepScript.shootCooldown = GA.clip;
                    GameMan.instance.ammoMax = GA.reserve;
                    //_selection.SetActive(false);
                    Destroy(_selection);
                }
                else if (_selection.tag == "MedPack" & dist <= 5)
                {
                    if (newFPS.health < 5)
                    {
                        newFPS.health = newFPS.health + newFPS.healAmmount;
                        if (newFPS.health > 5)
                        {
                            newFPS.health = 5;
                        }
                            Destroy(_selection);
                    }
                }
                else if (_selection.tag == "Ammo" & dist <= 5)
                {
                    if (GameMan.instance.ammoMax < GameMan.instance.ammoCap)
                    {
                        GameMan.instance.ammoMax = GameMan.instance.ammoMax + GameMan.instance.ammoPickUp;//change to currect primary weapon
                        if (GameMan.instance.ammoMax > GameMan.instance.ammoCap)
                        {
                            GameMan.instance.ammoMax = GameMan.instance.ammoCap;
                            
                        }
                        Destroy(_selection);
                    }
                }
            }
        }
    }
}
