using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridDetector : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(1, 1); // Size of each grid cell
    public int gridWidth = 10; // Corresponds to the number of columns (z-axis)
    public int gridDepth = 3; // Corresponds to the number of rows (x-axis)
    public string targetLayerName = "FloorTiles"; // Layer name of the objects to detect
    public List<List<GameObject>> detectedObjects = new List<List<GameObject>>(); // List of lists to simulate a 2D array
    public bool showOverlapBoxGizmos = true; // Toggle to show/hide the overlap box gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Draw grid along the xz-plane with gridDepth as the x-axis (depth) and gridWidth as the z-axis (width)
        for (int x = 0; x < gridDepth; x++) // x corresponds to depth (gridDepth)
        {
            for (int z = 0; z < gridWidth; z++) // z corresponds to width (gridWidth)
            {
                Vector3 cellPosition = new Vector3(
                    x * gridSize.x + gridSize.x / 2,
                    0,
                    z * gridSize.y + gridSize.y / 2
                );

                Gizmos.DrawWireCube(cellPosition, new Vector3(gridSize.x, 0, gridSize.y));

                // Draw the overlap box gizmo if enabled
                if (showOverlapBoxGizmos)
                {
                    Gizmos.color = Color.red; // Use a different color for the overlap box
                    Vector3 boxSize = new Vector3((gridSize.x / 2) * 0.9f, 0.5f, (gridSize.y / 2) * 0.9f);
                    Gizmos.DrawWireCube(cellPosition, boxSize * 2); // Multiply by 2 to match the actual overlap box size
                }
            }
        }
    }

    private void Awake()
    {
        // Initialize the detectedObjects list of lists
        detectedObjects = new List<List<GameObject>>();
        for (int x = 0; x < gridDepth; x++) // x corresponds to depth (gridDepth)
        {
            detectedObjects.Add(new List<GameObject>());
            for (int z = 0; z < gridWidth; z++) // z corresponds to width (gridWidth)
            {
                detectedObjects[x].Add(null); // Initialize with null
            }
        }

        DetectObjects();
    }

    private void DetectObjects()
    {
        int targetLayer = LayerMask.NameToLayer(targetLayerName);
        LayerMask layerMask = 1 << targetLayer; // Create a layer mask for filtering

        // Clear the detected objects list before starting detection
        for (int x = 0; x < gridDepth; x++) // x corresponds to depth (gridDepth)
        {
            for (int z = 0; z < gridWidth; z++) // z corresponds to width (gridWidth)
            {
                detectedObjects[x][z] = null; // Reset to null before detection
            }
        }

        // Check for objects within each grid cell along the xz-plane
        for (int x = 0; x < gridDepth; x++) // x corresponds to depth (gridDepth)
        {
            for (int z = 0; z < gridWidth; z++) // z corresponds to width (gridWidth)
            {
                Vector3 cellPosition = new Vector3(
                    x * gridSize.x + gridSize.x / 2,
                    0,
                    z * gridSize.y + gridSize.y / 2
                );

                Debug.Log($"Checking cell at position: {cellPosition}");

                // Define the size of the overlap box to match the grid cell, with a slight reduction
                Vector3 boxSize = new Vector3((gridSize.x / 2) * 0.9f, 0.5f, (gridSize.y / 2) * 0.9f);

                // Perform the overlap check
                Collider[] colliders = Physics.OverlapBox(
                    cellPosition,
                    boxSize,
                    Quaternion.identity,
                    layerMask // Use the layer mask to filter by the target layer
                );

                // Log the number of detected colliders in this cell
                Debug.Log($"Number of colliders found: {colliders.Length}");

                foreach (Collider collider in colliders)
                {
                    Debug.Log($"Detected object: {collider.gameObject.name} at position: {collider.transform.position}");

                    // Ensure it's the correct layer before assigning
                    if (collider.gameObject.layer == targetLayer)
                    {
                        detectedObjects[x][z] = collider.gameObject; // Store the detected object in the list of lists
                        break; // Stop after storing the first detected object in the cell
                    }
                }
            }
        }
    }
}