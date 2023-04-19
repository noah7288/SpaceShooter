using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Pathfinding;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{
    
    public Transform body;

    //level
    public Text nameLVL;
    public int level;
    public Transform dropPoint;
    public GameObject ammo;
    public GameObject healthDrop;
    private float dropSpread;
    Quaternion angle;
    public GameObject aimAssist;

    public Transform playerOb;
    public Transform Player;
    public GameObject lineofsight;

    public GameObject projectile1;
    public GameObject Tracklight;

    public float health;
    private float healthMax;

    public bool playerVis;
    public bool playerInAttackRange;

    public Transform gunEnd;

    public GameObject rig;
    public Animator anim;
    private float animtime;
    float dist;

    //RaycastHit hit;

    //private int patCount;
    public GameObject[] patPoints;
    public Transform patPre;

    [Header("cover")]
    public NavMeshAgent Agent;

    public LayerMask cover;
    public LayerMask groundLayer, playerlayer;

    //patrol
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //attack
    public float wait;

    //states?
    public float sightRange;
    public float attackRange;

    public float sightRangeDef;
    bool isDead;
    Collider col;

    public Slider healthBar;

    //[Range(-1, 1)]
    //[Tooltip("Lower is a better hiding spot")]
    //public float HideSensitivity = 0;
    //[Range(1, 10)]
    //public float MinPlayerDistance = 5f;
    //[Range(0, 5f)]
    //public float MinObstacleHeight = 1.25f;
    //[Range(0.01f, 1f)]
    //public float UpdateFrequency = 0.25f;


    private Collider[] Colliders = new Collider[10];

    public enum state
    {
        patrol,
        follow,
        attack,
        flee,
        idle,
        dead,
    }

    public state currentState;
    //private float dist;
    void Start()
    {
        aimAssist.SetActive(true);
        dropchance = Random.Range(0, 100);
        level = Random.Range(1, 3);
        nameLVL.text = "Mechanoid LVL." + level.ToString();
        Agent = GetComponent<NavMeshAgent>();

        playerOb = GameObject.FindWithTag("playerTracker").transform;
        Player = GameObject.FindWithTag("Player").transform;
        
        health = 1 + level;
        healthMax = 1 + level;
        sightRangeDef = sightRange;
        playerVis = false;

        
        currentState = state.patrol;
        anim = rig.GetComponent<Animator>();

        isDead = false;
        col = gameObject.GetComponent<BoxCollider>();
        col.enabled = true;
        Tracklight.SetActive(false);
        
    }

    

    //private IEnumerator Hide(Transform Target)
    //{
    //    WaitForSeconds Wait = new WaitForSeconds(UpdateFrequency);
    //    while (true)
    //    {
    //        for (int i = 0; i < Colliders.Length; i++)
    //        {
    //            Colliders[i] = null;
    //        }
    //        int hits = Physics.OverlapSphereNonAlloc(gameObject.transform.position, 20f, Colliders, cover);

    //        int hitReduction = 0;
    //        for (int i = 0; i < hits; i++)
    //        {
    //            if (Vector3.Distance(Colliders[i].transform.position, Target.position) < MinPlayerDistance || Colliders[i].bounds.size.y < MinObstacleHeight)
    //            {
    //                Colliders[i] = null;
    //                hitReduction++;
    //            }
    //        }
    //        hits -= hitReduction;

    //        System.Array.Sort(Colliders, ColliderArraySortComparer);

    //        for (int i = 0; i < hits; i++)
    //        {
    //            if (NavMesh.SamplePosition(Colliders[i].transform.position, out NavMeshHit hit, 2f, Agent.areaMask))
    //            {
    //                if (!NavMesh.FindClosestEdge(hit.position, out hit, Agent.areaMask))
    //                {
    //                    Debug.LogError($"Unable to find edge close to {hit.position}");
    //                }

    //                if (Vector3.Dot(hit.normal, (Target.position - hit.position).normalized) < HideSensitivity)
    //                {
    //                    Agent.SetDestination(hit.position);
    //                    break;
    //                }
    //                else
    //                {
    //                    if (NavMesh.SamplePosition(Colliders[i].transform.position - (Target.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, Agent.areaMask))
    //                    {
    //                        if (!NavMesh.FindClosestEdge(hit2.position, out hit2, Agent.areaMask))
    //                        {
    //                            Debug.LogError($"Unable to find edge close to {hit2.position} (second attempt)");
    //                        }

    //                        if (Vector3.Dot(hit2.normal, (Target.position - hit2.position).normalized) < HideSensitivity)
    //                        {
    //                            Agent.SetDestination(hit2.position);
    //                            break;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        yield return Wait;
    //    }
    //}

    public int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if (A != null && B == null)
        {
            return -1;
        }
        else if (A == null && B == null)
        {
            return 0;
        }
        else
        {
            return Vector3.Distance(Agent.transform.position, A.transform.position).CompareTo(Vector3.Distance(Agent.transform.position, B.transform.position));
        }
    }

    private void FixedUpdate()
    {
        if (wait >= -0.1f)
        {
            wait = wait - Time.deltaTime;
        }
        if (animtime >= 0)
        {
            anim.SetBool("shooting", true);
            animtime = animtime - Time.deltaTime;
        }
        else
        {
            anim.SetBool("shooting", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.transform.LookAt(Player); 
        healthBar.value = health / healthMax;
        if(health <= 0)
        {
            healthBar.gameObject.SetActive(false);
            currentState = state.dead;
            aiBehavior();
        }
        //Vector3 dir = (gameObject.transform.position - playerOb.position);
        

        playerVis = Physics.CheckSphere(transform.position, sightRange, playerlayer);
        if (playerVis == true)
        {
            sightRange = sightRangeDef * 1.5f;
        }
        else if (playerVis == false)
        {
            sightRange = sightRangeDef;
        }
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerlayer);

        



        dist = Vector3.Distance(Player.position, gameObject.transform.position);
        
        if (playerVis && !playerInAttackRange && isDead != true) //dist <= 30 & playerVis == true)
        {
            currentState = state.follow;
            aiBehavior();
        }
        if (!playerVis && !playerInAttackRange && isDead != true) //dist >= 40 & playerVis == false)
        {
            if (currentState != state.patrol)
            {
                currentState = state.patrol;
            }
            aiBehavior();
            
        }
        if (playerVis && playerInAttackRange && isDead != true) //dist >= 55)
        {
            currentState = state.attack;
            aiBehavior();
        }

        //Death();
    }
    public float dropchance;
    void aiBehavior()
    {
        GameObject drop;
        Rigidbody rb;
        
        switch (currentState)
        {
            case state.patrol:
                Tracklight.SetActive(false);
                body.localEulerAngles = new Vector3(body.rotation.x, 0, body.rotation.z);
                anim.SetBool("moving", true);
                anim.SetBool("shooting", false);
                if (!walkPointSet && dist <= 150) //make distance variable
                {
                    
                    float randomZ = Random.Range(-walkPointRange, walkPointRange);
                    float randomX = Random.Range(-walkPointRange, walkPointRange);

                    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

                    NavMeshHit hit;
                    if(NavMesh.SamplePosition(walkPoint, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        walkPoint = hit.position;
                        walkPointSet = true;
                    }

                    
                }
                if (walkPointSet)
                {
                    Agent.SetDestination(walkPoint);
                }

                Vector3 distanceToWalkPoint = transform.position - walkPoint;

                if (distanceToWalkPoint.magnitude < 1f)
                {
                    walkPointSet = false;
                }

                break;
            case state.follow:
                Tracklight.SetActive(true);
                anim.SetBool("moving", true);
                anim.SetBool("shooting", false);
                body.LookAt(playerOb);

                Agent.SetDestination(Player.position);

                if(dist <= ((attackRange + sightRange)/2))
                {
                    Shoot();
                }

                break;
            case state.attack:
                Tracklight.SetActive(true);
                anim.SetBool("moving", false);

                //attack behavior here
                
                Agent.SetDestination(transform.position);

                body.LookAt(playerOb);

                Shoot();
                

                break;
            case state.flee:
                
                //Hide(Player);
                //run away or to cover
                break;
            case state.dead:
                
                if (dropchance > 70)
                {
                    dropSpread = Random.Range(0, 2);
                    angle = Random.rotation;
                    drop = Instantiate(ammo, dropPoint.position, dropPoint.rotation);
                    drop.transform.rotation = Quaternion.RotateTowards(drop.transform.rotation, angle, dropSpread);
                    rb = drop.GetComponent<Rigidbody>();
                    rb.AddForce(drop.transform.forward * 3, ForceMode.Impulse);
                    dropchance = -1f;

                }
                else if(dropchance < 20 & dropchance >= 0)
                {
                    dropSpread = Random.Range(0, 2);
                    angle = Random.rotation;
                    drop = Instantiate(healthDrop, dropPoint.position, dropPoint.rotation);
                    drop.transform.rotation = Quaternion.RotateTowards(drop.transform.rotation, angle, dropSpread);
                    rb = drop.GetComponent<Rigidbody>();
                    rb.AddForce(drop.transform.forward * 3, ForceMode.Impulse);
                    dropchance = -1f;
                }

                Tracklight.SetActive(false);
                anim.SetBool("moving", false);
                anim.SetBool("shooting", false);
                isDead = true;
                Agent.SetDestination(transform.position);
                transform.localEulerAngles = new Vector3(-90, transform.rotation.y, transform.rotation.z);
                body.localEulerAngles = new Vector3(body.rotation.x, 0, body.rotation.z);
                col.enabled = false;
                break;
            default:
                break;
        }
    }

    void Shoot()
    {
        gunEnd.transform.LookAt(Player);


        if (wait <= 0)
        {
            GameObject bullet = Instantiate(projectile1, gunEnd.position, gunEnd.rotation);
            Rigidbody rb2 = bullet.GetComponent<Rigidbody>();
            rb2.AddForce(bullet.transform.forward * 35, ForceMode.Impulse);
            wait = Random.Range(0.7f, 1);
            animtime = 0.25f;
        }
    }

    void Patrol()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == ("projectile"))
        {
            health--;
        }
    }

    //void Death()
    //{
    //    if (health <= 0)
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
