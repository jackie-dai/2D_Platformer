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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (currentScene.name == "Level1")
            {
                SceneManager.LoadScene("Level2");
            }
        }
    }
}
