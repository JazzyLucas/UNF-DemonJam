using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool visited = false;
    public bool[] status = new bool[4];
}

[System.Serializable]
public class Rule
{
    // Externals
    [SerializeField]
    public GameObject room;
    [Header("Optional Fields")]
    [SerializeField]
    public Vector2 minPosition;
    [SerializeField]
    public Vector2 maxPosition;
    [SerializeField]
    public bool obligatory;

    [HideInInspector]
    public int ProbabilityOfSpawning(int x, int y)
    {
        // 0 - cannot spawn 1 - can spawn 2 - HAS to spawn
        if (x>= minPosition.x && x<=maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
        {
            return obligatory ? 2 : 1;
        }

        return 0;
    }
}

[CreateAssetMenu]
public class DungeonGenerationConfigurationSO : ScriptableObject
{
    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset;

    
    [HideInInspector]
    public uint roomsReady, roomCount;
    [HideInInspector]
    public List<Cell> board;
    [HideInInspector]
    public List<GameObject> objectPool;
}
