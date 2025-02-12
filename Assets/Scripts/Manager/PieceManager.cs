using System.Collections.Generic;
using UnityEngine;
public class PieceManager : IProvidable
{
    #region Private Variables
        private List<Piece> activePieces = new();
        private int _placedPieceCounterOnBoard;
        private LevelManager levelManager;
        public Bounds spawnBounds;
        public Bounds innerSpawnBounds;

        private bool IsSpawnBoundsInitialized;
    #endregion

    #region Properties

        public int InPlacedSortingOrder { get =>  -1 ;}
        public bool IsLevelCompleted { get => _placedPieceCounterOnBoard == activePieces.Count; }
        public int OnDraggedSortingOrder { get =>  activePieces.Count + 1; }

    #endregion


    public PieceManager()
    {
        ServiceProvider.Register(this);
        levelManager = ServiceProvider.LevelManager;
    }

    public void AddPiece(Piece piece)
    {
        activePieces.Add(piece);
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

        spawnBounds = new Bounds(spawnAreaCenter, new Vector3(spawnAreaWidth, spawnAreaHeight, 1));

        float innerWidth = spawnAreaWidth / 2;
        float innerHeight = spawnAreaHeight / 2;

        innerSpawnBounds = new Bounds(spawnAreaCenter, new Vector3(innerWidth, innerHeight, 1));


    }

    public Vector3 GetSpawnPoint()
    {
        if(!IsSpawnBoundsInitialized){
            InitializeSpawnArea();
            IsSpawnBoundsInitialized = true;
        }

        float randomX = Random.Range(innerSpawnBounds.min.x, innerSpawnBounds.max.x);
        float randomY = Random.Range(innerSpawnBounds.min.y, innerSpawnBounds.max.y);

        return new Vector3(randomX, randomY, 0);
    }

    public IReadOnlyList<Piece> GetActivePieces()
    {
        return activePieces.AsReadOnly();
    }

    public void ShufflePieces()
    {

        for(int i = 0 ; i < activePieces.Count ; i++)
        {
            var piece = activePieces[i];

            var zValues = piece.transform.position.z;
            Vector3 spawnPoint = GetSpawnPoint();
            piece.SetPosition(spawnPoint - piece.InitialPositionOffset);
            piece.transform.position += new Vector3(0,0,zValues);
        }
    }

    public void ArrangeSortingOrders()
    {
        for (int i = 0; i < activePieces.Count; i++)
        {
            activePieces[i].SetSortingOrder(i);
        }
    }

    public void OnPieceRemoved()
    {
        _placedPieceCounterOnBoard--;
    }

    public void OnPiecePlaced()
    {
        _placedPieceCounterOnBoard++;
        if(IsLevelCompleted)
        {
            levelManager.PrepareNextLevel();
            Debug.Log(" Level Won !");
        }
    }

    public void Reset()
    {
        activePieces.Clear();
        _placedPieceCounterOnBoard = 0;
    }



}