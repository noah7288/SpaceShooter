using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    private float fuseTime;
    private float radius = 12.0f;
    private float damage = 3;
    public ParticleSystem explosion;

    void Start()
    {
        fuseTime = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (fuseTime <= 0)
        {
            Boom();
        }
    }

    private void FixedUpdate()
    {
        fuseTime = fuseTime - Time.deltaTime;
    }

    void Boom()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider near in colliders)
        {
            enemy enm = near.GetComponent<enemy>();
            newFPSControl fps = near.GetComponent < newFPSControl>();

            if (enm != null)
            {
                enm.health = enm.health - damage;
            }
            else if(fps != null)
            {
                fps.health = fps.health - damage;
            }
        }
        fuseTime = 10f;
        explosion.Play();
        //Destroy(gameObject);
        Invoke(nameof(Delete),0.5f);
    }

    void Delete()
    {
        Destroy(gameObject);
    }
}
