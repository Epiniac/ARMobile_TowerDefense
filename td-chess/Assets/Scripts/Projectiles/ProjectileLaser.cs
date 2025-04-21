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
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, target.position);

            // On tente d'appliquer des dégâts, seulement si l'ennemi a un composant EnemyHealth
            var enemyHealth = target.GetComponent<MonoBehaviour>();
            enemyHealth?.Invoke("TakeDamage", damage);

            // Effet visuel temporaire
            Destroy(gameObject, 0.1f);
        }
    }
}
