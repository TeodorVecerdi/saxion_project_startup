using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleGroup : MonoBehaviour {
    [SerializeField] private List<Toggle> Toggles;
    [SerializeField] private bool CanTogglesBeDisabled;
    [SerializeField] private bool AllowMultiple;
    [SerializeField] private UnityEvent<int> OnChange;

    private int groupMask;

    public void RequestToggle(Toggle toggle) {
        var index = Toggles.IndexOf(toggle);
        if(index == -1) return;
        RequestToggle(index);
    }

    public void RequestToggle(int index) {
        bool currentToggleValue = Toggles[index].IsActive;
        if(currentToggleValue && !CanTogglesBeDisabled) return;
        
        if (!AllowMultiple) {
            for (var i = 0; i < Toggles.Count; i++) {
                if(i == index) continue;
                Toggles[i].SetState(false);
            }
        }
        Toggles[index].SetState(!currentToggleValue);

        UpdateToggleValue();
        OnChange?.Invoke(groupMask);
    }

    private void UpdateToggleValue() {
        groupMask = 0;
        for (var i = 0; i < Toggles.Count; i++) {
            bool isActive = Toggles[i].IsActive;
            if (isActive) groupMask |= 1 << i;
        }
    }
}