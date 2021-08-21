using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public BoxCollider collider;
    public enum finishLineType
    {
        FinishLine,
        LevelFinish
    }
    public finishLineType type;

    void Start()
    {

    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (type == finishLineType.FinishLine)
            {
                GameManager._instance.FinishLine();
                collider.enabled = false;
            }
            else if (type == finishLineType.LevelFinish)
            {
                GameManager._instance.LevelFinish(true);
                collider.enabled = false;
            }
        }
    }

}
