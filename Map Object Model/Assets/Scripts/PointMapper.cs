using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class PointMapper : MonoBehaviour
{
    public string csvFilePath = "Assets/Data/Dataset.csv";      // Path to the CSV file
    //public char delimiter = ',';          // CSV file delimiter
    public float scale;             // Scale of the points
    public Material pointMaterial;  // Material for the points

    private HashSet<Vector2> latLongSet;   // HashSet to store unique lat-long pairs
    private List<Vector3> worldCoordinates;  // List to store world coordinates

    void Start()
    {
        // Initialize the HashSet and the List
        latLongSet = new HashSet<Vector2>();
        worldCoordinates = new List<Vector3>();

        // Read the CSV file and convert the lat-long pairs to world coordinates
        using (StreamReader reader = new StreamReader(csvFilePath))
        {
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');
                if (values[3].Length == 0)
                {
                    continue;
                }
                else
                {
                    float lat = float.Parse(values[3]);
                    float lon = float.Parse(values[4]);
                    Vector2 latLong = new Vector2(lat, lon);
                    if (latLongSet.Add(latLong))
                    {
                        Vector3 worldCoord = LatLongToWorldCoord(latLong);
                        worldCoordinates.Add(worldCoord);
                    }
                }
            }
        }

        // Create a mesh to hold the points
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = worldCoordinates.ToArray();
        int[] indices = new int[worldCoordinates.Count];
        for (int i = 0; i < worldCoordinates.Count; i++)
        {
            indices[i] = i;
        }
        mesh.SetIndices(indices, MeshTopology.Points, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // Create a GameObject to hold the mesh
        GameObject pointCloud = new GameObject("PointCloud");
        pointCloud.transform.localScale = new Vector3(scale, scale, scale);
        pointCloud.transform.SetParent(transform);

        // Add a MeshFilter and MeshRenderer to the GameObject
        MeshFilter meshFilter = pointCloud.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = pointCloud.AddComponent<MeshRenderer>();
        meshRenderer.material = pointMaterial;
    }

    void Update()
    {
        // Rotate the point cloud around the Y-axis
        transform.Rotate(Vector3.up, Time.deltaTime * 10f);
    }

    private Vector3 LatLongToWorldCoord(Vector2 latLong)
    {
        // Convert the lat-long pair to a world coordinate using the Mercator projection
        float x = (latLong.y + 180f) / 360f;
        float y = Mathf.Log(Mathf.Tan((latLong.x + 90f) * Mathf.Deg2Rad / 2f)) / Mathf.PI + 0.5f;
        return new Vector3(x, y, 0f);
    }
}
