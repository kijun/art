using UnityEngine;
using UnityEngine.UI;

public class MeshUtil {
    const string DEFAULT_MESH_NAME = "mesh";
    const string DEFAULT_MATERIAL_NAME = "material";

    public static void UpdateMesh(MeshFilter filt, VertexHelper vh, string meshName = DEFAULT_MESH_NAME) {
        Mesh newMesh = Mesh.Instantiate(filt.sharedMesh);
        vh.FillMesh(newMesh);
        newMesh.name = filt.sharedMesh.name;
        filt.mesh = newMesh;
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

    /*
    void AddRect(Vector2 leftAnchor, float rectWidth, float rectHeight, VertexHelper vh) {
        int vertIdx = vh.currentVertCount;
        float localWidth = rectWidth/Width;
        float localHeight = rectHeight/Height;
        vh.AddVert(new Vector3(leftAnchor.x, leftAnchor.y-localHeight/2, 0), Color.white, TextureMidPoint);
        vh.AddVert(new Vector3(leftAnchor.x, leftAnchor.y+localHeight/2, 0), Color.white, TextureMidPoint);
        vh.AddVert(new Vector3(leftAnchor.x+localWidth, leftAnchor.y-localHeight/2, 0), Color.white, TextureMidPoint);
        vh.AddVert(new Vector3(leftAnchor.x+localWidth, leftAnchor.y+localHeight/2, 0), Color.white, TextureMidPoint);
        vh.AddTriangle(vertIdx, vertIdx+1, vertIdx+2);
        vh.AddTriangle(vertIdx+2, vertIdx+1, vertIdx+3);
    }
    */
}
