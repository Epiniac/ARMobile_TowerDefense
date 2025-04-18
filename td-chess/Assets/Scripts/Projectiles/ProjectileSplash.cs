using UnityEngine;

public class ProjectileSplash : MonoBehaviour
{
    public float speed = 18f;
    public float damage = 20f;
    public float splashRadius = 2f;
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
            Collider[] hits = Physics.OverlapSphere(transform.position, splashRadius);
            foreach (Collider col in hits)
            {
                if (col.CompareTag("Enemy"))
                {
                    Debug.Log("Splash damage applied");
                }
            }
            Destroy(gameObject);
        }
    }
}
