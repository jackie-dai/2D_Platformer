using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private Scene currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(currentScene.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (currentScene.name == "Level1")
            {
                SceneManager.LoadScene("Level2");
            }

            if (currentScene.name == "Level2")
            {
                Debug.Log("OPrtal2");
                SceneManager.LoadScene("Level3");
            }
        }
    }
}
