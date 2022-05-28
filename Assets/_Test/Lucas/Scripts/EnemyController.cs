using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    // Externals
    public EnemyConfigurationSO enemyConfigurationSo;
    public NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;

    void Start()
    {
    }

    private void Update()
    {
        // Animation controlling
        animator.SetBool(Walking, navMeshAgent.velocity.magnitude > 0.1f);
        animator.SetFloat(Speed, navMeshAgent.velocity.magnitude/2);
    }
}
