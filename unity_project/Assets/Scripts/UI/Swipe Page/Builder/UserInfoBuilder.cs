using TMPro;
using UnityEngine;

public class UserInfoBuilder : MonoBehaviour, IBuilder {
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Age;
    [SerializeField] private TextMeshProUGUI About;

    public void Build(UserModel model) {
        Name.text = model.Name;
        var age = Date.Age(model.BirthDate);
        Age.text = $"Age: {age}";
        About.text = model.About;
    }

    public void Cleanup() {
    }
}