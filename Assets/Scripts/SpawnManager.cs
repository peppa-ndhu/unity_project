using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bulletPrefab;
    private float spawnRange = 30;
    private int enemyWave = 1;
    public int enemyCount;
    private int level;
    public TextMeshProUGUI levelText;
    public GameObject titleScreen;
    public bool isGameActive = false;
    public float enemySpeed = 0;



    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(enemyWave);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            enemyWave++; SpawnEnemyWave(enemyWave);
            UpdateLevel();
        }

        GameObject[] bullet = GameObject.FindGameObjectsWithTag("Bullet");
        if (enemyCount != 0 && enemyCount > bullet.Length)
        {
            SpawnShut();
        }
    }

    void SpawnEnemyWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    void SpawnShut()
    {
        GameObject[] bulletFloor = GameObject.FindGameObjectsWithTag("Floor");
        int randomIndex = Random.Range(0, bulletFloor.Length);
        GameObject randomTarget = bulletFloor[randomIndex];
        Instantiate(bulletPrefab, randomTarget.transform.position + new Vector3(0, 1, 0), randomTarget.transform.rotation);
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    private void UpdateLevel()
    {
        level += 1;
        levelText.text = "Level : " + level;
    }

    public void StartGame(float difficulty)
    {
        isGameActive = true;
        level = 0;
        UpdateLevel();
        titleScreen.gameObject.SetActive(false);
        enemySpeed = difficulty;
    }
}
