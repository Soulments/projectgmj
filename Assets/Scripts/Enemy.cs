using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;

    bool isChase;
    bool isMove;
    bool isAttack = false;
    bool isDead;
    bool isHit = false;

    float attackRange = 3f;
    float closeRange = 1.5f;

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
        isMove = true;
        isChase = true;
        animator.SetBool("isMove", true);
    }

    private void Update()
    {
        if (navMeshAgent.enabled && Vector3.Distance(transform.position, target.position) < attackRange * 30)
        {
            navMeshAgent.SetDestination(target.position);
            
            navMeshAgent.isStopped = !isChase;
        }
        //float distance = Vector3.Distance(tr)
    }

    private void FixedUpdate()
    {
        bool isOverRange = Vector3.Distance(transform.position, target.position) > attackRange;
        bool isTooClose = Vector3.Distance(transform.position, target.position) < closeRange;
        Targeting(isOverRange);
        FreezeVelocity(isOverRange, isTooClose);
    }

    void FreezeVelocity(bool isOverRange, bool isTooClose)
    {
        if(isChase && !isOverRange || isTooClose)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    void Targeting(bool isOverRange)
    {
        if (isOverRange && !isAttack)
        {
            return;
        }
        if (isAttack)
        {
            return;
        }
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        isChase = false;
        isMove = false;
        animator.SetBool("isMove", false);
        isAttack = true;
        animator.SetTrigger("doAttack");
        yield return new WaitForSeconds(1.8f);
        isChase = true;
        isMove = true;
        animator.SetBool("isMove", true);
        isAttack = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            StartCoroutine(Hit());
        }
    }

    IEnumerator Hit()
    {
        animator.SetTrigger("doHit");
        yield return new WaitForSeconds(0.5f);
        OnDie();
    }

    private void OnDie()
    {
        animator.SetTrigger("doHit");
        Destroy(gameObject);
    }

    //IEnumerator OnDamage()
    //{
    //    Destroy(gameObject);
    //}
}
