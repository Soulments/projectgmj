using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;

public class SDPT_StageManager : MonoBehaviour
{
    public static SDPT_StageManager Instance;

    [System.Serializable]
    public struct StageSetting
    {
        public List<sceneSetting> sceneList;

        [Tooltip("Minimum scene to use this scene")]
        public int minStage;
        [Tooltip("Maximum scene to use this scene")]
        public int maxStage;
    }
    [System.Serializable]
    public struct sceneSetting
    {
        public SceneAsset scene;

        [Tooltip("Minimum stage to use this scene")]
        public int minScene;
        [Tooltip("Maximum stage to use this scene")]
        public int maxScene;
    }

    public int currentStage;
    public int currentScene;

    [Tooltip("stageSceneLoopLength")]
    public int stageSceneLoopLength;

    [SerializeField]
    public List<StageSetting> stageList;

    public Transform mapTransform;

    private GameObject sceneRoot;

    public bool loading = false;

    private string sceneName;
    private Scene baseScene;
    private Scene loadedScene;


    // temp item cleaner
    public bool ItemCleaner()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in items)
        {
            Destroy(item);
        }

        Debug.Log(items.Length + "개의 Item 오브젝트가 삭제되었습니다.");
        
        return true;
    }

    // Change to Next Scene
    public bool NextScene()
    {
        if (!loading)
        {
            loading = true;
            currentScene++;
            if (currentScene > stageSceneLoopLength)
            {
                currentScene = 0;
                currentStage ++;
            }
            sceneName = GetNextSceneName(currentStage, currentScene);
            LoadScene();
            return true;
        }
        return false;
    }

    public bool NextScene(SceneAsset scene)
    {
        if (!loading)
        {
            loading = true;
            SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
            return true;
        }
        return false;
    }

    public bool NextScene(string sceneName)
    {
        if (!loading)
        {
            loading = true;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            return true;
        }
        return false;
    }

    // Get Next Scene Name
    private string GetNextSceneName(int currentStage, int currentScene)
    {
        List<StageSetting> availableStageList = new List<StageSetting>();

        // Deep copy & Stage filtering 
        foreach (var stageSetting in stageList)
        {
            if (stageSetting.minStage <= currentStage && stageSetting.maxStage >= currentStage )
            {
                StageSetting newStageSetting = new StageSetting
                {
                    minStage = stageSetting.minStage,
                    sceneList = new List<sceneSetting>()
                };

                foreach (var scene in stageSetting.sceneList)
                {
                    if (scene.minScene <= currentScene && scene.maxScene >= currentScene)
                    {
                        newStageSetting.sceneList.Add(new sceneSetting
                        {
                            scene = scene.scene,
                            minScene = scene.minScene
                        });
                    }
                }
                availableStageList.Add(newStageSetting);
            }
        }

        int randomStage = 0;
        int randomScene = 0;

        if (availableStageList.Count > 0) 
        {
            randomStage = Random.Range(0, availableStageList.Count);
            if (availableStageList[randomStage].sceneList.Count > 0)
            {
                randomScene = Random.Range(0, availableStageList[randomStage].sceneList.Count);
                return availableStageList[randomStage].sceneList[randomScene].scene.name;
            }    
        }

        return "";
    }

    // Load Scene Async then finalize
    private void LoadScene()
    {
        ItemCleaner();
        if (sceneRoot != null) Destroy(sceneRoot);

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += FinalizeSceneLoading;
        return;
    }

    // Scene loading finalize
    // Destroy previous scene then move scene to scene root
    private void FinalizeSceneLoading(AsyncOperation asyncOperation)
    {
        sceneRoot = new GameObject(sceneName + "_Root");

        loadedScene = SceneManager.GetSceneByName(sceneName);

        if (loadedScene.IsValid())
        {
            GameObject[] rootObjects = loadedScene.GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
            {
                obj.transform.SetParent(sceneRoot.transform);
            }
            sceneRoot.transform.SetParent(mapTransform);
        }
        SceneManager.MergeScenes(loadedScene, baseScene);
        loading = false;
        return;
    }

    private void Start()
    {
        Instance = this;
        baseScene = SceneManager.GetActiveScene();
        NextScene();
    }
}