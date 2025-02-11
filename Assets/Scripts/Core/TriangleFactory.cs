using UnityEngine;

public class TriangleFactory : MonoBehaviour
{
    public TriangleConfig triangleConfig;
    public Transform TriangleRoot;
    public static TriangleFactory Instance;


    public void Awake()
    {
        triangleConfig = Resources.Load<TriangleConfig>("ScriptableObjects/TriangleConfig");

        if(Instance != null && Instance != this ) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
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