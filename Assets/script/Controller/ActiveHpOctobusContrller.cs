using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveHpOctobusContrller : MonoBehaviour
{
    private void Awake()
    {
        this.RegisterListener(EventID.SinhQuaiXong, (sender, param) =>
        {
            this.gameObject.SetActive(true);
        });
    }
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
}
