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
    void Attack()
    {
        // ���� ĳ��Ʈ�� �̵�
        // ĳ��Ʈ A�� ������ ���
        // ĳ��Ʈ B�� ������ ���
        // ĳ��Ʈ C�� ������ ���
        // ���� Ÿ�̹� �ƴ� ���
    }

    // ĳ��Ʈ A ��ǿ�
    void CastA()
    {
        // �ü� ���̷��� ��ȯ

        // ���̷��� ����
        animator.SetTrigger("doCastA");
    }

    // ĳ��Ʈ B ��ǿ�
    void CastB()
    {
        // �˻� ���̷��� ��ȯ

        // ���� ���� ����
        animator.SetTrigger("doCastB");
    }

    // ĳ��Ʈ C ��ǿ�
    void CastC()
    {
        // ���� ����
        animator.SetTrigger("doCastA");
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
