using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDPT_PressSpaceBar : MonoBehaviour
{
    public CanvasGroup canvasGroup = null;
    public GameObject target = null;

    void Start()
    {
        if (canvasGroup == null) Destroy(this);
        if (target == null) Destroy(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canvasGroup.alpha > 0)
            {
                target.SetActive(true);
            }
        }
    }
}
