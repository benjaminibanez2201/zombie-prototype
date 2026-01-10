using UnityEngine;

public class ZombieIA : MonoBehaviour
{
	[Header("Objetivo")]
	[SerializeField] Transform target; // Jugador

	[Header("Movimiento")]
	[SerializeField] float moveSpeed = 2f;
	[SerializeField] float stopDistance = 1.5f;

	[Header("Ataque")]
	[SerializeField] int damagePerHit = 10;
	[SerializeField] float attackRate = 1.0f; // golpes por segundo

	PlayerHealth _playerHealth;
	float _nextAttackTime;

	void Start()
	{
		if (target == null && Camera.main != null)
		{
			// Intentar encontrar al jugador por el componente PlayerHealth
			PlayerHealth ph = FindObjectOfType<PlayerHealth>();
			if (ph != null)
			{
				target = ph.transform;
			}
		}

		if (target != null)
		{
			_playerHealth = target.GetComponent<PlayerHealth>();
		}
	}

	void Update()
	{
		if (target == null) return;

		// Mirar hacia el jugador
		Vector3 direction = target.position - transform.position;
		direction.y = 0f;

		float distance = direction.magnitude;

		if (distance > 0.01f)
		{
			Quaternion lookRot = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
		}

		// Moverse hacia el jugador si está fuera de la distancia de parada
		if (distance > stopDistance)
		{
			Vector3 move = direction.normalized * moveSpeed * Time.deltaTime;
			transform.position += move;
		}
		else
		{
			TryAttack();
		}
	}

	void TryAttack()
	{
		if (_playerHealth == null) return;
		if (Time.time < _nextAttackTime) return;

		_nextAttackTime = Time.time + 1f / attackRate;
		_playerHealth.TakeDamage(damagePerHit);
	}
}
