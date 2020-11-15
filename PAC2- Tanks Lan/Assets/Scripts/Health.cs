using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour {

    public bool destroyOnDeath;

    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public RectTransform healthBar;
    private NetworkStartPosition[] spawnPoints;

    private Camera cam;

    void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
        cam = Camera.allCameras[0];
    }

    public void TakeDamage(int amount) {
        if (!isServer)  {
            return;
        }

        currentHealth -= amount;
        if (currentHealth <= 0) {
            if (destroyOnDeath) {
                cam.GetComponent<FitTanks>().targets.Remove(this.gameObject.transform);
                Destroy(gameObject);
            }
            else {
                currentHealth = maxHealth;
                // called on the Server, but invoked on the Clients
                RpcRespawn();
            }
        }
    }

    private void OnChangeHealth (int oldHealth, int newHealth) {
        currentHealth = newHealth;
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn() {
        if (isLocalPlayer) {
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick a spawn point at random
            if (spawnPoints != null && spawnPoints.Length > 0) {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player's position to the chosen spawn point
            transform.position = spawnPoint;
        }
    }
}