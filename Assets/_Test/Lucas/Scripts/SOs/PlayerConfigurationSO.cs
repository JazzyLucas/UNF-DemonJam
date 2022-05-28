using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerConfigurationSO : ScriptableObject
{
    // Consts
    private const string SENSITIVITY_KEY = "PlayerConfigurationSensitivity";
    private const string VOLUME_KEY = "PlayerConfigurationVolume";
    public const float SENSITIVITY_MIN = 0.001f;
    public const float SENSITIVITY_MAX = 1f;
    private const string INVERT_X_KEY = "PlayerConfigurationInvertX";
    private const string INVERT_Y_KEY = "PlayerConfigurationInvertY";
    public const string lightObjectTag = "LightCollider";
    public const string enemyObjectTag = "EnemyCollider";
    public const string enemyDeathObjectTag = "EnemyDeathCollider";
    public const string safeObjectTag = "SafeCollider";
    public const string keyObjectTag = "KeyCollider";

    // Externals
    public GameObject playerPrefab;
    public GameObject deathUIPrefab;
    public GameObject winUIPrefab;

    // Hidden
    [HideInInspector] public GameObject playerReference;
    
    // Internals
    [HideInInspector]
    public bool isPlayerInALight;
    [HideInInspector]
    public bool isPlayerInRangeOfEnemy;
    [HideInInspector]
    public bool hasFoundKey;
    [HideInInspector]
    public bool gameEnd;

    // Properties
    public static float CameraSensitivity {
        get => Mathf.Clamp(PlayerPrefs.GetFloat(SENSITIVITY_KEY, 0.5f), SENSITIVITY_MIN, SENSITIVITY_MAX);
        set => PlayerPrefs.SetFloat(SENSITIVITY_KEY, Mathf.Clamp(value, SENSITIVITY_MIN, SENSITIVITY_MAX));
    }
    public static float Volume {
        get => Mathf.Clamp(PlayerPrefs.GetFloat(VOLUME_KEY, 1), 0, 1);
        set => PlayerPrefs.SetFloat(VOLUME_KEY, Mathf.Clamp(value, 0, 1));
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
}
