using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu]
public class Game : ScriptableObject {
    public int GameID;
    public Sprite GameTexture;
    [ResizableTextArea] public string GameDescription =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam commodo efficitur dapibus. Ut cursus tellus at velit ultrices condimentum. Etiam in dictum nunc. Suspendisse dignissim lacinia diam ut condimentum. Donec interdum volutpat ante, id fermentum velit interdum a. Vestibulum non enim lorem. Duis semper faucibus metus vel suscipit. Vivamus fringilla at nisi sed accumsan.";
}