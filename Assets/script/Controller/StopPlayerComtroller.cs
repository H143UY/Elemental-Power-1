using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlayerComtroller : MonoBehaviour
{
    private void Awake()
    {
        this.RegisterListener(EventID.SinhQuaiXong, (sender, param) =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
