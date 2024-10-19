using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SDPT_AnchorManager : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public string anchorName;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject anchor = GameObject.Find(anchorName);
        if (anchor != null)
        {
            foreach(GameObject gameObject in gameObjects)
            {
                gameObject.transform.position = anchor.transform.position;
            }
            Destroy(anchor);
        }
    }
}
