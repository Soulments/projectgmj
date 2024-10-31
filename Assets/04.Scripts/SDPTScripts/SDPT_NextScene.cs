using System.Collections;
using UnityEngine;

public class SDPT_NextScene : MonoBehaviour
{
#if UNITY_EDITOR
    public UnityEditor.SceneAsset scene;
#endif
    public string sceneName; // ����� �� �̸�

    // Start is called before the first frame update
    void Start()
    {
        // sceneName�� ��� ���� �ʴٸ� �� �̸��� ����Ͽ� �� ��ȯ
        if (!string.IsNullOrEmpty(sceneName))
        {
            SDPT_StageManager.Instance.NextScene(sceneName);
        }
        else
        {
            SDPT_StageManager.Instance.NextScene();
        }
    }

#if UNITY_EDITOR
    // ������ ��忡���� `scene` �̸��� `sceneName`���� ������Ʈ
    private void OnValidate()
    {
        if (scene != null)
        {
            sceneName = scene.name;
        }
    }
#endif
}
