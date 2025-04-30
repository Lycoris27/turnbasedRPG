using System.Collections.Generic;
using UnityEngine;

public class CharacterPathfinding : MonoBehaviour
{
    private Dictionary<Vector2Int, int> gScore = new();
    private Dictionary<Vector2Int, int> fScore = new();
    private HashSet<Vector2Int> hScore = new();

    private SortedList<int, Queue<Vector2Int>> sortList = new();
    private Queue<Vector2Int> backtrack;

    private GridDetector gridDetector;
    private CharacterMovement eMovement;
    private MovementDisplay eDisplay;
    private CharacterSheet eSheet;

    private static readonly Vector2Int[] directions = {
        new(0, 1), new(0, -1), new(1, 0), new(-1, 0)
    };

    private void Awake()
    {
        eMovement = GetComponent<CharacterMovement>();
        eDisplay = GetComponent<MovementDisplay>();
        eSheet = GetComponent<CharacterSheet>();
        gridDetector = GameObject.Find("LevelManager")?.GetComponent<GridDetector>();
    }

    public Dictionary<Vector2Int, int> BeginPathfinding()
    {
        bool foundTarget = false;
        gScore.Clear(); fScore.Clear(); hScore.Clear(); sortList.Clear(); 
        Vector2Int start = eMovement.ReturnPosition();
        int movement = eSheet.GetMovement();
        int range = eSheet.GetRange();
        Dictionary<Vector2Int, GameObject> targetList = null;
        if (CompareTag("Player")) { targetList = gridDetector.ReturnEnemyList(); }
        else if (CompareTag("Enemy")) { targetList = gridDetector.ReturnPlayerList(); }

        gScore[start] = 0;
        fScore[start] = 0;
        sortList[0] = new Queue<Vector2Int>();
        sortList[0].Enqueue(start);

        while (sortList.Count > 0)
        {
            int currentCost = sortList.Keys[0];
            Vector2Int current = sortList[currentCost].Dequeue(); // takes element from the sortlist, whilst removing it at the same time

            // Checks if a target is within range from tile, if yes, then adds to fScore

            foreach (var target in targetList.Keys)
            {
                if(ManhattanDist(target, current) <= range)
                {
                    fScore[current] = currentCost;
                    if (!hScore.Contains(target)) { hScore.Add(target); }
                    foundTarget = true;
                }
            }
            if (sortList[currentCost].Count == 0) sortList.Remove(currentCost); // removes the list if there is nothing left
            if ((foundTarget && currentCost == movement) || (foundTarget && currentCost == movement * 2) || (foundTarget && currentCost > movement * 2)) { sortList.Clear(); continue; }
            if ((foundTarget && currentCost == movement) || (foundTarget && currentCost == movement * 2)) continue;

            foreach (var dir in directions)
            {
                Vector2Int neighbor = current + dir;

                if (!IsWithinBounds(neighbor) || gScore.ContainsKey(neighbor)) continue; // Skips to next value if falls outside of bounds or gScore already contains key

                var tileData = gridDetector.ReturnTileData(neighbor);

                // skips neighbour if tileData[0] doesn't exist (creates value at same time), and if wall movenumber is active, or tile holds target (can't move on top of an enemy)
                if (!tileData[0].TryGetComponent(out MovementTileScript tile) || tile.moveNumber <= 0 || tileData[1] != null ) continue; //|| tileData[1].CompareTag(targetTag

                int totalCost = currentCost + tile.moveNumber; // totalCost is how far it takes to move to the tile
                gScore[neighbor] = totalCost; // adds value to gScore

                if(tile.BoostCheck() > 0) { fScore[neighbor] = totalCost; } // if the tiles boost values are greater than 0, they get placed into score selection

                if (!sortList.ContainsKey(totalCost)) { sortList[totalCost] = new Queue<Vector2Int>(); } // if doesnt contain key, then create a new queue in sortlist
                sortList[totalCost].Enqueue(neighbor);
            }
        }

        Vector2Int targetPlayer = new Vector2Int(-1, -1);
        Dictionary<string, int> heldStats = null;

        foreach (var target in hScore)
        {
            Dictionary<string, int> targetStats = targetList[target].GetComponent<CharacterAI>().ReturnStats();

            if (heldStats == null || heldStats["def"] < targetStats["def"])
            {
                heldStats = targetStats;
                targetPlayer = target;
            }
        }

        hScore.Clear();

        int? heldScore = null;
        Vector2Int? heldPos = null;

        //print($"fScore count is {fScore.Count} \n tilesInRange count is {tilesInRange.Count}");

        eDisplay.DisplayAttackTiles(fScore);

        foreach (var tile in fScore)
        {
            //EngageValue
            int score = 0;

            // Checks to see if tile is accessible by character, helps with if target is outside of range and positive locations are within range
            if (tile.Value <= movement && ManhattanDist(start, targetPlayer) < (movement * 2)) { score += 6; }

            // if the character can attack the target but the target cannot respond
            if (heldStats["range"] < ManhattanDist(tile.Key, targetPlayer) && ManhattanDist(tile.Key, targetPlayer) <= range) { score += 4; }

            // if at max range away from player
            if (range == ManhattanDist(tile.Key, targetPlayer)) { score += 3; }


            // Modifies score based on distance away from most direct route, further away means less likely
            score += ManhattanDist(targetPlayer, start) - (ManhattanDist(tile.Key, targetPlayer) + ManhattanDist(tile.Key, start));


            if (tile.Key == new Vector2Int(2, 6))
            {
                print($"the distance score is {ManhattanDist(targetPlayer, start) - (ManhattanDist(tile.Key, targetPlayer) + ManhattanDist(tile.Key, start))}");
            }
            // if the target is far enough from the player, it starts overriding positions to say you just need to focus on getting close
            score -= ManhattanDist(tile.Key, targetPlayer); 

            // for each boost or debuff, add it to score
            var tileData = gridDetector.ReturnTileData(tile.Key)[0].GetComponent<MovementTileScript>();
            if (tileData.BoostCheck() != 0) { score += (int)tileData.BoostCheck(); }

            //print($"Score is: {score} | heldScore is: {heldScore} \n heldPos is: {heldPos} | tile.Key is: {tile.Key} ");

            if (tile.Key == new Vector2Int(2, 6))
            {
                print($"score at 2,6 is: {score} ");
            }

            // if the score is larger than heldscore, then make this the location you access
            if ( heldScore < score || heldScore == null)
            {
                heldScore = score;
                heldPos = tile.Key;
            }
        }
        fScore.Clear();

        print($"heldPos is: {heldPos}, and heldScore is: {heldScore}");

        fScore.Add((Vector2Int)heldPos, gScore[(Vector2Int)heldPos]);
        
        return Backtrack((Vector2Int)heldPos);
    }
/*
    if (tileData[1].CompareTag(targetTag) == true) // this is where things need to be modified
{
    fScore[neighbor] = totalCost;
    return Backtrack(neighbor);
}
*/

