using UnityEngine;

public class ProjectileFast : MonoBehaviour
{
    public float speed = 35f;
    public float damage = 15f;
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Fast projectile hit!");
            Destroy(gameObject);
        }
    }
}
