using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public float turnSpeed = 10;
    private float horizontalInput;
    private float forwardInput;
    private Rigidbody playerRb;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    private Animator playerAnim;
    public GameObject projectilePrefab;
    public int bullet;
    public TextMeshProUGUI gameOverText;
    public bool gameOver = false;
    private PlayerController playerControllerScript;
    public Button restartButton;
    private SpawnManager gameManager;
    public ParticleSystem explosionParticle;
    public ParticleSystem fireworkParticle;
    private GameObject firework;
    public AudioClip shutSound;
    private AudioSource playerAudio;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
        playerRb.freezeRotation = true;
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        firework = GameObject.Find("Fireworks");
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        if (playerControllerScript.gameOver == false && gameManager.isGameActive == true)
        {
            // Move Player forward
            transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
            transform.Translate(Vector3.right * Time.deltaTime * turnSpeed * horizontalInput);
            //Debug.Log(forwardInput);

            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
                playerAnim.SetTrigger("Jump_trig");
            }

            // 根據玩家的輸入設置觸發器
            // 假設按下Shift鍵代表跑步
            if (forwardInput != 0 || horizontalInput != 0)
            {
                playerAnim.SetFloat("Speed_f", 0.75f);

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed = 15;
                    turnSpeed = 15;
                    playerAnim.SetFloat("Speed_f", 1.0f);
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    speed = 10;
                    turnSpeed = 10;
                    playerAnim.SetFloat("Speed_f", 0.75f);
                }
            }
            else
            {
                playerAnim.SetFloat("Speed_f", 0.5f);
            }

            if (Input.GetKeyDown(KeyCode.C) && bullet > 0)
            {
                // Launch a projectile from the player
                //取得玩家當前的y Rotate
                Quaternion playerRotation = Quaternion.Euler(90f, transform.eulerAngles.y, 0f);
                Vector3 spawnPosition = transform.position + transform.forward * 2f;
                Instantiate(projectilePrefab, spawnPosition + new Vector3(0, 0.5f, 0), playerRotation);
                bullet = bullet - 1;
                playerAudio.PlayOneShot(shutSound, 1.0f);
            }
        }

        if (transform.position.y < -3)
        {
            gameOver = true;
            playerAnim.SetFloat("Speed_f", 0.5f);
            GameOver();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isOnGround = true;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameOver = true;
            playerAnim.SetFloat("Speed_f", 0.5f);
            GameOver();
        }

        transform.position -= collision.contacts[0].normal * collision.relativeVelocity.magnitude * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            bullet = bullet + 1;
            Destroy(other.gameObject);
            firework.transform.position = other.transform.position;
            fireworkParticle.Play();
        }
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}