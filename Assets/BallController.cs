using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BallController : MonoBehaviour
{
    public GameObject panel;
    public Rigidbody2D rb2D;
    public float speed = 5f;
    public Vector3 vel;
    public bool isPlaying;
    public ScoreManager scoreManager;
    public TrailRenderer ballTrail;
    public AudioSource audioSource;
    public AudioClip collisionSound;



    // Start is called before the first frame update
    void Start()
    {   
        panel.SetActive(true);
        rb2D = GetComponent<Rigidbody2D>();
        ballTrail = GetComponentInChildren<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = collisionSound;
        
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && isPlaying == false)
        {
            ResetBallInRandomDirection();
            panel.SetActive(false);
        }

        if (rb2D.velocity.magnitude < speed * 0.5f)
        {
            ResetBall();
        }
    }

    private void ResetBall()
    {
        rb2D.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        isPlaying = false;
        ballTrail.enabled = false;
    }

    private void ResetBallInRandomDirection()

    {
        ResetBall();
        rb2D.velocity = GenerateRandomVelocity(true) * speed;
        vel = rb2D.velocity;
        isPlaying = true;
        ballTrail.enabled = true;
        ballTrail.Clear();
    }

    private Vector3 GenerateRandomVelocity(bool shouldReturnNormalized)
    {
        Vector3 velocity = new Vector3();
        bool shouldGoRight = Random.Range(1, 100) > 20;
        velocity.x = shouldGoRight ? Random.Range(.8f, .3f) : Random.Range(-.8f, -.3f);
        velocity.y = shouldGoRight ? Random.Range(.8f, .3f) : Random.Range(-.8f, -.3f);

        return shouldReturnNormalized ? velocity.normalized : velocity;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
        }

        rb2D.velocity = Vector3.Reflect(vel, collision.contacts[0].normal);
        Vector3 newVelocityWithOffset = rb2D.velocity;
        newVelocityWithOffset += new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        rb2D.velocity = newVelocityWithOffset.normalized * speed;
        vel = rb2D.velocity;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.x > 0)
            scoreManager.IncrementLeftPlayerScore();

        if (transform.position.x < 0)
            scoreManager.IncrementRightPlayerScore();

        ResetBall();
        panel.SetActive(true);
    }
}