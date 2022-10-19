using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBombs : MonoBehaviour
{
    [SerializeField] Transform _projectileSpawn;
    [SerializeField] GameObject _projectile;
    [SerializeField] float _fireRate;
    [SerializeField] ParticleSystem _fireParticles;
    [SerializeField] AudioClip _fireAudio;

    [SerializeField] Transform _playerTransform;

    private float _lastFire;
    private bool _refsFilled;

    private void Awake()
    {
        _lastFire = Time.time;
        if (_projectileSpawn != null && _projectile != null && _fireParticles != null && _fireAudio != null)
        {
            _refsFilled = true;
        }
        else
        {
            _refsFilled = false;
        }
    }

    public void Fire()
    {
        if (_lastFire + (60 / _fireRate) <= Time.time)//if it is time to fire
        {
            if (_refsFilled)
            {
                //Instantiate projectile at _projectileSpawn
                GameObject project = Instantiate(_projectile, _projectileSpawn.position, _projectileSpawn.rotation);
                Bomb droppedBomb = project.GetComponent<Bomb>();
                if (droppedBomb != null && _playerTransform !=null)
                {
                    droppedBomb.FillPlayerTransform(_playerTransform);
                }
                //Visual and Audio Feedback
                ParticleSystem particles = Instantiate(_fireParticles, _projectileSpawn.position, _projectileSpawn.rotation);
                Destroy(particles.gameObject, particles.main.duration);
                AudioHelper.PlayClip2D(_fireAudio, 1);
            }
            //set next fire time
            _lastFire = Time.time;
        }
    }
}
