using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefunctMovement : MonoBehaviour
{
    [SerializeField] private Vector2Int position;
    [SerializeField] private Dictionary<Vector2Int, int> heldMoveTiles;
    [SerializeField] private SortedList<int, Queue<Vector2Int>> sortList;
    [SerializeField] private GameObject self;
    [SerializeField] private GameObject heldTile;
    [SerializeField] private GameObject moveTileRef;
    [SerializeField] private GameObject atkTile;

    private Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0) };


    public GridEngager gridDetector;
    public CharacterSheet characterSheet;

    [SerializeField] private bool canPlayerMove = true;

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            SetPosition(position);
        }
    }
    
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            SetPosition(position);
        }
    }

    public void InitiateGrid()
    {

        //grab player movement held in playerCharacterSheets on this gameObject
        int movement = characterSheet.GetMovement();

        sortList = new SortedList<int, Queue<Vector2Int>>();
        heldMoveTiles = new Dictionary<Vector2Int, int>();

        for (int i = 0; i <= movement; i++) { sortList[i] = new Queue<Vector2Int>(); }

        sortList[0].Enqueue(position);
        heldMoveTiles.Add(position, 0);
        AddNewTile(position);

        for (int i = 0; i < movement; i++)
        {
            while (sortList[i].Count > 0)
            {
                Vector2Int pos = sortList[i].Peek();
                foreach (Vector2Int direction in directions)
                {
                    Vector2Int position = pos + direction;

                    if (CheckTileValid(position, i) && !heldMoveTiles.ContainsKey(position)) // this will break
                    {
                        int tileNo = gridDetector.ReturnTileData(position)[0].GetComponent<MovementTileScript>().moveNumber;

                        if (sortList.ContainsKey(i + tileNo))
                        {
                            sortList[i + tileNo].Enqueue(position);
                            heldMoveTiles.Add(position, i + tileNo);

                            AddNewTile(position);
                        }
                    }
                }
                sortList[i].Dequeue();
            }
        }

        sortList = new SortedList<int, Queue<Vector2Int>>();
        int range = characterSheet.GetRange();

        for (int i = 0; i <= range; i++) { sortList[i] = new Queue<Vector2Int>(); }

        foreach (var tile in heldMoveTiles)
        {
            sortList[0].Enqueue(tile.Key);
        }

        for (int i = 0; i < range; i++)
        {
            while (sortList[i].Count > 0)
            {
                foreach (Vector2Int direction in directions)
                {
                    
                    Vector2Int curPos = sortList[i].Peek();
                    Vector2Int newPos = direction + curPos;

                    if (CheckAtkTileValid(newPos))
                    {
                        List<GameObject> tileData = gridDetector.ReturnTileData(newPos);
                        sortList[i + 1].Enqueue(newPos);
                        if (tileData[0].GetComponent<MovementTileScript>().moveNumber != 0)
                        {
                            heldMoveTiles.Add(newPos, i + 1);
                            AddAttackTIle(newPos);
                        }
                        
                    }
                }
                sortList[i].Dequeue();
            }
        }
    }

    private bool CheckAtkTileValid(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0) { return false; }

        if (pos.x > gridDetector.ReturnGridSize().x || pos.y > gridDetector.ReturnGridSize().y) { return false; }

        List<GameObject> tileData = gridDetector.ReturnTileData(pos);

        if (tileData[2] != null) { return false; }

        if (tileData[3] != null) { return false; }

        if (tileData[1]?.CompareTag("Player") == true) { return false; }

        return true;
    }

    private bool CheckTileValid(Vector2Int pos, int i)
    {
        if (pos.x < 0 || pos.y < 0) { return false; }

        if (pos.x > gridDetector.ReturnGridSize().x || pos.y > gridDetector.ReturnGridSize().y) { return false; }

        int tileNo = gridDetector.ReturnTileData(pos)[0].GetComponent<MovementTileScript>().moveNumber;

        if (i + tileNo < 0 || tileNo == 0) { return false; }

        List<GameObject> tileData = gridDetector.ReturnTileData(pos);

        if (tileData[2] == null && tileData[1] == null) { return true; }

        if (tileData[2] != null) { return false; }

        if (tileData[1]?.CompareTag("Enemy") == true) { return false; }

        return true;
    }

    public Vector2Int ReturnPosition() { return position; }

    public void SetPosition(Vector2Int pos)
    {
        position = pos;
        transform.position = new Vector3(pos.x + 0.5f, 0, pos.y + 0.5f);
    }

    public bool GetIfMove() { return canPlayerMove; }
    public void SetIfMove()
    {
        if (canPlayerMove)
        {
            print("setting to false");
            canPlayerMove = false;
        }
        else
        {
            canPlayerMove = true;
        }
    }

    private void AddNewTile(Vector2Int pos)
    {
        GameObject tile = Instantiate(heldTile);
        List<GameObject> newLoc = gridDetector.ReturnTileData(pos);
        gridDetector.SetTileData(pos, 2, tile);
        tile.transform.SetParent(moveTileRef.transform);
        tile.transform.position = new Vector3(pos.x + 0.5f, newLoc[0].transform.position.y + 0.3f, pos.y + 0.5f);
    }
    private void AddAttackTIle(Vector2Int pos)
    {
        GameObject tile = Instantiate(atkTile);
        List<GameObject> newLoc = gridDetector.ReturnTileData(pos);
        gridDetector.SetTileData(pos, 3, tile);
        tile.transform.SetParent(moveTileRef.transform);
        tile.transform.position = new Vector3(pos.x + 0.5f, newLoc[0].transform.position.y + 0.3f, pos.y + 0.5f);

    }


    // needs to go in a "playermovementDisplay" script
    public void RemoveTiles()
    {
        foreach (Vector2Int entry in heldMoveTiles.Keys)
        {
            gridDetector.SetTileData(entry, 2, null);
        }
        foreach (Transform tilesRef in moveTileRef.transform)
        {
            GameObject.Destroy(tilesRef.gameObject);
        }
    }
}
