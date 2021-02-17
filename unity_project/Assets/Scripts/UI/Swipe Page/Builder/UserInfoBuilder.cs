using TMPro;
using UnityEngine;

public class UserInfoBuilder : MonoBehaviour, IBuilder {
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Age;
    [SerializeField] private TextMeshProUGUI About;

    public void Build(UserModel model) {
        if (Name != null) Name.text = model.Name;
        if (Age != null) {
            var age = Date.Age(model.BirthDate);
            Age.text = $"Age: {age}";
        }

        if (About != null) About.text = model.About;
    }

    public void Cleanup() {
    }
}