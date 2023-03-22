using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;
public class PopulateCrowd : MonoBehaviour
{
    public Player playerPrefab;
    public Transform crowdPivot;
    public int totalPlayer = 0;
    public List<PositionSlot> playerSlots;
    public bool canPlay = false;
    public List<Action> actions = new List<Action>();
    public BoxCollider boxCollider;
    public float moveToPositionSpeed;
    public float moveToTriangleSpeed;
    public bool finishLineCrossed = false;

    // this list is use to object pooling
    public List<Player> deadPlayer;

    void Start()
    {
        InstantiateCircle();
        GameManager._instance.screenEvent += GetMouseEvent;
        GameManager._instance.levelStart += LevelStart;
        GameManager._instance.addPlayers += OnAddPlayers;
        GameManager._instance.levelFinish += LevelFinish;
        GameManager._instance.finishLine += FinishLine;
        GameManager._instance.enemyAround += EnemyAround;
        GameManager._instance.buyMoreCrowdWithCrowd += AddPlayers;
        GameManager._instance.coinsCollect += AddCoins;
        AddPlayers(StaticPrefs.NoOFPlayers);
    }

    void EnemyAround(bool isEnemyAround, EnemyPatch patch)
    {
        AddEnqueue(() =>
        {
            if (isEnemyAround)
            {
                canPlay = false;
                transform.DOKill();
                transform.DOLocalMove(Vector3.zero, moveToPositionSpeed);
            }
            else
            {
                canPlay = true;
                ResetingCrowdPositions();
                if (!Reseting)
                {
                    StartCoroutine(ResetingCrowdAlignemnts());
                }
            }
        });
    }
    void LevelStart()
    {
        canPlay = true;
    }
    void LevelFinish(bool isComplete)
    {
        canPlay = false;
    }

    void FinishLine()
    {
        finishLineCrossed = true;
        canPlay = false;
        MakeTriangle();
    }


    void OnAddPlayers(int addPl, PropAddPlayer.AddPlayerType type)
    {
        if (type == PropAddPlayer.AddPlayerType.add)
        {
            AddPlayers(addPl);
        }
        else if (type == PropAddPlayer.AddPlayerType.multiply)
        {
            int inc = totalPlayer * addPl;
            AddPlayers(inc);
        }
        
    }
    void AddCoins(int coins)
    {
        levelCoins += coins;
    }

    //level coins collected
    int levelCoins=0;
    public void PlayerDeduct(Player pl)
    {
        totalPlayer--;
        if (totalPlayer <= 0)
        {
            if (finishLineCrossed)
            {
                GameManager._instance.CoinsCollect(levelCoins *perLinePlayer);
                GameManager._instance.LevelFinish(true);
            }
            else
            {
                GameManager._instance.LevelFinish(false);
            }
        }

        deadPlayer.Add(pl);
    }

    void ResetingCrowdPositions()
    {
        for (int i = 0; i < playerSlots.Count; i++)
        {
            if (playerSlots[i].player != null && playerSlots[i].player.canPlay)
            {
                playerSlots[i].player.transform.DOKill();
                playerSlots[i].player.transform.DOLocalMove(playerSlots[i].position, moveToPositionSpeed);
            }
        }
    }

    bool Reseting = false;
    IEnumerator ResetingCrowdAlignemnts()
    {
        Reseting = true;
        for (int i = 0; i < playerSlots.Count; i++)
        {
            if (playerSlots[i].player != null && !playerSlots[i].player.canPlay) // 
            {
                for (int j = i; j < playerSlots.Count; j++)
                {
                    if (playerSlots[j].player != null && playerSlots[j].player.canPlay)
                    {
                        playerSlots[i].player = playerSlots[j].player;
                        playerSlots[j].player = null;
                        j = playerSlots.Count;
                    }
                }
            }
            if (playerSlots[i].player != null && playerSlots[i].player.canPlay)
            {
                playerSlots[i].player.transform.DOKill();
                playerSlots[i].player.transform.DOLocalMove(playerSlots[i].position, moveToPositionSpeed);
                yield return new WaitForSeconds(moveToPositionSpeed / playerSlots.Count);
            }
        }
        yield return new WaitForSeconds(moveToPositionSpeed);
        Reseting = false;
    }

    void ColliderResizing()
    {
        AddEnqueue(() =>
          {
              if (transform.childCount - 1 < 25)
              {
                  boxCollider.size = new Vector3(5, 3, 5);
              }
              else
              {
                  float divider = ((transform.childCount - 1) / 25);
                  divider += 2;
                  float size = (transform.childCount - 1) / divider;
                  boxCollider.size = new Vector3(size, boxCollider.size.y, size);
              }
              boxCollider.center = new Vector3(0, 1.5f, 0);
          });
    }


