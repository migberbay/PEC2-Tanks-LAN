using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class FindClientTank : MonoBehaviour
{
    public Button applyButton;
    EditTankData editData;

    void Start()
    {
        Invoke("Setup", 1);
    }

    void Setup() {
        foreach (var tank in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (tank.GetComponent<NetworkBehaviour>().isLocalPlayer)
            {
                editData = tank.GetComponent<EditTankData>();
            }
        }
        Debug.Log(editData);
        applyButton.onClick.AddListener(editData.CmdSetNewValues);
    }
}
    
