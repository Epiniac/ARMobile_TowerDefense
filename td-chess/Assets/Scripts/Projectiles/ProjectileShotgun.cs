
using UnityEngine;

public class ProjectileShotgun : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 5f;
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Pellet hit: " + other.name);
            Destroy(gameObject);
        }
    }
}
