using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    // Externals
    public EnemyConfigurationSO enemyConfigurationSo;
    
    // Internals
    public NavMeshAgent navMeshAgent;
    
    void Start()
    {
    }
}
