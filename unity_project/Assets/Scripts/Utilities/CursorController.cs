using System;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {
    public static CursorController Instance { get; private set; }

    public List<Cursor> Cursors;
    public int CursorSize = 48;
    public CursorState State { get; private set; } = CursorState.Default;
    
    private void Awake() {
        DontDestroyOnLoad(gameObject);
        if (Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        for (var i = 0; i < Cursors.Count; i++) {
            var cursor = Cursors[i];
            cursor.Texture = MiscUtils.ResizeTexture(cursor.Texture, CursorSize, CursorSize, true, false);
            Cursors[i] = cursor;
        }
    }

    public void Game() { State = CursorState.Game; }
    public void Default() { State = CursorState.Default; }
    public void Link() { State = CursorState.Link; }

    private void Update() {
        SetCursor(Cursors.Find(cursor => cursor.State == State));
    }

    private void SetCursor(Cursor cursor) {
        var hotspot = new Vector2(cursor.Texture.width * cursor.Hotspot.x, cursor.Texture.height * cursor.Hotspot.y);
        UnityEngine.Cursor.SetCursor(cursor.Texture, hotspot, CursorMode.ForceSoftware);
    }

    public enum CursorState {
        Game,
        Default,
        Link
    }

    [Serializable]
    public struct Cursor {
        public CursorState State;
        public Texture2D Texture;
        public Vector2 Hotspot;
    }
}