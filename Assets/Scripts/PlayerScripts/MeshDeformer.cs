using UnityEngine;

public class MeshDeformer : MonoBehaviour
{
    private LineRenderer lr;
    public MapGenerator mapGenerator;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();

        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
    }

    private void Update()
    {
        lr.SetPosition(0, transform.position);

        if (Input.GetMouseButton(0))
        {
            lr.enabled = true;

            if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.forward), out RaycastHit hit, 100) && hit.transform.GetComponent<MeshFilter>() != null)
            {
                Mesh mesh = hit.transform.GetComponent<MeshFilter>().sharedMesh;
                Vector3[] vertices = mesh.vertices;
                int[] triangles = mesh.triangles;

                int closestVertex0 = GetClosestVertex(hit, mesh.triangles);
                int closestVertex1 = 0;
                int closestVertex2 = 0;

                switch(closestVertex0 % 3)
                {
                    case 0:
                        closestVertex1 = closestVertex0 + 1;
                        closestVertex2 = closestVertex0 + 2;
                        break;

                    case 1:
                        closestVertex1 = closestVertex0 + 1;
                        closestVertex2 = closestVertex0 - 1;
                        break;

                    case 2:
                        closestVertex1 = closestVertex0 - 1;
                        closestVertex2 = closestVertex0 - 2;
                        break;
                }

                vertices[closestVertex0] += -transform.forward / 10;
                vertices[closestVertex1] += -transform.forward / 10;
                vertices[closestVertex2] += -transform.forward / 10;

                mesh.vertices = vertices;

                lr.SetPosition(1, hit.point);
            }
            else
            {
                lr.SetPosition(1, transform.parent.parent.forward * 1000);
            }
        }
        else
        {
            if (lr.enabled)
            {
                lr.enabled = false;
            }
        }
    }

    public static int GetClosestVertex(RaycastHit aHit, int[] aTriangles)
    {
        var b = aHit.barycentricCoordinate;
        int index = aHit.triangleIndex * 3;
        if (aTriangles == null || index < 0 || index + 2 >= aTriangles.Length)
            return -1;
        if (b.x > b.y)
        {
            if (b.x > b.z)
                return aTriangles[index]; // x
            else
                return aTriangles[index + 2]; // z
        }
        else if (b.y > b.z)
            return aTriangles[index + 1]; // y
        else
            return aTriangles[index + 2]; // z
    }
}
