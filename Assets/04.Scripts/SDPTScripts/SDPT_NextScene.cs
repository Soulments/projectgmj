using System.Collections;
using UnityEngine;

public class SDPT_NextScene : MonoBehaviour
{
#if UNITY_EDITOR
    public UnityEditor.SceneAsset scene;
#endif
    public string sceneName; // 빌드용 씬 이름

    // Start is called before the first frame update
    void Start()
    {
        // sceneName이 비어 있지 않다면 그 이름을 사용하여 씬 전환
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
    // 에디터 모드에서만 `scene` 이름을 `sceneName`으로 업데이트
    private void OnValidate()
    {
        if (scene != null)
        {
            sceneName = scene.name;
        }
    }
#endif
}
