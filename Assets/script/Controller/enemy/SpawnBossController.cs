using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBossController : MonoBehaviour
{
    //private bool SpawnBoss;
    public GameObject Boss;
    public Transform spawnPos;
    private void Awake()
    {
        this.RegisterListener(EventID.SpawnBoss, (sender, param) =>
        {
            SmartPool.Instance.Spawn(Boss.gameObject, spawnPos.position, spawnPos.rotation);
            this.gameObject.SetActive(false);
        });
    }
}
