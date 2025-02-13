using UnityEngine;

[CreateAssetMenu(menuName = "BiggerGamesCase/GameConfig")]
public class GameConfig : ScriptableObject
{
    public bool IsPieceCreationAnimated;
    public bool ShouldCreateLevelDynamically;
}