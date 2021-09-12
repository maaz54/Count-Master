using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool canPlay = false;
    public bool isDead = false;
    Transform target;
    EnemyPatch enemyPatch;
    public Animator anim;
    void Start()
    {
        // GameManager._instance.enemyAround += EnemyAround;
    }
    public void EnemyAround(bool isEnemyAround, EnemyPatch patch)
    {
        enemyPatch = patch;
        if (isEnemyAround)
        {
            canPlay = true;
            target = GameManager._instance.level.MainPlayer.transform;
            anim.SetFloat("Blend", 1);
        }
    }
    public void Dead()
    {
        gameObject.SetActive(false);
        enemyPatch.DeductEnemy(this);
        //GameManager._instance.enemyAround -= EnemyAround;
        isDead = true;
        canPlay = false;
    }


    public float speed;
    void Update()
    {
        if (canPlay)
        {
            if (!IsTeamMatInFront())
            {
                if (target.gameObject.activeInHierarchy)
                {
                    transform.position = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
                }
            }
            if (target.gameObject.activeInHierarchy)
            {
                transform.LookAt(target.transform, transform.up);
            }
        }
    }
    bool IsTeamMatInFront()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        origin.y += 1;
        // origin.z += 1;
        // if (Physics.Raycast(origin, transform.forward, out hit, 1))


        if (Physics.Raycast(origin, Vector3.forward, out hit, 1))
        {
            if (hit.transform.gameObject.tag == transform.tag)
            {
                return true;
            }
            else if (hit.transform.CompareTag("crowd"))
            {
                target = hit.transform;
            }
        }

        return false;
    }
}
