using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

// SupaDupaPersonalTest가 작성

// 비활성화된 여러 카메라를 가져와 평균 값을 설정합니다.
// 활성화된 메인 카메라는 평균 값으로 설정됩니다.
// 평균 값으로 설정되는 속도 값를 정할 수 있습니다.
public class CameraInterpolation : MonoBehaviour
{
    // 메인 카메라 
    [Header ("Main Camera")]
    [Tooltip ("이 스크립트를 구동할 메인 카메라")]
    public Camera mainCamera;
    [Range (0.0f, 1.0f)]
    [Tooltip ("속도 값")]
    public float followSpeed;

    [Header ("Camera Points")]
    // 아래 카메라는 비활성화 필요
    [Space (1)]
    [Tooltip ("컨트롤에 사용되는 카메라 컴포넌트")]
    public Camera controlCameraPoint;
    [Tooltip ("모션에 사용되는 카메라 컴포넌트")]
    public Camera motionCameraPoint;
    [Tooltip ("컨트롤와 모션 사이의 카메라 가중치")]
    [Range (0.0f, 1.0f)]
    public float weight;


    // 카메라의 평균 값
    private Vector3 targetPosition = new Vector3(0.0f, 0.0f, 0.0f);
    private Quaternion targetRotation = new quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    private float targetFOV = 0.0f;
    private float targetSpeed = 0.0f;

    // 카메라의 평균값을 계산합니다.
    void CalculateCameraPostition()
    {
        if (controlCameraPoint != null && motionCameraPoint != null)
        {
            targetPosition = Vector3.Lerp(controlCameraPoint.transform.position, motionCameraPoint.transform.position, weight);
            targetRotation = Quaternion.Lerp(controlCameraPoint.transform.rotation, motionCameraPoint.transform.rotation, weight);
            targetFOV = Mathf.Lerp(controlCameraPoint.fieldOfView, motionCameraPoint.fieldOfView, weight);
            //targetSpeed =  Mathf.Lerp(1, Mathf.Min(followSpeed*Time.deltaTime*10, 1), weight);
            targetSpeed =  Mathf.Min(followSpeed*Time.deltaTime*10, 1);
        }
    }

    // 메인 카메라를 평균값으로 이동시킵니다.
    void followTargetCamera()
    { 
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, targetSpeed);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetRotation, targetSpeed);
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, targetSpeed);
    }

    // 활성화 첫 프레임에 호출됩니다.
    void Start()
    {
        CalculateCameraPostition();
    }

    // 프레임마다 호출됩니다.
    void Update()
    {
        CalculateCameraPostition();
        followTargetCamera();
    }
}