using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : ProjectileBase
{
    protected override void Impact(Collision otherCollision)
    {
        Enemy _isEnemy = GetComponent<Enemy>();
        if (_isEnemy != null)
        {
            Destroy(gameObject);
        }
    }

    protected override void Move()
    {
        base.Move();
    }

}
