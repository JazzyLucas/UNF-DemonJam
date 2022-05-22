using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyScript : MonoBehaviour
{

    [SerializeField] private KeyType keyType;
    public enum KeyType
    {
        unlock,
        unlock_2,
        unlock_3
    }
    public KeyType GetKeyType()
    {
        return keyType;
    }
}
