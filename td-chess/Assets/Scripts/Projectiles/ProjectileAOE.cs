using UnityEngine;

public class ProjectileAOE : MonoBehaviour
{
    public float damage = 30f;
    public float explosionRadius = 2f;
    public float speed = 10f;
    public GameObject explosionEffect;

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Aller vers la cible
        Vector3 direction = target.position - transform.position;
        float step = speed * Time.deltaTime;

        if (direction.magnitude <= step)
        {
            Explode();
        }
        else
        {
            transform.Translate(direction.normalized * step, Space.World);
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }
}
