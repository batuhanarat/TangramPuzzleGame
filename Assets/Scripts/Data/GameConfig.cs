using UnityEngine;

[CreateAssetMenu(menuName = "BiggerGamesCase/GameConfig")]
public class GameConfig : ScriptableObject
{
    public bool ShouldAnimatePieceCreation;
    public bool ShouldCreateLevelDynamically;
}