    private int ManhattanDist(Vector2Int pos1, Vector2Int pos2)
    {
        return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
    }

    private bool IsWithinBounds(Vector2Int pos)
    {
        var size = gridDetector.ReturnGridSize();
        return pos.x >= 0 && pos.y >= 0 && pos.x <= size.x && pos.y <= size.y; // if position is greater than 0, but less than or equal to the size of the grid
    }

    private bool IsAtkTileValid(Vector2Int pos)
    {
        if (!IsWithinBounds(pos)) return false;
        var tile = gridDetector.ReturnTileData(pos);
        return tile[2] == null && tile[3] == null && (tile[1] == null || !tile[1].CompareTag("Player"));
    }

  

    private Dictionary<Vector2Int, int> Backtrack(Vector2Int goal)
    {
        if (goal == eMovement.ReturnPosition()) { return null; }

        backtrack = new Queue<Vector2Int>();
        backtrack.Enqueue(goal);

        while (true)
        {
            Vector2Int current = backtrack.Peek();
            int minScore = int.MaxValue;
            Vector2Int next = current;

            foreach (var dir in directions)
            {
                Vector2Int neighbor = current + dir;
                if (fScore.ContainsKey(neighbor) || !gScore.TryGetValue(neighbor, out int score)) continue;

                var tileData = gridDetector.ReturnTileData(neighbor)[0].GetComponent<MovementTileScript>();

                if (tileData.BoostCheck() < 0) { score += 3; print("WEEWOO"); }

                if (score < minScore)
                {
                    minScore = score;
                    next = neighbor;
                }
            }

            if (minScore == int.MaxValue || minScore == 0) break;

            backtrack.Dequeue();
            backtrack.Enqueue(next);
            fScore[next] = minScore;
        }

        return fScore;
    }

    public void BeginPredictPathfinding()
    {
        int movement = eSheet.GetMovement(), range = eSheet.GetRange(), max = Mathf.Max(movement, range);
        gScore.Clear(); fScore.Clear(); sortList.Clear();

        for (int i = 0; i <= max; i++) sortList[i] = new Queue<Vector2Int>();

        Vector2Int start = eMovement.ReturnPosition();
        gScore[start] = 0;
        sortList[0].Enqueue(start);

        // Movement Range
        for (int i = 0; i < movement; i++)
        {
            while (sortList[i].Count > 0)
            {
                Vector2Int pos = sortList[i].Dequeue();
                foreach (var dir in directions)
                {
                    Vector2Int next = pos + dir;
                    if (gScore.ContainsKey(next) || !IsTileValid(next, i)) continue;

                    var tileData = gridDetector.ReturnTileData(next);
                    if (!tileData[0].TryGetComponent(out MovementTileScript tile)) continue;

                    int cost = tile.moveNumber, total = i + cost;
                    if (cost <= 0 || total > movement) continue;

                    gScore[next] = total;
                    sortList[total].Enqueue(next);
                    //print($" adding position {next} with value {total} to gscore");
                }
            }
        }

        // Attack Range
        for (int i = 0; i <= range; i++) sortList[i].Clear();
        foreach (var key in gScore.Keys) sortList[0].Enqueue(key);

        for (int i = 0; i < range; i++)
        {
            while (sortList[i].Count > 0)
            {
                Vector2Int cur = sortList[i].Dequeue();
                foreach (var dir in directions)
                {
                    Vector2Int next = cur + dir;
                    if (fScore.ContainsKey(next) || !IsAtkTileValid(next) || gScore.ContainsKey(next)) continue;

                    var tileData = gridDetector.ReturnTileData(next);
                    if (tileData[0].TryGetComponent(out MovementTileScript tile) && tile.moveNumber != 0)
                        fScore[next] = i + 1;

                    if (i + 1 <= range) sortList[i + 1].Enqueue(next);
                }
            }
        }

        eDisplay.DisplayPredictionTiles(gScore);
        eDisplay.DisplayAttackTiles(fScore);
    }
    private bool IsTileValid(Vector2Int pos, int currentCost)
    {
        if (!IsWithinBounds(pos)) return false;
        var tile = gridDetector.ReturnTileData(pos);
        if (!tile[0].TryGetComponent(out MovementTileScript mts)) return false;
        int moveCost = mts.moveNumber;

        if (moveCost <= 0 || currentCost + moveCost < 0) return false;
        return tile[2] == null && (tile[1] == null || !tile[1].CompareTag("Enemy"));
    }
}