using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : ProjectileBase
{

    [SerializeField] int _damage;
    [SerializeField] float _lifeDuration;
    [SerializeField] GameObject _projectile;
    protected override void Impact(Collision otherCollision)
    {
        IDamageable health = otherCollision.gameObject.GetComponent<IDamageable>();
        Player player = otherCollision.gameObject.GetComponent<Player>();
        if (health != null && player == null)
        {
            health.TakeDamage(_damage);
        }
        Split();
    }

    protected override void Move()
    {
        base.Move();
    }

    private void Awake()
    {
        Destroy(this.gameObject, _lifeDuration);
    }

    private void Split()
    {
        GameObject split1 = Instantiate(_projectile, this.gameObject.transform.position, this.gameObject.transform.rotation);
        GameObject split2 = Instantiate(_projectile, this.gameObject.transform.position, this.gameObject.transform.rotation);
        split1.transform.Rotate(0, 135, 0);
        split1.transform.localScale = this.transform.localScale / 2;
        split2.transform.Rotate(0, 215, 0);
        split2.transform.localScale = this.transform.localScale / 2;
        Destroy(this.gameObject);
    }
}
