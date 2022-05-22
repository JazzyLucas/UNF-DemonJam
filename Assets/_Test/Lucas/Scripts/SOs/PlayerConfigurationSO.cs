using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerConfigurationSO : ScriptableObject
{
    // Consts
    private const string SENSITIVITY_KEY = "PlayerConfigurationSensitivity";
    private const int SENSITIVITY_MIN = 100;
    private const int SENSITIVITY_MAX = 1000;
    private const string INVERT_X_KEY = "PlayerConfigurationInvertX";
    private const string INVERT_Y_KEY = "PlayerConfigurationInvertY";
    
    // Properties
    public static float CameraSensitivity {
        get =>  Mathf.Clamp(PlayerPrefs.GetFloat(SENSITIVITY_KEY), SENSITIVITY_MIN, SENSITIVITY_MAX);
        set => PlayerPrefs.SetFloat(SENSITIVITY_KEY, Mathf.Clamp(value, SENSITIVITY_MIN, SENSITIVITY_MAX));
    }
    public bool InvertX
    {
        get => PlayerPrefs.GetInt(INVERT_X_KEY) > 0;
        set => PlayerPrefs.SetInt(INVERT_X_KEY, value ? 1 : 0);
    }
    public bool InvertY
    {
        get => PlayerPrefs.GetInt(INVERT_Y_KEY) > 0;
        set => PlayerPrefs.SetInt(INVERT_Y_KEY, value ? 1 : 0);
    }
    
    // Fields
    
}
