using UnityEngine;

public class TriangleFactory : MonoBehaviour
{
    public TriangleConfig triangleConfig;
    public Transform TriangleRoot;
    public static TriangleFactory Instance;


    public void Awake()
    {
        triangleConfig = Resources.Load<TriangleConfig>("ScriptableObjects/TriangleConfigSO");

        if(Instance != null && Instance != this ) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    public Triangle GetTriangleFromType(TriangleType type)
    {
        return type switch
        {
            TriangleType.LEFT => Instantiate(triangleConfig.leftTrianglePrefab, TriangleRoot).GetComponent<Triangle>(),
            TriangleType.RIGHT => Instantiate(triangleConfig.rightTrianglePrefab, TriangleRoot).GetComponent<Triangle>(),
            TriangleType.UP => Instantiate(triangleConfig.upperTrianglePrefab, TriangleRoot).GetComponent<Triangle>(),
            TriangleType.DOWN => Instantiate(triangleConfig.bellowTrianglePrefab, TriangleRoot).GetComponent<Triangle>(),
            _ =>  Instantiate(triangleConfig.upperTrianglePrefab, TriangleRoot).GetComponent<Triangle>()
        };
    }
}