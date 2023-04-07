using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PointMapper : MonoBehaviour
{
    public GameObject worldMap;
    public GameObject whale;

    private List<Vector3> points;
    private float worldMapWidth;
    private float worldMapHeight;
    private float latitudeScale;
    private float longitudeScale;

    void Start()
    {
        points = new List<Vector3>();

        // Load CSV file
        string filePath = Application.dataPath + "/Data/Dataset.csv";
        //string[] lines = File.ReadAllLines(filePath);
        using (StreamReader reader = new StreamReader(filePath))
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
                    float lon= float.Parse(values[4]);
                    Vector3 point = ProjectLatLongToMap(lat,lon);
                    points.Add(point);
                }   
            }
        }

            // Parse CSV data
            /*   for (int i = 0; i < lines.Length; i++)
               {

                   string[] fields = lines[i].Split(',');
                   float longitude = float.Parse(fields[3]);
                   float latitude = float.Parse(fields[4]);
                   Vector3 point = ProjectLatLongToMap(longitude, latitude);

                   points.Add(point);
               } */

            // Set line renderer positions
            LineRenderer lineRenderer = worldMap.GetComponent<LineRenderer>();
        lineRenderer.positionCount = points.Count;
        

        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
        //DrawPath(lineRenderer);
        // Set initial position of whale
        whale.transform.position = points[0];
    }

    Vector3 ProjectLatLongToMap(float longitude, float latitude)
    {
        float x = (longitude + 180f) * (worldMapWidth / 360f);
        float y = (latitude + 90f) * (worldMapHeight / 180f);

        return new Vector3(x, 1f, y);
    }
    /*void DrawPath(LineRenderer lineRenderer)
    {
        // Set the line width
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Set the number of points to draw
        lineRenderer.positionCount = points.Count;

        // Loop through the path points and set the positions of the line renderer
        for (int i = 0; i < points.Count; i++)
        {
            // Convert the lat-long to world coordinates using the world map's scale and position
            Vector3 worldPoint = new Vector3(points[i].y / 180f * worldMap.transform.localScale.x - worldMap.transform.position.x, 0f, points[i].x / 90f * worldMap.transform.localScale.y - worldMap.transform.position.z);
            lineRenderer.SetPosition(i, worldPoint);
        }
    } */
    void Awake()
    {
        // Get dimensions of world map
        Renderer worldMapRenderer = worldMap.GetComponent<Renderer>();
        worldMapWidth = 60f;
        worldMapHeight = 50f;
        //worldMapWidth = worldMapRenderer.bounds.size.x;
        //worldMapHeight = worldMapRenderer.bounds.size.y;
        

        // Calculate scaling factors for latitude and longitude
        latitudeScale = worldMapHeight / 180f;
        longitudeScale = worldMapWidth / 360f;
    }
}