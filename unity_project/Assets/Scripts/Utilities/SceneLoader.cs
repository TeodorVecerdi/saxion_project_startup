using System;
using System.Collections;
using UnityCommons;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoSingleton<SceneLoader> {
    protected override void OnAwake() {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int sceneIndex, Action callback) {
        StartCoroutine(LoadSceneImpl(sceneIndex, callback));
    }

    private IEnumerator LoadSceneImpl(int sceneIndex, Action callback) {
        
        var operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        while (!operation.isDone) {
            yield return null;
        }
        
        callback?.Invoke();
    }
}