/*==========================================
 Title:  Gradient Generator
 Author: Oskar Lindkvist
==========================================*/
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Gradient : MonoBehaviour
{
    public ColorPosition[] colors = new ColorPosition[2];

    MeshFilter meshFilter;
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] vertColors;

    ColorPosition[] sortedColors;

    [HideInInspector] public float width;
    [HideInInspector] public float height;

    public void UpdateGradient(bool useCameraSize)
    {
        if (!meshFilter)
        {
            meshFilter = GetComponent<MeshFilter>();
            if (!meshFilter)
            {
                Debug.LogError("No MeshFilter On Gradient GameObject");
                return;
            }
        }
        if (colors.Length < 2)
        {
            Debug.LogError("Need atleast 2 points for gradient");
            return;
        }
        sortedColors = colors;
        sortedColors = sortedColors.OrderBy(w => w.position).ToArray();

        if(useCameraSize)
            GenerateMesh(Camera.main.aspect * Camera.main.orthographicSize * 2, Camera.main.orthographicSize * 2, sortedColors);
        else
            GenerateMesh(width, height, sortedColors);

        SetColors(sortedColors);
    }

    public void SetColors(ColorPosition[] colors)
    {
        vertColors = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            if (i % 2 == 0)
            {
                vertColors[i] = colors[i / 2].color;
            }
            else
            {
                vertColors[i] = colors[(i - 1) / 2].color;
            }
        }
        meshFilter.sharedMesh.colors = vertColors;
    }


    void GenerateMesh(float w, float h, ColorPosition[] positions)
    {
        int p = positions.Length;

        mesh = new Mesh();
        vertices = new Vector3[p * 2];
        triangles = new int[(p - 1) * 6];

        for (int i = 0; i < p; i++)
        {
            float height = positions[i].position * h - h / 2;
            float width = w / 2;
            vertices[2*i] = new Vector3(-width, height, 0);
            vertices[2*i+1] = new Vector3(width, height, 0);
        }

        for (int i = 0; i < p-1; i++) // Clockwise mesh winding
        {
            triangles[6 * i + 0] = 2 * i;
            triangles[6 * i + 1] = 2 * i + 3;
            triangles[6 * i + 2] = 2 * i + 1;
            triangles[6 * i + 3] = 2 * i;
            triangles[6 * i + 4] = 2 * i + 2;
            triangles[6 * i + 5] = 2 * i + 3;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        meshFilter.sharedMesh = mesh;
    }

    [System.Serializable]
    public class ColorPosition
    {
        public Color color;
        [Tooltip("Height Position (0=bottom, 1=top)")]
        [Range(0f, 1f)] public float position;
    }
}
