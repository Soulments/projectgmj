using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{

    // �÷��̾ �Դ� ������
    public Damage(float damagePercent, PlayerController damagedPlayer)
    {
        damagedPlayer.status.CurrentHP -= (int)damagePercent;
    }

    // ���� �Դ� ������
    public Damage(float damagePercent, Enemy damagedEnemy)
    {
        damagedEnemy.status.CurrentHP -= (int)damagePercent;
    }
}
