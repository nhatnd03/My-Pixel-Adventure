using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    //private int totalScene = 2;
    private AudioSource finishSoundEffect;
    private int currentLevel; 
    private bool levelCompleted = false;
    void Start()
    {
        finishSoundEffect = GetComponent<AudioSource>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !levelCompleted)
        {
            if (collision.gameObject.GetComponent<ItemCollector>().cherries >= (currentLevel * 5))
            {
                finishSoundEffect.Play();
                levelCompleted = true;
                Invoke("CompleteLevel", 2f);
            }
        }
    }
    private void CompleteLevel()
    {
        if (true)
        {
            SceneManager.LoadScene(currentLevel + 1);
        }

    }
}
