using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class PopulateCrowd : MonoBehaviour
{
    public Player playerPrefab;
    public Transform crowdPivot;
    public int totalPlayer = 0;
    public List<PositionSlot> playerSlots;
    public bool canPlay = false;
    public List<Action> actions = new List<Action>();
    void Start()
    {
        InstantiateCircle();
        GameManager._instance.screenEvent += GetMouseEvent;
        GameManager._instance.levelStart += LevelStart;
        GameManager._instance.addPlayers += OnAddPlayers;
        GameManager._instance.levelFinish += LevelFinish;
        AddPlayers(addPlayers);
    }
    void LevelStart()
    {
        canPlay = true;
    }
    void LevelFinish(bool isComplete)
    {
        canPlay = false;
        Debug.Log("Level Finish "+(transform.childCount-1));
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

    public void PlayerDeduct()
    {
        totalPlayer--;
        if (totalPlayer <= 0)
        {
            GameManager._instance.LevelFinish(false);
        }
        if (!Reseting)
        {
            StartCoroutine(ResetingCrowdPositions());
        }
    }
    bool Reseting = false;
    IEnumerator ResetingCrowdPositions()
    {
        Reseting = true;

        for (int i = 0; i < playerSlots.Count; i++)
        {
            if (playerSlots[i].player != null)
            {
                for (int j = i; j < playerSlots.Count; j++)
                {
                    if (playerSlots[j].player != null && playerSlots[j].player.transform.parent != null)
                    {
                        playerSlots[i].player = playerSlots[j].player;
                        playerSlots[i].player.transform.DOKill();
                        playerSlots[i].player.transform.DOLocalMove(playerSlots[i].position, 1);
                        playerSlots[j].player = null;
                        j = playerSlots.Count;
                    }
                }
            }
        }
        yield return new WaitForSeconds(3);
        Reseting = false;
    }


    public void AddPlayers(int inc)
    {

        int nPlayers = inc;
        int positionsNeed = totalPlayer + nPlayers;

        while (playerSlots.Count < positionsNeed)
        {
            InstantiateCircle();
        }

        for (int i = 0; i < playerSlots.Count; i++)
        {
            // if (playerSlots[i].player == null)
            if (playerSlots[i].player == null || playerSlots[i].player.transform.parent == null)
            {
                // Player pl = Instantiate(playerPrefab, playerSlots[i].position, playerPrefab.transform.rotation, transform);
                Player pl = Instantiate(playerPrefab);
                pl.transform.parent = transform;
                pl.transform.localPosition = playerSlots[i].position;
                playerSlots[i].player = pl;

                playerSlots[i].player.crowd = this;
                
                totalPlayer++;
                nPlayers--;
            }
            if (nPlayers < 1)
            {
                break;
            }
        }


    }

    void InstantiateCircle()
    {
        float angle = 360f / (float)playersCount;
        for (int i = 0; i < playersCount; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(i * angle, Vector3.up);
            Vector3 direction = rotation * Vector3.forward;
            // Vector3 position = transform.localPosition + (direction * radius);
            Vector3 position = crowdPivot.localPosition + (direction * radius);
            // crowdPivot
            //  Player pl = Instantiate(playerPrefab, position, playerPrefab.transform.rotation, transform);
            PositionSlot slot = new PositionSlot(position);
            playerSlots.Add(slot);

        }
        playersCount += 4;
        radius += 1.1f;

    }



    // radius 1.1
    // +4


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
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DeletePlayers();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!Reseting)
            {
                StartCoroutine(ResetingCrowdPositions());
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

                //transform.localPosition = Vector3.Lerp(transform.localPosition,
                //    new Vector3(pos.x, transform.localPosition.y, transform.localPosition.z),
                //    speedX * Time.deltaTime
                //    );

                transformPos = new Vector3(pos.x, transformPos.y, transform.localPosition.z);

                lastmousePos = Input.GetTouch(0).position;
            }
            else
            {

                Vector3 deltaPos = (Input.mousePosition - lastmousePos);
                //Vector3 deltaPos = Input.mouseScrollDelta;
                Vector3 pos = transform.localPosition;
                pos.x += deltaPos.x;// / Screen.width;
                transformPos.x = pos.x; //= new Vector3(pos.x, transformPos.y, transform.localPosition.z);

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
