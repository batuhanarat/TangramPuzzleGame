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
    [SerializeField] private LevelConfig levelConfig;


    private Transform _blocksRoot;
    private Transform _piecesRoot;

    private  void Awake()
    {
        ServiceProvider.Register(this);

        _blocksRoot = new GameObject("---Blocks---").transform;
        _piecesRoot = new GameObject("---Pieces---").transform;
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
            AssetType.Block => Instantiate(BlockPrefab, _blocksRoot),
            AssetType.Piece => Instantiate(PiecePrefab, _piecesRoot),
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
    public LevelConfig GetLevelConfig()
    {
        return levelConfig;
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