using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile1 : MonoBehaviour
{
    
    //public int projectileSpeed = 400;
    public float projectileLifeTime = 5.0f;
    private Collider cl;
    private Rigidbody rb;
    
    //public Component Rigidbody2d;
    void Start()
    {
        cl = gameObject.GetComponent<CapsuleCollider>();
        rb = gameObject.GetComponent<Rigidbody>();
        
        
    }

    
    void Update()
    {

        DestroyProjectile();

        

    }

    void FixedUpdate()
    {
        
    }

    

    void DestroyProjectile()
    {
        projectileLifeTime = projectileLifeTime - Time.deltaTime;

        if (projectileLifeTime <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag != "Player" & other.gameObject.layer != 11 & other.gameObject.tag != "projectile" & other.gameObject.tag != "projectile1" & other.gameObject.tag != "projectile2" & other.gameObject.tag != "projectile3" & other.gameObject.tag != "enemydetect")
        if (other.gameObject.tag == "Player" || other.gameObject.layer == 6 || other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {

            Destroy(gameObject);
        }
    }
    
}
