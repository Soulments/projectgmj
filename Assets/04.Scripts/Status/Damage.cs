using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{

    // 플레이어가 입는 데미지
    public Damage(float damagePercent, PlayerController damagedPlayer)
    {
        damagedPlayer.status.CurrentHP -= (int)damagePercent;
    }

    // 적이 입는 데미지
    public Damage(float damagePercent, Enemy damagedEnemy)
    {
        damagedEnemy.status.CurrentHP -= (int)damagePercent;
    }
}
