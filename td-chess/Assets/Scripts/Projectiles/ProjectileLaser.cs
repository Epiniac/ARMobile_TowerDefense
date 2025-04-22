using UnityEngine;

public class Projectilelaser : MonoBehaviour
{
    public float damage = 50f;
    private LineRenderer lineRenderer;
    private Transform target;

    public void SetTarget(Transform enemy, float damageAmount)
    {
        target = enemy;
        damage = damageAmount;
        lineRenderer = GetComponent<LineRenderer>();

        if (target != null)
        {
            // Ligne laser visuelle
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, target.position);

            // Infliger les dégâts à l'ennemi
            Enemy enemyComponent = target.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(damage);
            }

            // Effet temporaire
            Destroy(gameObject, 0.1f);
        }
    }
}
