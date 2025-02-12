using UnityEngine;

public class AssetLibrary : MonoBehaviour, IProvidable
{
    [SerializeField] private GameObject BlockPrefab;

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
            _ => null
        };
    }
}

public enum AssetType
{
    Block,

}