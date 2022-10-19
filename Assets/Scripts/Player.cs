using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TankController))]
public class Player : MonoBehaviour
{
    [SerializeField] int _maxHealth = 3;
    int _currentHealth;

    [SerializeField] Text treasureValueText;
    int _treasureCount;

    [SerializeField] Health _playerHealth;
    [SerializeField] Slider _playerHealthbar;

    TankController _tankController;

    //private boolean and public property for managing invincibility
    private bool isInvincible = false;
    public bool invincible
    {
        get { return isInvincible; }
        set { isInvincible = value; }
    }

    private void OnEnable()
    {
        _playerHealth.DamageTaken += UpdateHealthbar;
    }

    private void OnDisable()
    {
        _playerHealth.DamageTaken -= UpdateHealthbar;
    }

    private void Awake()
    {
        _tankController = GetComponent<TankController>();
    }
    
    private void Start()
    {
        _playerHealthbar.maxValue = _playerHealth.GetHealth();
        _playerHealthbar.value = _playerHealthbar.maxValue;
        _currentHealth = _maxHealth;
        _treasureCount = 0;
        treasureValueText.text = "Treasure: 0";
    }

    public void IncreaseHealth(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        Debug.Log("Player's health: " + _currentHealth);
    }

    public void DecreaseHealth(int amount)
    {
        if (!isInvincible)
        {
            _currentHealth -= amount;
        }
        Debug.Log("Player's health: " + _currentHealth);
        if(_currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        //play particles
        //play sounds
    }

    public void IncreaseTreasure(int amount)
    {
        _treasureCount += amount;
        treasureValueText.text = ("Treasure: " + _treasureCount);
    }

    void UpdateHealthbar()
    {
        _playerHealthbar.value = _playerHealth.GetHealth();
    }
}
