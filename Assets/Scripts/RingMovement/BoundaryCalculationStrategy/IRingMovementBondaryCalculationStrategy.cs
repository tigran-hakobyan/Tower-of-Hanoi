using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRingMovementBondaryCalculationStrategy {

    /// <summary>
    /// Normalize input value to boundary limits
    /// </summary>
    /// <param name="inputPosition">Position to normalize</param>
    /// <returns></returns>
    Vector2 GetNormalizedPosition(Vector2 inputPosition);

    /// <summary>
    /// Checks for input value out of vertical bondary limit
    /// </summary>
    /// <param name="inputPosition">Position to check</param>
    /// <returns></returns>
    bool IsVerticalLimitCrossed(Vector2 inputPosition);


    /// <summary>
    /// Checks for any boundary limit crossed
    /// </summary>
    /// <param name="inputPosition">Position to check</param>
    /// <returns></returns>
    bool IsAnyLimitCrossed(Vector2 inputPosition);

    /// <summary>
    /// Provides vertical limit of boundary
    /// </summary>
    /// <returns>Vertial limit value</returns>
    float GetVerticalLimit();
}
