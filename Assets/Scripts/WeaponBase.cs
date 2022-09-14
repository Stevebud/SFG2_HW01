using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] Transform _projectileSpawn;
    [SerializeField] GameObject _projectile;
    [SerializeField] float _fireRate;
    [SerializeField] ParticleSystem _fireParticles;
    [SerializeField] AudioClip _fireAudio;

    protected virtual void Fire()
    {
        GameObject project = Instantiate(_projectile, _projectileSpawn);

    }
}
