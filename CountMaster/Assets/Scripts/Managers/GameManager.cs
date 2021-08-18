using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameManager : MonoBehaviour
{

    public Level level;
    public static int LevelNo = 1;


    #region Events
    public event Action<bool> levelFinish;
    public event Action levelStart;
    public event Action<int, PropAddPlayer.AddPlayerType> addPlayers;
    public event Action<ScreenEvents> screenEvent;
    public enum ScreenEvents
    {
        MouseDown = 0,
        MouseUp = 1,
        DragBegins = 2,
        DragEnds = 3
    }
    #endregion
    public static GameManager _instance;
    public bool isGameStart = false;
    void Awake()
    {
        _instance = this;
        CreateLevel();
    }
    void CreateLevel()
    {
        level.CreateLevel(LevelNo);
    }

    public void LevelFinish(bool isComplete)
    {
        if (isComplete)
        {
            LevelNo++;
            if (LevelNo > level.gameSetting.levelSettings.Count)
            {
                LevelNo = 1;
            }
            Debug.Log("LEvel NO " + LevelNo);
        }
        isGameStart = false;
        levelFinish?.Invoke(isComplete);
    }
    public void LevelStart()
    {
        isGameStart = true;
        levelStart?.Invoke();
    }
    public void AddPlayert(int add, PropAddPlayer.AddPlayerType type)
    {
        addPlayers?.Invoke(add, type);
    }
    public void ScreenEvent(ScreenEvents eventType)
    {
        screenEvent?.Invoke(eventType);
    }

}
