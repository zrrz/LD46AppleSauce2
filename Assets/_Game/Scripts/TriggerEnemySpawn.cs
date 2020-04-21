using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySpawnPoint))]
public class TriggerEnemySpawn : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerHealth>())
        {
            EnemySpawnPoint spawnPoint = GetComponent<EnemySpawnPoint>();
            var enemyToSpawn = (EnemyManager.EnemyType)Random.Range(0, System.Enum.GetValues(typeof(EnemyManager.EnemyType)).Length);
            EnemyManager.Instance.SpawnEnemy(enemyToSpawn, spawnPoint);
            Destroy(gameObject);
        }
    }
}
