using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;
    public Rigidbody playerRb;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private float upBoundary;
    

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;
    private float velo;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip hitTheGroundSound;



    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        upBoundary = 13;

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        playerRb = gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && transform.position.y < 15)
        {
            playerRb.AddForce(Vector3.up * floatForce * Time.deltaTime, ForceMode.Impulse);
        }
        else if(transform.position.y >= 15)
        {
            // Don't go up anymore
            transform.position = new Vector3(-3, 15, 0);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 5.0f);
            Destroy(other.gameObject);

        }

        else if (other.gameObject.CompareTag("Ground"))
        {
            gameOver = true;
            Debug.Log("Game Over");
            playerAudio.PlayOneShot(hitTheGroundSound, 1.0f);
            playerRb.AddForce(Vector3.up * floatForce / 2);
        }

    }

}
