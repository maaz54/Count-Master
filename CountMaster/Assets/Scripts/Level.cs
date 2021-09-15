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
    public GameObject ladderObject;
    public EnemyPatch enemyPatch;
    public Enemy EnemyPrefabe;
    public Transform finishLineTransform;
    public float TrackTotalLenght;

    public Color[] ladderColors;
    public int ladderSteps;

    [SerializeField]
    PlayerMain _mainPlayer;
    public PlayerMain MainPlayer
    {
        get
        {
            if (_mainPlayer == null)
            {
                _mainPlayer = FindObjectOfType<PlayerMain>();
            }
            return _mainPlayer;
        }
    }

    public void CreateLevel(int levelNo)
    {
        // PlayerSetting(levelNo);
        TrackSize(gameSetting.levelSettings[levelNo - 1].trackLenght, gameSetting.levelSettings[levelNo - 1].trackWidth, levelNo);
        CreateHurdles(levelNo);
        CreateEnemies(levelNo);

    }


    void PlayerSetting(int levelNo)
    {
    }
    void CreateEnemies(int levelNo)
    {
        for (int i = 0; i < gameSetting.levelSettings[levelNo - 1].enemyPatches.Count; i++)
        {
            EnemyPatch patch = Instantiate(enemyPatch);
            patch.transform.position = gameSetting.levelSettings[levelNo - 1].enemyPatches[i].pos;
            patch.transform.parent = levelThings;
            patch.totalEnemy = gameSetting.levelSettings[levelNo - 1].enemyPatches[i].enemyCount;
            patch.InstantiatePLayers(gameSetting.levelSettings[levelNo - 1].enemyPatches[i].type);
        }
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
            pp.addPlayers = gameSetting.levelSettings[levelNo - 1].addPlayersProps[i].addPlayers;
            pp.propType = gameSetting.levelSettings[levelNo - 1].addPlayersProps[i].type;
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
        finishlinePos.z = trackLenght - 100;
        finish.transform.position = finishlinePos;
        // finishLine.transform.parent = levelThings;
        finishLineTransform = finish.transform;
        finishLineTransform.parent = levelThings;


        GameObject ladder = Instantiate(ladderObject);
        Vector3 ladderPos = Vector3.zero;
        ladderPos.z = trackLenght - 1f;
        ladderPos.y = -1f;
        ladder.transform.position = ladderPos;
        ladder.transform.parent = levelThings;

        GameObject ladderStep = ladder.transform.GetChild(0).gameObject;
        GameObject finishObject = ladder.transform.GetChild(1).gameObject;
        Vector3 steps = ladderStep.transform.localPosition;
        int colorIndex = 0;
        for (int i = 0; i < ladderSteps; i++)
        {
            steps.y += 4;
            steps.z += 4;
            GameObject step = Instantiate(ladderStep);
            step.transform.parent = ladder.transform;
            step.transform.localPosition = steps;
            step.GetComponent<MeshRenderer>().material.color = ladderColors[colorIndex];
            colorIndex++;
            if (colorIndex > ladderColors.Length - 1)
            {
                colorIndex = 0;
            }
        }
        steps.y += 2.5f;
        finishObject.transform.localPosition = steps;
        TrackTotalLenght = finishObject.transform.position.z;
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
