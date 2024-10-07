using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SDPT_TitleTrans : MonoBehaviour
{

    public CanvasGroup canvasGroup = null;
    public float transTime = 1.0f;
    public float transSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        if (canvasGroup == null) Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        transTime -= transSpeed * Time.deltaTime;
        if (transTime < 0) canvasGroup.alpha -= transSpeed * Time.deltaTime;
        if (canvasGroup.alpha == 0) Destroy(this);
    }
}
