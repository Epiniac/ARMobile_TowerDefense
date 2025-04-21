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
            transform.LookAt(currentTarget);

            if (currentFlame == null)
            {
                currentFlame = Instantiate(flameEffectPrefab, firePoint.position, firePoint.rotation, firePoint);

                if (currentFlame.TryGetComponent(out FlameFollow follow))
                    follow.SetTarget(currentTarget);
            }

            // Orientation constante vers la cible
            firePoint.LookAt(currentTarget);
            currentFlame.transform.position = firePoint.position;

            // Infliger des dégâts en continu
            var health = currentTarget.GetComponent<MonoBehaviour>();
            health?.Invoke("TakeDamage", damagePerSecond * Time.deltaTime);
        }
        else
        {
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
