
using UnityEngine;

public class SpawnManager : IProvidable
{

    #region Private Variables

        private Bounds _spawnBounds;
        private Bounds _innerSpawnBounds;
        private bool _isSpawnBoundsInitialized;

    #endregion

    public SpawnManager()
    {
        ServiceProvider.Register(this);
    }

    private void InitializeSpawnArea()
    {
        SpriteRenderer boardSprite =  ServiceProvider.BoardRenderer.GetComponent<SpriteRenderer>();
        Vector2 boardSize = boardSprite.bounds.size;
        Vector3 boardPosition =  ServiceProvider.BoardRenderer.transform.position;

        float spawnAreaHeight = boardSize.y;
        float spawnAreaWidth = boardSize.x;

        float offsetY = -(boardSize.y / 2 + boardSize.y * 0.1f + spawnAreaHeight / 2);
        Vector3 spawnAreaCenter = boardPosition + new Vector3(0, offsetY, 0);

        _spawnBounds = new Bounds(spawnAreaCenter, new Vector3(spawnAreaWidth, spawnAreaHeight, 1));

        float innerWidth = spawnAreaWidth / 2;
        float innerHeight = spawnAreaHeight / 2;

        _innerSpawnBounds = new Bounds(spawnAreaCenter, new Vector3(innerWidth, innerHeight, 1));


    }

    public Vector3 GetSpawnPoint()
    {
        if(!_isSpawnBoundsInitialized){
            InitializeSpawnArea();
            _isSpawnBoundsInitialized = true;
        }

        float randomX = Random.Range(_innerSpawnBounds.min.x, _innerSpawnBounds.max.x);
        float randomY = Random.Range(_innerSpawnBounds.min.y, _innerSpawnBounds.max.y);

        return new Vector3(randomX, randomY, 0);
    }
}