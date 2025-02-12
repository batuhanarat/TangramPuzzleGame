using System.Collections.Generic;
using UnityEngine;
public class PieceManager : IProvidable
{
    #region Private Variables
        private List<Piece> activePieces = new();
        private int _placedPieceCounterOnBoard;
        private LevelManager levelManager;
        private List<Vector3> _spawnPositions = new List<Vector3>
        {
            new(0.615835726f, -3.57771254f, 0f),
            new(-2.17008781f, -5.54252195f, 0f),
            new (1.73020518f, -6.26099682f, 0f),
            new (1.9941349f, -3.84164238f, 0f),
            new (0.615835726f, -3.57771254f, 0f),
            new (-2.17008781f, -5.54252195f, 0f),
            new (1.73020518f, -6.26099682f, 0f),
            new (1.9941349f, -3.84164238f, 0f),
            new (0.615835726f, -3.57771254f, 0f),
            new (-2.17008781f, -5.54252195f, 0f),
            new (1.73020518f, -6.26099682f, 0f),
            new (1.9941349f, -3.84164238f, 0f)
        };
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

    public IReadOnlyList<Piece> GetActivePieces()
    {
        return activePieces.AsReadOnly();
    }

    public void ShufflePieces()
    {

        for(int i = 0 ; i < activePieces.Count ; i++)
        {
            var zValues = activePieces[i].transform.position.z;
            activePieces[i].transform.position = _spawnPositions[i];
            activePieces[i].transform.position = activePieces[i].transform.position  + new Vector3(0,0,zValues);
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
            Debug.Log("Kazandık !");
        }
    }

    public void Reset()
    {
        activePieces.Clear();
        _placedPieceCounterOnBoard = 0;
    }

}