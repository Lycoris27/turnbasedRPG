using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDisplay : MonoBehaviour
{
    [Header("Held GameObjects")]
    [SerializeField] private GameObject moveTilesRef;

    [Header("Held Scripts")]
    [SerializeField] private GridDetector gridDetector;

    // Movement and Attack Grids
    private Dictionary<Vector2Int, int> moveGrid = new();
    private Dictionary<Vector2Int, int> attackGrid = new();

    // Related Components
    private CharacterPathfinding ePathfinding;
    private CharacterMovement eMovement;
    private CharacterAI eAI;

    private void Awake()
    {
        ePathfinding = GetComponent<CharacterPathfinding>();
        eMovement = GetComponent<CharacterMovement>();
        eAI = GetComponent<CharacterAI>();
    }
    public void DisplayPredictionTiles(Dictionary<Vector2Int, int> predictedTiles)
    {
        foreach (var pair in predictedTiles)
        {
            GridTileActivator gridPos = gridDetector.ReturnTileData(pair.Key)[0].GetComponent<GridTileActivator>();

            gridPos.ActivateMoveTile();
        }
        moveGrid = predictedTiles;
    }
    public void DisplayAttackTiles(Dictionary<Vector2Int, int> predictedTiles)
    {
        foreach (var pair in predictedTiles)
        {
            GridTileActivator gridPos = gridDetector.ReturnTileData(pair.Key)[0].GetComponent<GridTileActivator>();

            gridPos.ActivateAttackTile();
        }
        attackGrid = predictedTiles;
    }
    public void RemoveDisplayedTiles()
    {
        foreach (var pair in moveGrid)
        {
            GridTileActivator gridPos = gridDetector.ReturnTileData(pair.Key)[0].GetComponent<GridTileActivator>();

            gridPos.DeactivateMoveTile();
        }
        foreach (var pair in attackGrid)
        {
            GridTileActivator gridPos = gridDetector.ReturnTileData(pair.Key)[0].GetComponent<GridTileActivator>();

            gridPos.DeactivateAttackTile();
        }
    }
}
