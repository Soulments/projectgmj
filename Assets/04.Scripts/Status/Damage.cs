using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    PlayerController damagedPlayer;
    Enemy damagedEnemy;

    public Damage(float damagePercent, GameObject otherObject)
    {
        if (otherObject.CompareTag("Player"))
        {
            damagedPlayer = otherObject.transform.parent.gameObject.GetComponent<PlayerController>();

            damagedPlayer.status.CurrentHP -= (int)damagePercent;
        }
        else
        {
            damagedEnemy = otherObject.GetComponent<Enemy>();

            if (damagedEnemy.status.CurrentHP < 0) return;

            damagedEnemy.status.CurrentHP -= (int)damagePercent;
        }
    }
}
