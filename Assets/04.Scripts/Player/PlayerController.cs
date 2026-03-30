using System;
using System.Collections;
using CharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public float groundCheckDistance = 0;
    public float adjustmentSpeed = 5f;
    public float jumpForce;
    public float speed = 10;
    private int comboIndex;
    public bool dontDamage;
    public bool isDead = false;

    [SerializeField]
    public Inventory inventory;
    [SerializeField]
    private CanvasGroup inventoryCanvasGroup;
    private bool isInventoryOpen = false;

    public GameObject escCanvas;
    private bool isEscOpen = false;

    private IObjectItem itemPickup = null;

    public UIController uiController;
    public TextMeshProUGUI itemStatus;

    // FSM
    private StateMachine stateMachine;

    void Awake()
    {
        status = new Status(UnitCode.Player, "플레이어", 1);
        groundLayer = LayerMask.GetMask("Ground");
        HitBoxDamage();
    }

    void Start()
    {
        animator = playerBody.GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = playerBody.GetComponent<CapsuleCollider>();
        SkillControlAttach();
        InitStateMachine();
    }

    void InitStateMachine()
    {
        stateMachine = new StateMachine(StateName.IDLE, new IdleState(this));
        stateMachine.AddState(StateName.MOVE, new MoveState(this));
        stateMachine.AddState(StateName.JUMP, new JumpState(this));
        stateMachine.AddState(StateName.ATTACK, new AttackState(this));
        stateMachine.AddState(StateName.SKILL, new SkillState(this));
        stateMachine.AddState(StateName.WINDMILL, new WindmillState(this));
        stateMachine.AddState(StateName.HIT, new HitState(this));
        stateMachine.AddState(StateName.DIE, new DieState(this));
    }

    void Update()
    {
        GetInput();
        DetermineState();
        stateMachine.UpdateState();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdateState();
    }

    // 매 프레임 상태 전환 판단
    void DetermineState()
    {
        if (isDead)
        {
            if (stateMachine.CurrentState is not DieState)
                stateMachine.ChangeState(StateName.DIE);
            return;
        }

        if (isAirborne && stateMachine.CurrentState is not HitState)
        {
            stateMachine.ChangeState(StateName.HIT);
            return;
        }

        // Windmill은 지속 스킬이므로 별도 처리
        if (isAttack3 && stateMachine.CurrentState is not WindmillState)
        {
            stateMachine.ChangeState(StateName.WINDMILL);
            return;
        }

        // 점프 중 (Windmill/Hit/Die 제외)
        if (isJump && !isAttack3 && !isAirborne &&
            stateMachine.CurrentState is not JumpState &&
            stateMachine.CurrentState is not HitState &&
            stateMachine.CurrentState is not DieState)
        {
            stateMachine.ChangeState(StateName.JUMP);
            return;
        }

        if (isAttack && !isAttack3 && !isJump &&
            stateMachine.CurrentState is not AttackState &&
            stateMachine.CurrentState is not SkillState)
        {
            AnimatorStateInfo anim = animator.GetCurrentAnimatorStateInfo(0);
            bool isSkillAnim = anim.IsName("Upper") || anim.IsName("SwordGust") || anim.IsName("Attack4a");
            if (isSkillAnim)
                stateMachine.ChangeState(StateName.SKILL);
            else
                stateMachine.ChangeState(StateName.ATTACK);
            return;
        }

        if (!isAttack && !isAttack3 && !isJump && !isAirborne)
        {
            if (isMove && stateMachine.CurrentState is not MoveState)
                stateMachine.ChangeState(StateName.MOVE);
            else if (!isMove && stateMachine.CurrentState is not IdleState)
                stateMachine.ChangeState(StateName.IDLE);
        }
    }

    void SkillControlAttach()
    {
        skillControls[0] = GameObject.Find("Player Canvas/Player Panel/Skill Group/Skill Q").GetComponent<SkillControl>();
        skillControls[1] = GameObject.Find("Player Canvas/Player Panel/Skill Group/Skill E").GetComponent<SkillControl>();
        skillControls[2] = GameObject.Find("Player Canvas/Player Panel/Skill Group/Skill R").GetComponent<SkillControl>();
        skillControls[3] = GameObject.Find("Player Canvas/Player Panel/Skill Group/Skill TAB").GetComponent<SkillControl>();
    }

    void HitBoxDamage()
    {
        normalAttack.GetComponent<HitBox>().skillPercent = status.AttackDamage;
        jumpAttack.GetComponent<HitBox>().skillPercent = status.SkillPercent[(int)SkillCode.Jump];

        skillAttack[(int)SkillCode.SwordGust].GetComponent<HitBox>().skillPercent = status.SkillPercent[(int)SkillCode.SwordGust];
        skillAttack[(int)SkillCode.Upper].GetComponent<HitBox>().skillPercent = status.SkillPercent[(int)SkillCode.Upper];
        skillAttack[(int)SkillCode.Windmill].GetComponent<HitBox>().skillPercent = status.SkillPercent[(int)SkillCode.Windmill];

        swordGust.GetComponent<SwordGust>().skillPercent = (int)(status.SkillPercent[(int)SkillCode.SwordGust] * 0.75);
    }

    // --- State에서 호출하는 public 메서드들 ---

    public void DisableHitBox()
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

    public void DisableAttack()
    {
        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.IsTag("NotAttack"))
        {
            isAttack = false;
        }
    }

    public void HandleInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.I) || (Input.GetKeyDown(KeyCode.Escape) && isInventoryOpen))
        {
            ToggleInventory();
        }
    }

    public void HandleEscInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEscOpen = !isEscOpen;
            escCanvas.SetActive(isEscOpen);
        }
    }

    public void HandleItemPickupInput()
    {
        if (itemPickup != null && Input.GetKeyDown(KeyCode.F) && !isInventoryOpen)
        {
            ItemData item = itemPickup.ClickItem();
            Status itemStat = itemPickup.GetStatus();
            inventory.AddItem(item, itemStat);
            itemPickup.OnPickup();
            itemPickup = null;
            uiController.CloseMessagePanel();
        }
    }

    public void LookAround()
    {
        if (!isInventoryOpen)
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector3 cameraAngle = cameraArm.rotation.eulerAngles;
            float x = cameraAngle.x - mouseDelta.y;

            if (x < 180f)
                x = Mathf.Clamp(x, -1f, 70f);
            else
                x = Mathf.Clamp(x, 335f, 361f);

            cameraArm.rotation = Quaternion.Euler(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);
        }
    }

    public void Move()
    {
        moveInput = new Vector2(horizontalAxis, verticalAxis);
        isMove = moveInput.magnitude != 0;
        if (usingPortal) animator.SetBool("isMove", false);
        else animator.SetBool("isMove", isMove);

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

    public void Jump()
    {
        if (jumpDown && !isJump && !isAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isJump", true);
            isJump = true;
            animator.SetTrigger("doJump");
        }
    }

    public void SyncBodyPosition()
    {
        if (!isAttack)
            playerBody.transform.position = Vector3.Lerp(playerBody.transform.position, transform.position, 0.5f);
    }

    // 일반공격 + 점프공격 처리 (AttackState에서 호출)
    public void ProcessNormalAttack()
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
    }

    public void ProcessComboAttack()
    {
        ComboAttack();
    }

    // 스킬 입력 처리 - Windmill 제외 (SkillState에서 호출)
    public void ProcessSkillInputWithoutWindmill()
    {
        if (!isJump && swordGustSkill) SkillSwordGust();
        if (!isJump && upperSkill) SkillUpper();
        if (!isJump && bufSkill) SkillBuf();
    }

    // MoveState/AttackState에서 Windmill 시작 입력 감지
    public void ProcessWindmillStart()
    {
        if (!isJump && windmillSkill[0]) SkiilWindmill();
    }

    // WindmillState에서 종료 입력 감지
    public void ProcessWindmillStop()
    {
        if (windmillSkill[1]) SkillWindmillStop();
    }

    // WindmillState OnEnter에서 호출 - 코루틴 등 시작
    public void StartWindmill()
    {
        // isAttack3, 애니메이터, 코루틴은 SkiilWindmill()에서 처리됨
        // DetermineState에서 windmillSkill[0] 감지 후 SkiilWindmill() 호출 → WINDMILL 전환
        // 이미 SkiilWindmill()이 호출된 상태이므로 OnEnter에서 추가 작업 없음
    }

    // JumpState에서 점프공격 입력 감지
    public void ProcessJumpAttack()
    {
        if (mouseLeft) JumpAttack();
    }

    public void ActionCheck()
    {
        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.IsTag("NormalAttack"))
        {
            if (!isNormal && currentAnimation.normalizedTime > 0.2f && currentAnimation.normalizedTime < 0.8f)
            {
                normalAttack.enabled = true;
                isNormal = true;
            }
            if (currentAnimation.normalizedTime >= 0.8f)
            {
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
                SpawnSwordGust();
            }
            if (currentAnimation.normalizedTime >= 0.8f)
                skillAttack[(int)SkillCode.SwordGust].enabled = false;
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

    public void CheckDeath()
    {
        if (!isDead) OnDie();
    }

    // HitState 진입 시 호출
    public void OnHitEnter()
    {
        if (hitCount > 1 && !isEnhanced)
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

    // DieState 진입 시 호출
    public void OnDieEnter()
    {
        isDead = true;
        animator.SetTrigger("doAirborne");
        animator.SetBool("isDead", isDead);
        DisableAllHitBox();
        isAttack = false;
        capsuleCollider.enabled = false;
        rigidBody.useGravity = false;
    }

    // --- private 내부 메서드 ---

    private void GetInput()
    {
        horizontalAxis = Input.GetAxis("Horizontal") * (!isInventoryOpen ? 1 : 0) * (!isAirborne ? 1 : 0);
        verticalAxis = Input.GetAxis("Vertical") * (!isInventoryOpen ? 1 : 0) * (!isAirborne ? 1 : 0);
        jumpDown = Input.GetButtonDown("Jump") && !isInventoryOpen && !isAirborne;
        mouseLeft = Input.GetMouseButtonDown(0) && !isInventoryOpen && !isAirborne;
        mouseRight = Input.GetMouseButtonDown(1) && !isInventoryOpen && !isAirborne;

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
        inventoryCanvasGroup.interactable = isInventoryOpen;
        inventoryCanvasGroup.blocksRaycasts = isInventoryOpen;
    }

    private void NormalAttack()
    {
        if (!isAttack)
        {
            isAttack = true;
            comboIndex = 1;
            animator.SetTrigger("doAttack1a");
        }
    }

    private void ComboCheck()
    {
        AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.IsName("Attack1a"))
        {
            if (currentAnimation.normalizedTime > 0.5f && currentAnimation.normalizedTime < 0.99f)
                comboTrigger = true;
        }
        if (currentAnimation.IsName("Attack1b"))
        {
            if (currentAnimation.normalizedTime > 0.5f && currentAnimation.normalizedTime < 0.99f)
                comboTrigger = true;
        }
    }

    private void ComboAttack()
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

    private float RaycastCheck()
    {
        RaycastHit hit;
        float distance = -1;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            distance = transform.position.y - hit.point.y;
        return distance;
    }

    private void JumpAttack()
    {
        isAttack = true;
        float distance = RaycastCheck() != 1 ? RaycastCheck() : 0;
        if (distance <= 0.75f) return;
        if (isUpper)
        {
            distance = (float)Math.Ceiling(distance);
            skillAttack[(int)SkillCode.Upper].GetComponent<HitBox>().skillPercent = status.SkillPercent[(int)SkillCode.Upper] * (1 + (0.1f * distance / 2));
        }
        animator.SetTrigger("doAttack2");
        rigidBody.AddForce(Vector3.down * jumpForce * 5, ForceMode.Impulse);
    }

    private void SkillBuf()
    {
        isAttack = true;
        animator.SetTrigger("doAttack4");
        status.CurrentHP += (int)status.Defense;
        CoolTimeTrigger(3);
        StartCoroutine(WaitForCooltime(skillControls[3].GetComponent<SkillControl>().coolTime, 3));
        StartCoroutine(CoroutineBuf());
    }

    IEnumerator CoroutineBuf()
    {
        yield return new WaitForSeconds(5.0f);
    }

    private void SkillSwordGust()
    {
        isAttack = true;
        animator.SetTrigger("doAttack5");
        CoolTimeTrigger(0);
        StartCoroutine(WaitForCooltime(skillControls[0].GetComponent<SkillControl>().coolTime, 0));
    }

    private void SpawnSwordGust()
    {
        GameObject instantSwordGust = Instantiate(swordGust, transform.position, playerBody.transform.rotation);
        Rigidbody rigidGust = instantSwordGust.GetComponent<Rigidbody>();
        rigidGust.velocity = playerBody.forward * 20;
    }

    private void SkillUpper()
    {
        isUpper = true;
        animator.SetBool("isJump", true);
        animator.SetTrigger("doAttack6");
        CoolTimeTrigger(1);
        StartCoroutine(WaitForCooltime(skillControls[1].GetComponent<SkillControl>().coolTime, 1));
    }

    private void SkiilWindmill()
    {
        isAttack3 = true;
        animator.SetTrigger("doAttack3");
        StartCoroutine(WindmillReady());
        StartCoroutine(Windmill());
        Invoke(nameof(SkillWindmillStop), 10f);
    }

    IEnumerator WindmillReady()
    {
        yield return new WaitForSeconds(0.5f);
    }

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

    private void SkillWindmillStop()
    {
        if (isAttack3)
        {
            isAttack3 = false;
            animator.SetTrigger("stopAttack3");
            skillAttack[(int)SkillCode.Windmill].enabled = false;
            CoolTimeTrigger(2);
            StartCoroutine(WaitForCooltime(skillControls[3].GetComponent<SkillControl>().coolTime, 2));
        }
    }

    private void CoolTimeTrigger(int num)
    {
        skillControls[num].GetComponent<SkillControl>().isUseSkill = true;
        skillControls[num].GetComponent<SkillControl>().StartCooltime();
    }

    private void OnHit()
    {
        if (!isEnhanced)
            stateMachine.ChangeState(StateName.HIT);
    }

    private void OnDie()
    {
        if (status.CurrentHP > 0) return;
        stateMachine.ChangeState(StateName.DIE);
    }

    private IEnumerator Enhance()
    {
        animator.SetTrigger("doAirborne");
        DisableAllHitBox();
        isAttack = false;
        isEnhanced = true;
        yield return new WaitForSeconds(5.0f);
        isEnhanced = false;
        hitCount = 0;
        comboIndex = 0;
    }

    private void DisableAllHitBox()
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
                OnHit();
            return;
        }

        IObjectItem clickInterface = other.GetComponent<IObjectItem>();
        if (clickInterface != null)
        {
            ItemData item = clickInterface.ClickItem();
            Status itemStat = clickInterface.GetStatus();
            uiController.OpenMessagePanel(item.itemName, itemStat);
            itemPickup = clickInterface;
        }
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

    IEnumerator WaitForCooltime(float coolTime, int coolDownNum)
    {
        switch (coolDownNum)
        {
            case 0: cooldownswordGustSkill = true; break;
            case 1: cooldownupperSkill = true; break;
            case 2: cooldownwindmillSkill = true; break;
            case 3: cooldownbufSkill = true; break;
        }
        yield return new WaitForSeconds(coolTime);
        switch (coolDownNum)
        {
            case 0: cooldownswordGustSkill = false; break;
            case 1: cooldownupperSkill = false; break;
            case 2: cooldownwindmillSkill = false; break;
            case 3: cooldownbufSkill = false; break;
        }
    }
}
