using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour
{
    [SerializeField] float _powerupDuration = 3;

    [SerializeField] ParticleSystem _collectParticles;
    [SerializeField] AudioClip _collectSound;

    Collider puCollider;
    MeshRenderer meshRenderer;

    private Player _player;
    private float timerStarted = 0f;
    private float timer = 0f;
    protected abstract void PowerUp(Player player);
    protected abstract void PowerDown(Player player);

    private void Awake()
    {
        puCollider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (timerStarted != 0f)
        {
            timer += Time.deltaTime;
            if (timer - timerStarted >= _powerupDuration)
            {
                PowerDown(_player);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _player = other.gameObject.GetComponent<Player>();
        if (_player != null)
        {
            PowerUp(_player);
            // spawn particles & sfx becaise we need to disable object
            Feedback();

            if(puCollider != null)
            {
                puCollider.enabled = false;
            }
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
            timerStarted = Time.time;
        }
    }

    private void Feedback()
    {
        //particles
        if (_collectParticles != null)
        {
            _collectParticles = Instantiate(_collectParticles, transform.position, Quaternion.identity);
        }
        //audio
        if (_collectSound != null)
        {
            AudioHelper.PlayClip2D(_collectSound, 1f);
        }
    }
}
