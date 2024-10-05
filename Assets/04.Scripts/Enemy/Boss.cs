using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Boss : Enemy
{
    enum Skeleoton : int
    {
        SkeletonSword,
        SkeletonBow
    }

    public int summonCount;

    public float DirectAttack_A_cooldownTime = 5.0f;
    public float DirectAttack_B_cooldownTime = 20.0f;

    public GameObject sword;
    public GameObject bow;
    public GameObject attackA;
    public GameObject attackB;
    public GameObject castCPoint;
    public GameObject[] castBPoints;
    public GameObject[] enemyPrefabs;
    public GameObject[] summonPoints;

    int phaseCount = 1;
    int enemyCount = 0;
    int checkenemyCount = 0;

    bool bufReady = false;
    bool directAttack_A_Ready = false;
    bool directAttack_B_Ready = false;

    Vector3 currentPosition;
    Vector3[] range = new Vector3[4];

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        status = new Status(UnitCode.Boss, "보스", 1);
        target = GameObject.FindWithTag("Player").transform;
        StartCoroutine(Phase1());
    }

    private void FixedUpdate()
    {
        // 플레이어를 항상 바라보게
        transform.LookAt(target);
        EnemyDestoryCheck();
    }

    // 보스 페이즈 1
    IEnumerator Phase1()
    {
        enemyPrefabs = new GameObject[4];
        while (status.CurrentHP > 2000)
        {
            // 몹 소환
            if (summonCount > 0 && enemyCount == 0)
            {
                summonCount--;
                Attack('B', 0);
                yield return new WaitForSeconds(5.0f);
                bufReady = true;
            }
            // 몹 버프
            if (bufReady == true)
            {
                Attack('A', 1);
                bufReady = false;
            }
            yield return null;
        }
        // Phase2로 넘어가기 위한 단계
        phaseCount++;
        summonCount = 1;
        BossResize(phaseCount);
        StartCoroutine(Phase2());
    }

    // 보스 페이즈 2
    IEnumerator Phase2()
    {
        enemyPrefabs = new GameObject[6];
        capsuleCollider.enabled = true;
        StartCoroutine(CoolDown(directAttack_A_Ready, DirectAttack_A_cooldownTime));
        while (status.CurrentHP > 1000)
        {
            // 몹 소환
            if (summonCount > 0 && enemyCount == 0)
            {
                Attack('B', 0);
                Attack('A', 0);
            }
            // 직접 공격 A
            if (directAttack_A_Ready == true)
            {
                Attack('B', 1);
                StartCoroutine(CoolDown(directAttack_A_Ready, DirectAttack_A_cooldownTime));
            }
            yield return null;
        }
        // Phase3로 넘어가기 위한 단계
        phaseCount++;
        capsuleCollider.enabled = false;
        BossResize(phaseCount);
        StartCoroutine(Phase3());
    }

    // 보스 페이즈 3
    IEnumerator Phase3()
    {
        if (status.CurrentHP != 1000) status.CurrentHP = 1000;

        capsuleCollider.enabled = true;
        StartCoroutine(CoolDown(directAttack_A_Ready, DirectAttack_A_cooldownTime));
        StartCoroutine(CoolDown(directAttack_B_Ready, DirectAttack_B_cooldownTime));
        while (phaseCount == 3)
        {
            // 직접 공격 A
            if (directAttack_A_Ready == true)
            {
                Attack('B', 1);
                StartCoroutine(CoolDown(directAttack_A_Ready, DirectAttack_A_cooldownTime));
            }
            // 직접 공격 B
            if (directAttack_B_Ready == true)
            {
                Attack('C', 1);
                StartCoroutine(CoolDown(directAttack_B_Ready, DirectAttack_B_cooldownTime));
            }
            yield return null;
        }
        // 보스 사망
        capsuleCollider.enabled = false;
        OnDie();
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
        animator.SetTrigger("doCastC");
        // 직선 공격
        DirectAttack(1);
    }

    // 스켈레톤 소환
    void Summon(int summonNum)
    {
        // 궁수
        if (summonNum == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject skeletonBow = Instantiate(bow, summonPoints[i].transform.position, transform.rotation);
                enemyPrefabs[i] = skeletonBow;
            }
            enemyCount += 2;
        }
        // 전사
        else
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject skeletonSword = Instantiate(sword, summonPoints[i].transform.position, transform.rotation);
                enemyPrefabs[i] = skeletonSword;
            }
            enemyCount += 4;
        }
    }

    // 스켈레톤 버프
    void Buff()
    {
        // 뭐넣지
    }

    // 보스 직접 공격
    void DirectAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            // 느린 공격
            for(int i = 0; i < 5; i++)
            {
                // 공격 소환
                GameObject projectileA = Instantiate(attackA, castBPoints[i].transform.position, transform.rotation);
            }
        }
        else
        {
            // 빠른 공격
            GameObject projectileB = Instantiate(attackB, castCPoint.transform.position, transform.rotation);
        }
    }

    void EnemyDestoryCheck()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i] != null)
            {
                checkenemyCount++;
            }
        }
        if (checkenemyCount != enemyCount)
        {
            if (phaseCount == 1)
            {
                status.CurrentHP -= (enemyCount - checkenemyCount) * 125;
            }
            else
            {
                status.CurrentHP -= (enemyCount - checkenemyCount) * 100;
            }
            enemyCount = checkenemyCount;
        }
        checkenemyCount = 0;
    }

    void BossResize(int phaseCount)
    {
        while (transform.localScale.x > (5 - phaseCount))
        {
            transform.localScale *= 0.9f;
        }
        transform.localScale = new Vector3((5 - phaseCount), (5 - phaseCount), (5 - phaseCount));
        int height;
        if (phaseCount == 2) height = 2;
        else height = 0;
        while (transform.position.y > height)
        {
            transform.position -= new Vector3(0, 0.1f, 0);
        }
        StartCoroutine(Wait());
    }

    // 쿨타임 거는용
    IEnumerator CoolDown(bool ready, float cooldownTime)
    {
        ready = false;
        yield return new WaitForSeconds(cooldownTime);
        ready = true;
    }

    protected override void OnDie()
    {
        animator.SetTrigger("doDie");
        StartCoroutine(Wait());
        Destroy(gameObject);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
    }
}
