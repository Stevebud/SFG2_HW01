using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField] float _timeLeft;
    [SerializeField] float _warningDelay;
    [SerializeField] float _blastRadius;
    [SerializeField] GameObject _warningCircle;
    [SerializeField] AudioClip _explosionAudio;
    [SerializeField] ParticleSystem _explosionParticles;

    private Transform _playerTrans;

    private void Awake()
    {
        StartCoroutine(Detonate());
        _warningCircle.SetActive(false);
    }

    private IEnumerator Detonate()
    {
        yield return new WaitForSeconds(_warningDelay);
        if(_warningCircle != null)
        {
            _warningCircle.SetActive(true);
        }
        yield return new WaitForSeconds(_timeLeft-_warningDelay);
        if(_playerTrans != null)
        {
            Health playerHealth = _playerTrans.gameObject.GetComponent <Health>();
            if ((Vector3.Distance(this.transform.position, _playerTrans.position) <= _blastRadius) && (playerHealth!= null))
            {
                playerHealth.TakeDamage(_damage);
            }
        }
        if(_explosionAudio != null)
        {
            AudioHelper.PlayClip2D(_explosionAudio, 1f);
        }
        if(_explosionParticles != null)
        {
            ParticleSystem explosion = Instantiate(_explosionParticles, transform.position, transform.rotation);
            explosion.transform.Rotate(0f, 180f, 0f);
            Destroy(explosion, explosion.main.duration);
        }
        Destroy(this.gameObject);
    }

    public void FillPlayerTransform(Transform transform)
    {
        _playerTrans = transform;
    }
}
