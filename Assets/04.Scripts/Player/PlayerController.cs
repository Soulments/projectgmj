using System;
using System.Collections;
using TMPro;
using UnityEditor;
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
    bool swordGustSkill;
    bool cooldownswordGustSkill;
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
    Rigidbody rigidBody;
    CapsuleCollider capsuleCollider;

    public Collider normalAttack;
    public Collider jumpAttack;
    public Collider[] skillAttack = new Collider[4];
    public GameManager manager;
    public LayerMask groundLayer;
    public GameObject[] startPortals;
    public GameObject[] endPortals;
    public Material[] materials;
    public SkillControl[] skillControls = new SkillControl[4];
    public Status status;
    public GameObject swordGust;

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
    public TextMeshProUGUI itemStatus;
    // -----------------------------------

    void Awake()
    {
        status = new Status(UnitCode.Player, "플레이어", GameObject.Find("GameManager").GetComponent<GameManager>().stageCount);
        HitBoxDamage();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = playerBody.GetComponent<Animator>();
        rigidBody = playerBody.GetComponent<Rigidbody>();
        capsuleCollider = playerBody.GetComponent<CapsuleCollider>();
        SkillControlAttach();
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
        ComboAttack();
        EndAttack();
        //Weapon();
        OnDie();
    }

    private void FixedUpdate()
    {
        if (!isAttack)
            playerBody.transform.position = Vector3.Lerp(playerBody.transform.position, transform.position, 0.5f);
        //playerBody.transform.position = Vector3.Lerp(playerBody.transform.position, transform.position, 0.1f);
    }

    // 스킬 아이콘 연결
    void SkillControlAttach()
    {
        skillControls[0] = GameObject.Find("Player Canvas/Player Panel/Skill Group/Skill Q").GetComponent<SkillControl>();
        skillControls[1] = GameObject.Find("Player Canvas/Player Panel/Skill Group/Skill E").GetComponent<SkillControl>();
        skillControls[2] = GameObject.Find("Player Canvas/Player Panel/Skill Group/Skill R").GetComponent<SkillControl>();
        skillControls[3] = GameObject.Find("Player Canvas/Player Panel/Skill Group/Skill TAB").GetComponent<SkillControl>();
    }

    // 공격별 데미지 설정
    void HitBoxDamage()
    {
        normalAttack.GetComponent<HitBox>().skillPercent = status.AttackDamage;
        jumpAttack.GetComponent<HitBox>().skillPercent = status.SkillPercent[(int)SkillCode.Jump];
        

        skillAttack[(int)SkillCode.SwordGust].GetComponent<HitBox>().skillPercent = status.SkillPercent[(int)SkillCode.SwordGust];
        skillAttack[(int)SkillCode.Upper].GetComponent<HitBox>().skillPercent = status.SkillPercent[(int)SkillCode.Upper];
        skillAttack[(int)SkillCode.Windmill].GetComponent<HitBox>().skillPercent = status.SkillPercent[(int)SkillCode.Windmill];

        swordGust.GetComponent<SwordGust>().skillPercent = (int)(status.SkillPercent[(int)SkillCode.SwordGust] * 0.75);
    }
    

    // Input값 정리
    private void GetInput()
    {
        horizontalAxis = Input.GetAxis("Horizontal") * (!isInventoryOpen ? 1 : 0);
        verticalAxis = Input.GetAxis("Vertical") * (!isInventoryOpen ? 1 : 0);
        jumpDown = Input.GetButtonDown("Jump") && !isInventoryOpen;
        mouseLeft = Input.GetMouseButtonDown(0) && !isInventoryOpen;
        mouseRight = Input.GetMouseButtonDown(1) && !isInventoryOpen;

        // 인벤토리 on/off----------------
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        // ------------------------------
        // 아이템 획득-------------------------------------------
        if (itemPickup != null && Input.GetKeyDown(KeyCode.F) && !isInventoryOpen)
        {
            ItemData item = itemPickup.ClickItem();
            Status itemStatus = itemPickup.GetStatus();

            inventory.AddItem(item, itemStatus);

            itemPickup.OnPickup(); // 아이템 획득하면 오브젝트 파괴
            itemPickup = null;

            Debug.Log($"{item.itemName}");

            uiController.CloseMessagePanel();
        }
        // ------------------------------------------------------

        swordGustSkill = Input.GetKeyDown(KeyCode.Q) && !cooldownswordGustSkill && !isInventoryOpen;
        upperSkill = Input.GetKeyDown(KeyCode.E) && !cooldownupperSkill && !isInventoryOpen;
        windmillSkill[0] = Input.GetKeyDown(KeyCode.R) && !cooldownwindmillSkill && !isInventoryOpen;
        windmillSkill[1] = Input.GetKeyUp(KeyCode.R) && !cooldownwindmillSkill && !isInventoryOpen;
        bufSkill = Input.GetKeyDown(KeyCode.Tab) && !cooldownbufSkill && !isInventoryOpen;
    }
    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryCanvasGroup.alpha = isInventoryOpen ? 1 : 0;
        inventoryCanvasGroup.interactable = isInventoryOpen; // UI 상호작용 가능 여부 설정
        inventoryCanvasGroup.blocksRaycasts = isInventoryOpen; // UI 클릭 가능 여부 설정
    }

    // 화면 회전
    private void LookAround()
    {
        if (!isInventoryOpen)
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
    }

    // 이동
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

    // 점프
    private void Jump()
    {
        if (jumpDown && !isJump && !isAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

    // 공격 액션
    private void ActionPlayer()
    {
        if (!isJump && mouseLeft)
        {
            if (!isAttack && comboIndex == 0) NormalAttack();
            else ComboCheck();
        }
        if (isJump && mouseLeft)
        {
            JumpAttack();
        }
        if (!isJump && swordGustSkill)
        {
            SkillSwordGust();
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

    // 일반공격
    private void NormalAttack()
    {
        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
        if (!isAttack)
        {
            isAttack = true;
            animator.SetTrigger("doAttack1a");
            comboIndex = 1;
            normalAttack.enabled = true;
        }
    }

    void ComboCheck()
    {
        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimation.IsName("Attack1a"))
        {
            if (currentAnimation.normalizedTime > 0.5f && currentAnimation.normalizedTime < 0.99f)
            {
                comboTrigger = true;
            }
        }
        if (currentAnimation.IsName("Attack1b"))
        {
            if (currentAnimation.normalizedTime > 0.5f && currentAnimation.normalizedTime < 0.99f)
            {
                comboTrigger = true;
            }
        }
    }

    void ComboAttack()
    {
        if (!comboTrigger) return;

        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimation.normalizedTime >= 0.99f)
        {
            if (currentAnimation.IsName("Attack1a"))
            {
                animator.SetTrigger("doAttack1b");
                comboIndex = 2;
            }
            else if (currentAnimation.IsName("Attack1b"))
            {
                animator.SetTrigger("doAttack1c");
                comboIndex = 3;
            }
            isAttack = true;
            StartCoroutine(WaitForCombo());
        }
    }

    IEnumerator WaitForCombo()
    {
        yield return new WaitForSeconds(0.1f);

        comboTrigger = false;
    }

    // 점프공격
    private void JumpAttack()
    {
        isAttack = true;
        animator.SetTrigger("doAttack2");
        rigidBody.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
        StartCoroutine(WaitForJumpAttack(1));
    }

    // 스킬 1번 버프
    private void SkillBuf()
    {
        isAttack = true;
        animator.SetTrigger("doAttack4");
        CoolTimeTrigger(3);
        StartCoroutine(WaitForCooltime(skillControls[3].GetComponent<SkillControl>().coolTime, 3));
        StartCoroutine(CoroutineBuf());
    }

    IEnumerator CoroutineBuf()
    {
        yield return new WaitForSeconds(5.0f);
    }

    // 스킬 2번 올려치기 + 검풍
    private void SkillSwordGust()
    {
        isAttack = true;
        animator.SetTrigger("doAttack5");
        StartCoroutine(UpperSkill());
        CoolTimeTrigger(0);
        StartCoroutine(WaitForCooltime(skillControls[0].GetComponent<SkillControl>().coolTime, 0));
    }

    // 올려치기용 코루틴
    IEnumerator UpperSkill()
    {
        yield return new WaitForSeconds(1f);
        skillAttack[1].enabled = true;
        yield return new WaitForSeconds(1.5f);
        skillAttack[1].enabled = false;
        SwordGust();
    }

    // 검풍 날리기
    void SwordGust()
    {
        GameObject instantSwordGust = Instantiate(swordGust, transform.position, playerBody.transform.rotation);
        Rigidbody rigidGust = instantSwordGust.GetComponent<Rigidbody>();
        rigidGust.velocity = playerBody.forward * 20;
    }

    // 스킬 3번 도약 내려치기
    void SkillUpper()
    {

    }

    // 스킬 4번 윈드밀
    private void SkiilWindmill()
    {
        isAttack3 = true;
        animator.SetTrigger("doAttack3");
        StartCoroutine(WindmillReady());
        StartCoroutine(Windmill());
        StartCoroutine(WindmillDuration());
    }

    // 윈드밀 시전 대기
    IEnumerator WindmillReady()
    {
        yield return new WaitForSeconds(0.5f);
    }

    // 윈드밀 시전중
    IEnumerator Windmill()
    {
        WaitForSeconds waitWindmil = new WaitForSeconds(0.5f);
        while (true)
        {
            if (!isAttack3) break;
            skillAttack[3].enabled = true;
            yield return waitWindmil;
            skillAttack[3].enabled = false;
        }
    }
    
    IEnumerator WindmillDuration()
    {
        yield return new WaitForSeconds(10.0f);
        SkillWindmillStop();
    }

    // 윈드밀 종료
    private void SkillWindmillStop()
    {
        isAttack3 = false;
        animator.SetTrigger("stopAttack3");
        skillAttack[3].enabled = false;
        CoolTimeTrigger(2);
        StartCoroutine(WaitForCooltime(skillControls[3].GetComponent<SkillControl>().coolTime, 2));
    }

    private void EndAttack()
    {
        if (comboTrigger) return;
        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.IsName("Attack1a") || currentAnimation.IsName("Attack1b") || currentAnimation.IsName("Attack1c"))
        {
            if (currentAnimation.normalizedTime >= 0.99f)
            {
                isAttack = false;
                comboIndex = 0;
            }
        }
        if (currentAnimation.IsName("Attack4a") || currentAnimation.IsName("Attack5a"))
        {
            if (currentAnimation.normalizedTime >= 0.99f)
            {
                isAttack = false;
            }
        }
    }

    // UI 쿨타임 관리
    private void CoolTimeTrigger(int num)
    {
        skillControls[num].GetComponent<SkillControl>().isUseSkill = true;
        skillControls[num].GetComponent<SkillControl>().StartCooltime();
    }

    // 피격 함수
    private void OnHit()
    {
        if (hitCount > 3)
        {
            StartCoroutine(Enhance());
        }
        else if (!isAttack3 && !isEnhanced)
        {
            animator.SetTrigger("doHit");
            StartCoroutine(Hit());
            hitCount++;
        }
    }

    // 피격 강화
    IEnumerator Enhance()
    {
        animator.SetTrigger("doAirborn");
        capsuleCollider.enabled = false;
        isEnhanced = true;
        yield return new WaitForSeconds(5.0f);
        isEnhanced = false;
        hitCount = 0;
        capsuleCollider.enabled = true;
    }

    IEnumerator Hit()
    {
        ishit = true;
        yield return new WaitForSeconds(1.0f);
        ishit = false;
    }

    private void OnDie()
    {
        if (status.CurrentHP > 0) return;

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
            Status itemStatus = clickInterface.GetStatus();

            uiController.OpenMessagePanel(item.itemName, itemStatus);
            //itemName.text = item.itemName;
            //itemStatus.text = itemStat.status.ToString();

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

    IEnumerator WaitForJumpAttack(int attackNum)
    {
        jumpAttack.enabled = true;
        yield return new WaitForSeconds(0.5f);
        jumpAttack.enabled = false;
        isAttack = false;
    }

    // 쿨타임 체크용 코루틴
    IEnumerator WaitForCooltime(float coolTime, int coolDownNum)
    {
        switch (coolDownNum)
        {
            case 0:
                cooldownswordGustSkill = true;
                break;
            case 1:
                cooldownupperSkill = true;
                break;
            case 2:
                cooldownwindmillSkill = true;
                break;
            case 3:
                cooldownbufSkill = true;
                break;
        }
        yield return new WaitForSeconds(coolTime);
        switch (coolDownNum)
        {
            case 0:
                cooldownswordGustSkill = false;
                break;
            case 1:
                cooldownupperSkill = false;
                break;
            case 2:
                cooldownwindmillSkill = false;
                break;
            case 3:
                cooldownbufSkill = false;
                break;
        }
    }
}
