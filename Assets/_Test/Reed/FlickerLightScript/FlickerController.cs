using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerController : MonoBehaviour
{
    public bool isFlickering = false;
    public Vector2 offTime;
    public Vector2 onTime;

    private void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }

    }

    private IEnumerator FlickeringLight()
    {
        isFlickering = true;
        this.gameObject.GetComponent<Light>().enabled = false;
        yield return new WaitForSeconds(Random.Range(offTime.x, offTime.y));
        this.gameObject.GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(Random.Range(onTime.x, onTime.y));
        isFlickering = false;
    }
}
