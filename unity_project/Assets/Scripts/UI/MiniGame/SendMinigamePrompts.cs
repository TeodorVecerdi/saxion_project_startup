using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SendMinigamePrompts : MonoBehaviour {
    [SerializeField] private MinigameManager Manager;
    [SerializeField] private TMP_InputField Truth1;
    [SerializeField] private TMP_InputField Truth2;
    [SerializeField] private TMP_InputField Lie;
    
    public void OnClick() {
        var truth1 = Truth1.text.Trim();
        var truth2 = Truth2.text.Trim();
        var lie = Lie.text.Trim();
        if (!string.IsNullOrWhiteSpace(truth1) && !string.IsNullOrWhiteSpace(truth2) && !string.IsNullOrWhiteSpace(lie)) {
            Manager.SubmitPrompts(new List<string> {truth1, truth2, lie});
        }
    }
}