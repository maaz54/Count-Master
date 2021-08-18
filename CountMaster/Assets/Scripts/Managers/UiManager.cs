using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public enum Screens
    {
        Menu,
        GamePlay,
        LevelComplete,
        LevelFail
    }
    public Screens currentScreen;

    [Header("Panels")]
    public GameObject _panel_Menu;
    public GameObject _panel_LevelComplete;
    public GameObject _panel_Fail;
    public GameObject _panel_GamePlay;

    [Header("Buttons")]
    public Button _buttons_GameStart;
    public Button _buttons_Restart;
    public Button _buttons_NextLevel;



    private void Start()
    {
        _buttons_GameStart.onClick.AddListener(() =>
        {
            OnGameStartButton();
        });
        _buttons_Restart.onClick.AddListener(() =>
        {
            OnNextLevelButton();
        });
        _buttons_NextLevel.onClick.AddListener(() =>
        {
            OnNextLevelButton();
        });
        GameManager._instance.levelFinish += OnLevelComplete;
    }
    public void OnScreenEvent(int ev)
    {
        GameManager._instance.ScreenEvent((GameManager.ScreenEvents)ev);
    }

    void OnLevelComplete(bool isComplete)
    {
        if (isComplete)
        {
            ChangeScreen(Screens.LevelComplete);
        }
        else
        {
            ChangeScreen(Screens.LevelFail);
        }
    }

    public void ChangeScreen(Screens screen)
    {
        currentScreen = screen;
        _panel_Menu.SetActive(false);
        _panel_LevelComplete.SetActive(false);
        _panel_Fail.SetActive(false);
        _panel_GamePlay.SetActive(false);

        if (screen == Screens.Menu)
        {
            _panel_Menu.SetActive(true);
        }
        else if (screen == Screens.GamePlay)
        {
            _panel_GamePlay.SetActive(true);
        }
        else if (screen == Screens.LevelComplete)
        {
            _panel_LevelComplete.SetActive(true);
        }
        else if (screen == Screens.LevelFail)
        {
            _panel_Fail.SetActive(true);
        }

    }

    public void OnGameStartButton()
    {
        ChangeScreen(Screens.GamePlay);
        GameManager._instance.LevelStart();
    }
    public void OnRestartButton()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    public void OnNextLevelButton()
    {
        Application.LoadLevel(Application.loadedLevel);
    }



}
