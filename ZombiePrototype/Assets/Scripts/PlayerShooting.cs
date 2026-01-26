using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Referencia")]
    [SerializeField] Camera playerCamera;

    [Header("Audio")]
    [SerializeField] AudioSource gunAudio;
    [SerializeField] AudioClip gunShotClip;
    [Header("Disparo")]
    [SerializeField] int damagePerShot = 10;
    [SerializeField] float fireRate = 0.25f; // segundos entre disparos
    [SerializeField] float maxDistance = 100f;
    [SerializeField] LayerMask hitMask = ~0; // por defecto, todo

    float _nextFireTime;

    void Awake()
    {
        if (gunAudio == null)
        {
            gunAudio = GetComponent<AudioSource>();
        }
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (playerCamera != null)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
        }

        if (Input.GetButton("Fire1") && Time.time >= _nextFireTime)
        {
            Shoot();
        }
    }


    void Shoot()
    {
        if (gunShotClip != null)
        {
            gunAudio.PlayOneShot(gunShotClip);
        }
        if (playerCamera == null) return;

        _nextFireTime = Time.time + fireRate;

        // Ray desde el centro de la pantalla (0.5, 0.5)
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);


        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, hitMask, QueryTriggerInteraction.Ignore))
        {

            // Intentar hacer daño a un zombie
            if (hit.collider.TryGetComponent<ZombieHealth>(out ZombieHealth zombieHealth))
            {
                zombieHealth.TakeDamage(damagePerShot);
            }
        }
    }
}
