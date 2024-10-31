using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SDPT_NextScene : MonoBehaviour
{
    public SceneAsset scene;

    // Start is called before the first frame update
    void Start()
    {
        if (scene != null)
        {
            SDPT_StageManager.Instance.NextScene(scene);
        }
        else
        {
            SDPT_StageManager.Instance.NextScene();
        }
    }
}
