using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using Cinemachine;

public class NetworkManagerTanks : NetworkManager
{
    [Header("Configuration options")]
    public TMP_Dropdown menuDropdownColor;
    public TMP_InputField inputText;

    public GameObject TankNPCPrefab;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        foreach (var o in conn.clientOwnedObjects)
        {
            if (o.gameObject.tag == "Player")
            {
                Debug.Log("are the tank renderers found?");
                Debug.Log(o.gameObject.transform.Find("TankRenderers"));
                
                Debug.Log("Found a player in the client");
                string newName = PlayerPrefs.GetString("playerName");
                string newColor = PlayerPrefs.GetString("playerColor");
                Debug.Log("Preferences values are: " + newName + " , " + newColor);

                TankControl t = o.gameObject.GetComponent<TankControl>();
                t.playerName = newName;
                t.playerColor = (Color)typeof(Color).GetProperty(newColor.ToLowerInvariant()).GetValue(null, null);
            }
        }
    }

   
    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("Spawn NPC Tanks");
        for(int i = 0; i < 4; i++)
        {
            Transform loc = GetStartPosition();
            GameObject inst = Instantiate(TankNPCPrefab, loc.position, loc.rotation);
            NetworkServer.Spawn(inst);
        }
    }
    
    /*
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        CameraTargets = FindObjectOfType<CinemachineTargetGroup>();
        Debug.Log("Spawn NPC Tanks");
        foreach(var tank in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            CameraTargets.AddMember(tank.transform, 1, 2);
        }
    }
    */
}
