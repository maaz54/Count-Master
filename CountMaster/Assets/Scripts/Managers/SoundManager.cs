using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SoundManager : MonoBehaviour {
    AudioSource audioSource;
    public AudioClip popSound;
    public AudioClip addPlayerSound;
    public AudioClip tapSound;
    public AudioClip collectSound;
    public static SoundManager instance;
    void Start() 
    {
        instance=this;
        if(GetComponent<AudioSource>())
        {
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void AddAndPOpSound(AudioClip clip) 
    {
            audioSource.PlayOneShot(clip);
      
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
            yield return new WaitForEndOfFrame();
        }
        isQueueProcessing = false;
    }
}
