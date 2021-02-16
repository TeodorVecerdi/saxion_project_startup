using System.Collections.Generic;
using UnityCommons;
using UnityEngine;

public class TestGameListBuilder2 : MonoBehaviour {
    [SerializeField] private RectTransform TargetTransform;
    [SerializeField] private RectTransform Container;
    [SerializeField] private GameObject NoGamesText;
    [SerializeField] private GameObject OpenGamesButton;
    [Space]
    [SerializeField] private GameListItem Prefab;
    [SerializeField] private GameListItem LastItem;
    [SerializeField] private List<Game> GameData;
    [Space]
    [SerializeField] private Sprite EmptySprite;
    [SerializeField] private Color EmptyColor;

    public void Build(UserModel model) {
        var games = UserModel.GetEnumIndices(model.PlayedGames);
        if (games.Count == 0) {
            LastItem.gameObject.SetActive(false);
            Container.sizeDelta = new Vector2(1029, 320);
            NoGamesText.SetActive(true);
            OpenGamesButton.SetActive(false);
        } else if (games.Count < 3) {
            var emptyGames = 3 - games.Count;
            for (var i = 0; i < emptyGames; i++) {
                var game = Instantiate(Prefab, TargetTransform);
                game.Poster.sprite = EmptySprite;
                game.Poster.color = EmptyColor;
            }
            
            for (var i = 0; i < games.Count - 1; i++) {
                var game = Instantiate(Prefab, TargetTransform);
                game.Poster.sprite = GameData[games[i]].GameTexture;
            }
            LastItem.transform.SetAsLastSibling();
            LastItem.Poster.sprite = GameData[games[games.Count - 1]].GameTexture;
        } else {
            for (var i = 0; i < games.Count-1; i++) {
                var game = Instantiate(Prefab, TargetTransform);
                game.Poster.sprite = GameData[games[i]].GameTexture;
            }

            LastItem.Poster.sprite = GameData[games[games.Count - 1]].GameTexture;
            LastItem.transform.SetAsLastSibling();
        }
    }
    
    private void Start() {
        GameData.QuickSort((game, game1) => game.GameID.CompareTo(game1.GameID));
    }
}

