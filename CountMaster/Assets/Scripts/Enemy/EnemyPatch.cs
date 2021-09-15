using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatch : MonoBehaviour
{
    public List<Enemy> enemies;
    public BoxCollider collider;
    public int totalEnemy = 0;
    public bool canTriger = true;

    public enum EnemyType
    {
        enemy,
        monsterEnemy
    }
    public EnemyType type;

    private void Start()
    {

    }
    public float radius = 1;
    public void InstantiatePLayers(EnemyType type)
    {
        this.type = type;
        if (type == EnemyType.enemy)
        {
            enemies.Clear();
            Enemy enemePrefab = GameManager._instance.level.EnemyPrefabe;
            float angle = 360f / (float)totalEnemy;
            int enemyCount = totalEnemy;
            int row = enemyCount / 5;
            Vector3 pos = transform.position;
            pos.z = transform.position.z - (row / 2);
            pos.x = -4f;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Vector3 position = pos;
                    Enemy enemi = Instantiate(enemePrefab, position, enemePrefab.transform.rotation, transform);
                    enemies.Add(enemi);
                    pos.x += 1.5f;
                    enemyCount--;
                    if (enemyCount <= 0)
                    {
                        break;
                    }
                }
                pos.z += 1.5f;
                pos.x = -4f;
            }
        }
        else if (type == EnemyType.monsterEnemy)
        {
            
        }
    }
    public void DeductEnemy(Enemy enemy)
    {
        totalEnemy--;
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            GameManager._instance.EnemyAround(false, this);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (canTriger)
        {
            if (col.CompareTag("Player"))
            {
                canTriger = false;
                foreach (Enemy item in enemies)
                {
                    item.EnemyAround(true, this);
                }
                collider.enabled = false;
                GameManager._instance.EnemyAround(true, this);
            }
        }
    }
}
