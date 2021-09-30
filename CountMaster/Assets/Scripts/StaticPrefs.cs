using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticPrefs
{
    static string pref_LevelNo = "pref_LevelNo";
    static string pref_Coins = "pref_Coins";

    public static int LevelNo
    {
        get
        {
            return PlayerPrefs.GetInt(pref_LevelNo, 1);
        }
        set
        {
            PlayerPrefs.SetInt(pref_LevelNo, value);
        }
    }
    public static int Coins
    {
        get
        {
            return PlayerPrefs.GetInt(pref_Coins, 0);
        }
        set
        {
            PlayerPrefs.SetInt(pref_Coins, value);
        }
    }
}
