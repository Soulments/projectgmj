using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float skillPercent;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
        {
            _ = new Damage(skillPercent, other.gameObject);
            Debug.Log("damage ok");
        }
    }
}
