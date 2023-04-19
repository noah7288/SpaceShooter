using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAmmo : MonoBehaviour
{
    public GameObject pickUpSel;
    public pickupSelector pickup;
    

    public float clip;
    public float reserve;

    public enum Weptype
    {
        gun1,
        gun2,
        gun3,
    }

    public Weptype wep;

    private void Awake()
    {
        GunSel();
    }

    private void Start()
    {
        pickUpSel = GameObject.FindWithTag("PickupManager");
        pickup = pickUpSel.GetComponent<pickupSelector>();
        
    }

    void GunSel()
    {
        switch (wep)
        {
            case Weptype.gun1://keep these updated
                clip = 18;
                reserve = 100;
                break;
            case Weptype.gun2:
                clip = 10;
                reserve = 50;
                break;
            case Weptype.gun3:
                clip = 3;
                reserve = 10;
                break;
            default:
                break;
        }
    }



}
