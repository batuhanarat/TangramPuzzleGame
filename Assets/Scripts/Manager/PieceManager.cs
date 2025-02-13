using System.Collections.Generic;
using UnityEngine;


public interface IPieceManager
{
    public int InPlacedSortingOrder { get; }
    public int OnDraggedSortingOrder { get; }

    public void AddPiece(Piece piece);
    public IReadOnlyList<Piece> GetActivePieces();
    public void OnPiecePlaced();
    public void OnPieceRemoved();
    public void ScatterPieces();
    public void ArrangeSortingOrders();
    public void Reset();
}

public class PieceManager : IProvidable, IPieceManager
{
    #region Private Variables

        private List<Piece> _activePieces = new();
        private int _placedPieceCounterOnBoard;
        private ILevelManager _levelManager;
        private ISpawnManager _spawnManager;

    #endregion

    #region Properties

        public int InPlacedSortingOrder { get =>  -1 ;}
        public int OnDraggedSortingOrder { get =>  _activePieces.Count + 1; }
        private bool IsLevelCompleted { get => _placedPieceCounterOnBoard == _activePieces.Count; }

    #endregion


    public PieceManager()
    {
        ServiceProvider.Register(this);
        _levelManager = ServiceProvider.LevelManager;
        _spawnManager = ServiceProvider.SpawnManager;
    }

    public void AddPiece(Piece piece)
    {
        _activePieces.Add(piece);
    }

    public IReadOnlyList<Piece> GetActivePieces()
    {
        return _activePieces.AsReadOnly();
    }

    public void ScatterPieces()
    {
        for(int i = 0 ; i < _activePieces.Count ; i++)
        {
            var piece = _activePieces[i];

            var zValues = piece.transform.position.z;
            Vector3 spawnPoint = _spawnManager.GetSpawnPoint();
            piece.SetPosition(spawnPoint - piece.InitialPositionOffset);
            piece.transform.position += new Vector3(0,0,zValues);
        }
    }

    public void ArrangeSortingOrders()
    {
        for (int i = 0; i < _activePieces.Count; i++)
        {
            _activePieces[i].SetSortingOrder(i);
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
            _levelManager.PrepareNextLevel();
            Debug.Log(" Level Won !");
        }
    }

    public void Reset()
    {
        _activePieces.Clear();
        _placedPieceCounterOnBoard = 0;
    }



}