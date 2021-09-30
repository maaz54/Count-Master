using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Coins : MonoBehaviour
{
    bool triggered = false;
    private void OnTriggerEnter(Collider col)
    {
        if (!triggered)
        {
            if (col.CompareTag("Player"))
            {
                triggered = true;
                StartCoroutine(CoinsAnim());
            }
        }
    }
    IEnumerator CoinsAnim()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 pos = transform.GetChild(i).position;
            pos.x += 50;
            pos.y += 40;
            pos.z += 20;

            transform.GetChild(i).DOMove(pos,.5f);
            yield return new WaitForSeconds(.01f);
        }
    }
}
