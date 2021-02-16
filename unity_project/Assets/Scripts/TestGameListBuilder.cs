using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;

public class TestGameListBuilder : MonoBehaviour {
    [SerializeField] private List<Game> GameData;
    [SerializeField] private List<int> GameIDs;
    [SerializeField] private GameListItem GamePrefab;
    [SerializeField] private RectTransform TargetTransform;

    private void Start() {
        if (TargetTransform == null) TargetTransform = GetComponent<RectTransform>();
        
        GameData.QuickSort((game, game1) => game.GameID.CompareTo(game1.GameID));
        
        GameIDs.ForEach(id => {
            var gameItem = Instantiate(GamePrefab, TargetTransform);
            gameItem.Poster.sprite = GameData[id].GameTexture;
            gameItem.Description.text = GameData[id].GameDescription;
        });
    }
}

