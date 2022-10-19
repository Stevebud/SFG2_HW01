using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] int _maxHealth;
    [SerializeField] AudioClip _damageAudio;
    [SerializeField] ParticleSystem _damageParticles;
    [SerializeField] Image _damageFlashImage;
    [SerializeField] float _flashDuration =1f;
    [SerializeField] float _maxFlash = 0.5f;
    [SerializeField] AudioClip _deathAudio;
    [SerializeField] ParticleSystem _deathParticles;

    private int health;

    // event that will notify observers
    public event Action DamageTaken = delegate { };

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        //feedback
        if (_damageParticles != null) 
        {
            ParticleSystem particle = Instantiate(_damageParticles, this.transform.position, this.transform.rotation);
            Destroy(particle, particle.main.duration);
        }
        if(_damageFlashImage != null)
        {
            StartCoroutine(FlashFade());
        }
        if (_damageAudio != null)
        {
            AudioHelper.PlayClip2D(_damageAudio, 1f);
        }
        if (health <= 0)
        {
            Kill();
        }
        //notify any observers
        DamageTaken?.Invoke();
    }

    // Start is called before the first frame update
    void Awake()
    {
        health = _maxHealth;
    }

    public void Kill()
    {
        
        //feedback
        if (_deathParticles != null)
        {
            ParticleSystem particles = Instantiate(_deathParticles, this.transform.position, this.transform.rotation);
            Destroy(particles, _deathParticles.main.duration);
        }
        if (_deathAudio != null)
        {
            AudioHelper.PlayClip2D(_deathAudio, 1f);
        }
        Destroy(this.gameObject);
    }

    public int GetHealth()
    {
        return health;
    }

    IEnumerator FlashFade()
    {
        Color currentColor = _damageFlashImage.color;
        for (float alpha = _maxFlash; alpha >= 0; alpha -= (_maxFlash/_flashDuration))
        {
            currentColor.a = alpha;
            _damageFlashImage.color = currentColor;
            yield return null;
        }
    }
}
