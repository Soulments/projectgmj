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
    public int maxHP = 40;
    public int currentHP;

    public float DirectAttack_A_cooldownTime = 5.0f;
    public float DirectAttack_B_cooldownTime = 20.0f;


    int phaseCount = 1;
    int enemyCount = 0;

    bool bufReady = false;
    bool directAttack_A_Ready = false;
    bool directAttack_B_Ready = false;

    Status status;
    Vector3 currentPosition;
    Vector3[] range = new Vector3[4];
    EnemySkeletonSword sword;
    EnemySkeletonBow bow;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        status = new Status();
        target = GameObject.FindWithTag("Player").transform;
        currentHP = maxHP;
        StartCoroutine(Phase1());
    }

    private void FixedUpdate()
    {
        SummonPosition();
        // �÷��̾ �׻� �ٶ󺸰�
        transform.LookAt(target);
    }

    // ���� ������ 1
    IEnumerator Phase1()
    {
        while (currentHP > 32)
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
        capsuleCollider.enabled = true;
        StartCoroutine(CoolDown(directAttack_A_Ready, DirectAttack_A_cooldownTime));
        while (currentHP > 7)
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
        animator.SetTrigger("doCastA");
        // ���� ����
        DirectAttack(1);
    }

    // ��ȯ ��ġ ������
    void SummonPosition()
    {
        currentPosition = transform.position;
        for (int i = 0; i < 4; i++)
        {
            range[i] = currentPosition;
        }
        range[0].x += 2;
        range[1].x -= 2;
        range[2].z += 2;
        range[3].z -= 2;
    }

    // ���̷��� ��ȯ
    void Summon(int summonNum)
    {
        // �ü�
        if (summonNum == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                EnemySkeletonBow skeletonBow = Instantiate(bow, range[i], transform.rotation);
            }
            enemyCount += 2;
        }
        // ����
        else
        {
            for (int i = 0; i < 4; i++)
            {
                EnemySkeletonSword skeletonSword = Instantiate(sword, range[i], transform.rotation);
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
            }
        }
        else
        {
            // ���� ����
        }
    }

    IEnumerator BossResize(int phaseCount)
    {
        while (transform.localScale.x > phaseCount * 2)
        {
            transform.localScale *= 0.9f;
        }
        transform.localScale = new Vector3(phaseCount * 2, phaseCount * 2, phaseCount * 2);
        yield return null;
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
