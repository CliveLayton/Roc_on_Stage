using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverBase : MonoBehaviour
{
    private Vector2 lookDirection;

    /// <summary>
    /// get the position of the gameobject
    /// </summary>
    /// <returns>transform.position</returns>
    public Vector2 GetPosition()
    {
        return transform.position;
    }

    /// <summary>
    /// get the direction, the object is facing
    /// </summary>
    /// <returns>Vector2</returns>
    public Vector2 GetLookDirection()
    {
        return lookDirection;
    }

    /// <summary>
    /// sets the lookDirection to the new Vector2 direction
    /// </summary>
    /// <param name="newLookDirection">Vector2</param>
    public void SetLookDirection(Vector2 newLookDirection)
    {
        lookDirection = newLookDirection;
    }
}
