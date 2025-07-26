using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SelectedArt")]
public class SelectedArt : ScriptableObject
{
    public Texture2D artTexture;
    public string artName;

    public Sprite GetSprite()
    {
        if (selectedTexture == null)
            return null;

        return Sprite.Create(
            selectedTexture,
            new Rect(0, 0, selectedTexture.width, selectedTexture.height),
            new Vector2(0.5f, 0.5f)
        );
    }
}