using UnityEngine;

public class FlameFollow : MonoBehaviour
{
    public Transform target;
    public bool followPosition = false; // active si tu veux que la flamme se déplace

    void Update()
    {
        if (target == null) return;

        // Faire tourner la flamme vers la cible
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Garde l'orientation horizontale
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        // (Optionnel) Faire suivre la position aussi
        if (followPosition)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 5f);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
