using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class GameManager : NetworkBehaviour {

    //[SyncVar]
    //public static Player[] players;
    
    public List<Player> players;

   
    public static GameManager instance;

    [SyncVar]
    public int numPlayers = 0;

    
    public string[] playerTags;
    //player1 player2 player3 player4

    //public GameObject[] maxPlayers;


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
        //players = 2;

     //   for (int i = 0; i < maxPlayers; i++)
     //   {
     //       if (players[i] != null)
     //           Debug.Log("player" + i + "   " + players[i]);
     //      else
     //          return;
     //  }
    }

    //[Server]

        //[ClientRpc]

        [ServerCallback]
    public void RpcAddPlayerToList(Player playerToAdd)
    {
        //if (!isServer)
       // {
        //    return;
       // }

        numPlayers++;
        players.Add(playerToAdd);
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].randomID == playerToAdd.randomID)
            {
                playerToAdd.playerNumber = i + 1;
                Debug.Log("Added player" + (i + 1));
            }
        }
        //players[numPlayers] = playerToAdd;
        //Debug.Log("Added player" + numPlayers);
        //playerToAdd.playerNumber = numPlayers;

      //  for (int i = 0; i < 10; i++)
      //  {
      //      if (players[i] != null)
      //      {
       //         Debug.Log("Player in slot");
        //    }
        //    else
        //    {
         //       players.Add(playerToAdd); // = playerToAdd;
         //       playerToAdd.playerNumber = i;
          //      Debug.Log("added player" + i);
          //      return;
           // }
       // }
    }

    public Player getPlayer(int playerNumber)
    {
        return players[playerNumber];
    }

}
