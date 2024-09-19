using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform playerBody;
    [SerializeField]
    private Transform cameraArm;

    int hitCount = 0;

    float horizontalAxis;
    float verticalAxis;
    float currentTime;

    bool jumpDown;
    bool mouseLeft;
    bool mouseRight;
    bool bufSkill;
    bool cooldownbufSkill;
    bool upperSkill;
    bool cooldownupperSkill;
    bool[] windmillSkill = new bool[2];
    bool cooldownwindmillSkill;

    bool isMove;
    bool isAttack;
    bool isAttack3;
    bool isJump;
    bool ishit;
    bool isGrounded;
    bool isEnhanced;
    bool usingPortal;
    bool comboTrigger;
    public bool isDead = false;

    Vector2 moveInput;
    Vector3 lookForward;
    Vector3 lookRight;
    Vector3 moveDirection;

    Animator animator;
    Rigidbody rigidbody;

    public Collider[] weaponArea;
    public GameManager manager;
    public LayerMask groundLayer;
    public GameObject[] startPortals;
    public GameObject[] endPortals;
    public Material[] materials;
    public SkillControl[] skillControls = new SkillControl[4];

    public float jumpForce = 30000;
    public float speed = 10;
    private int comboIndex;

    [SerializeField]
    public Inventory inventory;
    [SerializeField]
    private CanvasGroup inventoryCanvasGroup;
    private bool isInventoryOpen = false; // 인벤토리 활성화 상태

    // 아이템 메세지 관련 변수-------------
    private IObjectItem itemPickup = null;

    public UIController uiController;
    public TextMeshProUGUI itemName;
    // -----------------------------------

    // Start is called before the first frame update
    void Start()
    {
        animator = playerBody.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        LookAround();
        Move();
        Jump();
        //GroundCheck();
        ActionPlayer();
        End_Attack();
        //Weapon();
    }

    private void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, transform.GetChild(0).position, Time.deltaTime);
    }

    private void GetInput()
    {
        // Input값 정리
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        jumpDown = Input.GetButtonDown("Jump");
        mouseLeft = Input.GetMouseButtonDown(0);
        mouseRight = Input.GetMouseButtonDown(1);

        // 인벤토리 on/off----------------
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        // ------------------------------
        // 아이템 획득-------------------------------------------
        if (itemPickup != null && Input.GetKeyDown(KeyCode.G))
        {
            ItemData item = itemPickup.ClickItem();

            inventory.AddItem(item);
            itemPickup.OnPickup(); // 아이템 획득하면 오브젝트 파괴
            Debug.Log($"{item.itemName}");

            uiController.CloseMessagePanel();
        }
        // ------------------------------------------------------

        if (!cooldownupperSkill) upperSkill = Input.GetKeyDown(KeyCode.E);
        if (!cooldownwindmillSkill)
        {
            windmillSkill[0] = Input.GetKeyDown(KeyCode.R);
            windmillSkill[1] = Input.GetKeyUp(KeyCode.R);
        }
        if (!cooldownbufSkill) bufSkill = Input.GetKeyDown(KeyCode.F);
    }
    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryCanvasGroup.alpha = isInventoryOpen ? 1 : 0;
        inventoryCanvasGroup.interactable = isInventoryOpen; // UI 상호작용 가능 여부 설정
        inventoryCanvasGroup.blocksRaycasts = isInventoryOpen; // UI 클릭 가능 여부 설정
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
        if (ishit) moveDirection = Vector3.zero;
        else if (isMove && !isAttack && !usingPortal)
        {
            lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            moveDirection = lookForward * moveInput.y + lookRight * moveInput.x;

            if (!isAttack3) playerBody.forward = moveDirection;
            if (!isAttack || !ishit) transform.position += moveDirection * Time.deltaTime * speed;
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

    //private void GroundCheck()
    //{
    //    isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, LayerMask.GetMask("Floor"));
    //    if (isGrounded)
    //    {
    //        animator.SetBool("isJump", false);
    //        isJump = false;
    //    }
    //}

    private void ActionPlayer()
    {
        if (!isJump && mouseLeft)
        {
            NormalAttack();
        }
        if (isJump && mouseLeft)
        {
            JumpAttack();
        }
        if (!isJump && upperSkill)
        {
            SkillUpper();
        }
        if (!isJump && windmillSkill[0])
        {
            SkiilWindmill();
        }
        if (!isJump && windmillSkill[1])
        {
            SkillWindmillStop();
        }
        if (!isJump && bufSkill)
        {
            SkillBuf();
        }

    }

    private void NormalAttack()
    {
        Debug.Log(comboIndex);
        Debug.Log(isAttack);
        if (comboIndex == 0)
        {
            isAttack = true;
            animator.SetTrigger("doAttack1a");
            comboIndex = 1;
        }
        else if (comboIndex == 1 && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1a"))
        {
            isAttack = true;
            comboTrigger = true;
            animator.SetTrigger("doAttack1b");
            comboIndex = 2;
        }
        else if (comboIndex == 2 && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1b"))
        {
            isAttack = true;
            animator.SetTrigger("doAttack1c");
            comboIndex = 3;
        }
        weaponArea[0].enabled = true;
    }

    private void JumpAttack()
    {
        isAttack = true;
        animator.SetTrigger("doAttack2");
        rigidbody.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
        StartCoroutine(Wait(1));
    }

    private void SkillUpper()
    {
        isAttack = true;
        animator.SetTrigger("doAttack5");
        StartCoroutine(UpperSkill());
        CoolTimeTrigger(1);
        cooldownupperSkill = true;
        StartCoroutine(WaitForCooltime(skillControls[1].GetComponent<SkillControl>().coolTime));
        cooldownupperSkill = false;
    }

    IEnumerator UpperSkill()
    {
        yield return new WaitForSeconds(1f);
        weaponArea[2].enabled = true;
        yield return new WaitForSeconds(1.5f);
        weaponArea[2].enabled = false;
    }

    private void SkiilWindmill()
    {
        isAttack3 = true;
        animator.SetTrigger("doAttack3");
        StartCoroutine(WindmillReady());
        StartCoroutine(Windmill());
    }

    IEnumerator WindmillReady()
    {
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Windmill()
    {
        WaitForSeconds waitWindmil = new WaitForSeconds(0.5f);
        while (true)
        {
            if (!isAttack3) break;
            weaponArea[1].enabled = true;
            yield return waitWindmil;
            weaponArea[1].enabled = false;
        }
    }

    private void SkillWindmillStop()
    {
        isAttack3 = false;
        animator.SetTrigger("stopAttack3");
        weaponArea[1].enabled = false;
        CoolTimeTrigger(2);
        cooldownwindmillSkill = true;
        StartCoroutine(WaitForCooltime(skillControls[2].GetComponent<SkillControl>().coolTime));
        cooldownwindmillSkill = false;
    }

    private void SkillBuf()
    {
        isAttack = true;
        animator.SetTrigger("doAttack4");
        CoolTimeTrigger(3);
        cooldownbufSkill = true;
        StartCoroutine(WaitForCooltime(skillControls[3].GetComponent<SkillControl>().coolTime));
        cooldownbufSkill = false;
    }

    private void End_Attack()
    {
        if (comboTrigger == true) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1a") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1b") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1c"))
        {
            Debug.Log("isAttackDisable");
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                isAttack = false;
                comboIndex = 0;
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack4a") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack5a"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                isAttack = false;
            }
        }
    }

    private void CoolTimeTrigger(int num)
    {
        skillControls[num].GetComponent<SkillControl>().isUseSkill = true;
        skillControls[num].GetComponent<SkillControl>().StartCooltime();
    }

    private void OnHit()
    {
        if (hitCount > 3)
        {
            StartCoroutine(Enhance());
        }
        else if (!isAttack3)
        {
            animator.SetTrigger("doHit");
            hitCount++;
        }
    }

    IEnumerator Enhance()
    {
        isEnhanced = true;
        yield return new WaitForSeconds(5.0f);
        isEnhanced = false;
        hitCount = 0;
    }

    IEnumerator Hit()
    {
        ishit = true;
        yield return new WaitForSeconds(1.0f);
        ishit = false;
    }

    private void OnDie()
    {
        isDead = true;
        animator.SetTrigger("doDie");
        animator.SetBool("isDead", isDead);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool("isJump", false);
            isJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arrow" || other.gameObject.tag == "Weapon")
        {
            if (!isEnhanced)
            {
                OnHit();
            }
            return;
        }
        int i = 0;
        foreach (GameObject gameObject in startPortals)
        {
            if (other.gameObject == gameObject)
            {
                StartCoroutine(PortalMove(i));
            }
            i++;
        }
        
        // 아이템 획득 관련 코드---------------------------------------
        IObjectItem clickInterface = other.GetComponent<IObjectItem>();
        if (clickInterface != null)
        {
            ItemData item = clickInterface.ClickItem();

            uiController.OpenMessagePanel("");
            itemName.text = item.itemName;

            itemPickup = clickInterface;
            /*
            inventory.AddItem(item);
            clickInterface.OnPickup(); // 아이템 획득하면 오브젝트 파괴
            Debug.Log($"{item.itemName}");
            */
        }
        // -----------------------------------------------------------

    }
    private void OnTriggerExit(Collider other)
    {
        IObjectItem clickInterface = other.GetComponent<IObjectItem>();
        if (clickInterface != null)
        {
            uiController.CloseMessagePanel();
            itemPickup = null;
        }
    }
    IEnumerator PortalMove(int i)
    {
        usingPortal = true;
        manager.portalTrigger = true;
        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector3(endPortals[i].transform.position.x, 0, endPortals[i].transform.position.z);
        yield return new WaitForSeconds(1.5f);
        usingPortal = false;
    }

    IEnumerator Wait(int attackNum)
    {
        int hitbox = Convert.ToInt32(Convert.ToBoolean(attackNum));
        if (attackNum != 1)
            yield return new WaitForSeconds(0.3f);
        weaponArea[hitbox].enabled = true;
        yield return new WaitForSeconds(0.5f);
        weaponArea[hitbox].enabled = false;
        isAttack = false;
    }

    IEnumerator WaitForCooltime(float coolTime)
    {
        yield return new WaitForSecondsRealtime(coolTime);
    }
}
