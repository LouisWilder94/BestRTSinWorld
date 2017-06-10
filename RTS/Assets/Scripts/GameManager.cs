using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
    public static Player[] players;
    public static GameManager instance;

    public string[] playerTags;
    //player1 player2 player3 player4


    private void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void AddPlayerToList(Player playerToAdd)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] = null)
            {
                players[i] = playerToAdd;
                playerToAdd.playerNumber = i;
                return;
            }
        }
    }

    public Player getPlayer(int playerNumber)
    {
        return players[playerNumber];
    }

}
