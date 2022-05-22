using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    NavMeshAgent enemy;
    public float xPos;
    public Vector3 pos;
    Animator animator;
    float rotationFactorPerFrame = 1.0f;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<NavMeshAgent>();


    }
    void handleRotation(GameObject player, float distance)
    {
        
        bool isWalking = animator.GetBool("isWalking");
        Vector3 look;

        look.x = player.transform.position.x;
        look.y = 0.0f;
        look.z = player.transform.position.z;
        
        Quaternion current = transform.rotation;
        if (isWalking && distance < 30)
        {
            Quaternion tarRotation = Quaternion.LookRotation(look);
            Quaternion.Slerp(current, tarRotation, rotationFactorPerFrame);
        }
    }
    void handleAnimation(GameObject player, float distance)
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isStill = animator.GetBool("isStill");
        if (distance > 30)
        {
            if (isWalking && !isStill)
            {
                animator.SetBool("isWalking", false);

            }
            else if (!isWalking && isStill)
            {
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

    }
    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector3.Distance(player.transform.position, transform.position);
        handleAnimation(player,distance);
        handleRotation(player, distance);
    }
}
