using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Boss : Enemy
{
    enum Skeleoton : int
    {
        SkeletonSword,
        SkeletonBow
    }

    public int summonCount;
    
    int waveCount = 3;
    int enemyCount = 0;

    Vector3 currentPosition;
    Vector3[] range = new Vector3[4];
    Animator animator;
    EnemySkeletonSword sword;
    EnemySkeletonBow bow;
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        SummonPosition();
        transform.LookAt(target);
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
    void Attack(char castType, int castNum)
    {
        // 각종 캐스트로 이동
        switch(castType)
        {
            // 캐스트 A로 가야할 경우
            case 'A':
                CastA(castNum);
                break;
            // 캐스트 B로 가야할 경우
            case 'B':
                CastB(castNum);
                break;
            // 캐스트 C로 가야할 경우
            case 'C':
                CastC();
                break;
            // 공격 타이밍 아닐 경우
            default:
                break;
        }
    }

    // 캐스트 A 모션용
    void CastA(int castNum)
    {
        animator.SetTrigger("doCastA");
        // 궁수 스켈레톤 소환
        if (castNum == 0)
        {
            Summon(0);
        }
        // 스켈레톤 버프
        else
        {
            Buff();
        }
    }

    // 캐스트 B 모션용
    void CastB(int castNum)
    {
        animator.SetTrigger("doCastB");
        // 검사 스켈레톤 소환
        if (castNum == 0)
        {
            Summon(1);
        }
        // 느린 유도 공격
        else
        {
            DirectAttack(0);
        }
    }

    // 캐스트 C 모션용
    void CastC()
    {
        animator.SetTrigger("doCastA");
        // 직선 공격
        DirectAttack(1);
    }

    // 소환 위치 참조용
    void SummonPosition()
    {
        currentPosition = transform.position;
        for (int i = 0; i < 4; i++)
        {
            range[i] = currentPosition;
        }
        range[0].x += 2;
        range[1].y -= 2;
        range[2].z += 2;
        range[3].z -= 2;
    }

    // 스켈레톤 소환
    void Summon(int summonNum)
    {
        // 궁수
        if (summonNum == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                EnemySkeletonBow skeletonBow = Instantiate(bow, range[i], transform.rotation);
            }
            enemyCount += 2;
        }
        // 전사
        else
        {
            for (int i = 0; i < 4; i++)
            {
                EnemySkeletonSword skeletonSword = Instantiate(sword, range[i], transform.rotation);
            }
            enemyCount += 4;
        }
    }

    // 스켈레톤 버프
    void Buff()
    {

    }

    // 보스 직접 공격
    void DirectAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            // 느린 공격
        }
        else
        {
            // 빠른 공격
        }
    }

    IEnumerator BossResize(int waveCount)
    {
        while (transform.localScale.x > waveCount * 2)
        {
            transform.localScale *= 0.9f;
        }
        transform.localScale = new Vector3(waveCount * 2, waveCount * 2, waveCount * 2);
        yield return null;
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
