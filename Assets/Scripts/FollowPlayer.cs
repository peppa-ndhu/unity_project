using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0f, -3f, 6f);
    public float rotationSpeed;
    private PlayerController playerControllerScript;
    private SpawnManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // ����ƹ���X�b���ʭ�
        float mouseX = Input.GetAxis("Mouse X");

        // �ھڷƹ��������ʪ��t�רӭp����ਤ��
        float rotationAmount = mouseX * rotationSpeed;

        if (playerControllerScript.gameOver == false && gameManager.isGameActive == true)
        {
            // �ϥ�Quaternion.Euler�N���ਤ���ഫ��Quaternion
            Quaternion rotation = Quaternion.Euler(0f, rotationAmount, 0f);

            // �N�ṳ�����α���
            player.transform.rotation *= rotation;

            // �p��ؼЪ����ਤ�ס]�u�O�dy�b����^
            float playerRotation = player.transform.eulerAngles.y;

            // �ϥΥ|���ƨӳ]�w�ṳ��������
            Quaternion cameraRotation = Quaternion.Euler(0f, playerRotation, 0f);

            // �p��۾����ؼЦ�m
            Vector3 playerPosition = player.transform.position - (cameraRotation * offset);

            // ��s�۾���m�M����
            transform.position = playerPosition;
            transform.rotation = cameraRotation;
        }
    }
}
