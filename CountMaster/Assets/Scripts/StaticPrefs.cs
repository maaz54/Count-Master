using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticPrefs
{
    static string pref_LevelNo = "pref_LevelNo";

    public static int LevelNo
    {
        get
        {
            return PlayerPrefs.GetInt(pref_LevelNo,1);
        }
        set
        {
            PlayerPrefs.SetInt(pref_LevelNo,value);
        }
    }
}
