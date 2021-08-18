using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameSetting gameSetting;


    public GameObject[] hurdles;
    public Transform propAddPlayer;
    public GameObject finishLine;
    public GameObject track;


    public Transform finishLineTransform;


    public void CreateLevel(int levelNo)
    {
        // PlayerSetting(levelNo);
        // CreateEnemies(levelNo);
        TrackSize(gameSetting.levelSettings[levelNo - 1].trackLenght, gameSetting.levelSettings[levelNo - 1].trackWidth, levelNo);
        CreateHurdles(levelNo);
    }


    void PlayerSetting(int levelNo)
    {
    }

    void CreateHurdles(int levelNo)
    {
        for (int i = 0; i < gameSetting.levelSettings[levelNo - 1].hurdleSettings.Count; i++)
        {
            GameObject hurdle = Instantiate(hurdles[gameSetting.levelSettings[levelNo - 1].hurdleSettings[i].hurdleIndex]);
            hurdle.transform.position = gameSetting.levelSettings[levelNo - 1].hurdleSettings[i].hurdlePos;
            hurdle.transform.parent = levelThings;
            hurdle.name = "Hurdle" + i;
        }
        for (int i = 0; i < gameSetting.levelSettings[levelNo - 1].addPlayersProps.Count; i++)
        {
            GameObject go = Instantiate(propAddPlayer.gameObject);
            PropAddPlayer pp = go.GetComponentInChildren<PropAddPlayer>();
            go.transform.position = gameSetting.levelSettings[levelNo - 1].addPlayersProps[i].pos;
            pp.addPlayers =gameSetting.levelSettings[levelNo - 1].addPlayersProps[i].addPlayers;
            pp.propType =gameSetting.levelSettings[levelNo - 1].addPlayersProps[i].type;
            go.transform.parent = levelThings;
        }

    }
    void TrackSize(float trackLenght, float trackwidth, int levelNo)
    {
        Vector3 trackPos = new Vector3(0, -1, 0);
        Vector3 trackScale = new Vector3(10, 1, 1);
        trackScale.z = trackLenght;
        trackScale.x = trackwidth;
        trackPos.z = (trackLenght / 2) - 1;
        track.transform.localScale = trackScale;
        track.transform.position = trackPos;

        GameObject finish = Instantiate(finishLine);
        Vector3 finishlinePos = Vector3.zero;
        finishlinePos.z = trackLenght - 3;
        finish.transform.position = finishlinePos;
        // finishLine.transform.parent = levelThings;
        finishLineTransform = finish.transform;
        finishLineTransform.parent=levelThings;


    }


    void DeleteLevel()
    {

        for (int i = 0; i < levelThings.childCount; i++)
        {
            Destroy(levelThings.GetChild(i).gameObject);
        }
        //for (int i = 0; i < enemyHolder.transform.childCount; i++)
        //{
        //    Destroy(enemyHolder.transform.GetChild(i).gameObject);
        //}

    }


    void CreateEnemies(int levelNo)
    {

    }





    public Transform levelThings;


#if UNITY_EDITOR
    public int levelNo;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            DeleteLevel();
            CreateLevel(levelNo);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DeleteLevel();
        }
    }
#endif
}
