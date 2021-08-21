using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Player : MonoBehaviour
{
    public Animator anim;
    public bool canPlay = false;
    public bool finishLineCrossed = false;
    public PopulateCrowd crowd;
    Transform characeterTr;
    public float randomLimit;
    private void Start()
    {
        GameManager._instance.levelStart += GameStart;
        GameManager._instance.levelFinish += Gamefinish;
        GameManager._instance.finishLine += FinishLine;

        if (GameManager._instance.isGameStart)
        {
            GameStart();
        }
        characeterTr = transform.GetChild(0);
    }

    public float randomMovementSpeed = 1;
    Vector3 randomPos = Vector3.zero;
    float time = 4;
    private void Update()
    {
        if (canPlay && !finishLineCrossed)
        {
            time += Time.deltaTime;
            if (time > 2)
            {
                time = 0;
                randomPos = new Vector3(Random.RandomRange(-randomLimit, randomLimit), 0, Random.RandomRange(-randomLimit, randomLimit));
            }

            characeterTr.localPosition = Vector3.Lerp(characeterTr.localPosition, randomPos, randomMovementSpeed * Time.deltaTime);
        }
    }


    public void GameStart()
    {
        anim.SetFloat("Blend", .5f);
        canPlay = true;
    }
    void FinishLine()
    {
        finishLineCrossed = true;
        characeterTr.DOLocalMove(Vector3.zero, .5f);
        anim.SetFloat("Blend", 0);
    }

    void Gamefinish(bool isComplete)
    {
        if (isComplete)
        {
            if (canPlay)
            {
                anim.SetFloat("Blend", 1);
            }

            canPlay = false;
        }
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.transform.CompareTag("hurdle"))
        {
            Vector3 p = transform.position;
            p.z -= .1f;
            transform.position = p;
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (canPlay)
        {
            if (col.transform.CompareTag("hurdle"))
            {
                canPlay = false;
                transform.parent = null;
                anim.SetFloat("Blend", 0);
                crowd.PlayerDeduct();
                Vector3 p = transform.position;
                p.z -= .5f;
                transform.position = p;
            }
        }

    }
}
