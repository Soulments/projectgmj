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

    // ���� ���Ͽ� �Լ�
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
        range[1].y -= 2;
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

    }

    // ���� ���� ����
    void DirectAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            // ���� ����
        }
        else
        {
            // ���� ����
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
