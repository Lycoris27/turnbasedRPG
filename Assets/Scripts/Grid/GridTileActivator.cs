using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileActivator : MonoBehaviour
{
    private GameObject atkZoneTile;
    private GameObject movementTile;

    private void Awake()
    {
        atkZoneTile = transform.Find("AtkZoneTile")?.gameObject;
        movementTile = transform.Find("MovementTile")?.gameObject;
    }

    public void ActivateMoveTile()
    {
        movementTile.SetActive(true);
    }
    public void DeactivateMoveTile()
    {
        movementTile.SetActive(false);
    }

    public void ActivateAttackTile()
    {
        atkZoneTile.SetActive(true);
    }
    public void DeactivateAttackTile()
    {
        atkZoneTile.SetActive(false);
    }
}
