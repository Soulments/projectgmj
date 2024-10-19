using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordGust : MonoBehaviour
{
    public float skillPercent;

    void Start()
    {
        StartCoroutine(CoroutineDestory());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            _ = new Damage(skillPercent, other.gameObject);
        }
    }

    IEnumerator CoroutineDestory()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}
