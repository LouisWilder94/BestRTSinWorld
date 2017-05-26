using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setLineRendererOrder : MonoBehaviour {

    public int lineRenderSortingorder;

	// Use this for initialization
	void Start () {
        transform.GetComponent<LineRenderer>().sortingOrder = lineRenderSortingorder;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
