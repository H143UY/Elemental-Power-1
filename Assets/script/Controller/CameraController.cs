using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float SpeedCamera;
    public float CameraY;

    private void LateUpdate()
    {
        Transform player = PlayerController.Instance.transform;
        Vector3 newPos = new Vector3(player.position.x, player.transform.position.y , -10f);
    }
}