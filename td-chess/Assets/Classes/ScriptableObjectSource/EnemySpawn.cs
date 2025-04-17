using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New EnemySpawn", menuName= "Create EnemySpawn")]
public class EnemySpawn : ScriptableObject
{
    public GameObject EnemyPrefab;
	public int EnemyID;
	
}
