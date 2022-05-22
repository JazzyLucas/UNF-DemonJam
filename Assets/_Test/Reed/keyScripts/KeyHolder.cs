using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//goes on player
public class KeyHolder : MonoBehaviour
{
    // Start is called before the first frame update

    private List<keyScript.KeyType> keylist;

    private void AddKey(keyScript.KeyType keytype)
    {
        keylist.Add(keytype);
    }

    private void RemoveKey(keyScript.KeyType keytype)
    {
        keylist.Remove(keytype);
    }

    private bool ContainsKey(keyScript.KeyType keytype)
    {
        return keylist.Contains(keytype);
    }

    private void OnTriggerEnter(Collider other)
    {
        keyScript key = other.GetComponent<keyScript>();
        if(key != null)
        {
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }

        KeyDoor keyDoor = other.GetComponent<KeyDoor>();
        if(keyDoor != null)
        {
            if (ContainsKey(keyDoor.GetKeyType()))
            {
                //have key to open door
                RemoveKey(keyDoor.GetKeyType());
                keyDoor.OpenDoor();
                
            }
        }
    }
}
