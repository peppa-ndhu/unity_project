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
        // 獲取滑鼠的X軸移動值
        float mouseX = Input.GetAxis("Mouse X");

        // 根據滑鼠水平移動的速度來計算旋轉角度
        float rotationAmount = mouseX * rotationSpeed;

        if (playerControllerScript.gameOver == false && gameManager.isGameActive == true)
        {
            // 使用Quaternion.Euler將旋轉角度轉換為Quaternion
            Quaternion rotation = Quaternion.Euler(0f, rotationAmount, 0f);

            // 將攝像機應用旋轉
            player.transform.rotation *= rotation;

            // 計算目標的旋轉角度（只保留y軸旋轉）
            float playerRotation = player.transform.eulerAngles.y;

            // 使用四元數來設定攝像機的旋轉
            Quaternion cameraRotation = Quaternion.Euler(0f, playerRotation, 0f);

            // 計算相機的目標位置
            Vector3 playerPosition = player.transform.position - (cameraRotation * offset);

            // 更新相機位置和旋轉
            transform.position = playerPosition;
            transform.rotation = cameraRotation;
        }
    }
}
