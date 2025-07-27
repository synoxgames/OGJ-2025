using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SelectedArt")]
public class SelectedArt : ScriptableObject
{
    public Texture2D artTexture;
    public string artName;

    public float upperBound; // this is the upper bound for the accuracy threshold
    public float lowerBound; // this is the lower bound for the accuracy threshold

    public float moneyMultiplier = 1f; // multiplier for the money earned from this art per accuracy point
    public Sprite GetSprite()
    {
        if (artTexture == null)
            return null;

        return Sprite.Create(
            artTexture,
            new Rect(0, 0, artTexture.width, artTexture.height),
            new Vector2(0.5f, 0.5f)
        );
    }
}