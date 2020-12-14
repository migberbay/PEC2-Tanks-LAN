using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class LobbyManager : MonoBehaviour
{

    public NetworkManagerTanks networkManager;

    [Header("Configuration options")]
    public TMP_Dropdown menuDropdownColor;
    public TMP_InputField inputText;


    public void UpdatePlayerPrefWithMenuInfo() {
        PlayerPrefs.SetString("playerName", inputText.text);
        PlayerPrefs.SetString("playerColor", menuDropdownColor.options[menuDropdownColor.value].text);
    }

    public void StartHost()
    {
        UpdatePlayerPrefWithMenuInfo();
        networkManager.StartHost();
    }

    public void StartServer()
    {
        networkManager.StartServer();
    }
    public void StartClient()
    {
        UpdatePlayerPrefWithMenuInfo();
        networkManager.StartClient();
    }
}
