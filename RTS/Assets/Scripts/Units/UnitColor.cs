using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitColor : MonoBehaviour {

    [Header("By default gets a mat in a renderer")]
    public Renderer[] renderers;
    public int[] matToChange;

  //  public Material[] mats;
 
    //public bool changeRenderers = true;

    public Color playerColor;
    public int playerNumber;

	// Use this for initialization
	void Start () {
        try
        {
            playerColor = GameManager.instance.playerColors[playerNumber];
        }
        catch
        {
            return;
        }


            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].materials[matToChange[i]].color = playerColor;
            }
	}


}
