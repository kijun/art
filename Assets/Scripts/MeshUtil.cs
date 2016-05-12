using UnityEngine;
using UnityEngine.UI;

public class MeshUtil {
    const string DEFAULT_MESH_NAME = "mesh";
    const string DEFAULT_MATERIAL_NAME = "material";

    public static void UpdateMesh(MeshFilter filt, VertexHelper vh, string meshName = DEFAULT_MESH_NAME) {
        var mesh = new Mesh();
        vh.FillMesh(mesh);
        mesh.name = meshName;
        filt.mesh = mesh;
    }

    public static void UpdateColor(MeshRenderer rend, Color c, string matName = DEFAULT_MATERIAL_NAME) {
        Material newMat = Material.Instantiate(rend.sharedMaterial);
        newMat.color = c;
        newMat.name = rend.sharedMaterial.name;
        rend.material = newMat;
    }

    public static void AddRect(
            Bounds b,
            VertexHelper vh,
            Color vertexColor = default(Color),
            Vector2 uv=default(Vector2))
    {
        uv = new Vector2(0.5f, 0.5f);
        int vertIdx = vh.currentVertCount;
        vh.AddVert(b.BottomLeft(), vertexColor, uv);
        vh.AddVert(b.TopLeft(), vertexColor, uv);
        vh.AddVert(b.BottomRight(), vertexColor, uv);
        vh.AddVert(b.TopRight(), vertexColor, uv);
        vh.AddTriangle(vertIdx, vertIdx+1, vertIdx+2);
        vh.AddTriangle(vertIdx+2, vertIdx+1, vertIdx+3);
    }

    public static void AddTriangle(
        Vector2 p1,
        Vector2 p2,
        Vector2 p3,
        VertexHelper vh,
        Color vertexColor = default(Color),
        Vector2 uv=default(Vector2))
    {
        uv = new Vector2(0.5f, 0.5f);
        int vertIdx = vh.currentVertCount;
        vh.AddVert(p1, vertexColor, uv);
        vh.AddVert(p2, vertexColor, uv);
        vh.AddVert(p3, vertexColor, uv);
        vh.AddTriangle(vertIdx, vertIdx+1, vertIdx+2);
    }

    // two triangles: (1,2,3) (2,3,4)
    public static void AddQuad(
            Vector2 p1,
            Vector2 p2,
            Vector2 p3,
            Vector2 p4,
            VertexHelper vh,
            Color vertexColor = default(Color),
            Vector2 uv=default(Vector2))
    {
        int vertIdx = vh.currentVertCount;
        vh.AddVert(p1, vertexColor, uv);
        vh.AddVert(p2, vertexColor, uv);
        vh.AddVert(p3, vertexColor, uv);
        vh.AddVert(p4, vertexColor, uv);
        vh.AddTriangle(vertIdx, vertIdx+1, vertIdx+2);
        vh.AddTriangle(vertIdx+1, vertIdx+2, vertIdx+3);
    }
}
