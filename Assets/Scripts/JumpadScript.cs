using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpadScript : MonoBehaviour
{
    [SerializeField] private float jumpPadForce = 15f;
    private Animator anim;
    private bool isActive = false;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isActive = true;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpPadForce;
            StartCoroutine(DelayActiveAnimation());
        }
    }
    private IEnumerator DelayActiveAnimation()
    {
        anim.SetBool("active", isActive);
        yield return new WaitForSeconds(0.4f);
        isActive = false;
        anim.SetBool("active", isActive);
    }
}
