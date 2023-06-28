using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPlayerAnimation : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite hitSprite;
    private float waitTime = 1f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite; // Set the initial sprite to the normal sprite
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Switch to the hit sprite immediately
            spriteRenderer.sprite = hitSprite;
        }
    }

    private IEnumerator Time(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            spriteRenderer.sprite = normalSprite;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Switch back to the normal sprite when the ball is no longer in contact
            StartCoroutine(Time(waitTime));
        }
    }
}
