using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletShard : ProjectileBase
{
    [SerializeField] int _damage;
    [SerializeField] float _lifeDuration;

    protected override void Impact(Collision otherCollision)
    {
        IDamageable health = otherCollision.gameObject.GetComponent<IDamageable>();
        Player player = otherCollision.gameObject.GetComponent<Player>();
        if (health != null)// && player == null)
        {
            health.TakeDamage(_damage);
        }
        Destroy(gameObject);
    }

    private void Awake()
    {
        Destroy(this.gameObject, _lifeDuration);
    }

    protected override void Move()
    {
        base.Move();
    }
}
