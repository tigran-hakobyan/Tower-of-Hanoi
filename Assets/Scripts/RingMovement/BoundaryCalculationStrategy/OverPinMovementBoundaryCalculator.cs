using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverPinMovementBoundaryCalculator : IRingMovementBondaryCalculationStrategy
{
    private float _YPositionToCheck;
    private bool _IsPositionNormalized = false;

    /// <summary>
    /// Boudary strategy for the ring which is not sticked to any pin
    /// </summary>
    /// <param name="yPositionToCheck">Vertical limit to work with</param>
    public OverPinMovementBoundaryCalculator(float yPositionToCheck)
    {
        _YPositionToCheck = yPositionToCheck;
    }

    public Vector2 GetNormalizedPosition(Vector2 inputPosition)
    {
        _IsPositionNormalized = false;
        Vector2 valueToReturn = new Vector2(inputPosition.x, inputPosition.y);
        if (valueToReturn.y < _YPositionToCheck)
        {
            valueToReturn.y = _YPositionToCheck;
            _IsPositionNormalized = true;
        }

        return valueToReturn;
    }

    public bool IsVerticalLimitCrossed(Vector2 inputPosition)
    {
        return _IsPositionNormalized;
    }

    public bool IsAnyLimitCrossed(Vector2 inputPosition)
    {
        return IsVerticalLimitCrossed(inputPosition);
    }

    public float GetVerticalLimit()
    {
        return _YPositionToCheck;
    }
}
