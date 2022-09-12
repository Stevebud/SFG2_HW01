using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class ProjectileBase : MonoBehaviour
{
    
    [SerializeField] float speed = 1f;
    [SerializeField] protected Rigidbody _rigidBody;

    [SerializeField] AudioClip impact_sound;
    [SerializeField] ParticleSystem impact_particles;

    //to be called whenever a collision is made
    protected abstract void Impact(Collision otherCollision);

    //On collision call Impact() and provide feedback
    private void OnCollisionEnter(Collision collision)
    {
        Impact(collision);

        //Feedback
        if(impact_sound != null)
        {
            AudioHelper.PlayClip2D(impact_sound, 1f);
        }
        if (impact_particles != null)
        {
            impact_particles = Instantiate(impact_particles, transform.position, Quaternion.identity);
        }
    }


    protected virtual void Move()
    {
        Vector3 moveOffset = transform.forward * speed;
        _rigidBody.MovePosition(_rigidBody.position + moveOffset);
    }
}
