using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public enum EnemyType
    {
        Roller,
        FlyBot,
        SpiderLobber
    }

    [System.Serializable]
    private class EnemySpawnData
    {
        public EnemyType enemyType;
        public GameObject enemyPrefab;
    }

    public static EnemyManager Instance { get; private set; }

    private List<EnemySpawnPoint> spawnPoints;
    

    [SerializeField]
    private List<EnemySpawnData> spawnData;

    Dictionary<EnemyType, EnemySpawnData> spawnDataMap;

    List<GameObject> spawnedEnemies = new List<GameObject>();

    void Awake()
    {
        Instance = this;
        spawnPoints = new List<EnemySpawnPoint>();

        spawnDataMap = new Dictionary<EnemyType, EnemySpawnData>();
        foreach (EnemySpawnData spawnData in spawnData)
        {
            spawnDataMap.Add(spawnData.enemyType, spawnData);
        }
    }

    public void AddSpawnPoint(EnemySpawnPoint enemySpawnPoint)
    {
        spawnPoints.Add(enemySpawnPoint);
    }

    public GameObject SpawnEnemy(EnemyType enemyType, EnemySpawnPoint spawnPoint)
    {
        GameObject enemyPrefab = spawnDataMap[enemyType].enemyPrefab;
        var enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        spawnedEnemies.Add(enemy);
        return enemy;
    }

    public void ClearEnemies()
    {
        foreach(GameObject enemy in spawnedEnemies)
        {
            Destroy(enemy.gameObject);
        }
        spawnedEnemies.Clear();
    }
}
