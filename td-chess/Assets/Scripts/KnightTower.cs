
using UnityEngine;
using System.Collections.Generic;

public class KnightTower : MonoBehaviour
{
    [Header("Tower Stats")]
    public string towerName = "Knight Tower";
    public float cost = 150f;
    public float damage = 5f;
    public float fireRate = 1f;
    public float range = 5f;
    public int pelletCount = 5;

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
        for (int i = 0; i < pelletCount; i++)
        {
            float spread = Random.Range(-10f, 10f);
            Quaternion rotation = Quaternion.Euler(0, spread, 0) * firePoint.rotation;

            Vector3 spawnOffset = firePoint.right * Random.Range(-0.3f, 0.3f);
            GameObject pellet = Instantiate(projectilePrefab, firePoint.position + spawnOffset, rotation);

            ProjectileShotgun p = pellet.GetComponent<ProjectileShotgun>();
            if (p != null && currentTarget != null)
            {
                Vector3 direction = (currentTarget.position - firePoint.position).normalized;
                p.SetDirection(direction);
                p.damage = damage;
            }
        }
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
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null)
        {
            currentTarget = nearestEnemy;
        }
    }
}
