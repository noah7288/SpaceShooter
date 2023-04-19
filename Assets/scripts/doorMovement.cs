using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorMovement : MonoBehaviour
{
    public Collider enemyDetect;

    public GameObject doorL;
    public GameObject doorR;
    public float doorSpeed;
    public Transform enemyChecker;

    

    public bool doorOpen;
    
    public Transform openLeft;
    public Transform openRight;
    public Transform closedLeft;
    public Transform closedRight;

    public LayerMask enemy;
    public bool isOpenable;

    // Start is called before the first frame update
    void Start()
    {
        enemyDetect = gameObject.GetComponent<BoxCollider>();

        

    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenable == true)
        {
            if (Physics.CheckBox(enemyChecker.position, new Vector3(20, 5, 30), enemyChecker.rotation, enemy))
            {
                doorOpen = false;
            }
            else
            {
                doorOpen = true;
            }
        }

        if (doorOpen == true)
        {
            doorL.transform.position = Vector3.MoveTowards(doorL.transform.position, openLeft.position, doorSpeed * Time.deltaTime);
            doorR.transform.position = Vector3.MoveTowards(doorR.transform.position, openRight.position, doorSpeed * Time.deltaTime);

            //if (doorR.transform.localPosition.z >= openMax)
            //{
            //    doorR.transform.Translate(0f, 0f, doorSpeed);
            //}
            //if (doorL.transform.position.x <= -openMax)
            //{
            //    doorL.transform.Translate(0f, 0f, -doorSpeed);
            //}
        }
        else if (doorOpen == false)
        {
            doorL.transform.position = Vector3.MoveTowards(doorL.transform.position, closedLeft.position, doorSpeed * Time.deltaTime);
            doorR.transform.position = Vector3.MoveTowards(doorR.transform.position, closedRight.position, doorSpeed * Time.deltaTime);


            //if (doorR.transform.localPosition.z >= openMin)
            //{
            //    doorR.transform.Translate(0f, 0f, -doorSpeed);
            //}
            //if (doorL.transform.position.x <= -openMin)
            //{
            //    doorL.transform.Translate(0f, 0f, doorSpeed);
            //}
        }

    }

     private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != 8)
        {
            doorOpen = true;
            
        }

    }
}
