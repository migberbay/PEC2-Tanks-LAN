using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

// this class edits the player color and name at runtime so it needs to sync.
public class EditTankData : NetworkBehaviour{
    public GameObject tank;

    public Text newUsername;
    public FlexibleColorPicker picker;

    void Start()
    {
        var IGM = GameObject.FindGameObjectWithTag("InGameMenu");
        newUsername = IGM.transform.Find("NameChangeInput").Find("Text").gameObject.GetComponent<Text>();
        picker = IGM.GetComponentInChildren<FlexibleColorPicker>();

        IGM.SetActive(false);

        Camera.allCameras[0].transform.gameObject.GetComponent<FitTanks>().targets.Add(gameObject.transform);
    }

    [Command]
    public void CmdSetNewValues(){

        foreach (MeshRenderer child in tank.GetComponentsInChildren<MeshRenderer>()){
            child.material.color = picker.color;
        }

        if(newUsername.text != ""){
            tank.GetComponentInChildren<TextMeshProUGUI>().text = newUsername.text;
        }
    }
}
