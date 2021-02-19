using System.Collections.Generic;
using DG.Tweening;
using UnityCommons;
using UnityEngine;

public class MinigamePanelController : MonoBehaviour {
    [SerializeField] private CanvasGroup Panel;
    [Space]
    [SerializeField] private CanvasGroup Prompts;
    [SerializeField] private CanvasGroup WaitingPrompts;
    [SerializeField] private CanvasGroup Answers;
    [SerializeField] private CanvasGroup WaitingAnswers;
    [SerializeField] private CanvasGroup Results;
    [SerializeField] private CanvasGroup WaitingForNewGame;
    private CanvasGroup currentState;
    
    private bool isOpen = false;
    private bool showResultsOnOpen = false;
    private GameModel gameModel;

    public void Open() {
        if(isOpen) return;
        gameObject.SetActive(true);
        Panel.DOFade(1f, 0.25f).From(0f);
        isOpen = true;
        if (showResultsOnOpen) {
            showResultsOnOpen = false;
            ShowResults(gameModel);
        }
    }

    public void Close() {
        if(!isOpen) return;
        
        Panel.DOFade(0f, 0.25f).From(1f).OnComplete(() => {
            gameObject.SetActive(false);
        });
        isOpen = false;
    }

    public void ShowPrompts() {
        ShowState(Prompts);
    }

    public void ShowWaitingPrompts() {
        ShowState(WaitingPrompts);
    }

    public void ShowAnswers(GameModel game) {
        var answers = Answers.GetComponent<MinigameAnswersController>();
        var indices = new List<int> {0, 1, 2};
        indices.Shuffle();
        var prompts = new List<string>();
        foreach (var newIdx in indices) {
            prompts.Add(game.Prompts[newIdx]);
        }
        answers.Load(prompts, indices);
        ShowState(Answers);
    }

    public void ShowWaitingAnswers() {
        ShowState(WaitingAnswers);
    }

    public void ShowResults(GameModel game) {
        if (!isOpen) {
            showResultsOnOpen = true;
            gameModel = game;
            return;
        }
        
        var results = Results.GetComponent<ShowMinigameResults>();
        var indices = new List<int> {0, 1, 2};
        indices.Shuffle();
        var lieIdx = indices.FindIndex(i => i == 2);
        var selectedIdx = indices.FindIndex(i => i == game.Answer);
        var prompts = new List<string>();
        foreach (var newIdx in indices) {
            prompts.Add(game.Prompts[newIdx]);
        }
        results.Load(game.CurrentPlayer != UserState.Instance.UserId, lieIdx, selectedIdx, prompts);
        ShowState(Results);
    }

    public void ShowWaitingForNewGame() {
        ShowState(WaitingForNewGame);
    }

    private void ShowState(CanvasGroup state) {
        if(state == currentState) return;
        Debug.Log($"Showing state {state.gameObject}");
        if (currentState != null) {
            var oldState = currentState;
            currentState.DOFade(0f, 0.25f).From(1f).OnComplete(()=>oldState.gameObject.SetActive(false));
        }

        currentState = state;
        currentState.gameObject.SetActive(true);
        currentState.DOFade(1f, 0.25f).From(0f);
    }
}
