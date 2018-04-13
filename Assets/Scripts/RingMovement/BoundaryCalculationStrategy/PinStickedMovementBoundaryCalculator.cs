using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinStickedMovementBoundaryCalculator : IRingMovementBondaryCalculationStrategy
{
    private const float _HORIZONTAL_DELTA   = 0.12f;
    private const float _VERTICAL_DELTA     = 0.3f;

    private GameObject _ActiveObject     = null;
    private Transform _PinSticked        = null;
    private Vector3 _InitialPosition;

    /// <summary>
    /// Boudary strategy for the ring which is sticked to pin
    /// </summary>
    /// <param name="activeObject">ActiveObject to track</param>
    /// <param name="pinSticked">Sticked Pin</param>
    public PinStickedMovementBoundaryCalculator(GameObject activeObject, Transform pinSticked)
    {
        _ActiveObject = activeObject;
        _InitialPosition = activeObject.transform.position;
        _PinSticked = pinSticked;
    }

    public Vector2 GetNormalizedPosition(Vector2 touchPosition)
    {
        Vector2 valueToReturn = new Vector2(touchPosition.x, touchPosition.y);
        float pinStickedPositionX = _PinSticked.transform.position.x;
        if (valueToReturn.x > pinStickedPositionX + _HORIZONTAL_DELTA)
        {
            valueToReturn.x = pinStickedPositionX + _HORIZONTAL_DELTA;
        }
        if (valueToReturn.x < pinStickedPositionX - _HORIZONTAL_DELTA)
        {
            valueToReturn.x = pinStickedPositionX - _HORIZONTAL_DELTA;
        }
        if(valueToReturn.y < _InitialPosition.y)
        {
            valueToReturn.y = _InitialPosition.y;
        }

        return valueToReturn;
    }

    public bool IsVerticalLimitCrossed(Vector2 inputPosition)
    {
        return _ActiveObject.transform.position.y > _PinSticked.transform.position.y + _VERTICAL_DELTA;
    }

    public bool IsAnyLimitCrossed(Vector2 inputPosition)
    {
        return IsVerticalLimitCrossed(inputPosition);
    }

    public float GetVerticalLimit()
    {
        return _PinSticked.transform.position.y;
    }
}
