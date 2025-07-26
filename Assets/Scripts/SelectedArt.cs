using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SelectedArt")]
public class SelectedArt : ScriptableObject
{
    public Sprite selectedArt;
    public Texture2D artTexture;
    public string artName;
}