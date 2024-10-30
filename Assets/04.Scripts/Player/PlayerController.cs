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
    bool isNormal;
    bool isUpper;
    bool isGust;
    bool isGrounded;
    bool isAirborne;
    bool isEnhanced;
    bool usingPortal;
    bool comboTrigger;

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

    public float groundCheckDistance = 0;  // 바닥으로부터 플레이어가 유지하고 싶은 거리
    public float adjustmentSpeed = 5f;  // 보정 속도
    public float jumpForce;
    public float speed = 10;
    private int comboIndex;
    public bool dontDamage;
    public bool isDead = false;

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
        // status = new Status(UnitCode.Player, "플레이어", GameObject.Find("GameManager").GetComponent<GameManager>().waveCount);
        status = new Status(UnitCode.Player, "플레이어", 1);
        groundLayer = LayerMask.GetMask("Ground");
        HitBoxDamage();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = playerBody.GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = playerBody.GetComponent<CapsuleCollider>();
        SkillControlAttach();
    }

    // Update is called once per frame
    void Update()
    {
        DisableHitBox();
        DisableAttack();
        GetInput();
        LookAround();
        Move();
        Jump();
        //AdjustPlayerHeight();
        //GroundCheck();
        ActionPlayer();
        ComboAttack();
        ActionCheck();
        //Weapon();
        if (!isDead) OnDie();
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

    // 히트박스 관리용
    void DisableHitBox()
    {
        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.IsTag("NotAttack"))
        {
            normalAttack.enabled = false;
            jumpAttack.enabled = false;
            skillAttack[(int)SkillCode.Upper].enabled = false;
            skillAttack[(int)SkillCode.SwordGust].enabled = false;
            skillAttack[(int)SkillCode.Windmill].enabled = false;
        }
    }

    void DisableAttack()
    {
        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.IsTag("NotAttack"))
        {
            isAttack = false;
        }
    }
    

    // Input값 정리
    private void GetInput()
    {
        horizontalAxis = Input.GetAxis("Horizontal") * (!isInventoryOpen ? 1 : 0) * (!isAirborne ? 1 : 0);
        verticalAxis = Input.GetAxis("Vertical") * (!isInventoryOpen ? 1 : 0) * (!isAirborne ? 1 : 0);
        jumpDown = Input.GetButtonDown("Jump") && !isInventoryOpen && !isAirborne;
        mouseLeft = Input.GetMouseButtonDown(0) && !isInventoryOpen && !isAirborne;
        mouseRight = Input.GetMouseButtonDown(1) && !isInventoryOpen && !isAirborne;

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

            uiController.CloseMessagePanel();
        }
        // ------------------------------------------------------

        swordGustSkill = Input.GetKeyDown(KeyCode.Q) && !cooldownswordGustSkill && !isInventoryOpen && !isAirborne;
        upperSkill = Input.GetKeyDown(KeyCode.E) && !cooldownupperSkill && !isInventoryOpen && !isAirborne;
        windmillSkill[0] = Input.GetKeyDown(KeyCode.R) && !cooldownwindmillSkill && !isInventoryOpen && !isAirborne;
        windmillSkill[1] = Input.GetKeyUp(KeyCode.R) && !cooldownwindmillSkill && !isInventoryOpen && !isAirborne;
        bufSkill = Input.GetKeyDown(KeyCode.Tab) && !cooldownbufSkill && !isInventoryOpen && !isAirborne;
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
        else if (isMove && !isAttack && !isAirborne && !usingPortal)
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
            isJump = true;
            animator.SetTrigger("doJump");
        }
    }

    private void AdjustPlayerHeight()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position;

        // 플레이어 아래로 Ray를 쏴서 바닥과의 거리 확인
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, Mathf.Infinity))
        {
            float distanceToGround = hit.distance;
            Debug.Log(distanceToGround);

            // 바닥과의 거리가 지정한 거리보다 가까울 경우 위치 재조정
            if (distanceToGround < groundCheckDistance)
            {
                float targetYPosition = hit.point.y + groundCheckDistance;  // 목표 높이
                Vector3 targetPosition = new Vector3(transform.position.x, targetYPosition, transform.position.z);

                // SmoothDamp을 사용해 부드럽게 높이 보정
                transform.position = Vector3.Lerp(transform.position, targetPosition, adjustmentSpeed * Time.deltaTime);
            }
        }
    }

    //private void GroundCheck()
    //{
    //    isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, LayerMask.GetMask("Ground"));
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

    // 일반공격
    private void NormalAttack()
    {
        if (!isAttack)
        {
            isAttack = true;
            comboIndex = 1;
            animator.SetTrigger("doAttack1a");
        }
    }

    // 콤보 활성화 여부 확인
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
    
    // 콤보 공격
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

    float RaycastCheck()
    {
        RaycastHit hit;
        float distance = -1;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            distance = transform.position.y - hit.point.y;
        }

        return distance;
    }

    // 점프공격
    private void JumpAttack()
    {
        isAttack = true;
        float distance = RaycastCheck() != 1 ? RaycastCheck() : 0;
        Debug.Log(distance);
        if (distance <= 0.75f) return;
        if (isUpper)
        {
            distance = (float)Math.Ceiling(distance);
            skillAttack[(int)SkillCode.Upper].GetComponent<HitBox>().skillPercent = status.SkillPercent[(int)SkillCode.Upper] * (1 + (0.1f * distance / 2));
        }
        animator.SetTrigger("doAttack2");
        rigidBody.AddForce(Vector3.down * jumpForce * 5, ForceMode.Impulse);
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
        CoolTimeTrigger(0);
        StartCoroutine(WaitForCooltime(skillControls[0].GetComponent<SkillControl>().coolTime, 0));
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
        isUpper = true;
        animator.SetBool("isJump", true);
        animator.SetTrigger("doAttack6");
        CoolTimeTrigger(1);
        StartCoroutine(WaitForCooltime(skillControls[1].GetComponent<SkillControl>().coolTime, 1));
    }

    // 스킬 4번 윈드밀
    private void SkiilWindmill()
    {
        isAttack3 = true;
        animator.SetTrigger("doAttack3");
        StartCoroutine(WindmillReady());
        StartCoroutine(Windmill());
        //StartCoroutine(WindmillDuration());
    }

    // 윈드밀 시전 대기
    IEnumerator WindmillReady()
    {
        yield return new WaitForSeconds(0.5f);
    }

    // 윈드밀 시전중
    IEnumerator Windmill()
    {
        WaitForSeconds waitWindmil = new WaitForSeconds(0.3f);
        while (true)
        {
            if (!isAttack3) break;
            skillAttack[(int)SkillCode.Windmill].enabled = !skillAttack[(int)SkillCode.Windmill].enabled;
            yield return waitWindmil;
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
        skillAttack[(int)SkillCode.Windmill].enabled = false;
        CoolTimeTrigger(2);
        StartCoroutine(WaitForCooltime(skillControls[3].GetComponent<SkillControl>().coolTime, 2));
    }

    private void ActionCheck()
    {
        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.IsTag("NormalAttack"))
        {
            if (!isNormal && currentAnimation.normalizedTime > 0.2f && currentAnimation.normalizedTime < 0.8f)
            {
                Debug.Log("노말공격 시작");
                normalAttack.enabled = true;
                isNormal = true;
            }
            if (currentAnimation.normalizedTime >= 0.8f)
            {
                Debug.Log("노말공격 종료");
                isNormal = false;
                normalAttack.enabled = false;
            }
            if (comboTrigger) return;
            if (currentAnimation.normalizedTime >= 0.99f)
            {
                isAttack = false;
                comboIndex = 0;
            }
        }
        if (currentAnimation.IsName("Attack2b"))
        {
            if (currentAnimation.normalizedTime > 0.1f && currentAnimation.normalizedTime < 0.8f && !skillAttack[(int)SkillCode.Upper].enabled)
            {
                if (isUpper) skillAttack[(int)SkillCode.Upper].enabled = true;
                else jumpAttack.enabled = true;
            }
            if (currentAnimation.normalizedTime >= 0.8f)
            {
                if (isUpper) skillAttack[(int)SkillCode.Upper].enabled = false;
                else jumpAttack.enabled = false;
            }
            
            if (currentAnimation.normalizedTime >= 0.99f)
            {
                isUpper = false;
                isAttack = false;
            }
        }
        if (currentAnimation.IsName("Attack4a") || currentAnimation.IsName("SwordGust"))
        {
            if (!isGust && currentAnimation.normalizedTime > 0.55f && currentAnimation.normalizedTime < 0.8f && !skillAttack[(int)SkillCode.SwordGust].enabled)
            {
                skillAttack[(int)SkillCode.SwordGust].enabled = true;
                isGust = true;
                SwordGust();
            }
            if (currentAnimation.normalizedTime >= 0.8f)
            {
                skillAttack[(int)SkillCode.SwordGust].enabled = false;
            }
            if (currentAnimation.normalizedTime >= 0.99f)
            {
                isGust = false;
                isAttack = false;
            }
        }
        if (currentAnimation.IsName("Upper") && currentAnimation.normalizedTime >= 0.99f && !isJump)
        {
            rigidBody.AddForce(Vector3.up * jumpForce * 2f, ForceMode.Impulse);
            isJump = true;
        }
        if (currentAnimation.IsName("Airborne") || currentAnimation.IsName("GetUp"))
        {
            isAirborne = true;
            dontDamage = true;
        }
        else
        {
            isAirborne = false;
            dontDamage = false;
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
        if (hitCount > 1)
        {
            StartCoroutine(Enhance());
        }
        else if (!isAttack3 && !isEnhanced)
        {
            animator.SetTrigger("doHit");
            isAttack = false;
            comboIndex = 0;
            StartCoroutine(Hit());
            hitCount++;
        }
    }

    // 피격 강화
    IEnumerator Enhance()
    {
        animator.SetTrigger("doAirborne");
        //rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        DisableAllHitBox();
        isAttack = false;
        isEnhanced = true;
        yield return new WaitForSeconds(5.0f);
        isEnhanced = false;
        hitCount = 0;
        comboIndex = 0;
    }

    void DisableAllHitBox()
    {
        normalAttack.enabled = false;
        jumpAttack.enabled = false;
        skillAttack[(int)SkillCode.SwordGust].enabled = false;
        skillAttack[(int)SkillCode.Upper].enabled = false;
        skillAttack[(int)SkillCode.Windmill].enabled = false;
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
        animator.SetTrigger("doAirborne");
        animator.SetBool("isDead", isDead);
        DisableAllHitBox();
        isAttack = false;
        capsuleCollider.enabled = false;
        rigidBody.useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
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
