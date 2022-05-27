using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

/// <summary>
/// The overall manager for the game itself. Functions pseudo-asynchronously with with <see cref="SBDungeonGenerator"/>
/// </summary>
public class GameGeneratorManager : MonoBehaviour
{
    public const float BIG_OFFSET = 0.5f;
    public const float SMALL_OFFSET = 0.3f;
    
    // Externals
    [SerializeField] private DungeonGenerationConfigurationSO DungeonGenerationConfigurationSO;
    [SerializeField] private PlayerConfigurationSO playerConfigurationSO;
    [Space(10)]
    [SerializeField] private bool verboseDebug;
    
    // Internals
    private GameObject playerReference;
    private GameObject enemyReference;
    private List<GameObject> objectPool;
    
    void LateUpdate()
    {
        // Pseudo-Async with SBDungeonGenerator
        if (DungeonGenerationConfigurationSO.canDoGameGeneration)
        {
            Log("GameGeneratorManager sensed that it 'canDoGameGeneration,' proceeding!");
            DungeonGenerationConfigurationSO.canDoGameGeneration = false;
            DoGameGeneration();
            //TODO: DungeonGenerationConfigurationSO.forceNavMeshRecheck = true;
        }
    }

    private void DoGameGeneration()
    {
        if (objectPool != null)
        {
            foreach (var obj in objectPool)
            {
                Destroy(obj);
            }
            objectPool.Clear();
        }
        objectPool ??= new List<GameObject>();
        
        Log("Placing player in a room...");
        FindHookAndSpawnPlayer();
        Log("Placing enemy in a room...");
        FindHookAndSpawnAnEnemy();
        
        Log("Generating hooks for all rooms...");
        foreach (HooksManager hooksManager in DungeonGenerationConfigurationSO.hooksManagers)
        {
            GenerateCrates(hooksManager);
            GenerateLights(hooksManager);
        }
        
        Log("Re-baking scene...");
        DungeonGenerationConfigurationSO.forceNavMeshRecheck = true;
        
        Log("Activating enemy random-movement...");
        EnemyController enemyController = enemyReference.GetComponent<EnemyController>();
        enemyController.navMeshAgent.destination = DungeonGenerationConfigurationSO.hooksManagers[Random.Range(0, DungeonGenerationConfigurationSO.hooksManagers.Count)].playerSpawnHook.transform.position;
    }

    private void FindHookAndSpawnPlayer()
    {
        // Generate a random spawn
        int spawnSelection = Random.Range(0, DungeonGenerationConfigurationSO.hooksManagers.Count);
        Vector3 bigOffset = new Vector3(Random.Range(-BIG_OFFSET, BIG_OFFSET), 0f, Random.Range(-BIG_OFFSET, BIG_OFFSET));
        // Set the crate generation of the random spawn preliminarily
        // (we don't want to spawn the player inside a crate)
        CrateLayoutEnum spawnCrateLayoutEnumSelection = Random.Range(0, 2) switch
        {
            0 => CrateLayoutEnum.NONE,
            1 => CrateLayoutEnum.CORNERS,
            _ => CrateLayoutEnum.NONE
        };

        DungeonGenerationConfigurationSO.hooksManagers[spawnSelection].crateLayoutEnum = spawnCrateLayoutEnumSelection;
        
        if (playerReference == null)
        {
            playerReference = Instantiate(playerConfigurationSO.playerPrefab,  DungeonGenerationConfigurationSO.hooksManagers[spawnSelection].playerSpawnHook.transform.position + bigOffset, Quaternion.identity);
        }
        else
        {
            playerReference.transform.position = DungeonGenerationConfigurationSO.hooksManagers[spawnSelection].playerSpawnHook.transform.position + bigOffset;
        }
    }
    
    private void FindHookAndSpawnAnEnemy()
    {
        // Generate a random spawn
        int spawnSelection = Random.Range(0, DungeonGenerationConfigurationSO.hooksManagers.Count);
        Vector3 bigOffset = new Vector3(Random.Range(-BIG_OFFSET, BIG_OFFSET), 0f, Random.Range(-BIG_OFFSET, BIG_OFFSET));
        // Set the crate generation of the random spawn preliminarily
        // (we don't want to spawn the player inside a crate)
        CrateLayoutEnum spawnCrateLayoutEnumSelection = Random.Range(0, 2) switch
        {
            0 => CrateLayoutEnum.NONE,
            1 => CrateLayoutEnum.CORNERS,
            _ => CrateLayoutEnum.NONE
        };

        DungeonGenerationConfigurationSO.hooksManagers[spawnSelection].crateLayoutEnum = spawnCrateLayoutEnumSelection;
        
        if (enemyReference == null)
        {
            enemyReference = Instantiate(DungeonGenerationConfigurationSO.enemyPrefab,  DungeonGenerationConfigurationSO.hooksManagers[spawnSelection].playerSpawnHook.transform.position + bigOffset, Quaternion.identity);
        }
        else
        {
            enemyReference.transform.position = DungeonGenerationConfigurationSO.hooksManagers[spawnSelection].playerSpawnHook.transform.position + bigOffset;
        }
    }

