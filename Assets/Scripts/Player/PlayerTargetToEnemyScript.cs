using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetToEnemyScript : MonoBehaviour
{
    public Transform MainTarget { get; private set; }
    public List<Transform> Targets { get; private set; }
    private void Start()
    {
        MainTarget = transform.Find("bone_1").Find("Target");
        Targets = new List<Transform> { MainTarget };
        Targets.AddRange(MainTarget.GetComponentsInChildren<Transform>());
    }

}
