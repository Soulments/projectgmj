using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    bool isChase;
    bool isAttackReady;
    bool isDead;
    bool isHit = false;

    Rigidbody rigidbody;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    NavMeshAgent navMeshAgent;
    Animator animator;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        isChase = true;
        animator.SetBool("isWalk", true);
    }

    void LookAtMovingDirection()
    {
        //Vector3 direction = en
    }
}
