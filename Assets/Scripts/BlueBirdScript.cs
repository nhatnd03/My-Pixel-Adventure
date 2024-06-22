using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBirdScript : MonoBehaviour
{
    public Transform[] waypoints; // Mảng waypoint
    private int currentWaypoint = 0; // Waypoint hiện tại
    public float moveSpeed = 3f;


    private Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer spriteRenderer;

    public LayerMask playerLayer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 targetPosition = waypoints[currentWaypoint].position;
        Vector3 moveDirection = targetPosition - transform.position;
        // Khi độ dài vector moveDirection < 0.1, tức đã tới target, chuyển target tiếp theo và dừng
        // Ở đây dùng distance cũng đc, tiện dùng moveDirection luôn
        if (moveDirection.magnitude < 0.1f)
        {
            // 1 cách thông minh hơn để chuyển waypoint, thay vì kiểm tra >= Length rồi đặt lại = 0
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
        else
        {
            // Di chuyển tới target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (moveDirection.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    public bool TouchFromAbove()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size * 0.9f, 0f, Vector2.up, 0.2f, playerLayer);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && TouchFromAbove())
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
            anim.SetTrigger("die");
            StartCoroutine(DelayDie());
        }
        else
        {
            collision.gameObject.GetComponent<PlayerLife>().Die();
        }
    }
    private IEnumerator DelayDie()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
