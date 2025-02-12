using UnityEngine;

public class BlockRenderer : MonoBehaviour
{
    #region Serialized Variables

        [SerializeField] private SpriteRenderer blockSpriteRenderer;

    #endregion


    public void Configure(Vector3 position, float cellSize)
    {
        SetPosition(position);
        SetScale(cellSize);
    }

    private void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void SetScale(float cellSize)
    {
        float scale  = cellSize / blockSpriteRenderer.sprite.bounds.size.x;
        transform.localScale = new Vector3(scale, scale, 1f);
    }

}