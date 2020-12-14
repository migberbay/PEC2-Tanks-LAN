using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class TankControl : NetworkBehaviour
{
    // functions to change the name and color of players during play.
    
    [SyncVar(hook = nameof(OnChangeName))]
    public string playerName;

    [SyncVar(hook = nameof(OnChangeColor))]
    public Color playerColor;

    // Canvas
    public GameObject playerUI;

    // Canvas Name
    public TMP_Text playerNameText;

    // Prefab canvas settings.
    public TMP_InputField playerNameInput;
    public TMP_Dropdown playerColorDropdown;

    public void Start()
    {
        if(!isLocalPlayer)
            return;

        playerUI.SetActive(true);
    }

    public void SendInfo()
    {
        if (!isLocalPlayer) return;

        CmdChangeSyncVarsFromMenu(this.gameObject,playerNameInput.text, playerColorDropdown.options[playerColorDropdown.value].text);
    }

    [Command]
    public void CmdChangeSyncVarsFromMenu(GameObject playerGO, string newName, string newColor)
    {
        TankControl player = playerGO.GetComponent<TankControl>();
        player.playerColor = (Color)typeof(Color).GetProperty(newColor.ToLowerInvariant()).GetValue(null, null);
        player.playerName = newName;
    }

    private void OnChangeName(string oldString, string newName)
    {
        Debug.Log("Changing the name of player to: " + newName);
        if(newName == "")
        {
            playerNameText.text = "Player";
        }
        else
        {
            playerNameText.text = newName;
        }
        
    }

    private void OnChangeColor(Color oldColor, Color newColor)
    {
        Debug.Log("Changing the color of player to: " + newColor.ToString());
        foreach (MeshRenderer child in GetComponentsInChildren<MeshRenderer>())
        {
            child.material.color = newColor;
        }
    }

    public override void OnStartLocalPlayer()
    {
        // Change color of player based on menu input.

    }
}
