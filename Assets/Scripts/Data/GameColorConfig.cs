using UnityEngine;

[CreateAssetMenu(menuName = "BiggerGamesCase/GameColorConfig")]
public class GameColorConfig : ScriptableObject
{
    [Header("Game Colors")]
    [Tooltip("All available colors for game pieces")]
    [SerializeField] private Color[] availableColors = {

    };
    public Color[] AvailableColors => availableColors;



}