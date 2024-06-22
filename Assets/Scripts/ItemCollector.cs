using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public int cherries = 0;
    private int currentLevel;
    [SerializeField] private TextMeshProUGUI cherriesText;


    [SerializeField] private AudioSource itemCollectingSoundEffect;
    private void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {
            itemCollectingSoundEffect.Play();
            GameObject item = collision.gameObject;
            cherries++;
            cherriesText.text = "Cherries: " + cherries + " / " + currentLevel * 5;
            StartCoroutine(DelayDetroyItem(item));
            
        }
    }
    private IEnumerator DelayDetroyItem(GameObject item)
    {
        item.GetComponent<Animator>().SetTrigger("isCollected");
        yield return new WaitForSeconds(0.25f);
        Destroy(item);
    }
}
