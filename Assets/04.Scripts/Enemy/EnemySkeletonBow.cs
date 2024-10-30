using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySkeletonBow : Enemy
{
    public GameObject arrow;
    public GameObject arrowPoint;

    bool ishit;

    protected override void Start()
    {
        base.Start();
        status = new Status(UnitCode.Enemy, "원거리", 1);
        isChase = true;
        attackRange = 30.0f;
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
        AnimationCheck();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        bool isOverRange = Vector3.Distance(transform.position, target.position) > attackRange;
        bool isTooClose = Vector3.Distance(transform.position, target.position) < closeRange;
        if (status.CurrentHP > 0) Targeting(isOverRange);
        FreezeVelocity(isOverRange, isTooClose);
        transform.LookAt(target);
    }
    void AnimationCheck()
    {
        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
        
        if (currentAnimation.IsName("Hit"))
        {
            ishit = true;
        }
        else
        {
            ishit = false;
        }
        if (currentAnimation.IsName("Airborne") || currentAnimation.IsName("GetUp"))
        {
            dontDaamge = true;
        }
        else
        {
            dontDaamge = false;
        }
    }

    // 스켈레톤 궁수 공격 함수
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
        if (ishit)
        {
            return;
        }
        StartCoroutine(Attack());
    }

    protected override IEnumerator Attack()    
    {
        isChase = false;
        isAttack = true;
        animator.SetTrigger("doShoot");
        yield return new WaitForSeconds(2.0f);
        Shoot();
        yield return new WaitForSeconds(3.0f);
        isChase = true;
        isAttack = false;
    }

    void Shoot()
    {
        GameObject instantarrow = Instantiate(arrow, arrowPoint.transform.position, arrowPoint.transform.rotation);
        Rigidbody rigidarrow = instantarrow.GetComponent<Rigidbody>();
        Arrow arrowScrpit = instantarrow.GetComponent<Arrow>();
        if (arrowScrpit != null)
        {
            arrowScrpit.SetSummoner(this);
        }
        rigidarrow.velocity = transform.forward * 20;
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

        if (status.CurrentHP < 0) OnDie();
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
