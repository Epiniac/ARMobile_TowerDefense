using UnityEngine;

public class KingTower : MonoBehaviour
{
    public float range = 5f;
    public float fireRate = 1f;
    public float damagePerSecond = 20f;

    public Transform firePoint;
    public GameObject flameEffectPrefab;

    private Transform currentTarget;
    private GameObject currentFlame;

    void Update()
    {
        FindTarget();

        if (currentTarget != null)
        {
            // La tour regarde sa cible
            transform.LookAt(currentTarget);

            // Instancie la flamme si pas encore active
            if (currentFlame == null)
            {
                currentFlame = Instantiate(flameEffectPrefab, firePoint.position, firePoint.rotation, firePoint);

                if (currentFlame.TryGetComponent(out FlameFollow follow))
                    follow.SetTarget(currentTarget);
            }

            // Le firePoint suit la cible, la flamme suit le firePoint
            firePoint.LookAt(currentTarget);
            currentFlame.transform.position = firePoint.position;

            // Infliger les dégâts continus
            Enemy enemy = currentTarget.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
        else
        {
            // Plus de cible → arrêter l’effet de feu
            if (currentFlame != null)
            {
                var ps = currentFlame.GetComponent<ParticleSystem>();
                if (ps != null) ps.Stop();

                Destroy(currentFlame, 1.5f);
                currentFlame = null;
            }
        }
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortest = Mathf.Infinity;
        Transform nearest = null;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < shortest && dist <= range)
            {
                shortest = dist;
                nearest = enemy.transform;
            }
        }

        currentTarget = nearest;
    }
}
