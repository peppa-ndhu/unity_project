using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shut : MonoBehaviour
{
    public float speed = 40.0f;
    private Rigidbody shutRigidbody;
    private PlayerController playerControllerScript;
    public GameObject smoke;

    // Start is called before the first frame update
    void Start()
    {
        shutRigidbody = GetComponent<Rigidbody>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        smoke = GameObject.Find("Smoke");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        if (transform.position.x > 40 || transform.position.x < -40 || transform.position.z > 40 || transform.position.z < -40)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            smoke.transform.position = other.transform.position;
            playerControllerScript.explosionParticle.Play();
        }
    }

}
