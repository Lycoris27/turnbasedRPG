using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridDetector : MonoBehaviour
{

    [SerializeField] private int gridWidth = 0; // Corresponds to the number of columns (z-axis)
    [SerializeField] private int gridDepth = 0; // Corresponds to the number of rows (x-axis)

    private Dictionary<Vector2Int, GameObject> tileList = new();
    private Dictionary<Vector2Int, GameObject> playerObjectsList = new();
    private Dictionary<Vector2Int, GameObject> enemyObjectsList = new();
    [SerializeField] private Dictionary<Vector2Int, List<GameObject>> gridDetectedObjects;

    private void Awake()
    {
        if (!Application.isPlaying) return;

        gridDetectedObjects = new Dictionary<Vector2Int, List<GameObject>>();
        for (int x = 0; x <= gridDepth; x++)
        {
            for (int z = 0; z <= gridWidth; z++)
            {
                InitGrid(x, z);
            }
        }
    }
    private void InitGrid(int depth, int width)
    {
        Vector2Int position = new Vector2Int(depth, width);

        if (!gridDetectedObjects.ContainsKey(position))
        {
            gridDetectedObjects[position] = new List<GameObject>();
        }

        // Add 4 null objects if not already added
        while (gridDetectedObjects[position].Count < 4)
        {
            gridDetectedObjects[position].Add(null);
        }
    }
    private void Start()
    {
        if (!Application.isPlaying) return;
        FindForGrid();
    }



    private void FindForGrid()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tileObjects)
        {
            if (tile == null) continue;

            MovementTileScript moveTileScript = tile.GetComponent<MovementTileScript>();
            if (moveTileScript == null) continue;

            Vector2Int pos = moveTileScript.ReturnPosition();
            EnsureGridKeyExists(pos);
            gridDetectedObjects[pos][0] = tile;
            tileList[pos] = tile;
        }

        foreach (GameObject player in playerObjects)
        {
            if (player == null) continue;

            CharacterAI characterAI = player.GetComponent<CharacterAI>();
            if (characterAI == null) continue;

            Vector2Int pos = characterAI.ReturnPosition();
            EnsureGridKeyExists(pos);
            gridDetectedObjects[pos][1] = player;
            playerObjectsList[pos] = player;

        }

        foreach (GameObject enemy in enemyObjects)
        {
            if (enemy == null) continue;

            CharacterAI enemyAI = enemy.GetComponent<CharacterAI>();
            if (enemyAI == null) continue;

            Vector2Int pos = enemyAI.ReturnPosition();
            EnsureGridKeyExists(pos);
            gridDetectedObjects[pos][1] = enemy;
            enemyObjectsList[pos] = enemy;
        }
    }

    private void EnsureGridKeyExists(Vector2Int pos)
    {
        if (!gridDetectedObjects.ContainsKey(pos))
        {
            gridDetectedObjects[pos] = new List<GameObject> { null, null, null, null };
        }
        else
        {
            while (gridDetectedObjects[pos].Count < 4)
            {
                gridDetectedObjects[pos].Add(null);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (gridWidth > 0 && gridDepth > 0)
        {
            Gizmos.color = Color.green;

            for (int x = 0; x <= gridDepth; x++)
            {
                for (int z = 0; z <= gridWidth; z++)
                {
                    Vector3 bottomLeft = new Vector3(x, 0, z);
                    Vector3 topLeft = new Vector3(x, 0, z + 1);
                    Vector3 bottomRight = new Vector3(x + 1, 0, z);
                    Vector3 topRight = new Vector3(x + 1, 0, z + 1);

                    Gizmos.DrawLine(bottomLeft, topLeft);
                    Gizmos.DrawLine(bottomLeft, bottomRight);
                    Gizmos.DrawLine(topLeft, topRight);
                    Gizmos.DrawLine(bottomRight, topRight);
                }
            }
        }
    }

    // External access
    public List<GameObject> ReturnTileData(Vector2Int pos) => gridDetectedObjects.ContainsKey(pos) ? gridDetectedObjects[pos] : null;

    public GameObject ReturnPlayerTileData(Vector2Int pos)
    {
        return playerObjectsList[pos];
    }
    public GameObject ReturnEnemyTileData(Vector2Int pos)
    {
        return enemyObjectsList[pos];
    }
    public Dictionary<Vector2Int, GameObject> ReturnEnemyList()
    {
        return enemyObjectsList;
    }
    public Dictionary<Vector2Int, GameObject> ReturnPlayerList()
    {
        return playerObjectsList;
    }


    public Vector2Int ReturnGridSize() => new Vector2Int(gridDepth, gridWidth);
    public void SetTileData(Vector2Int pos, int index, GameObject obj)
    {
        EnsureGridKeyExists(pos);
        gridDetectedObjects[pos][index] = obj;
    }
    
    public void MoveCharacter(Vector2Int pos, Vector2Int oldPos, GameObject obj)
    {

        gridDetectedObjects[oldPos][1] = null;
        gridDetectedObjects[pos][1] = obj;

        if (obj.tag == "Player")
        {
            playerObjectsList.Remove(oldPos);
            playerObjectsList[pos] = obj;
        }
        else if (obj.tag == "Enemy")
        {
            enemyObjectsList.Remove(oldPos);
            enemyObjectsList[pos] = obj;
        }
    }
}
