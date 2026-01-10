using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] Image HealthBar;

    int _currentHealth;

    void Awake()
    {
        _currentHealth = maxHealth;
        UpdateHealthBar();
    }
    void UpdateHealthBar()
    {
        if (HealthBar != null)
        {
            HealthBar.fillAmount = (float)_currentHealth / maxHealth;
        }
    }
    public void TakeDamage(int amount)
    {
        if (amount <= 0 || _currentHealth <= 0) return;

        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
        UpdateHealthBar();
    }


    void Die()
    {
        // Aquí puedes desactivar controles, mostrar pantalla de muerte, etc.
        Debug.Log("Player muerto");
    }
}