    private void GenerateCrates(HooksManager hook)
    {
        hook.crateLayoutEnum = hook.crateLayoutEnum == 0 ? RandomEnumValue<CrateLayoutEnum>(true) : hook.crateLayoutEnum;
        
        bool[] lay = GetCrateLayout(hook.crateLayoutEnum);
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                int index = i + (j * 6);
                if (lay[index])
                {
                    if (Random.Range(0, DungeonGenerationConfigurationSO.crateSpawnMultiplier) == 0)
                    {
                        Vector3 smallOffset = new Vector3(Random.Range(-SMALL_OFFSET, SMALL_OFFSET), 0f, Random.Range(-SMALL_OFFSET, SMALL_OFFSET));
                        var g = Instantiate(
                            DungeonGenerationConfigurationSO.cratePrefabs[
                                Random.Range(0, DungeonGenerationConfigurationSO.cratePrefabs.Length)],
                            hook.cratesHook.transform.position + new Vector3(i, 0f, j) + smallOffset,
                            Quaternion.Euler(0, Random.Range(0, 20), 0));
                        objectPool.Add(g);
                    }
                }
            }
        }
    }

    private void GenerateLights(HooksManager hook)
    {
        bool[] lay = GetCrateLayout(CrateLayoutEnum.CENTER);
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                int index = i + (j * 6);
                if (lay[index])
                {
                    if ((int)Random.Range(0, DungeonGenerationConfigurationSO.lightSpawnMultiplier) == 0)
                    {
                        Vector3 smallOffset = new Vector3(Random.Range(-SMALL_OFFSET, SMALL_OFFSET), 0f, Random.Range(-SMALL_OFFSET, SMALL_OFFSET));
                        var g = Instantiate(
                            DungeonGenerationConfigurationSO.lightPrefabs[
                                Random.Range(0, DungeonGenerationConfigurationSO.lightPrefabs.Length)],
                            hook.lightsHook.transform.position + new Vector3(i, 0f, j) + smallOffset,
                            Quaternion.Euler(0, Random.Range(0, 360), 0));
                        objectPool.Add(g);
                        return; // We want to return because we want maximum of 1 light per room
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method and its usages are a complete mess, don't look at it lol.
    /// <br></br>
    /// ~Lucas
    /// </summary>
    /// <param name="layoutEnum"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public bool[] GetCrateLayout(CrateLayoutEnum layoutEnum)
    {
        int[] layout = new int[36];
        switch (layoutEnum)
        {
            case CrateLayoutEnum.NONE:
                layout = new int[]
                {
                    0,0,0,0,0,0,
                    0,0,0,0,0,0,
                    0,0,0,0,0,0,
                    0,0,0,0,0,0,
                    0,0,0,0,0,0,
                    0,0,0,0,0,0,
                };
                break;
            case CrateLayoutEnum.CORNERS:
                layout = new int[]
                {
                    1,1,0,0,1,1,
                    1,0,0,0,0,1,
                    0,0,0,0,0,0,
                    0,0,0,0,0,0,
                    1,0,0,0,0,1,
                    1,1,0,0,1,1,
                };
                break;
            case CrateLayoutEnum.CENTER:
                layout = new int[]
                {
                    0,0,0,0,0,0,
                    0,0,0,0,0,0,
                    0,0,1,1,0,0,
                    0,0,1,1,0,0,
                    0,0,0,0,0,0,
                    0,0,0,0,0,0,
                };
                break;
            case CrateLayoutEnum.HORIZONTAL:
                layout = new int[]
                {
                    0,0,0,0,0,0,
                    0,0,0,0,0,0,
                    1,1,1,1,1,1,
                    1,1,1,1,1,1,
                    0,0,0,0,0,0,
                    0,0,0,0,0,0,
                };
                break;
            case CrateLayoutEnum.VERTICAL:
                layout = new int[]
                {
                    0,0,1,1,0,0,
                    0,0,1,1,0,0,
                    0,0,1,1,0,0,
                    0,0,1,1,0,0,
                    0,0,1,1,0,0,
                    0,0,1,1,0,0,
                };
                break;
            case CrateLayoutEnum.DIAGONAL_LR:
                layout = new int[]
                {
                    1,0,0,0,0,0,
                    0,1,1,0,0,0,
                    0,1,0,1,0,0,
                    0,0,1,0,1,0,
                    0,0,0,1,0,0,
                    0,0,0,0,0,1,
                };
                break;
            case CrateLayoutEnum.DIAGONAL_RL:
                layout = new int[]
                {
                    0,0,0,0,0,1,
                    0,0,0,1,0,0,
                    0,0,1,0,1,0,
                    0,1,0,1,0,0,
                    0,0,1,0,0,0,
                    1,0,0,0,0,0,
                };
                break;
            case CrateLayoutEnum.CHECKERBOARD:
                layout = new int[]
                {
                    0,1,0,1,0,1,
                    1,0,1,0,1,0,
                    0,1,0,1,0,1,
                    1,0,1,0,1,0,
                    0,1,0,1,0,1,
                    1,0,1,0,1,0,
                };
                break;
            case CrateLayoutEnum.CHECKERBOARD_ALT:
                layout = new int[]
                {
                    1,0,1,0,1,0,
                    0,1,0,1,0,1,
                    1,0,1,0,1,0,
                    0,1,0,1,0,1,
                    1,0,1,0,1,0,
                    0,1,0,1,0,1,
                };
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(layoutEnum), layoutEnum, null);
        }

        bool[] value = new bool[36];
        for (int i = 0; i < layout.Length; i++)
        {
            value[i] = Convert.ToBoolean(i);
        }
        return value;
    }
    
    public static T RandomEnumValue<T>(bool ignoreFirst)
    {
        var values = Enum.GetValues(typeof(T));
        int random = UnityEngine.Random.Range(ignoreFirst ? 1 : 0, values.Length);
        return (T)values.GetValue(random);
    }

    private void Log(string message)
    {
        if (verboseDebug)
            Debug.Log(message);
    }
}

public enum CrateLayoutEnum
{
    UNASSIGNED,
    NONE,
    CORNERS,
    CENTER,
    HORIZONTAL,
    VERTICAL,
    DIAGONAL_LR,
    DIAGONAL_RL,
    CHECKERBOARD,
    CHECKERBOARD_ALT,
}