using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    //movement fields
    [SerializeField] float _maxSpeed = .25f;
    [SerializeField] float _turnSpeed = 2f;

    //weapon fields
    [SerializeField] Transform _projectileSpawn;
    [SerializeField] GameObject _projectile;
    [SerializeField] float _fireCooldown = 1f;
    [SerializeField] ParticleSystem _fireParticles;
    [SerializeField] AudioClip _fireAudio;

    private float _nextFire;

    public float MaxSpeed
    {
        // => is what is known as a lambda operator and is shorthand for get{} and set{}
        get => _maxSpeed;
        set => _maxSpeed = value;
    }

    Rigidbody _rb = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //get inputs for tank
        Fire();
    }

    private void FixedUpdate()
    {
        MoveTank();
        TurnTank();
    }

    public void MoveTank()
    {
        // calculate the move amount
        float moveAmountThisFrame = Input.GetAxis("Vertical") * _maxSpeed;
        // create a vector from amount and direction
        Vector3 moveOffset = transform.forward * moveAmountThisFrame;
        // apply vector to the rigidbody
        _rb.MovePosition(_rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
    }

    public void TurnTank()
    {
        // calculate the turn amount
        float turnAmountThisFrame = Input.GetAxis("Horizontal") * _turnSpeed;
        // create a Quaternion from amount and direction (x,y,z)
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // apply quaternion to the rigidbody
        _rb.MoveRotation(_rb.rotation * turnOffset);
    }

    private void Fire()
    {
        bool _firePressed = Input.GetKeyDown(KeyCode.Space);
        if (_firePressed && (_nextFire <= Time.time))
        {
            if(_projectileSpawn != null)
            {
                //Instantiate projectile at _projectileSpawn
                if (_projectile != null)
                {
                    Instantiate(_projectile, _projectileSpawn.position, _projectileSpawn.rotation);
                }
                //Visual and Audio Feedback
                if (_fireParticles != null)
                {
                    ParticleSystem particles = Instantiate(_fireParticles, _projectileSpawn.position, _projectileSpawn.rotation);
                    Destroy(particles.gameObject, particles.main.duration);
                }
                if (_fireAudio != null)
                {
                    AudioHelper.PlayClip2D(_fireAudio, 1);
                }
            }
            //set next fire time
            _nextFire = Time.time+_fireCooldown;
        }
    }
}
