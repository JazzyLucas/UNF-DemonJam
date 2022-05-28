using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickUp : MonoBehaviour
{
    [SerializeField]
    private TextMesh textObject = GameObject.Find("Pickup").GetComponent<TextMesh>();

    private bool pickUpAllowed;
    // Start is called before the first frame update
    private void Start()
    {
        textObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if(pickUpAllowed && Input.GetKeyDown(KeyCode.E)){
            PickU();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            textObject.gameObject.SetActive(true);
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            textObject.gameObject.SetActive(false);
            pickUpAllowed = false;
        }
    }


    //Below we can probably put score functionality, reference the item itself and establish what score would be given to the player for picking it up
    private void PickU()
    {
        Destroy(gameObject);
    }
}
