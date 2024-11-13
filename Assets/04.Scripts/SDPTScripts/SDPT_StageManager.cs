using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class SDPT_StageManager : MonoBehaviour
{
    public static SDPT_StageManager Instance;

    [System.Serializable]
    public class StageSetting
    {
        public List<sceneSetting> sceneList;

        [Tooltip("Minimum scene to use this scene")]
        public int minStage;
        [Tooltip("Maximum scene to use this scene")]
        public int maxStage;
    }

    [System.Serializable]
    public class sceneSetting
    {

#if UNITY_EDITOR
        public UnityEditor.SceneAsset scene;
#endif

        public void UpdateSceneName()
        {
#if UNITY_EDITOR
            if (scene != null)
            {
                sceneName = scene.name;
            }
#endif
        }


        public string sceneName;

        [Tooltip("Minimum stage to use this scene")]
        public int minScene;
        [Tooltip("Maximum stage to use this scene")]
        public int maxScene;
    }

    public int currentStage;
    public int currentScene;

    private int currentSceneIndex;
    private int currentStageIndex;

    private List<int> availableSceneIndex = new List<int>();
    private List<int> availableStageIndex = new List<int>();

    private string sceneName;


    [Tooltip("stageSceneLoopLength")]
    public int stageSceneLoopLength;

    [SerializeField]
    public List<StageSetting> stageList;

    public Transform mapTransform;

    private GameObject sceneRoot;

    public bool loading = false;


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
                currentStage++;
                ResetAvailableStageIndex();
                currentStageIndex = GetAvailableStageIndex();
            }
            ResetAvailableSceneIndex(currentStageIndex);
            currentSceneIndex = GetAvailableSceneIndex();
            sceneName = stageList[currentStageIndex].sceneList[currentSceneIndex].sceneName;
            LoadScene();
            return true;
        }
        return false;
    }

    public bool NextScene(int stageIndex, int sceneIndex)
    {
        if (!loading)
        {
            loading = true;
            currentScene++;
            if (currentScene > stageSceneLoopLength)
            {
                currentScene = 0;
                currentStage++;
            }
            sceneName = stageList[stageIndex].sceneList[sceneIndex].sceneName;
            LoadScene();
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

    private int GetAvailableSceneIndex()
    {
        return availableSceneIndex[UnityEngine.Random.Range(0, availableSceneIndex.Count)];
    }
    private int GetAvailableStageIndex()
    {
        return availableStageIndex[UnityEngine.Random.Range(0, availableStageIndex.Count)];
    }

    private void ResetAvailableSceneIndex(int stageIndex)
    {
        availableSceneIndex.Clear();
        var stageSetting = stageList[stageIndex]; 

        for(int i = 0; i < stageSetting.sceneList.Count; i++)
            if (stageSetting.sceneList[i].minScene <= currentScene && stageSetting.sceneList[i].maxScene >= currentScene)
                availableSceneIndex.Add(i);
    }
    private void ResetAvailableStageIndex()
    {
        availableStageIndex.Clear();

        for(int i = 0; i < stageList.Count; i++)
            if (stageList[i].minStage <= currentStage && stageList[i].maxStage >= currentStage)
                availableStageIndex.Add(i);
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

        ResetAvailableStageIndex();
        currentStageIndex = GetAvailableStageIndex();

        NextScene();
    }


    private void OnValidate()
    {
        foreach (var stage in stageList)
        {
            foreach (var scene in stage.sceneList)
            {
                scene.UpdateSceneName();
            }
        }
    }
}
