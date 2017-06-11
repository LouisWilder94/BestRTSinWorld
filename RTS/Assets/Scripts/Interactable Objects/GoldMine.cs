using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMine : MonoBehaviour {

    int _gold = 3000;
    public int gold
    {
        get
        {
            return _gold;
        }
        set
        {
            _gold = value;
        }
    }
    // for builders to use
    public int HarvestGold()
    {
        gold -= 5;
        return 5;
    }
	
}
