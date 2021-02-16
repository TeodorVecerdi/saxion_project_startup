using UnityEngine;

public class KeepBetweenScenes : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}