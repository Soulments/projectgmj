using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public char projectileType;
    public float speed;
    Transform target = null;
    Rigidbody rigidBody;
    Boss boss;

    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        rigidBody = GetComponent<Rigidbody>();
        boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();
        transform.LookAt(target);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (projectileType == 'A')
        {
            StartCoroutine(DestroyProjectile());
        }
        if (projectileType == 'B')
        {
            Shoot();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (projectileType == 'A')
        {
            Chase();
        }
    }

    void Chase()
    {
        if (target != null)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            Vector3 directionVec = (target.position - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, directionVec, 0.25f);
        }
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(gameObject);
    }

    void Shoot()
    {
        if (target != null)
        {
            // 타겟을 향해 발사하기 위한 방향 설정
            Vector3 shootDirection = (target.position - transform.position).normalized;

            // Rigidbody에 velocity를 주어 발사
            rigidBody.velocity = shootDirection * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Floor")
        {
            // 데미지 주는 기능
            if (projectileType == 'A')
            {
                _ = new Damage(boss.status.AttackDamage, other.gameObject);
            }
            else
            {
                _ = new Damage(boss.status.AttackDamage * 3, other.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
