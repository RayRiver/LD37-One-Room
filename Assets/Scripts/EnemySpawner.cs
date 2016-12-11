using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] _enemyPrefabs;

    private GameUtil.PrefabParamDict _enemyPrefabDict;

    private Transform _enemies;
    private Transform _points;

    private Dictionary<string, Vector2> _spawnPoints;

    void Awake()
    {
        _enemyPrefabDict = new GameUtil.PrefabParamDict(_enemyPrefabs);

        _enemies = transform.FindChild("Enemies");
        _points = transform.FindChild("Points");

        _spawnPoints = new Dictionary<string, Vector2>();

        var count = _points.childCount;
        for (int i = 0; i < count; ++i)
        {
            var child = _points.GetChild(i);
            _spawnPoints[child.name] = child.position;
        }
    }

    public void CreateEnemy(string enemyName, string pointName = null)
    {
        GameObject prefab = null;
        var prefabExists = _enemyPrefabDict.TryGetValue(enemyName, out prefab);
        if (!prefabExists) return;

        var go = Instantiate(prefab);
        go.transform.parent = _enemies;
        go.name = enemyName;

        if (pointName != null)
        {
            Vector2 pos;
            bool exists = _spawnPoints.TryGetValue(pointName, out pos);
            if (exists)
            {
                go.transform.position = _spawnPoints[pointName];
            }
        }
    }

    public int GetEnemyCount()
    {
        return _enemies.childCount;
    }
}
