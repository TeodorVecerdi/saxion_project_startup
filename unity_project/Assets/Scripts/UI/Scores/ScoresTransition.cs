using UnityEngine;

public class ScoresTransition : MonoBehaviour {
    [SerializeField] private Slide SwipeSlide;
    [SerializeField] private Slide ScoresSlide;

    public void Open() {
        SwipeSlide.SetActive(false);
        ScoresSlide.SetActive(true);
    }

    public void Close() {
        SwipeSlide.SetActive(true, true);
        ScoresSlide.SetActive(false, true);
    }
}

