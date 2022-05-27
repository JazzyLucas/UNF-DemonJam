using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeperateMe : MonoBehaviour
{
    private void Awake()
    {
        this.transform.parent = null;
    }
}
