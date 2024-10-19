using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SDPT_TransitionManager : MonoBehaviour
{
    public CanvasGroup canvasGroup = null;
    public float transSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        if (canvasGroup == null) Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        canvasGroup.alpha = Mathf.Max( canvasGroup.alpha - transSpeed * Time.deltaTime, 0);
    }

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
        canvasGroup.alpha = 1;
    }
}
