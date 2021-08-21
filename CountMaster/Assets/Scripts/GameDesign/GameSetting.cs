using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObject/GameSetting", order = 1)]
public class GameSetting : ScriptableObject
{
    public List<LevelSetting> levelSettings;
    public List<LevelData> Levels;
}

[System.Serializable]
public class LevelData
{
    public List<LevelSetting> levelSettings;
}
