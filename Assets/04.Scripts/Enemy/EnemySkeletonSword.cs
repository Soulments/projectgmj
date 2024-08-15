using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySkeletonSword : Enemy
{
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        isMove = true;
        isChase = true;
        animator.SetBool("isMove", true);
    }

    void Update()
    {
        if (navMeshAgent.enabled && Vector3.Distance(transform.position, target.position) < attackRange * 30)
        {
            navMeshAgent.SetDestination(target.position);

            navMeshAgent.isStopped = !isChase;
        }
    }

    void FixedUpdate()
    {
        bool isOverRange = Vector3.Distance(transform.position, target.position) > attackRange;
        bool isTooClose = Vector3.Distance(transform.position, target.position) < closeRange;
        Targeting(isOverRange);
        FreezeVelocity(isOverRange, isTooClose);
    }

    // ½ºÄÌ·¹Åæ °Ë»ç °ø°Ý ÇÔ¼ö
    new void Targeting(bool isOverRange)
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

    new IEnumerator Attack()
    {
        isChase = false;
        isMove = false;
        animator.SetBool("isMove", false);
        isAttack = true;
        animator.SetTrigger("doAttack");
        yield return new WaitForSeconds(2.8f);
        isChase = true;
        isMove = true;
        animator.SetBool("isMove", true);
        isAttack = false;
    }
}
