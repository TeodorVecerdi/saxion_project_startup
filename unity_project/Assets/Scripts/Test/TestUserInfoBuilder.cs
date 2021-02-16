using TMPro;
using UnityEngine;

    public class TestUserInfoBuilder : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI Name;
        [SerializeField] private TextMeshProUGUI Age;

        public void Build(UserModel model) {
            Name.text = model.Name;
            var age = Date.Age(model.BirthDate);
            Age.text = $"Age: {age}";
        }
    }