using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSetting
{

    public float trackLenght;
    public float trackWidth = 10;
    public float playerSpeed;
    public float playerSpeedX;
    public List<hurdleSetting> hurdleSettings;
    public List<AddPlayersProps> addPlayersProps;
    public List<EnemyPatches> enemyPatches;
}
[System.Serializable]
public class EnemyPatches
{
    public int enemyCount;
    public Vector3 pos;
    public EnemyPatch.EnemyType type;

}


[System.Serializable]
public class AddPlayersProps
{
    public PropAddPlayer.AddPlayerType type;
    public int addPlayers;
    public Vector3 pos;

}


[System.Serializable]
public class hurdleSetting
{
    /// <summary>
    /// if we use different types of hurdles we call it with there index
    /// </summary>
    public int hurdleIndex;
    public Vector3 hurdlePos;
}

