using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform playerBody;
    [SerializeField]
    private Transform cameraArm;

    bool isMove;

    Vector2 moveInput;

    float horizontalAxis;
    float verticalAxis;

    Animator animator;

    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        animator = playerBody.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        Move();
        Attack();
    }

    private void GetInput()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        //Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput = new Vector2(horizontalAxis, verticalAxis);
        isMove = moveInput.magnitude != 0;
        animator.SetBool("isMove", isMove);
        if(isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDirection = lookForward * moveInput.y + lookRight * moveInput.x;

            playerBody.forward = moveDirection;
            transform.position += moveDirection * Time.deltaTime * speed;
        }
        
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 cameraAngle = cameraArm.rotation.eulerAngles;
        float x = cameraAngle.x - mouseDelta.y;

        if(x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);
    }

    private void Attack()
    {

    }
}
