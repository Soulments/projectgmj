using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// SupaDupaPersonalTest가 작성

// 랜덤으로 오브젝트를 가져옵니다.
[Serializable]
public class SDPT_Item : MonoBehaviour
{
    public int skinCode = 0;
    public GameObject randomSeedObject = null; 
    public GameObject skinParent = null;

    [SerializeField]
    public List<GameObject> objectList;

    // Start is called before the first frame update
    void Start()
    {
        Item seedScript;
        if ( randomSeedObject != null && randomSeedObject.TryGetComponent<Item>(out seedScript))
        {
            skinCode = seedScript.randomSeed;
        }

        if (objectList.Count > 0)
        {
            GameObject skinObject = Instantiate(objectList[Math.Abs(skinCode) % objectList.Count], transform.position, transform.rotation);
            if (skinParent != null) skinObject.transform.SetParent(skinParent.transform);
            Destroy(this);
        }
    }
}
