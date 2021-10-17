using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameManager : MonoBehaviour {

    public Level level;
    public static int LevelNo {
        get {
            return StaticPrefs.LevelNo;
        }
        set {
            StaticPrefs.LevelNo = value;
        }
    }
    public static GameManager _instance;
    public bool isGameStart = false;
    void Awake() {
        _instance = this;
        CreateLevel();
    }
    void CreateLevel() {

        level.CreateLevel(LevelNo);
    }

    #region Events
    public event Action finishLine;
    public event Action<int> buyMoreCrowdWithCrowd;
    public event Action<bool> levelFinish;
    public event Action levelStart;
    public event Action<int> coinsCollect;
    public event Action<int, PropAddPlayer.AddPlayerType> addPlayers;
    public event Action<bool, EnemyPatch> enemyAround;
    public event Action<ScreenEvents> screenEvent;
    public enum ScreenEvents {
        MouseDown = 0,
        MouseUp = 1,
        DragBegins = 2,
        DragEnds = 3
    }

    public void LevelFinish(bool isComplete) {
        if (isComplete) {
            LevelNo++;
            if (LevelNo > level.gameSetting.levelSettings.Count) {
                LevelNo = 1;
            }
        }
        isGameStart = false;
        levelFinish?.Invoke(isComplete);
    }
    public void FinishLine() {
        finishLine?.Invoke();
    }
    public void LevelStart() {
        isGameStart = true;
        levelStart?.Invoke();
    }
    public void AddPlayert(int add, PropAddPlayer.AddPlayerType type) {
        addPlayers?.Invoke(add, type);
    }
    public void ScreenEvent(ScreenEvents eventType) {
        screenEvent?.Invoke(eventType);
    }

    public void EnemyAround(bool isEnemyAround, EnemyPatch enemyPatch) {
        enemyAround?.Invoke(isEnemyAround, enemyPatch);
    }
    public void CoinsCollect(int noOfCoins) {
        coinsCollect?.Invoke(noOfCoins);
    }
    public void BuyMoreCrowdWithCrowd() {
        if (StaticPrefs.Coins >= 2000) {
            StaticPrefs.Coins -= 2000;
            StaticPrefs.NoOFPlayers++;
            buyMoreCrowdWithCrowd?.Invoke(1);
        }
    }
    #endregion
}
