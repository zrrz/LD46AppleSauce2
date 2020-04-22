using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySpawnPoint))]
public class TriggerEnemySpawn : MonoBehaviour
{
    private GameObject enemy;
    private EnemyManager.EnemyType enemyType;

    private void Start()
    {
        EnemySpawnPoint spawnPoint = GetComponent<EnemySpawnPoint>();
        enemyType = (EnemyManager.EnemyType)Random.Range(0, System.Enum.GetValues(typeof(EnemyManager.EnemyType)).Length);
        enemy = EnemyManager.Instance.SpawnEnemy(enemyType, spawnPoint);
        SetEnemyState(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerHealth>())
        {
            
            SetEnemyState(true);
            Destroy(gameObject);
        }
    }

    private void SetEnemyState(bool on)
    {
        if(enemy == null)
        {
            return;
        }
        switch (enemyType)
        {
            case EnemyManager.EnemyType.Roller:
                enemy.GetComponent<RollerEnemy>().enemyActive = on;
                break;
            case EnemyManager.EnemyType.FlyBot:
                enemy.GetComponent<FlyBotEnemy>().enemyActive = on;
                break;
            case EnemyManager.EnemyType.SpiderLobber:
                enemy.GetComponent<SpiderLobberEnemy>().enemyActive = on;
                break;
            default:
                Debug.LogError("No type");
                break;
        }
    }
}
