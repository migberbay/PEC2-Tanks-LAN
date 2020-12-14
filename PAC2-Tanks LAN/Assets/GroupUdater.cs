using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GroupUdater : MonoBehaviour
{
    public CinemachineTargetGroup targets;

    private void Start()
    {
        StartCoroutine(CheckForPlayers());
    }

    IEnumerator CheckForPlayers()
    {
        FindInGroup(GameObject.FindGameObjectsWithTag("Enemy"));
        FindInGroup(GameObject.FindGameObjectsWithTag("Player"));

        yield return new WaitForSeconds(.5f);
        StartCoroutine(CheckForPlayers());
        yield return 0;
    }

    void FindInGroup(GameObject[] grp){
        bool inGroup;
        foreach (var p in grp)
        {
            inGroup = false;
            foreach (var t in targets.m_Targets)
            {
                if (t.target.Equals(p.transform)) {inGroup = true; break;}
            }
            if (!inGroup)
            {
                targets.AddMember(p.transform, 1, 4);
            }
        }
    }
}
