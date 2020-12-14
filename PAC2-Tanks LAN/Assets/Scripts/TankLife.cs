using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class TankLife : NetworkBehaviour
{
    public const int MAX_LIFE = 100;

    [SyncVar(hook = nameof(OnChangeHealth))]
    public int currentHealth;

    public TMP_Text tankHPText;

    private void Start()
    {
        currentHealth = MAX_LIFE;
    }

    public void TakeDamage(float damage)
    {
        if (!isServer) return;

        Debug.Log("Daño recibido:" + this.gameObject.name);
        currentHealth -= (int)damage;

        if (currentHealth <= 0)
        {
            if(gameObject.tag == "Player")
            {
                currentHealth = MAX_LIFE;
                RpcRespawn();
            }
            if(gameObject.tag == "Enemy")
            {
                Destroy(gameObject);
            } 
        }
    }

    [ClientRpc]
    public void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            NetworkManagerTanks nmt = FindObjectOfType<NetworkManagerTanks>();
            transform.position = nmt.GetStartPosition().position;
        }
    }

    private void OnChangeHealth(int old, int newhp)
    {
        tankHPText.text = newhp.ToString();
    }
}
