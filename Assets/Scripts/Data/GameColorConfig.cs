using UnityEngine;

[CreateAssetMenu(menuName = "BiggerGamesCase/GameColorConfig")]
public class GameColorConfig : ScriptableObject
{
    [Header("Game Colors")]
    [Tooltip("All available colors for game pieces")]
    [SerializeField] private Color[] availableColors = {
        new Color(0xE2/255f, 0x43/255f, 0x43/255f),
        new Color(0xC2/255f, 0xFF/255f, 0xB3/255f),
        new Color(0x43/255f, 0xD2/255f, 0xE2/255f),
        new Color(0xE2/255f, 0xE2/255f, 0x43/255f),
        new Color(0x98/255f, 0x43/255f, 0xE2/255f),
        new Color(0xE2/255f, 0x8D/255f, 0x43/255f)
    };
    public Color[] AvailableColors => availableColors;



}