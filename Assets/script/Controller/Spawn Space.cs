using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpace : MonoBehaviour
{
    public Transform PosSpawn1;
    public Transform PosSpawn2;
    public GameObject SpacePortal;
    private void Awake()
    {
        this.RegisterListener(EventID.SpawnSpace, (sender, param) =>
        {
            SmartPool.Instance.Spawn(SpacePortal, PosSpawn1.transform.position, PosSpawn1.transform.rotation);
            SmartPool.Instance.Spawn(SpacePortal, PosSpawn2.transform.position, PosSpawn2.transform.rotation);
        });
    }
}
