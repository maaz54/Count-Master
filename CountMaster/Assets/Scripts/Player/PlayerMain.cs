using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public bool canPlay = false;
    public Transform target;
    public float Speed;
    public float trackLenght;
    private void Start()
    {
        GameManager._instance.levelStart += GameStart;
        GameManager._instance.levelFinish += levelComplete;
        GameManager._instance.enemyAround += EnemyAround;
    }
    void EnemyAround(bool isEnemyAround, EnemyPatch patch)
    {
        if (isEnemyAround)
        {
            canPlay = false;
        }
        else
        {
            canPlay = true;
        }
    }
    private void Update()
    {
        if (canPlay)
        {
            transform.Translate(transform.forward * Speed * Time.deltaTime);
            if (transform.position.z > trackLenght)
            {
                canPlay = false;
                GameManager._instance.LevelFinish(true);
            }
        }
    }

    void GameStart()
    {
        canPlay = true;
        target = GameManager._instance.level.finishLineTransform;
        trackLenght = GameManager._instance.level.TrackTotalLenght;

    }
    void levelComplete(bool isComplete)
    {
        canPlay = false;
    }
}
