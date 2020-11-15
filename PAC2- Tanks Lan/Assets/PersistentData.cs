using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public string username;
    public Color userColor;

    public FlexibleColorPicker picker;
    public TMPro.TMP_InputField input;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveData() {
        userColor = picker.color;
        username = input.text;
    }
}
