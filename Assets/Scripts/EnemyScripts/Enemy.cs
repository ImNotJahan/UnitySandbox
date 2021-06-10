using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public ItemData itemData;

    public float addedDefense = 0;
    public float addedAtk = 0;
    public float health = 0;

    public bool alive = true;

    private GameObject player;
    private Transform groundCheck;

    MapGenerator mapGenerator;
    ChunkRendering chunkRendering;

    private void Awake()
    {
        player = GameObject.Find("FPC");
        groundCheck = transform.GetChild(0);

        health = enemyData.health;

        CalculateItemStats();
    }

    private void CalculateItemStats()
    {
        foreach (string item in enemyData.items)
        {
            if (itemData.itemStats.ContainsKey(item))
            {
                itemValues stats = itemData.itemStats[item];

                addedAtk += stats.atk;
                addedDefense += stats.def;
            }
        }
    }

    float GetGroundHeight()
    {
        mapGenerator = MapGenerator.instance;
        chunkRendering = ChunkRendering.instance;

        float groundHeight;

        Vector2 enemyChunkOffset = new Vector2(Mathf.RoundToInt(transform.position.x / mapGenerator.terrainData.uniformScale % mapGenerator.MapChunkSize), -Mathf.RoundToInt(transform.position.z / mapGenerator.terrainData.uniformScale % mapGenerator.MapChunkSize));
        
        int groundVertexIndex = Mathf.RoundToInt((mapGenerator.MapChunkSize - (mapGenerator.MapChunkSize / 2 + enemyChunkOffset.x) - 2) * mapGenerator.MapChunkSize + mapGenerator.MapChunkSize - (mapGenerator.MapChunkSize / 2 + enemyChunkOffset.y) - 4);

        int nextGroundVertexIndex = Mathf.RoundToInt((mapGenerator.MapChunkSize - (mapGenerator.MapChunkSize / 2 + enemyChunkOffset.x)) * mapGenerator.MapChunkSize + mapGenerator.MapChunkSize - (mapGenerator.MapChunkSize / 2 + enemyChunkOffset.y + 1));
        float percentNear = enemyChunkOffset.y % 1f;

        Vector2 enemyPos = new Vector2(Mathf.RoundToInt(transform.position.x / mapGenerator.terrainData.uniformScale / mapGenerator.MapChunkSize), -Mathf.RoundToInt(transform.position.z / mapGenerator.terrainData.uniformScale / mapGenerator.MapChunkSize));

        if (chunkRendering.terrainChunkDictionary.ContainsKey(enemyPos))
        {
            MeshFilter ground = chunkRendering.terrainChunkDictionary[enemyPos].meshObject.GetComponent<MeshFilter>();

            Matrix4x4 localToWorld = ground.transform.localToWorldMatrix;

            if (ground.mesh.vertices.Length > groundVertexIndex)
            {
                groundHeight = localToWorld.MultiplyPoint3x4(new Vector3(0, ground.mesh.vertices[groundVertexIndex].y, 0)).y;
                return groundHeight;
            }
        }

        return 0;
    }

    void Update()
    {
        //transform.position = new Vector3(Vector3.MoveTowards(transform.position, player.transform.position, 0.05f).x, GetGroundHeight(), Vector3.MoveTowards(transform.position, player.transform.position, 0.05f).z);
        transform.position = new Vector3(transform.position.x, GetGroundHeight(), transform.position.z);
    }

    public void Hurt(int dmg)
    {
        health = Mathf.Max(dmg - enemyData.baseDefense - addedDefense, 1);
    }
}
