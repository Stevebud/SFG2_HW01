using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : ProjectileBase
{

    [SerializeField] int _damage;
    [SerializeField] float _lifeDuration;
    protected override void Impact(Collision otherCollision)
    {
        Player _isPlayer = GetComponent<Player>();
        if (_isPlayer != null)
        {
            Destroy(gameObject);
            _isPlayer.DecreaseHealth(_damage);
        }
    }

    protected override void Move()
    {
        base.Move();
    }

    private void Awake()
    {
        Destroy(this.gameObject, _lifeDuration);
    }
}
