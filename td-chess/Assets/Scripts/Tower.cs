using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    public string towerName = "Default Tower";
    public float cost = 100f;
    public float damage = 50f;
    public float fireRate = 1f;
    public float range = 5f;

    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    private float fireCountdown = 0f;
    private Transform currentTarget;

    void Start()
    {
        SphereCollider rangeCollider = GetComponent<SphereCollider>();
        if (rangeCollider != null)
        {
            rangeCollider.isTrigger = true;
            rangeCollider.radius = range;
        }
    }

    void Update()
    {
        if (currentTarget == null)
        {
            FindNewTarget();
            return;
        }

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;

        Vector3 dir = currentTarget.position - transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        if (proj.TryGetComponent(out Projectile p)) p.SetDirection(currentTarget.position - firePoint.position);
        if (proj.TryGetComponent(out ProjectileFast f)) f.SetDirection(currentTarget.position - firePoint.position);
        if (proj.TryGetComponent(out ProjectileHeavy h)) h.SetDirection(currentTarget.position - firePoint.position);
        if (proj.TryGetComponent(out ProjectileSplash s)) s.SetDirection(currentTarget.position - firePoint.position);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.transform == currentTarget)
        {
            currentTarget = null;
        }
    }

    void FindNewTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= range)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        currentTarget = nearestEnemy;
    }
}
