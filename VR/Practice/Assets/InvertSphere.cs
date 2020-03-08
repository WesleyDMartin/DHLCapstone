using UnityEngine;

public class InvertSphere : MonoBehaviour
{
    private void Start()
    {
        var filter = GetComponent(typeof(MeshFilter)) as MeshFilter;
        if (filter != null)
        {
            var mesh = filter.mesh;

            var normals = mesh.normals;
            for (var i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            mesh.normals = normals;

            for (var m = 0; m < mesh.subMeshCount; m++)
            {
                var triangles = mesh.GetTriangles(m);
                for (var i = 0; i < triangles.Length; i += 3)
                {
                    var temp = triangles[i + 0];
                    triangles[i + 0] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }

                mesh.SetTriangles(triangles, m);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}