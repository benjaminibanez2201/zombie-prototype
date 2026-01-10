using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 50;
    
    int _currentHealth;

    void Awake()
    {
        _currentHealth = maxHealth;
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
    }


    void Die()
    {
        Debug.Log("Zombie muerto");
        Destroy(gameObject);
    }
}
