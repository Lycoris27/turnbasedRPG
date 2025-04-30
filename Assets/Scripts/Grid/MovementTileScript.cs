using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MovementTileScript : MonoBehaviour
{
    [field: SerializeField] public int moveNumber { get; private set; }

    [SerializeField]
    private Vector2Int currentPosition;

    [Header("Modifiers")]
    [Tooltip("Avoid Boost (shown as steps of 10)")]
    [Range(-5, 5)] public int avoidBoostSteps;

    [Tooltip("Defense Boost (actual value)")]
    [Range(-5, 5)] public int defBoost;

    [Tooltip("Heal Boost (shown as steps of 10)")]
    [Range(-5, 5)] public int healBoostSteps;

    [Header("Final Boost Values (auto-calculated)")]
    [SerializeField] private List<int> boostList = new List<int>();

    public List<int> GetBoostValues()
    {
        return boostList;
    }

    private void OnValidate()
    {
        // Convert world position to grid
        currentPosition = new Vector2Int(
            Mathf.FloorToInt(transform.position.x),
            Mathf.FloorToInt(transform.position.z)
        );

        // Update the boost list shown in Inspector
        boostList = new List<int>
        {
            avoidBoostSteps * 10,
            defBoost,
            healBoostSteps * 10
        };
    }

    public Vector2Int ReturnPosition()
    {
        return currentPosition;
    }

    public float BoostCheck()
    {
        // Only place that uses divided values
        return (avoidBoostSteps) + defBoost + (healBoostSteps);
    }

    public void ModifyBoosts(int avoiValue, int defValue, int healValue)
    {
        boostList[0] += avoiValue;
        boostList[1] += defValue;
        boostList[2] += healValue;
    }
    public void ModifiersToBase()
    {
        boostList = new List<int>
        {
            avoidBoostSteps * 10,
            defBoost,
            healBoostSteps * 10
        };
    }

}