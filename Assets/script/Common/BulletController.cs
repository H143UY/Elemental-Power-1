using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MoveController
{
    private float timer;
    protected override void Move(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            this.gameObject.transform.position += direction*Time.deltaTime;
        }
        base.Move(direction);
    }
    protected virtual void BulletEx()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            this.PostEvent(EventID.BoomBullet);
            timer = 0;
        }
    }

}