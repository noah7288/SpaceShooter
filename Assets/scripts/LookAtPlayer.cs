using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    Transform player;
    public GameObject enemy;
    public enemy eScript;

    void Start()
    {
        
        player = GameObject.FindWithTag("Player").transform;
        eScript = enemy.GetComponent<enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(eScript.health <= 0)
        {
            gameObject.SetActive(false);
        }
        transform.LookAt(player);
    }
}
