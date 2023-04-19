using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public static GameMan instance;

    public bool pickup;
    public float dashTimeg; //"g" stands for GameManager
    public int ammo;
    public float playerHealth;
    public float playerHealthMax;

    public float relT;
    public float relTM;
    public bool IsReloading;

    public float ammoMax;
    public float ammoPickUp;
    public float ammoCap;

    public GameObject gamemanager;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pickup = false;
    }
    public static GameMan Instance
    {
        get
        {
            if (instance == null)
            {
                
            }

            return instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
