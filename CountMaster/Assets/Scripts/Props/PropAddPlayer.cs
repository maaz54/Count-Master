using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PropAddPlayer : MonoBehaviour
{
    public enum AddPlayerType
    {
        add,
        multiply
    }
    public AddPlayerType propType;
    public TextMeshProUGUI text;
    public int addPlayers;
    public BoxCollider collider;
    bool canTrigger = true;
    private void Start()
    {
        GameManager._instance.levelStart += GameStart;
    }

    void GameStart()
    {
        if (propType == AddPlayerType.add)
        {
            text.text = "" + addPlayers + "+";
        }
        else if (propType == AddPlayerType.multiply)
        {
            text.text = "" + addPlayers + "X";
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        //Player
        if (canTrigger)
        {
            if (col.CompareTag("Player"))
            {
                GameManager._instance.AddPlayert(addPlayers, propType);
                canTrigger=false;
            }
        }
    }


}
