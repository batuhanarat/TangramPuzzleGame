using System;
using UnityEngine;

public class TriangleFactory :  IProvidable
{
    #region Private Variables

        private TriangleConfig _triangleConfig;

    #endregion

    #region Serialized Variables

        [SerializeField] public Transform TriangleRoot;

    #endregion

    public TriangleFactory()
    {
        ServiceProvider.Register(this);
    }


    public Triangle CreateTriangle(TriangleType type, Vector3 position)
    {
        AssetType assetType = type switch
        {
            TriangleType.LEFT => AssetType.LeftTriangle,
            TriangleType.RIGHT => AssetType.RightTriangle,
            TriangleType.UP => AssetType.UpTriangle,
            TriangleType.DOWN => AssetType.DownTriangle,
            _ => throw new NotImplementedException(),
        };

        Triangle triangle = ServiceProvider.AssetLibrary.GetAsset<Triangle>(assetType);
        triangle.transform.SetPositionAndRotation(position, Quaternion.identity);
        return triangle;
    }

}