using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform playerBody;
    [SerializeField]
    private Transform cameraArm;
    [SerializeField]
    //private GameObject[] BigSword;

    float horizontalAxis;
    float verticalAxis;

    bool jumpDown;
    bool mouseLeft;
    bool mouseRight;

    bool isMove;
    bool isAttack;
    bool isJump;
    bool usingPortal;
    public bool isDead = false;

    Vector2 moveInput;
    Vector3 lookForward;
    Vector3 lookRight;
    Vector3 moveDirection;

    Animator animator;
    Rigidbody rigidbody;
    public BoxCollider weaponArea;

    public GameObject[] startPortals;
    public GameObject[] endPortals;
    public Material[] materials;

    public float jumpForce = 30000;
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        animator = playerBody.GetComponent<Animator>();
        rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        LookAround();
        Move();
        Jump();
        Attack();
        Skill();
    }

    private void GetInput()
    {
        // Input값 정리
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        jumpDown = Input.GetButtonDown("Jump");
        mouseLeft = Input.GetMouseButtonDown(0);
        mouseRight = Input.GetMouseButtonDown(1);
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 cameraAngle = cameraArm.rotation.eulerAngles;
        float x = cameraAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);
    }

    private void Move()
    {
        // 플레이어 이동 값 가져오기
        moveInput = new Vector2(horizontalAxis, verticalAxis);
        // 이동 수평 값 확인
        isMove = moveInput.magnitude != 0;
        if (usingPortal) animator.SetBool("isMove", false);
        else animator.SetBool("isMove", isMove);
        // isMove 값 true일 때 이동
        if (isMove && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !usingPortal)
        {
            lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            moveDirection = lookForward * moveInput.y + lookRight * moveInput.x;

            playerBody.forward = moveDirection;
            if (!isAttack) transform.position += moveDirection * Time.deltaTime * speed;
            else moveDirection = Vector3.zero;
        }
        
    }

    private void Jump()
    {
        if (jumpDown && !isJump && !isAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isJump", true);
            animator.SetTrigger("doJump");
            isJump = true;
        }
    }

    private void Attack()
    {
        if (mouseLeft)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                animator.SetTrigger("doAttack2");
            }
            else
            {
                isAttack = true;
                animator.SetTrigger("doAttack1");
                StartCoroutine(Wait());
            }
        }
    }

    private void Skill()
    {
        if(mouseRight)
        {
            animator.SetTrigger("doSkill1");
            rigidbody.velocity = moveDirection * 2f;
            //transform.position += moveDirection * Time.deltaTime * speed * 1.3f;
        }
    }

    private void OnHit()
    {

    }

    private void OnDie()
    {
        isDead = true;
        animator.SetTrigger("doDie");
        animator.SetBool("isDead", isDead);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            animator.SetBool("isJump", false);
            isJump = false;
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        int i = 0;
        foreach(GameObject gameObject in startPortals)
        {
            if (other.gameObject == gameObject)
            {
                StartCoroutine(PortalMove(i));
            }
            i++;
        }
        
    }

    IEnumerator PortalMove(int i)
    {
        usingPortal = true; yield return new WaitForSeconds(1.5f);
        transform.position = new Vector3(endPortals[i].transform.position.x, 0, endPortals[i].transform.position.z);
        yield return new WaitForSeconds(1.5f);
        usingPortal = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.6f);
        weaponArea.enabled = true;
        yield return new WaitForSeconds(1.0f);
        weaponArea.enabled = false;
        isAttack = false;
    }
}
