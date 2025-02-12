using System.Collections;
using System.Linq;
using UnityEngine;

public class AnimatedTangramManager : BaseTangramManager
{
    public override void CreateTangram(int pieceCount)
    {
        CreateInitialPieces(pieceCount);
        CoroutineRunner.Instance.StartCoroutine(ProgressPiecesUntilCompleteWithAnimation());
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
        yield return new WaitForSeconds(1f);

        OnPiecesCreated();
        Debug.Log("All pieces are created");
    }
}