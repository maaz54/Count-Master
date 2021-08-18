﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public bool canPlay = false;
    public Transform target;
    public float Speed;
    private void Start()
    {
        GameManager._instance.levelStart += GameStart;
        GameManager._instance.levelFinish += levelComplete;
    }

    private void Update()
    {
        if(canPlay)
        {
            // transform.position = Vector3.Lerp(transform.position,target.position,Speed*Time.deltaTime);
            transform.Translate(transform.forward*Speed*Time.deltaTime);
        }
    }

    void GameStart()
    {
        canPlay = true;
        target = GameManager._instance.level.finishLineTransform;

    }
    void levelComplete(bool isComplete)
    {
        canPlay = false;
    }
}
