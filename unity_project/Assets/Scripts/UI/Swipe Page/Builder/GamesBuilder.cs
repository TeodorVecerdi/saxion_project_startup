using NaughtyAttributes;
using UnityEngine;

public class GamesBuilder : MonoBehaviour, IBuilder {
    [SerializeField, Foldout("Games List")] private GameListItem GameListPrefab;
    [SerializeField, Foldout("Games List")] private RectTransform GameListContainer;
    
    [SerializeField, Foldout("Games Picture")] private GameListItem GamePicturePrefab;
    [SerializeField, Foldout("Games Picture")] private RectTransform GamePictureContainer;
    [SerializeField, Foldout("Games Picture")] private RectTransform GamePictureParent;
    [SerializeField, Foldout("Games Picture")] private GameObject NoGamesText;
    [SerializeField, Foldout("Games Picture")] private GameObject OpenGamesButton;
    [SerializeField, Foldout("Games Picture")] private GameListItem LastItem;
    [SerializeField, Foldout("Games Picture")] private Sprite EmptySprite;
    [SerializeField, Foldout("Games Picture")] private Color EmptyColor;

    private static GameData gameData;

    public void Build(UserModel model) {
        if (gameData == null) {
            gameData = Resources.Load<GameData>("Games");
        }
        
        var games = UserModel.GetEnumIndices(model.PlayedGames);
        games.ForEach(id => {
            var gameItem = Instantiate(GameListPrefab, GameListContainer);
            gameItem.Poster.sprite = gameData.Games[id].GameTexture;
            gameItem.Description.text = gameData.Games[id].GameDescription;
        });
        
        if (games.Count == 0) {
            LastItem.gameObject.SetActive(false);
            GamePictureParent.sizeDelta = new Vector2(1029, 320);
            NoGamesText.SetActive(true);
            OpenGamesButton.SetActive(false);
        } else if (games.Count < 3) {
            var emptyGames = 3 - games.Count;
            for (var i = 0; i < emptyGames; i++) {
                var game = Instantiate(GamePicturePrefab, GamePictureContainer);
                game.Poster.sprite = EmptySprite;
                game.Poster.color = EmptyColor;
            }
            
            for (var i = 0; i < games.Count - 1; i++) {
                var game = Instantiate(GamePicturePrefab, GamePictureContainer);
                game.Poster.sprite = gameData.Games[games[i]].GameTexture;
            }
            LastItem.transform.SetAsLastSibling();
            LastItem.Poster.sprite = gameData.Games[games[games.Count - 1]].GameTexture;
        } else {
            for (var i = 0; i < games.Count-1; i++) {
                var game = Instantiate(GamePicturePrefab, GamePictureContainer);
                game.Poster.sprite = gameData.Games[games[i]].GameTexture;
            }

            LastItem.Poster.sprite = gameData.Games[games[games.Count - 1]].GameTexture;
            LastItem.transform.SetAsLastSibling();
        }
    }

    public void Cleanup() {
        while (GameListContainer.childCount > 0) {
            var child = GameListContainer.GetChild(0);
            child.SetParent(null);
            Destroy(child);
        }
        
        GamePictureParent.sizeDelta = new Vector2(1029, 768);
        LastItem.gameObject.SetActive(true);
        NoGamesText.SetActive(false);
        OpenGamesButton.SetActive(true);
        while (GamePictureContainer.childCount > 1) {
            var child = GamePictureContainer.GetChild(0);
            child.SetParent(null);
            Destroy(child);
        }
    }
}

