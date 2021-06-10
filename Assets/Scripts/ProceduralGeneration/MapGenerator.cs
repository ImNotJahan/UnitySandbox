using UnityEngine;
using System.Threading;
using System;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;

    public enum DrawMode {NoiseMap, Mesh}
    public DrawMode drawMode;

    public TerrainData terrainData;
    public NoiseData noiseData;

    public LayerMask layerMask;

    public SpawnablePrefabs[] spawnablePrefabs;

    static bool flatShaded;

    public Material terrainMaterial;

    [Range(0, MeshGenerator.numSupportedChunkSizes - 1)]
    public int chunkSizeIndex;
    [Range(0, MeshGenerator.numSupportedFlatshadedChunkSizes - 1)]
    public int flatshadedChunkSizeIndex;

    [Range(0, MeshGenerator.numSupportedLODs - 1)]
    public int levelOfDetail;

    public bool autoUpdate;

    public int noiseIterations;
    public float iterationStrength;

    public bool meshGenerated = false;

    Vector2 centerThing = Vector2.zero;

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    private void Awake()
    {
        instance = this;
        flatShaded = terrainData.isFlatShaded;
    }

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    public int MapChunkSize
    {
        get
        {
            if (flatShaded)
            {
                return MeshGenerator.supportedFlatshadedChunkSizes[flatshadedChunkSizeIndex] - 1;
            }
            else
            {
                return MeshGenerator.supportedChunkSizes[chunkSizeIndex] - 1;
            }
        }
    }

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = FindObjectOfType<MapDisplay>();

        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, levelOfDetail, flatShaded));
        }
    }

    public void RequestMapData(Vector2 center, Action<MapData> callBack)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(center, callBack);
        };

        new Thread(threadStart).Start();
    }

    void MapDataThread(Vector2 center, Action<MapData> callBack)
    {
        MapData mapData = GenerateMapData(center);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callBack, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, lod, callback);
        };

        new Thread(threadStart).Start();
    } 
    
    void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, lod, flatShaded);
        lock(meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }

    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
        {
            for(int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        if (meshDataThreadInfoQueue.Count > 0)
        {
            for(int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    public MapData GenerateMapData(Vector2 center)
    {
        centerThing = center;

        float[,] noiseMap = Noise.GenerateNoiseMap(MapChunkSize + 2, MapChunkSize + 2, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, center + noiseData.offset, noiseData.normalizeMode);

        for (int k = 0; k < noiseIterations; k++)
        {
            float[,] newNoiseMap = Noise.GenerateNoiseMap(MapChunkSize + 2, MapChunkSize + 2, noiseData.seed + k, noiseData.noiseScale / (iterationStrength * k), noiseData.octaves, noiseData.persistance, noiseData.lacunarity, center + noiseData.offset, noiseData.normalizeMode);

            for(int x = 0; x < newNoiseMap.GetLength(0); x++)
            {
                for(int y = 0; y < newNoiseMap.GetLength(1); y++)
                {
                    if(newNoiseMap[x, y] > noiseMap[x, y])
                    {
                        noiseMap[x, y] = newNoiseMap[x, y];
                    }
                }
            }
        }

        /*float[,] riverMap = Noise.GenerateNoiseMap(MapChunkSize + 2, MapChunkSize + 2, noiseData.seed + 555, noiseData.noiseScale / 4, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, center + noiseData.offset, noiseData.normalizeMode);
        for (int x = 0; x < noiseMap.GetLength(0); x++)
        {
            for(int y = 0; y < noiseMap.GetLength(1); y++)
            {
                if(riverMap[x, y] < 0.2f)
                {
                    noiseMap[x, y] -= (0.2f - riverMap[x, y]) * 3;
                }
            }
        }*/

        return new MapData(noiseMap);
    }

    private void OnValidate()
    {
        if(terrainData != null)
        {
            terrainData.OnValuesUpdated -= OnValuesUpdated;
            terrainData.OnValuesUpdated += OnValuesUpdated;
        }
        if(noiseData != null)
        {
            noiseData.OnValuesUpdated -= OnValuesUpdated;
            noiseData.OnValuesUpdated += OnValuesUpdated;
        }
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;
        
        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}

public struct MapData
{
    public readonly float[,] heightMap;

    public MapData(float[,] heightMap)
    {
        this.heightMap = heightMap;
    }
}