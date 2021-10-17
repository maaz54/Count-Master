using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticPrefs {
    static string pref_LevelNo = "pref_LevelNo";
    static string pref_Coins = "pref_Coins";
    static string pref_No_Of_Players = "pref_No_Of_Players";
    static string pref_SoundOn = "pref_SoundOn";
    public static int SoundOn {
        get {
            return PlayerPrefs.GetInt(pref_SoundOn, 1);// 1 = sound On and 0 = sound off
        }
        set {
            PlayerPrefs.SetInt(pref_SoundOn, value);
        }
    }

    public static int NoOFPlayers {
        get {
            return PlayerPrefs.GetInt(pref_No_Of_Players, 6);
        }
        set {
            PlayerPrefs.SetInt(pref_No_Of_Players, value);
        }
    }

    public static int LevelNo {
        get {
            return PlayerPrefs.GetInt(pref_LevelNo, 1);
        }
        set {
            PlayerPrefs.SetInt(pref_LevelNo, value);
        }
    }
    public static int Coins {
        get {
            return PlayerPrefs.GetInt(pref_Coins, 2000);
        }
        set {
            PlayerPrefs.SetInt(pref_Coins, value);
        }
    }
}
