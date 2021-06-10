using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public static void Generate(SpawnablePrefabs spawnablePrefabs, Vector2 regionSize, AnimationCurve aCurve, float[,] heightMap, Vector3 chunkPos, Transform parent, float reducingSize, float meshHeightMultiplier, LayerMask layerMask, float uniformScale)
    {
        List<Vector2> points;

        points = PoissonDiscSampling.GeneratePoints(spawnablePrefabs.radius, regionSize / reducingSize, spawnablePrefabs.instances);

        if (points != null)
        {
            foreach (Vector2 point in points)
            {
                float x = Mathf.Lerp(point.x / 2 * -1, point.x / 2, point.x) * (regionSize.x / 2) + chunkPos.x;
                float y = 100;
                float z = Mathf.Lerp(point.y / 2 * -1, point.y / 2, point.y) * (regionSize.y / 2) + chunkPos.z;

                GameObject obj = Instantiate(spawnablePrefabs.prefab, new Vector3(x, y, z) * uniformScale, spawnablePrefabs.prefab.transform.localRotation);
                obj.transform.SetParent(parent);

                obj.transform.localPosition = new Vector3((point.x / (regionSize.x / reducingSize) * 2 - 1) * (regionSize.x / 2), 0, (point.y / (regionSize.y / reducingSize) * 2 - 1) * (regionSize.y / 2));
                obj.transform.position = new Vector3(obj.transform.position.x, DetectGroundHeight(obj.transform.position), obj.transform.position.z);

                if(obj.transform.position.y < spawnablePrefabs.minHeight || obj.transform.position.y > spawnablePrefabs.maxHeight)
                {
                    Destroy(obj);
                }
            }
        }

        float DetectGroundHeight(Vector3 position)
        {
            position.y = 500;
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(position, Vector3.down);

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                return hit.point.y;
            }

            return 0;
        }
    }
}

[System.Serializable]
public struct SpawnablePrefabs
{
    public string name;
    public bool active;
    public GameObject prefab;
    public float minHeight;
    public float maxHeight;
    public int instances;
    public float amountInGround;

    public float radius;
}