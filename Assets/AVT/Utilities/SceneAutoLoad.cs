﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAutoLoad : MonoBehaviour
{
    public bool loadOnStart = true;
    public bool allowReload = false;
    public bool setActiveAfterLoad = false;
    public bool autoUnload = true;
    protected bool isLoaded = false;

    public bool disableLoaderAfterLoadScene = false;

	public void Start()
	{
        //verify if the scene is already open to avoid opening a scene twice
        if (SceneManager.sceneCount > 0) {
            for (int i = 0; i < SceneManager.sceneCount; ++i) {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == gameObject.name) {
                    isLoaded = true;
                }
            }
        }

        if (loadOnStart)
            LoadScene();
    }


    public void LoadScene()
    {
        if (allowReload && isLoaded && autoUnload)
            SceneManager.UnloadSceneAsync(gameObject.name);

        if (!isLoaded || allowReload) {
            //Loading the scene, using the gameobject name as it's the same as the name of the scene to load
            AsyncOperation async = SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            //We set it to true to avoid loading the scene twice
            isLoaded = true;

            if (setActiveAfterLoad) {
                StartCoroutine(ActiveAfterLoad(async));
            }
        }

        Scene sceneToLoad = SceneManager.GetSceneByName(gameObject.name);
        if (sceneToLoad.isLoaded && setActiveAfterLoad)
            SceneManager.SetActiveScene(sceneToLoad);
    }

    IEnumerator ActiveAfterLoad(AsyncOperation async)
    {
        while (!async.isDone) {
            // Check if the load has finished
            if (async.progress >= 0.9f) {
                async.allowSceneActivation = true;
            }

            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameObject.name));

        if (disableLoaderAfterLoadScene)
            gameObject.SetActive(false);
    }
}
