using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySkeletonSword : Enemy
{
    public BoxCollider attackBox;
    protected override void Awake()
    {
        base.Awake();
        status = new Status(UnitCode.Enemy, "근거리", 1);
        isMove = true;
        isChase = true;
        animator.SetBool("isMove", true);
        Spawn();
    }

    protected override void Update()
    {
        base.Update();
        if (navMeshAgent.enabled && Vector3.Distance(transform.position, target.position) < attackRange * 30)
        {
            navMeshAgent.SetDestination(target.position);
            navMeshAgent.isStopped = !isChase;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        bool isOverRange = Vector3.Distance(transform.position, target.position) > attackRange;
        bool isTooClose = Vector3.Distance(transform.position, target.position) < closeRange;
        Targeting(isOverRange);
        FreezeVelocity(isOverRange, isTooClose);
    }

    // 스켈레톤 검사 공격 함수
    protected override void Targeting(bool isOverRange)
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

    protected override IEnumerator Attack()
    {
        isChase = false;
        isMove = false;
        animator.SetBool("isMove", false);
        isAttack = true;
        animator.SetTrigger("doAttack");
        yield return new WaitForSeconds(1.0f);
        //attackBox.enabled = true;
        yield return new WaitForSeconds(1.8f);
        isChase = true;
        isMove = true;
        animator.SetBool("isMove", true);
        isAttack = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            StartCoroutine(Hit(1));
        }
        else if (other.gameObject.CompareTag("Weapon2"))
        {
            StartCoroutine(Hit(2));
        }
    }

    IEnumerator Hit(int hitnum)
    {
        if (hitnum == 1)
        {
            animator.SetTrigger("doHit");
        }
        else
        {
            animator.SetTrigger("doAirborne");
            isAirBorned = true;
            capsuleCollider.enabled = false;
            yield return new WaitForSeconds(3.0f);
            capsuleCollider.enabled = true;
            isAirBorned = false;
        }
        hitcount++;
        Debug.Log("hitcount: "+hitcount);

        // hitcount > 5 조건문 변경
        if (status.CurrentHP < 0) OnDie();
        else yield return new WaitForSeconds(0.5f);
    }

    protected override void OnDie()
    {
        base.OnDie();
        body.SetActive(false);
        dead.SetActive(true);
        StartCoroutine(CoroutineDie());
    }

    IEnumerator CoroutineDie()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}
