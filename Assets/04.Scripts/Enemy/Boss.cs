using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    // 보스 패턴용 함수
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
        //StartCoroutine(Attack());
    }

    // 보스 이동 함수
    void Move()
    {
        // 전후좌우 방향 설정
    }

    // 보스 공격 함수
    void Attack()
    {
        // 각종 캐스트로 이동
        // 캐스트 A로 가야할 경우
        // 캐스트 B로 가야할 경우
        // 캐스트 C로 가야할 경우
        // 공격 타이밍 아닐 경우
    }

    // 캐스트 A 모션용
    void CastA()
    {
        // 궁수 스켈레톤 소환

        // 스켈레톤 버프
        animator.SetTrigger("doCastA");
    }

    // 캐스트 B 모션용
    void CastB()
    {
        // 검사 스켈레톤 소환

        // 느린 유도 공격
        animator.SetTrigger("doCastB");
    }

    // 캐스트 C 모션용
    void CastC()
    {
        // 직선 공격
        animator.SetTrigger("doCastA");
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
