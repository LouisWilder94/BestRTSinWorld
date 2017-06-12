using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCache : MonoBehaviour {
    // This script is for cacheing the components on the UI button to make the dynamic UI script easier to write. 
    // (basically instead of writing getcomponent 50 times)

    public Button buttonComponent;
    public Text textComponent;

    private void Awake()
    {
        buttonComponent = GetComponent<Button>();
        textComponent = GetComponentInChildren<Text>();
    }
}
