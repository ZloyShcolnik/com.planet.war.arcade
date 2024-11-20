using System.Collections.Generic;
using UnityEngine;

public class InfiniteWorld : MonoBehaviour
{
    public GameObject treePrefab; // ������ ������
    public GameObject chunkPrefab; // ������ ����� (��������, ������� �����)
    public int chunkSize = 40; // ������ ������ ����� (��������, 16x16)
    public int treesPerChunk = 15; // ���������� �������� �� ����
    public float treeSpacing = 2f; // ����������� ���������� ����� ���������

    public Transform player; // ������ �� ������
    private Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        UpdateChunks();
        player.gameObject.SetActive(true);
    }

    void Update()
    {
        UpdateChunks();
    }

    void UpdateChunks()
    {
        Vector2Int playerChunk = GetChunkCoord(player.position);

        // ���������� ����� ������ ������
        int renderDistance = 2; // ������ � ������ ������ ������
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunk.x + x, playerChunk.y + z);
                if (!loadedChunks.ContainsKey(chunkCoord))
                {
                    GenerateChunk(chunkCoord);
                }
            }
        }

        // ������� ������ �����
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunk in loadedChunks.Keys)
        {
            if (Vector2Int.Distance(chunk, playerChunk) > renderDistance)
            {
                chunksToRemove.Add(chunk);
            }
        }

        foreach (var chunk in chunksToRemove)
        {
            Destroy(loadedChunks[chunk]);
            loadedChunks.Remove(chunk);
        }
    }

    Vector2Int GetChunkCoord(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / chunkSize);
        int z = Mathf.FloorToInt(position.z / chunkSize);
        return new Vector2Int(x, z);
    }

    void GenerateChunk(Vector2Int chunkCoord)
    {
        // ������� ����� � ������� �����������
        Vector3 chunkPosition = new Vector3(chunkCoord.x * chunkSize, 0, chunkCoord.y * chunkSize);

        // ������ ����
        GameObject chunk = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity);
        chunk.transform.localScale = new Vector3(chunkSize, 1, chunkSize); // ��� ����� �� ������������, ���� ��� ������ ��� ����� ���������� ������
        chunk.name = $"Chunk {chunkCoord.x},{chunkCoord.y}";

        // ��������� ��������
        HashSet<Vector3> treePositions = new HashSet<Vector3>();
        for (int i = 0; i < treesPerChunk; i++)
        {
            Vector3 treePosition;
            int attempts = 0;
            do
            {
                // ��������� ��������� ��������� ������ �����
                float localX = Random.Range(0f, chunkSize); // 0�40 �� x
                float localZ = Random.Range(0f, chunkSize); // 0�40 �� z

                // �������������� � ������� ����������
                treePosition = new Vector3(chunkPosition.x + localX, 0, chunkPosition.z + localZ);

                attempts++;
            } while (attempts < 100 && !IsValidTreePosition(treePosition, treePositions));

            if (attempts < 100)
            {
                // ������ ������
                treePositions.Add(treePosition);
                GameObject tree = Instantiate(treePrefab, treePosition, Quaternion.identity);
                tree.transform.parent = chunk.transform; // ����������� ������ � �����
            }
        }

        // ��������� ����
        loadedChunks[chunkCoord] = chunk;
    }


    bool IsValidTreePosition(Vector3 position, HashSet<Vector3> existingPositions)
    {
        foreach (var existingPosition in existingPositions)
        {
            if (Vector3.Distance(position, existingPosition) < treeSpacing)
            {
                return false; // ������� ������ � ������� ������
            }
        }
        return true;
    }

}