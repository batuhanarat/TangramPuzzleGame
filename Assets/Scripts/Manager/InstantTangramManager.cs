
using System.Collections.Generic;
using System.Linq;

public class InstantTangramManager : BaseTangramManager
{
    public override void CreateTangram(int pieceCount)
    {
        CreateInitialPieces(pieceCount);
        ProgressPiecesUntilComplete();
        OnPiecesCreated();
    }

    private void ProgressPiecesUntilComplete()
    {
        var piecesInProgress = pieceManager.GetActivePieces().ToList();
        while (piecesInProgress.Any())
        {
            ProcessPieceProgression(piecesInProgress);
            if (!piecesInProgress.Any())
            {
                //Debug.Log("All pieces are created");
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
}