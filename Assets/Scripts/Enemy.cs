using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    public GameObject player;
    private Rigidbody enemyRb;
    private float rotationSpeed = 2.0f;
    private PlayerController playerControllerScript;
    private SpawnManager gameManager;
    private Animator enemyAnim;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        enemyRb.freezeRotation = true;
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f; // �u�Ҽ{������V
        direction.Normalize();

        speed = gameManager.enemySpeed;
        //Debug.Log(speed);

        if (playerControllerScript.gameOver == false && gameManager.isGameActive == true)
        {
            enemyAnim.SetFloat("Speed_f", 1f);

            if (direction != Vector3.zero)
            {
                // �p��ĤH�ݭn���઺�ؼФ�V
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            enemyRb.AddForce((player.transform.position - transform.position).normalized * speed);
            //Debug.Log(speed);
        }
        else
        {
            enemyAnim.SetFloat("Speed_f", 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.position -= collision.contacts[0].normal * collision.relativeVelocity.magnitude * Time.deltaTime;
    }
}
