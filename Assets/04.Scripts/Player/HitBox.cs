using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float SkillPercent;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enmey" || other.gameObject.tag == "Boss")
        {
            _ = new Damage(SkillPercent, other.gameObject);
        }
    }
}