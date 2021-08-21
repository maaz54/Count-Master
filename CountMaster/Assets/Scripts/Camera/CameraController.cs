using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator.Play("state1");
        GameManager._instance.finishLine += FinishLine;
    }
    void FinishLine()
    {
        animator.Play("state2");
    }
}
