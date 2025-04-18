using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{
    public static List<Enemy> EnemiesInGame;
    public static Dictionary<int, GameObject> EnemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;
    
    private static bool isInitialized;
    public static void Init()
    {
        if(!isInitialized)
        {
        
            EnemyPrefabs = new Dictionary<int, GameObject>();
            EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();
            EnemiesInGame = new List<Enemy>();
            
            EnemySummonData[] Enemies = Resources.LoadAll<EnemySummonData>("Enemies");
        
            foreach (EnemySummonData enemy in Enemies)
            {
                EnemyPrefabs.Add(enemy.EnemyID, enemy.EnemyPrefab);
                EnemyObjectPools.Add(enemy.EnemyID, new Queue<Enemy>());
            }

            isInitialized = true;
        }
        else
        {
            Debug.LogWarning("EntitySummoner is already initialized.");
        }
    }

    public static Enemy SummonEnemy(int EnemyID)
    {
        Enemy SummonedEnemy = null;

        if(EnemyPrefabs.ContainsKey(EnemyID)){
            Queue<Enemy> ReferenceQueue = EnemyObjectPools[EnemyID];
            if(ReferenceQueue.Count > 0){
                SummonedEnemy = ReferenceQueue.Dequeue();
                SummonedEnemy.Init();

                SummonedEnemy.gameObject.SetActive(true);
            }
            else{
                GameObject NewEnemy = Instantiate(EnemyPrefabs[EnemyID],Vector3.zero, Quaternion.identity);
                SummonedEnemy = NewEnemy.GetComponent<Enemy>();
                SummonedEnemy.Init();
            }
        }
        else{
            Debug.LogError("Enemy ID not found in EnemyPrefabs: " + EnemyID);
            return null;
        }
        EnemiesInGame.Add(SummonedEnemy);
        SummonedEnemy.ID = EnemyID;
        return SummonedEnemy;
    }

    public static void RemoveEnemy(Enemy EnemyToRemove)
    {
        EnemyObjectPools[EnemyToRemove.ID].Enqueue(EnemyToRemove);
        EnemyToRemove.gameObject.SetActive(false);
    }
}
