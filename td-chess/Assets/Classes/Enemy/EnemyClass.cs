using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public float MaxHealth;
    public float Health;
    public float Speed;
    public int ID;
    public void Init()
    {
        Health = MaxHealth;
    }
}
