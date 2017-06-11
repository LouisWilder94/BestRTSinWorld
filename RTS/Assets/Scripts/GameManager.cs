using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System;

public class GameManager : NetworkBehaviour {
    public List<Player> players;
    public static GameManager instance;
    public static int numPlayers = 0;
    public string[] playerTags;



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

    [Server]
    public void RpcsortPlayers(List<Player> players)
    {
        players.Sort(new PlayerComparer());
    }

    public static void IncreasePlayerNumber()
    {
        numPlayers++;
    }


        [ServerCallback]
    public void RpcAddPlayerToList(Player playerToAdd)
    {
        players.Add(playerToAdd);
        playerToAdd.playerNumber = numPlayers;
        Debug.Log("Added player" + numPlayers);
        FunctionsHelper.AddPlayerTagToObject(playerToAdd, playerToAdd.gameObject);
        Debug.Log("Added player tag " + playerToAdd.gameObject.tag);
        Debug.Log("Returning Player" + players[numPlayers].playerNumber);
    }

    public Player getPlayer(int playerNumber)
    {
        return players[playerNumber];
    }

}

public class PlayerComparer : IComparer<Player>
{
    public int Compare(Player x, Player y)
    {
        if (x.playerNumber > y.playerNumber)
        {
            return -1;
        }
        else if (y.playerNumber > x.playerNumber)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
