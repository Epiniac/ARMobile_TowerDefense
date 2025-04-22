using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHealth;
    public float Health;
    public float Speed;
    public int ID;
    public int NodeIndex;
    public void Init()
    {
        Health = MaxHealth;
        transform.position = GameManager.NodePositions[0];
        NodeIndex = 0;
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;

        if (Health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Futur rajout de fx ou son ici
        Destroy(gameObject);
    }

}


