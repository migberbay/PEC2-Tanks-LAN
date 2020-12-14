using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TankShoots : NetworkBehaviour
{
    public Transform shootingPoint;
    public GameObject BulletPrefab;


    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("pew?");
            CmdFire();
        }
    }

    [Command]
    public void CmdFire()
    {
        GameObject bullet = Instantiate(BulletPrefab, shootingPoint.position, shootingPoint.rotation);
        NetworkServer.Spawn(bullet);
    }
}
