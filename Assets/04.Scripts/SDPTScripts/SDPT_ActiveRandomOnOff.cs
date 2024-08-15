using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SupaDupaPersonalTest가 작성

// 랜덤으로 오브젝트를 ON / OFF 합니다.
[Serializable]
public class SDPT_ActiveRandomOnOff : MonoBehaviour
{
    [Tooltip ("시작시 랜덤으로 액티브 여부 결정할 오브젝트 리스트")]
    
    [SerializeField]
    public List<GameObject> objectList;

    // 활성화 첫 프레임에 호출됩니다.
    void Start()
    {
        foreach( GameObject activeObject in objectList)
        {
            if (activeObject.gameObject != null)
            {
                activeObject.SetActive(UnityEngine.Random.value<0.5f);
            }
        }
    }
}
