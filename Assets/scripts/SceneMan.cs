using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    public GameObject Main;
    public GameObject Inst;


    private void Awake()
    {
        Main.SetActive(true);
        Inst.SetActive(false);
    }
    void Start()
    {
        if (Input.anyKey)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Chapter1");
    }

    public void Intructions()
    {
        Main.SetActive(false);
        Inst.SetActive(true);
    }

    public void ReturnToMain()
    {
        Main.SetActive(true);
        Inst.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
