using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class TangramManager : IProvidable
{
    #region Private variables

        private readonly GameColorConfig gameColorConfig;
        private Board board;
        private PieceManager pieceManager;

    #endregion

    public TangramManager()
    {
        ServiceProvider.Register(this);
        pieceManager = ServiceProvider.PieceManager;
        board = ServiceProvider.Board;
        gameColorConfig = ServiceProvider.GameColorConfig;
    }

    private IEnumerator ProgressPiecesUntilCompleteWithAnimation()
    {
        var piecesInProgress = pieceManager.GetActivePieces().ToList();

        while (piecesInProgress.Any())
        {
            for (int i = piecesInProgress.Count - 1; i >= 0; i--)
            {
                var progressRoutine = piecesInProgress[i].TryProgressWithAnimation();
                bool canContinue = false;

                while (progressRoutine.MoveNext())
                {
                    if (progressRoutine.Current != null)
                    {
                        yield return progressRoutine.Current;
                        canContinue = true;
                    }
                }

                if (!canContinue)
                {
                    piecesInProgress.RemoveAt(i);
                }
            }
        }

        Debug.Log("All pieces are created");
        ArrangePieces();
    }


    public void CreateTangram(int pieceCount)
    {
        CreateInitialPieces(pieceCount);
        ProgressPiecesUntilComplete();
        ArrangePieces();
    }

    public void CreateTangramWithAnimation(int pieceCount)
    {
        CreateInitialPieces(pieceCount);
        CoroutineRunner.Instance.StartCoroutine(ProgressPiecesUntilCompleteWithAnimation());
    }

    private void CreateInitialPieces(int pieceCount)
    {
        Color[] selectedColors = new Color[pieceCount];
        gameColorConfig.AvailableColors.Shuffle();
        Array.Copy(gameColorConfig.AvailableColors, selectedColors, pieceCount);

        for (int i = 0; i < pieceCount; i++)
        {
            CreateSinglePiece(selectedColors[i]);
        }
    }

    private void CreateSinglePiece(Color color)
    {
        var pieceGO = new GameObject("Piece");
        var piece = pieceGO.AddComponent<Piece>();
        var triangle = board.GetRandomTriangle();

        piece.Init(triangle, color);
        pieceManager.AddPiece(piece);
    }

    private void ProgressPiecesUntilComplete()
    {
        var piecesInProgress = pieceManager.GetActivePieces().ToList();

        while (piecesInProgress.Any())
        {
            ProcessPieceProgression(piecesInProgress);

            if (!piecesInProgress.Any())
            {
                Debug.Log("All pieces are created");
                break;
            }
        }
    }

    private void ProcessPieceProgression(List<Piece> piecesInProgress)
    {
        for (int i = piecesInProgress.Count - 1; i >= 0; i--)
        {
            if (!piecesInProgress[i].TryProgress())
            {
                piecesInProgress.RemoveAt(i);
            }
        }
    }

    private void ArrangePieces()
    {
        pieceManager.ArrangeSortingOrders();
        pieceManager.ShufflePieces();
    }

}