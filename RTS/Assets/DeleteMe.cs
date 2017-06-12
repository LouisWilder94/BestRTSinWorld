using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteMe : MonoBehaviour {

	void Start ()
    {
        GetComponent<Button>().onClick.AddListener(TestMethod);
	}

    void TestMethod()
    {
        Debug.Log("I worked");
    }
}
