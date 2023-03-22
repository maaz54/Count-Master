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
    public bool fightWithEnemy = false;
    EnemyPatch enemyPatch;
    ParticleSystem deathParticle;
    public SkinnedMeshRenderer meshRenderer;
    private void Start()
    {
    }

    Vector3 initPos = Vector3.zero;
    private void OnEnable()
    {
        mainColor = meshRenderer.materials[0].color;
        if (colorAnim != null)
        {
            StopCoroutine(colorAnim);
        }
        colorAnim = StartCoroutine(ColorBlinkAnim());
    }

    void OnAddPlayers(int addPl, PropAddPlayer.AddPlayerType type)
    {
        if (colorAnim != null)
        {
            StopCoroutine(colorAnim);
        }
        colorAnim = StartCoroutine(ColorBlinkAnim());
    }
    Color mainColor;//7FD6FD
    public Color blinkColor;
    Coroutine colorAnim;
    IEnumerator ColorBlinkAnim()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i % 2 == 0)
            {
                meshRenderer.materials[0].color = blinkColor;
            }
            else
            {
                meshRenderer.materials[0].color = mainColor;
            }
            yield return new WaitForSeconds(.05f);
        }
        yield return new WaitForSeconds(.05f);
        meshRenderer.materials[0].color = mainColor;
    }
    public void EnablePlayer(Vector3 initPos)
    {
        GameManager._instance.levelStart += GameStart;
        GameManager._instance.levelFinish += Gamefinish;
        GameManager._instance.finishLine += FinishLine;
        GameManager._instance.enemyAround += EnemyAround;
        GameManager._instance.addPlayers += OnAddPlayers;
        if (GameManager._instance.isGameStart)
        {
            GameStart();
        }
        characeterTr = transform.GetChild(0);
        this.initPos = initPos;
    }
    private void OnDisable()
    {
        Despawn();
    }
    void Despawn()
    {
        GameManager._instance.levelStart -= GameStart;
        GameManager._instance.levelFinish -= Gamefinish;
        GameManager._instance.finishLine -= FinishLine;
        GameManager._instance.enemyAround -= EnemyAround;
        GameManager._instance.addPlayers -= OnAddPlayers;
        enemyCollideCount = 0;
        fightWithEnemy = false;
        finishLineCrossed = false;
        canPlay = false;
        enemyPatch = null;
        randomPos = Vector3.zero;
        time = 4;
        transform.localEulerAngles = Vector3.zero;
        characeterTr.localPosition = Vector3.zero;
        targetedEnemy = null;
    }
    void EnemyAround(bool isEnemyAround, EnemyPatch patch)
    {
        enemyPatch = patch;
        if (isEnemyAround)
        {
            canPlay = false;
            fightWithEnemy = true;
            characeterTr.DOKill();
            characeterTr.DOLocalMove(Vector3.zero, .5f);
        }
        else
        {
            canPlay = true;
            fightWithEnemy = false;
            transform.localEulerAngles = Vector3.zero;
            transform.DOKill();
            transform.DOLocalMove(initPos, .5f);
        }
    }
    public float randomMovementSpeed = 1;
    public float fightSpeed = 1;
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
        else if (fightWithEnemy)
        {
            if (targetedEnemy == null || targetedEnemy.isDead == true)
            {
                targetedEnemy = GetTargetEnemy();
            }
            if (targetedEnemy)
            {
                if (!IsTeamMatInFront())
                {
                    transform.position = Vector3.Lerp(transform.position, targetedEnemy.transform.position, fightSpeed * Time.deltaTime);
                    transform.LookAt(targetedEnemy.transform, transform.up);
                }
            }
        }
    }
    bool IsTeamMatInFront()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        origin.y += 1;
        origin.z += 1;
        if (Physics.Raycast(origin, Vector3.forward, out hit, 1))
        {
            if (hit.transform.gameObject.tag == transform.tag)
            {
                return true;
            }
        }

        return false;
    }
    Enemy targetedEnemy = null;
    Enemy GetTargetEnemy()
    {
        for (int i = 0; i < enemyPatch.enemies.Count; i++)
        {
            if (!enemyPatch.enemies[i].isDead)
            {

                return enemyPatch.enemies[i];
                break;
            }
        }
        return null;
    }

    public void GameStart()
    {
        anim.SetFloat("Blend", 1);
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
                anim.SetFloat("Blend", 2);
            }

            canPlay = false;
        }
    }
    int enemyCollideCount = 1;
    private void OnCollisionStay(Collision col)
    {
        if (col.transform.CompareTag("hurdle"))
        {
            Vector3 p = transform.position;
            p.z -= .1f;
            transform.position = p;
        }
        if (col.transform.CompareTag("enemy"))
        {
            if (fightWithEnemy)
            {
                enemyCollideCount++;
                if (enemyCollideCount >= 2)
                {
                    fightWithEnemy = false;
                    canPlay = false;
                    transform.parent = null;
                    gameObject.SetActive(false);
                    crowd.PlayerDeduct(this);
                    Despawn();
                }
                col.transform.GetComponent<Enemy>().Dead();
                DeathEffect();
            }
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
                crowd.PlayerDeduct(this);
                Vector3 p = transform.position;
                p.z -= .5f;
                transform.position = p;
                Despawn();
                SoundManager.instance.AddAndPOpSound(SoundManager.instance.addPlayerSound);
            }
            else if (col.transform.CompareTag("blade"))
            {
                canPlay = false;
                transform.parent = null;
                gameObject.SetActive(false);
                crowd.PlayerDeduct(this);

                DeathEffect();
                Despawn(); 

            }

        }
    }
    void DeathEffect()
    {
        deathParticle = Pooling.Instance.SpawnDeathPlayerParticle();
        if (deathParticle != null)
        {
            Vector3 ptPos = transform.position;
            ptPos.y = 0.05f;
            deathParticle.transform.position = ptPos;
            deathParticle.Play();
        }
        SoundManager.instance.AddAndPOpSound(SoundManager.instance.popSound);
    }

}
