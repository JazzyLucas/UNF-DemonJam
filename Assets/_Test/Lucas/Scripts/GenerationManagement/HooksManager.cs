using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HooksManager : MonoBehaviour
{
    [SerializeField] public CrateLayoutEnum crateLayoutEnum = CrateLayoutEnum.UNASSIGNED;
    [SerializeField] public Transform playerSpawnHook;
    [SerializeField] public Transform cratesHook;
    [SerializeField] public Transform lightsHook;
}
