using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public Item item;
    public Status status;
    public GameObject body;
    public GameObject dead;
    public GameObject spawnObject;

    public bool dontDaamge;

    protected bool isSpawn;
    protected bool isChase;
    protected bool isMove;
    protected bool isAttack = false;
    protected bool isDead;
    protected bool isHit = false;
    protected bool isAirBorned = false;

    protected float attackRange = 1.5f;
    protected float closeRange = 0.5f;

    protected Rigidbody rigidBody;
    protected CapsuleCollider capsuleCollider;
    protected MeshRenderer meshRenderer;
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;
    protected AnimatorStateInfo animatorStateInfo;

    Boss boss;

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        //float distance = Vector3.Distance(tr)
    }

    protected virtual void FixedUpdate()
    {
        bool isOverRange = Vector3.Distance(transform.position, target.position) > attackRange;
        bool isTooClose = Vector3.Distance(transform.position, target.position) < closeRange;
        if (status.CurrentHP > 0) Targeting(isOverRange);
        FreezeVelocity(isOverRange, isTooClose);
    }

    protected void FreezeVelocity(bool isOverRange, bool isTooClose)
    {
        if(isChase && !isOverRange || isTooClose)
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }
    }

    protected virtual void Targeting(bool isOverRange)
    {
        if (isOverRange && !isAttack)
        {
            return;
        }
        if (isAttack)
        {
            return;
        }
        if (isAirBorned)
        {
            return;
        }
        StartCoroutine(Attack());
    }

    protected virtual IEnumerator Attack()
    {
        isChase = false;
        isMove = false;
        animator.SetBool("isMove", false);
        isAttack = true;
        animator.SetTrigger("doAttack");
        yield return new WaitForSeconds(1.8f);
        isChase = true;
        isMove = true;
        animator.SetBool("isMove", true);
        isAttack = false;
    }

    protected virtual void OnDie()
    {
        if (status.CurrentHP > 0) return;
        capsuleCollider.enabled = false;
        Vector3 position = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);
        Instantiate(item, position, Quaternion.identity);
    }

    public void Spawn()
    {
        animator.SetTrigger("doSpawn");
        StartCoroutine(WaitForSpawn());
    }

    IEnumerator WaitForSpawn()
    {
        isSpawn = true;
        spawnObject.SetActive(true);
        rigidBody.useGravity = false;
        capsuleCollider.enabled = false;
        navMeshAgent.enabled = false;
        yield return new WaitForSeconds(1.5f);
        navMeshAgent.enabled = true;
        capsuleCollider.enabled = true;
        rigidBody.useGravity = true;
        isSpawn = false;
        Destroy(spawnObject);
    }
}
