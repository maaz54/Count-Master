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
    public Button _buttons_BuyMoreCrowd;
    public Button _buttons_BuyMoreIncome;
    public Button _buttons_Setting;

    [Header("Settings")]
    public Button _buttons_haptic;
    public Button _buttons_sounds;
    public GameObject soundClosed;
    public GameObject hapticClosed;
    public GameObject settingPanel;

    [Header("Text")]
    public Text levelNo;
    public Text coinsText;
    public Text[] levelsCoinsTexts;

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
        _buttons_BuyMoreCrowd.onClick.AddListener(() =>
        {
            BuyMoreCrowdWithCrowd();
        });
        _buttons_BuyMoreIncome.onClick.AddListener(() =>
        {
            BuyMoreIncome();
        });
         _buttons_BuyMoreIncome.onClick.AddListener(() =>
        {
           
        });
        _buttons_sounds.onClick.AddListener(()=> {SoundToggle();});
        _buttons_Setting.onClick.AddListener(()=> {SettingPanelToggle();});
        GameManager._instance.levelFinish += OnLevelComplete;
        GameManager._instance.coinsCollect += AddCoins;
        TotalCoins();
        Settings();
    }
    void Settings()
    {
        AudioListener.volume= StaticPrefs.SoundOn;
        soundClosed.SetActive(!(StaticPrefs.SoundOn==1));
    }
    
    void SoundToggle()
    {
        if(StaticPrefs.SoundOn==1)
        {
            StaticPrefs.SoundOn=0;
            AudioListener.volume=0;
            soundClosed.SetActive(true);
        }
        else
        {
            StaticPrefs.SoundOn=1;
            AudioListener.volume=1;
            soundClosed.SetActive(false);
        }
    }
    void SettingPanelToggle()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    public void BuyMoreCrowdWithCrowd()
    {
        GameManager._instance.BuyMoreCrowdWithCrowd();
        TotalCoins();
    }
    public void BuyMoreIncome()
    {
        TotalCoins();
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
        levelCoins=0;
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


     void TotalCoins() {
        coinsText.text=StaticPrefs.Coins.ToString();
    }

    int levelCoins=0;
     void AddCoins(int noOfCoins)
    {
        StaticPrefs.Coins +=noOfCoins;
        coinsText.text=StaticPrefs.Coins.ToString();
        levelCoins +=noOfCoins;
        foreach (var item in levelsCoinsTexts)
        {
            item.text =levelCoins.ToString();
        }
    }

}
