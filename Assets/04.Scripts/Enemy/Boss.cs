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
        status = new Status(UnitCode.Boss, "����", 1);
        target = GameObject.FindWithTag("Player").transform;
        StartCoroutine(Phase1());
    }

    private void FixedUpdate()
    {
        // �÷��̾ �׻� �ٶ󺸰�
        transform.LookAt(target);
        EnemyDestoryCheck();
    }

    // ���� ������ 1
    IEnumerator Phase1()
    {
        enemyPrefabs = new GameObject[4];
        while (status.CurrentHP > 2000)
        {
            // �� ��ȯ
            if (summonCount > 0 && enemyCount == 0)
            {
                summonCount--;
                Attack('B', 0);
                yield return new WaitForSeconds(5.0f);
                bufReady = true;
            }
            // �� ����
            if (bufReady == true)
            {
                Attack('A', 1);
                bufReady = false;
            }
            yield return null;
        }
        // Phase2�� �Ѿ�� ���� �ܰ�
        phaseCount++;
        summonCount = 1;
        BossResize(phaseCount);
        StartCoroutine(Phase2());
    }

    // ���� ������ 2
    IEnumerator Phase2()
    {
        enemyPrefabs = new GameObject[6];
        capsuleCollider.enabled = true;
        StartCoroutine(CoolDown(directAttack_A_Ready, DirectAttack_A_cooldownTime));
        while (status.CurrentHP > 1000)
        {
            // �� ��ȯ
            if (summonCount > 0 && enemyCount == 0)
            {
                Attack('B', 0);
                Attack('A', 0);
            }
            // ���� ���� A
            if (directAttack_A_Ready == true)
            {
                Attack('B', 1);
                StartCoroutine(CoolDown(directAttack_A_Ready, DirectAttack_A_cooldownTime));
            }
            yield return null;
        }
        // Phase3�� �Ѿ�� ���� �ܰ�
        phaseCount++;
        capsuleCollider.enabled = false;
        BossResize(phaseCount);
        StartCoroutine(Phase3());
    }

    // ���� ������ 3
    IEnumerator Phase3()
    {
        if (status.CurrentHP != 1000) status.CurrentHP = 1000;

        capsuleCollider.enabled = true;
        StartCoroutine(CoolDown(directAttack_A_Ready, DirectAttack_A_cooldownTime));
        StartCoroutine(CoolDown(directAttack_B_Ready, DirectAttack_B_cooldownTime));
        while (phaseCount == 3)
        {
            // ���� ���� A
            if (directAttack_A_Ready == true)
            {
                Attack('B', 1);
                StartCoroutine(CoolDown(directAttack_A_Ready, DirectAttack_A_cooldownTime));
            }
            // ���� ���� B
            if (directAttack_B_Ready == true)
            {
                Attack('C', 1);
                StartCoroutine(CoolDown(directAttack_B_Ready, DirectAttack_B_cooldownTime));
            }
            yield return null;
        }
        // ���� ���
        capsuleCollider.enabled = false;
        OnDie();
    }

    // ���� �̵� �Լ�
    void Move()
    {
        // �����¿� ���� ����
    }

    // ���� ���� �Լ�
    void Attack(char castType, int castNum)
    {
        // ���� ĳ��Ʈ�� �̵�
        switch(castType)
        {
            // ĳ��Ʈ A�� ������ ���
            case 'A':
                CastA(castNum);
                break;
            // ĳ��Ʈ B�� ������ ���
            case 'B':
                CastB(castNum);
                break;
            // ĳ��Ʈ C�� ������ ���
            case 'C':
                CastC();
                break;
            // ���� Ÿ�̹� �ƴ� ���
            default:
                break;
        }
    }

    // ĳ��Ʈ A ��ǿ�
    void CastA(int castNum)
    {
        animator.SetTrigger("doCastA");
        // �ü� ���̷��� ��ȯ
        if (castNum == 0)
        {
            Summon(0);
        }
        // ���̷��� ����
        else
        {
            Buff();
        }
    }

    // ĳ��Ʈ B ��ǿ�
    void CastB(int castNum)
    {
        animator.SetTrigger("doCastB");
        // �˻� ���̷��� ��ȯ
        if (castNum == 0)
        {
            Summon(1);
        }
        // ���� ���� ����
        else
        {
            DirectAttack(0);
        }
    }

    // ĳ��Ʈ C ��ǿ�
    void CastC()
    {
        animator.SetTrigger("doCastC");
        // ���� ����
        DirectAttack(1);
    }

    // ���̷��� ��ȯ
    void Summon(int summonNum)
    {
        // �ü�
        if (summonNum == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject skeletonBow = Instantiate(bow, summonPoints[i].transform.position, transform.rotation);
                enemyPrefabs[i] = skeletonBow;
            }
            enemyCount += 2;
        }
        // ����
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

    // ���̷��� ����
    void Buff()
    {
        // ������
    }

    // ���� ���� ����
    void DirectAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            // ���� ����
            for(int i = 0; i < 5; i++)
            {
                // ���� ��ȯ
                GameObject projectileA = Instantiate(attackA, castBPoints[i].transform.position, transform.rotation);
            }
        }
        else
        {
            // ���� ����
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

    // ��Ÿ�� �Ŵ¿�
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
