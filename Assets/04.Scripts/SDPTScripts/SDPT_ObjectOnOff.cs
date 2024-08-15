using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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

    public float dissolveMax;
    public float dissolveMin;

    private void Update()
    {
        if (targetObject != null)
        {
            try
            {
                float currentValue = targetObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_DissolveAmount");
                float targetValue = dissolveMin;
                if (targetEnable) targetValue = dissolveMax;

                targetObject.SetActive(true);
                targetObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(currentValue, targetValue, Mathf.Min(dissolveSpeed*Time.deltaTime*10, 1)));
            } 
            catch
            {
                targetObject.SetActive(targetEnable);
            }
    }
    }
}

