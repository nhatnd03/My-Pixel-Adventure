using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleScript : MonoBehaviour
{
    private enum AnimationState { noSpikesIdle, spikeOut, spikesIdle, spikesIn };
    private AnimationState currentState = AnimationState.noSpikesIdle;

    public float spikeOutDelay = 2f;  // Time to wait before transitioning to spikeOut
    public float spikesIdleDelay = 4f; // Time to wait before transitioning to spikesIn
    public float spikeOutToSpikeIdleDelay = 0.2f;  
    public float spikesInToNoSpikeIdleDelay = 0.2f;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public LayerMask playerLayer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(StateCycle());
    }

    private IEnumerator StateCycle()
    {
        while (true)
        {
            /* Ban đầu, vô game thì không có spike, sau time thì mọc spike và spikeIdle (delay nhẹ để animation chạy)
             tiếp đó sau time thì spike thu lại, delay nhẹ rồi chuyển tới không spike, lặp như vậy*/
            switch (currentState)
            {
                case AnimationState.noSpikesIdle:
                    anim.SetInteger("state",(int)currentState);
                    yield return new WaitForSeconds(spikeOutDelay);
                    currentState = AnimationState.spikeOut;
                    break;
                case AnimationState.spikeOut:
                    anim.SetInteger("state", (int)currentState);
                    yield return new WaitForSeconds(spikeOutToSpikeIdleDelay); // short delay for the spikeOut animation to complete
                    currentState = AnimationState.spikesIdle;
                    break;
                case AnimationState.spikesIdle:
                    anim.SetInteger("state", (int)currentState);
                    yield return new WaitForSeconds(spikesIdleDelay);
                    currentState = AnimationState.spikesIn;
                    break;
                case AnimationState.spikesIn:
                    anim.SetInteger("state", (int)currentState);
                    yield return new WaitForSeconds(spikesInToNoSpikeIdleDelay); // short delay for the spikesIn animation to complete
                    currentState = AnimationState.noSpikesIdle;
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Nếu player va vào Turtle trong khi đang mọc gai thì player die, thêm cái lúc mọc ra cx đc, tạm như này thôi
            if (currentState == AnimationState.spikesIdle)
            {
                collision.gameObject.GetComponent<PlayerLife>().Die();
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
                anim.SetTrigger("die");
                StartCoroutine(DelayDie());
            }
        }
    }

    // Cho animation chạy hết
    private IEnumerator DelayDie()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

}
