using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Anchor 2 GameObjects together with extra options.
/// <br></br>
/// You could just parent objects if you don't want to use a script, but this gives more freedom.
/// </summary>
public class TransformAnchor : MonoBehaviour
{
    #region Public Field

    public Transform childTransform;
    public Transform parentTransform;

    #endregion

    #region Private Serialized Field

    [SerializeField] private Vector3 offset;
    [SerializeField] private bool matchTransform, matchRotation, matchYAxisOnly, dampTransform = false;
    [Header("DampSpeed is usually between 0.001 - 0.03")]
    [SerializeField] private float dampSpeed;

    // Reference Variables
    private float xVelocity, yVelocity, zVelocity, timePassed;

    #endregion

    #region Methods

    private void Anchor()
    {
        if (childTransform == null || parentTransform == null)
        {
            Debug.LogError(this.gameObject.name + "'s TransformAnchor script didn't have a parent/child transform! Destroying.");
            Destroy(this);
        }

        if (matchTransform)
        {
            if (dampTransform)
            {
                float dampX = Mathf.SmoothDamp(childTransform.position.x, parentTransform.position.x, ref xVelocity, dampSpeed, Mathf.Infinity, Time.smoothDeltaTime);
                float dampY = Mathf.SmoothDamp(childTransform.position.y, parentTransform.position.y, ref yVelocity, dampSpeed, Mathf.Infinity, Time.smoothDeltaTime);
                float dampZ = Mathf.SmoothDamp(childTransform.position.z, parentTransform.position.z, ref zVelocity, dampSpeed, Mathf.Infinity, Time.smoothDeltaTime);
                childTransform.position = new Vector3(dampX, dampY, dampZ);
            }
            else
            {
                childTransform.position = parentTransform.position;
            }
        }
        if (matchRotation)
        {
            childTransform.rotation = parentTransform.rotation;
        }
        if (matchYAxisOnly)
        {
            childTransform.rotation = Quaternion.Euler(0f, parentTransform.rotation.eulerAngles.y, 0f);
        }
        if (offset != Vector3.zero)
        {
            childTransform.Translate(offset, Space.Self);
        }
    }

    #endregion

    #region Unity Methods

    private void Update()
    {
        Anchor();
    }

    #endregion
}
