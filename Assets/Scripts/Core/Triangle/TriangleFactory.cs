using UnityEngine;

public class TriangleFactory : MonoBehaviour, IProvidable
{
    #region Private Variables

        private TriangleConfig triangleConfig;

    #endregion

    #region Serialized Variables

        [SerializeField] public Transform TriangleRoot;

    #endregion


    public void Awake()
    {
        ServiceProvider.Register(this);
        triangleConfig = ServiceProvider.TriangleConfig;

    }

    public Triangle GetTriangleFromType(TriangleType type, Vector3 position)
    {
        return type switch
        {
            TriangleType.LEFT => Instantiate(triangleConfig.leftTrianglePrefab, position, Quaternion.identity).GetComponent<Triangle>(),
            TriangleType.RIGHT => Instantiate(triangleConfig.rightTrianglePrefab, position, Quaternion.identity).GetComponent<Triangle>(),
            TriangleType.UP => Instantiate(triangleConfig.upperTrianglePrefab, position, Quaternion.identity).GetComponent<Triangle>(),
            TriangleType.DOWN => Instantiate(triangleConfig.bellowTrianglePrefab, position, Quaternion.identity).GetComponent<Triangle>(),
            _ =>  Instantiate(triangleConfig.upperTrianglePrefab, position, Quaternion.identity).GetComponent<Triangle>()
        };
    }
}