using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public BoxCollider collider;
    void Start()
    {

    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            GameManager._instance.LevelFinish(true);
            collider.enabled=false;
        }
    }

}
