using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

// SupaDupaPersonalTest가 작성
// ~가 수정

// 오브젝트 관리용 스크립트
// 필요한 오브젝트는 여기에서 추가해도됩니다.

public class SDPT_ObjectOnOff : MonoBehaviour
{
    public GameObject targetObject;
    public bool targetEnable;

    [Range (0.0f, 1.0f)]
    public float dissolveSpeed;

    public float dissolveMax = 0.6f;
    public float dissolveMin = -0.6f;
    public float dissolvePoint = -0.5f ;
    private float dissolveValue;

    private void Start()
    {
        if (targetEnable) dissolveValue = dissolveMax;
        else {dissolveValue = dissolveMin;}
    }
    
    private void Update()
    {
        if (targetObject != null)
        {
            float targetValue = dissolveMin;
            if (targetEnable) targetValue = dissolveMax;

            dissolveValue = Mathf.Lerp(dissolveValue, targetValue, Mathf.Min(dissolveSpeed*Time.deltaTime*10, 1));

            if (dissolveValue < dissolvePoint) targetObject.SetActive(false);
            else targetObject.SetActive(true);

            Renderer renderer;
            if (targetObject.TryGetComponent<Renderer>(out renderer))
            {
                foreach( Material material in renderer.sharedMaterials) 
                {
                    if (material.HasFloat("_DissolveAmount")) material.SetFloat("_DissolveAmount", dissolveValue);
                }
            }
        }
    }
}

