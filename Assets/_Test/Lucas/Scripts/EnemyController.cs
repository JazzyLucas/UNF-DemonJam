using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Speed = Animator.StringToHash("Speed");

    // Externals
    public EnemyConfigurationSO enemyConfigurationSO;
    public PlayerConfigurationSO playerConfigurationSO;
    public DungeonGenerationConfigurationSO dungeonGenerationConfigurationSO;
    public NavMeshAgent navMeshAgent;
    public float targetingSpeedMultiplier = 1.25f;
    [SerializeField] private Animator animator;

    // Internals
    private float regularSpeed = 0f;
    private float targetingSpeed = 0f;
    
    void Start()
    {
        regularSpeed = navMeshAgent.speed;
        targetingSpeed = navMeshAgent.speed * targetingSpeedMultiplier;
    }

    private void Update()
    {
        // Animation controlling
        animator.SetBool(Walking, navMeshAgent.velocity.magnitude > 0.1f);
        animator.SetFloat(Speed, navMeshAgent.velocity.magnitude/2);
        
        if (playerConfigurationSO.isPlayerInALight && playerConfigurationSO.isPlayerInRangeOfEnemy)
        {
            TargetThePlayer();
        }
        else if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    AssignNewRandomWaypoint();
                }
            }
        }
        else
        {
            navMeshAgent.speed = regularSpeed;
        }
    }

    public void AssignNewRandomWaypoint()
    {
        navMeshAgent.destination = dungeonGenerationConfigurationSO.hooksManagers[Random.Range(0, dungeonGenerationConfigurationSO.hooksManagers.Count)].playerSpawnHook.transform.position;
    }
    
    private void TargetThePlayer()
    {
        navMeshAgent.speed = targetingSpeed;
        Debug.Log("Targeting the player!");
        navMeshAgent.destination = playerConfigurationSO.playerReference.gameObject.transform.position;
    }
}
