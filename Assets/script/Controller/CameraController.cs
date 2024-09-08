using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float SpeedCamera;
    private Vector3 transPlayer;
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        if (player != null)
        {
            transPlayer = player.transform.position;
        }
    }
    private void LateUpdate()
    {
        Vector3 newPos = new Vector3(transPlayer.x, transPlayer.y, -10f);
        this.gameObject.transform.position = newPos;
    }
}