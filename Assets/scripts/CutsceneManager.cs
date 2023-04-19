using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public float timeToWait;//duration of cutscene
    public int sceneToLoad;
    //public GameObject trigger;
    public GameObject player;
    public GameObject canvas;
    public GameObject timeLine;
    //public cutscene

    public bool isTimeline;
    public bool isSomethingelse;

    private void Start()
    {
        player.SetActive(true);
        canvas.SetActive(true);
        timeLine.SetActive(false);
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            
            if (isTimeline)
            {
                timeLine.SetActive(true);
                player.SetActive(false);
                canvas.SetActive(false);
            }
            
            //load cutscene timeline or scene
            StartCoroutine(cutsceneTrigger());
        }
    }

    IEnumerator cutsceneTrigger()
    {
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(false);
        if (isTimeline)
        {
            player.SetActive(true);
            canvas.SetActive(true);
            timeLine.SetActive(false);
        }
    }
}
