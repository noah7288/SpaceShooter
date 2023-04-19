using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameObject Player;
    public newFPSControl FPSCon;


    public Image healthBar1;
    public Image healthBar2;
    public Text ammoCount;
    public Image dashBar;
    public Text pickUp;
    public Image reloadBar;
    public Image reloadBarBack;
    public Image ability;
    

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        FPSCon = Player.GetComponent<newFPSControl>();
        reloadBarBack.gameObject.SetActive(false);
        reloadBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMan.instance.pickup == true)
        {
            pickUp.gameObject.SetActive(true);
        }
        else
        {
            pickUp.gameObject.SetActive(false);
        }

        dashBar.fillAmount = GameMan.instance.dashTimeg / 5;
        ammoCount.text = GameMan.instance.ammo.ToString() + " / " + GameMan.instance.ammoMax.ToString(); ;
        healthBar1.fillAmount = GameMan.instance.playerHealth / GameMan.instance.playerHealthMax;
        healthBar2.fillAmount = GameMan.instance.playerHealth / GameMan.instance.playerHealthMax;
        if(GameMan.instance.IsReloading == true)
        {
            reloadBarBack.gameObject.SetActive(true);
            reloadBar.gameObject.SetActive(true);
            reloadBar.fillAmount = GameMan.instance.relT / GameMan.instance.relTM;
        }
        else
        {
            reloadBarBack.gameObject.SetActive(false);
            reloadBar.gameObject.SetActive(false);
        }
        ability.fillAmount = FPSCon.abilityCooldown / FPSCon.abilityCooldownMax;
    }
    
    

    
}