    public void AddPlayers(int inc)
    {
        AddEnqueue(() =>
        {
            int nPlayers = inc;
            int positionsNeed = totalPlayer + nPlayers;

            while (playerSlots.Count < positionsNeed)
            {
                InstantiateCircle();
            }

            for (int i = 0; i < playerSlots.Count; i++)
            {
                if (playerSlots[i].player == null || playerSlots[i].player.transform.parent == null)
                {
                    Player pl;
                    if (deadPlayer.Count > 0)
                    {
                        pl = deadPlayer[0];
                        deadPlayer.RemoveAt(0);
                        pl.gameObject.SetActive(true);
                    }
                    else
                    {
                        pl = Instantiate(playerPrefab);
                    }
                    pl.EnablePlayer(playerSlots[i].position);
                    pl.transform.parent = transform;
                    pl.transform.localPosition = Vector3.zero;
                    playerSlots[i].player = pl;

                    playerSlots[i].player.crowd = this;
                    pl.transform.DOKill();
                    pl.transform.DOLocalMove(playerSlots[i].position, moveToPositionSpeed);

                    totalPlayer++;
                    nPlayers--;
                    SoundManager.instance.AddAndPOpSound(SoundManager.instance.addPlayerSound);
                }
                if (nPlayers < 1)
                {
                    break;
                }
            }
            ColliderResizing();
        });
    }
    void InstantiateCircle()
    {
        float angle = 360f / (float)playersCount;
        for (int i = 0; i < playersCount; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(i * angle, Vector3.up);
            Vector3 direction = rotation * Vector3.forward;
            Vector3 position = crowdPivot.localPosition + (direction * radius);
            // crowdPivot
            PositionSlot slot = new PositionSlot(position);
            playerSlots.Add(slot);

        }
        playersCount += 4;
        radius += 1.1f;

    }
    void MakeTriangle()
    {
        AddEnqueue(() =>
        {
            StartCoroutine(IMakeTriangle());
        });
    }
    IEnumerator IMakeTriangle()
    {
        transform.DOKill();
        transform.DOLocalMove(Vector3.zero, moveToPositionSpeed);
        List<Vector3> pos = TrianglePositions();
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).DOKill();
            transform.GetChild(i).DOLocalMove(new Vector3(pos[i - 1].x, pos[i - 1].y, 0), moveToTriangleSpeed);
            yield return new WaitForSeconds(moveToPositionSpeed / transform.childCount);
        }
    }
    public int triangelXOffset;
    public int triangelYOffset;
    public int perLinePlayer;
    List<Vector3> TrianglePositions()
    {
        List<Vector3> trianglePos = new List<Vector3>();
        int size = transform.childCount - 1;
        int perLine = Mathf.CeilToInt(size / 5);
        perLinePlayer = perLine;
        int x = -6;
        int y = 0;

        for (int i = 0; i < size / 5; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Vector3 position = new Vector3(x, y);
                trianglePos.Add(position);
                x += triangelXOffset;
            }
            y += triangelYOffset;
            x = -6;
        }

        return trianglePos;
    }

    public float radius = 1;
    public int playersCount = 4;
    public int addPlayers;


    public float horizontalSpeed;


    bool executingAction = true;

    void Update()
    {
        if (canPlay == true)
        {
            if (dragBegins)
            {
                MOVEX();
                transform.localPosition = Vector3.Lerp(transform.localPosition,
                new Vector3(transformPos.x, transform.localPosition.y, transform.localPosition.z),
                horizontalSpeed * Time.deltaTime);
            }
            XBoundary();
        }



#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddPlayers(addPlayers);
            ColliderResizing();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DeletePlayers();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            MakeTriangle();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!Reseting)
            {
                StartCoroutine(ResetingCrowdAlignemnts());
            }
        }

#endif
    }
    public bool dragBegins = false;
    Vector3 lastmousePos = Vector3.zero;
    Vector3 transformPos = Vector3.zero;
    public bool touchControl = false;
    private void MOVEX()
    {
        if (dragBegins)
        {
            if (touchControl)
            {

                Vector3 deltaPos = Input.GetTouch(0).position - new Vector2(lastmousePos.x, lastmousePos.y);
                Vector3 pos = transform.localPosition;
                pos.x = deltaPos.x;


                transformPos = new Vector3(pos.x, transformPos.y, transform.localPosition.z);

                lastmousePos = Input.GetTouch(0).position;
            }
            else
            {

                Vector3 deltaPos = (Input.mousePosition - lastmousePos);
                Vector3 pos = transform.localPosition;
                pos.x += deltaPos.x;// / Screen.width;
                transformPos.x = pos.x; 

                lastmousePos = Input.mousePosition;
            }
        }
    }
    public float xboundary;
    void XBoundary()
    {
        Vector3 pos = transform.localPosition;
        if (pos.x > xboundary)
        {
            pos.x = xboundary;
        }
        else if (pos.x < -xboundary)
        {
            pos.x = -xboundary;
        }

        if (pos.y < .035f)
        {
            pos.y = .035f;
        }

        transform.localPosition = pos;

    }


    public void GetMouseEvent(GameManager.ScreenEvents events)
    {
        if (events == GameManager.ScreenEvents.DragBegins)
        {
            OnDrageBegins();
        }
        else if (events == GameManager.ScreenEvents.DragEnds)
        {
            OnDrageEnds();
        }
        else if (events == GameManager.ScreenEvents.MouseDown)
        {
            MouseDown();
        }
        else if (events == GameManager.ScreenEvents.MouseUp)
        {
            MouseUp();
        }
    }
    public void OnDrageBegins()
    {
        dragBegins = true;
    }
    public void OnDrageEnds()
    {
        dragBegins = false;
    }
    public void MouseDown()
    {
        lastmousePos = Input.mousePosition;
    }
    public void MouseUp()
    {
        lastmousePos.x = 0;
    }

    void DeletePlayers()
    {
        for (int i = 0; i < playerSlots.Count; i++)
        {
            Destroy(playerSlots[i].player.gameObject);
        }
        playerSlots.Clear();
        playersCount = 4;
        radius = 1;
    }
    private void OnTriggerExit(Collider col)
    {
        if (canPlay)
        {
            if (col.transform.CompareTag("hurdle"))
            {
                AddEnqueue(() =>
                {
                    if (!Reseting)
                    {
                        StartCoroutine(ResetingCrowdAlignemnts());
                    }
                });
            }
        }
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
[System.Serializable]
public class PositionSlot
{
    public Player player;
    public Vector3 position;
    public PositionSlot(Vector3 position)
    {
        this.position = position;
    }
}
