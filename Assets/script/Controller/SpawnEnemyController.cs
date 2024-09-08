using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SpawnEnemyController : MonoBehaviour
{
    private bool CanSpawn;
    private bool SpawnEnemy;
    public float spawnInterval;
    private float spawnTimer = 0f;
    public GameObject Quai1;
    public GameObject Quai2;
    public Transform PosSpawn1;
    public Transform PosSpawn2;
    private void Awake()
    {
        this.RegisterListener(EventID.SpawnSpace, (sender, param) =>
        {
            CanSpawn = true;
        });
        this.RegisterListener(EventID.EndSpace, (sender, param) =>
        {
            CanSpawn = false;
        });
    }
    void Start()
    {
        CanSpawn = false;
        SpawnEnemy = false;
        spawnTimer = 3.8f;
        spawnInterval = 5f;
    }
    void Update()
    {
        SpawnEnmey();
    }
    void SpawnEnmey()
    {
        if (CanSpawn)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy = true;
            }
            if (SpawnEnemy)
            {
                int randomQuai = Random.Range(0, 2);
                GameObject selectedQuai = (randomQuai == 0) ? Quai1 : Quai2;
                int randomPos = Random.Range(0, 2);
                Transform selectedPos = (randomPos == 0) ? PosSpawn1 : PosSpawn2;
                Instantiate(selectedQuai, selectedPos.position, selectedPos.rotation);
                SpawnEnemy = false;
                spawnTimer = 0f;
            }
        }
    }
}
