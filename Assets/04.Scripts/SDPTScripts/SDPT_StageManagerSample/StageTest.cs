using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTest : MonoBehaviour
{
    public bool test = false;

    void Update()
    {
        if (test)
        {
            test = false;
            SDPT_StageManager.Instance.NextScene();
        }
    }
}
