using System.Collections.Generic;
using UnityEngine;
public class PieceManager : IProvidable
{
    #region Private Variables

        private List<Piece> activePieces = new();
        private int _placedPieceCounterOnBoard;
        private LevelManager levelManager;
        private SpawnManager spawnManager;

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
        spawnManager = ServiceProvider.SpawnManager;
    }

    public void AddPiece(Piece piece)
    {
        activePieces.Add(piece);
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
            Vector3 spawnPoint = spawnManager.GetSpawnPoint();
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