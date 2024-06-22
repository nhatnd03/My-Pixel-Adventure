using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    private Animator anim;
    private bool isHitted = false;
    private bool isFiring = false;
    [SerializeField] private float delayFireOffDuration = 1f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isFiring", isFiring);
    }

    // Player đi vô, đợi 0.5s rồi FireOn
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("enter collision");
            isHitted = true;
            anim.SetBool("isHitted", isHitted);
            Invoke(nameof(FireOn), 0.5f);
        }
    }
    // Nếu player stay, và FireOn -> Die
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isFiring)
        {
            isFiring = true;
            anim.SetBool("isFiring", isFiring);
            collision.gameObject.GetComponent<PlayerLife>().Die();
            //Debug.Log("collision stay, Player die!!!");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("collision exit");
            StartCoroutine(DelayFireOff());
        }
    }
    // Chờ 0.5s để animation cháy hết rồi Off nếu Player Exit
    private IEnumerator DelayFireOff()
    {
        yield return new WaitForSeconds(delayFireOffDuration);
        isFiring = false;
        isHitted = false;
        anim.SetBool("isFiring", isFiring);
        anim.SetBool("isHitted", isHitted);

    }

    private void FireOn()
    {
        isFiring = true;
        anim.SetBool("isFiring", true);
    }



}
