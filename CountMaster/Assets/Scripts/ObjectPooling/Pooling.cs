using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Pooling : MonoBehaviour
{

    public static Pooling Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Pooling Objects")]
    public List<ParticleSystem> loadedPlayerDeathParticls;
    public List<ParticleSystem> loadedPnemyDeathParticls;


    public ParticleSystem SpawnDeathPlayerParticle()
    {
        ParticleSystem go = null;
        if (loadedPlayerDeathParticls.Count > 0)
        {
            go = loadedPlayerDeathParticls[0];

            loadedPlayerDeathParticls.RemoveAt(0);
            AddEnqueue(() =>
            {
                loadedPlayerDeathParticls.Add(go);
            });
        }
        if (go != null)
            go.gameObject.SetActive(true);
        return go;
    }
    public ParticleSystem SpawnDeathEnemyParticle()
    {
        ParticleSystem go = null;
        if (loadedPnemyDeathParticls.Count > 0)
        {
            go = loadedPnemyDeathParticls[0];
 
            loadedPnemyDeathParticls.RemoveAt(0);
            AddEnqueue(() =>
            {
                loadedPnemyDeathParticls.Add(go);
            });
        }
        if (go != null)
            go.gameObject.SetActive(true);
        return go;
    }



    Queue action_Queue = new Queue();
    bool isQueueProcessing = false;
    void AddEnqueue(Action action)
    {
        action_Queue.Enqueue(action);
        if (!isQueueProcessing)
        {
            StartCoroutine(ProcessQueue());
        }
    }
    IEnumerator ProcessQueue()
    {
        isQueueProcessing = true;
        while (action_Queue.Count > 0)
        {
            var action = action_Queue.Dequeue() as Action;
            action();
            yield return new WaitForSeconds(1);
        }
        isQueueProcessing = false;
    }
}
