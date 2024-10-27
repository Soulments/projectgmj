using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    //Transform makePos;

    //bool bStartCheck = false;

    //public float Angle = 45.0f;
    //public float Power = 0.01f;

    //float TimeDirection = Time.deltaTime;
    //float Gravity;

    //Vector3 v1;

    //public void SetArrowCheck(bool check, Vector3 pos, Quaternion q)
    //{
    //    bStartCheck = check;
    //    transform.position = pos;
    //    transform.rotation = q;
    //}

    //public bool GetArrowCheck() { return bStartCheck; }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    //makePos = GameObject.Find("").transform;
    //    Gravity = -(1.0f * TimeDirection * TimeDirection / 2.0f);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (bStartCheck == false) return;
    //    TimeDirection += Time.deltaTime;
    //    v1.z = Power = Mathf.Cos(Angle * Mathf.PI / 180.0f) * TimeDirection;
    //    v1.y = Power = Mathf.Cos(Angle * Mathf.PI / 180.0f) * TimeDirection * Gravity;
    //    transform.Translate(v1);

    //    transform.Rotate(new Vector3(Mathf.Cos(Angle * Mathf.PI / 180.0f), 0, 0));
    //}
    EnemySkeletonBow summoner;

    public void SetSummoner(EnemySkeletonBow bow)
    {
        summoner = bow;
    }

    private void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, 3);
    }

    private void Update()
    {
        GetComponent<Rigidbody>().transform.up = -GetComponent<Rigidbody>().velocity;
    }

    // 화살 맞았을 때
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Floor")
        {
            if (other.gameObject.tag == "Player")
            {
                // 데미지 주는 기능
                _ = new Damage(summoner.status.AttackDamage, other.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
