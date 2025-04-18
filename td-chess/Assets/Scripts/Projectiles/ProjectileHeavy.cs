using UnityEngine;

public class ProjectileHeavy : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 50f;
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, 2f); // auto destroy
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Heavy projectile hit: " + other.name);
            Destroy(gameObject);
        }
    }
}
