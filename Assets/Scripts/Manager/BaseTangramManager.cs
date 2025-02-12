using System;
using UnityEngine;

public interface ITangramManager
{
    public void CreateTangram(int pieceCount);
}

public abstract class BaseTangramManager : IProvidable, ITangramManager
{
    #region Protected Variables

        protected GameColorConfig gameColorConfig;
        protected Board board;
        protected IPieceManager pieceManager;

    #endregion

    public abstract void CreateTangram(int pieceCount);

    protected BaseTangramManager()
    {
        ServiceProvider.Register(this);
        pieceManager = ServiceProvider.PieceManager;
        board = ServiceProvider.Board;
    }

    protected void CreateInitialPieces(int pieceCount)
    {
        gameColorConfig = ServiceProvider.AssetLibrary.GetGameColorConfig();

        Color[] selectedColors = new Color[pieceCount];
        gameColorConfig.AvailableColors.Shuffle();
        Array.Copy(gameColorConfig.AvailableColors, selectedColors, pieceCount);

        for (int i = 0; i < pieceCount; i++)
        {
            CreateSinglePiece(selectedColors[i]);
        }
    }

    protected void CreateSinglePiece(Color color)
    {
        var piece = ServiceProvider.AssetLibrary.GetAsset<Piece>(AssetType.Piece);
        var triangle = board.GetRandomTriangle();
        piece.Init(triangle, color);
        pieceManager.AddPiece(piece);
    }

    protected void OnPiecesCreated()
    {
        pieceManager.ArrangeSortingOrders();
        pieceManager.ScatterPieces();
    }
}