using UnityEditor.VersionControl;
using UnityEngine;

public class AssetLibrary : MonoBehaviour, IProvidable
{
    [Header("Prefabs")]
    [SerializeField] private GameObject BlockPrefab;
    [SerializeField] private GameObject PiecePrefab;
    [SerializeField] private GameObject BoardPrefab;


    [Header("Configs")]
    [SerializeField] private TriangleConfig triangleConfig;
    [SerializeField] private GameColorConfig gameColorConfig;


    private  void Awake()
    {
        ServiceProvider.Register(this);
    }

    public T GetAsset<T>(AssetType assetType) where T : class
    {
        var asset = GetAsset(assetType);
        return asset == null ? null : asset.GetComponent<T>();
    }

    private GameObject GetAsset(AssetType assetType)
    {
        return assetType switch
        {
            AssetType.Block => Instantiate(BlockPrefab),
            AssetType.Piece => Instantiate(PiecePrefab),
            AssetType.Board => Instantiate(BoardPrefab),
            AssetType.RightTriangle => Instantiate(triangleConfig.rightTrianglePrefab),
            AssetType.LeftTriangle => Instantiate(triangleConfig.leftTrianglePrefab),
            AssetType.UpTriangle => Instantiate(triangleConfig.upperTrianglePrefab),
            AssetType.DownTriangle => Instantiate(triangleConfig.bellowTrianglePrefab),

            _ => null
        };
    }
    public GameColorConfig GetGameColorConfig()
    {
        return gameColorConfig;
    }
}

public enum AssetType
{
    Block,
    Piece,
    Board,
    RightTriangle,
    LeftTriangle,
    UpTriangle,
    DownTriangle,
}