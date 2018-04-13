using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingTouchRectangularBoundary : IRingMovementBondaryCalculationStrategy
{
    private Vector2 _StartPositionToCheck;
    private Vector2 _AcceptableRange;

    /// <summary>
    /// Boudary strategy for checking the touch position over the ring
    /// </summary>
    /// <param name="startPositionToCheck">Starting position for boundary</param>
    /// <param name="acceptableRange">Boundary acceptable range</param>
    public RingTouchRectangularBoundary(Vector2 startPositionToCheck, Vector2 acceptableRange)
    {
        _StartPositionToCheck = startPositionToCheck;
        _AcceptableRange = acceptableRange;
    }

    public Vector2 GetNormalizedPosition(Vector2 inputPosition)
    {
        return inputPosition;
    }

    public bool IsVerticalLimitCrossed(Vector2 inputPosition)
    {
        bool retVal = false;
        if ((inputPosition.y > _StartPositionToCheck.y + _AcceptableRange.y) || (inputPosition.y < _StartPositionToCheck.y - _AcceptableRange.y))
        {
            retVal = true;
        }
        return retVal;
    }

    public bool IsAnyLimitCrossed(Vector2 touchPosition)
    {
        bool retVal = false;
        if((touchPosition.x > _StartPositionToCheck.x + _AcceptableRange.x) || (touchPosition.x < _StartPositionToCheck.x - _AcceptableRange.x))
        {
            retVal = true;
        }
        else if((touchPosition.y > _StartPositionToCheck.y + _AcceptableRange.y) || (touchPosition.y < _StartPositionToCheck.y - _AcceptableRange.y))
        {
            retVal = true;
        }
        return retVal;
    }

    public float GetVerticalLimit()
    {
        return _StartPositionToCheck.y - _AcceptableRange.y;
    }
}
