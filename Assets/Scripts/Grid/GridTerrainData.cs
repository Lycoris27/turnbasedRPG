using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTerrainData : MonoBehaviour
{
    /// <summary>
    /// types of terrain that need to be implemented:
    /// - Typical Terrain
    /// - Mount half movement terrain
    /// - Half movement terrain
    /// - restricted
    /// </summary>




    [field: SerializeField] public bool halfMovement { get; private set; }
    [field: SerializeField] public bool horseDiscriminator { get; private set; }
    [field: SerializeField] public bool flierAntiDiscriminator { get; private set; }
    [field: SerializeField] public bool nuhUh { get; private set; }

}
