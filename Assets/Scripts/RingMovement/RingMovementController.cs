using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMovementController : MonoBehaviour {

    public delegate void OnItemSelect(GameObject objectToHandle);
    public event OnItemSelect onItemSelect;

    public delegate void OnItemDrop(GameObject objectToHandle);
    public event OnItemDrop onItemDrop;

    public delegate void OnVerticalMovementLimitCrossed(GameObject activeObject, float limitCrossed);
    public event OnVerticalMovementLimitCrossed onVerticalMovementLimitCrossed;

    public delegate void OnTouchLimitCrossed(GameObject touchedObject);
    public event OnTouchLimitCrossed onTouchLimitCrossed;

    public delegate void OnTouchDrop(GameObject objectToHandle);
    public event OnTouchDrop onTouchDrop;

    public string TrackingObjectTag = "Ring";
    public float MoveSpeed          = 0.1f;

    private GameObject _ActiveObject                                    = null;
    private IRingMovementBondaryCalculationStrategy _BoundaryCalculator = null;
    private Vector2 _TouchPosition;
    private bool _isObjectMovementHandlingEnabled                       = false;
    private bool _isTouchMovementHandlingEnabled                        = false;
        
    // Update is called once per frame
    void Update() {
#if UNITY_EDITOR
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _TouchPosition = new Vector2(mousePositionInWorld.x, mousePositionInWorld.y);

        if (Input.GetMouseButtonDown(0) && IsObjectcatchEnabled())
        {
            HandleObjectTouchStart();
        }
        if (Input.GetMouseButton(0) && IsObjectMovementHandlingNeeded())
        {
            HandleObjectTouchMove();
        }
        if (Input.GetMouseButton(0) && IsTouchMovementHandlingNeeded())
        {
            HandleTouchMove();
        }
        if (Input.GetMouseButtonUp(0) && IsObjectMovementHandlingNeeded())
        {
            HandleObjectTouchEnd();
        }
        if (Input.GetMouseButtonUp(0) && IsTouchMovementHandlingNeeded())
        {
            HandleTouchEnd();
        }
#else
		if(Input.touchCount == 1)
        {
			Touch currentTouch = Input.GetTouch(0);
			Vector3 touchPositionInWorld = Camera.main.ScreenToWorldPoint(currentTouch.position);
			_TouchPosition = new Vector2(touchPositionInWorld.x, touchPositionInWorld.y);
				
			if(currentTouch.phase == TouchPhase.Began && IsObjectcatchEnabled())
            {
				HandleObjectTouchStart();
			}
			if(currentTouch.phase == TouchPhase.Moved && IsObjectMovementHandlingNeeded())
            {
				HandleObjectTouchMove();
			}
			if (currentTouch.phase == TouchPhase.Moved && IsTouchMovementHandlingNeeded())
			{
				HandleTouchMove();
			}
			if((currentTouch.phase == TouchPhase.Canceled || currentTouch.phase == TouchPhase.Ended) && (IsObjectMovementHandlingNeeded()))
            {
				HandleObjectTouchEnd();
			}
			if ((currentTouch.phase == TouchPhase.Canceled || currentTouch.phase == TouchPhase.Ended) && IsTouchMovementHandlingNeeded())
			{
				HandleTouchEnd();
			}
		}
#endif
    }

    /// <summary>
    /// Start the object movement handling.
    /// </summary>
    /// <param name="objectToHandle">Object to move</param>
    /// <param name="boundaryCalculator">Movement bounds calculation strategy</param>
    public void StartObjectMovementHandling(GameObject objectToHandle, IRingMovementBondaryCalculationStrategy boundaryCalculator)
    {
        _ActiveObject = objectToHandle;
        _BoundaryCalculator = boundaryCalculator;
        _isObjectMovementHandlingEnabled = true;
    }

    /// <summary>
    /// Stop the object movement handling.
    /// </summary>
    public void StopObjectMovementHandling()
    {
        _isObjectMovementHandlingEnabled = false;
    }


    /// <summary>
    /// Start the touch point movement handling
    /// </summary>
    /// <param name="objectToHandle">Current active object</param>
    /// <param name="boundaryCalculator">Movement bounds calculation strategy</param>
    public void StartTouchPointMovementHandling(GameObject objectToHandle, IRingMovementBondaryCalculationStrategy boundaryCalculator)
    {
        _ActiveObject = objectToHandle;
        _BoundaryCalculator = boundaryCalculator;
        _isTouchMovementHandlingEnabled = true;
    }

    /// <summary>
    /// Stop the touch point movement handling.
    /// </summary>
    public void StopTouchPointMovementHandling()
    {
        _isTouchMovementHandlingEnabled = false;
    }

    /// <summary>
    /// Set movement bounds calculation strategy
    /// </summary>
    /// <param name="boundaryCalculator">Strategy</param>
    public void SetBoundaryCalculator(IRingMovementBondaryCalculationStrategy boundaryCalculator)
    {
        _BoundaryCalculator = boundaryCalculator;
    }

    /// <summary>
    /// Returns current touch position
    /// </summary>
    /// <returns>Touch position coodinates</returns>
    public Vector2 GetTouchPosition()
    {
        return _TouchPosition;
    }

    private void HandleObjectTouchStart()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(_TouchPosition);
        for (int i = 0; i < colliders.Length; ++i)
        {
            Collider2D collider = colliders[i];
            if (collider != null && collider.gameObject.tag == TrackingObjectTag)
            {
                if (onItemSelect != null)
                {
                    onItemSelect(collider.gameObject);
                }
            }
        }
    }

    private void HandleObjectTouchMove()
    {
        Vector2 moveToPosition = _BoundaryCalculator.GetNormalizedPosition(_TouchPosition);
        _ActiveObject.transform.position = Vector2.Lerp(_ActiveObject.transform.position, moveToPosition, MoveSpeed);
        if(_BoundaryCalculator.IsVerticalLimitCrossed(_TouchPosition) && onVerticalMovementLimitCrossed != null)
        {
            onVerticalMovementLimitCrossed(_ActiveObject, _BoundaryCalculator.GetVerticalLimit());
        }
    }

    private void HandleObjectTouchEnd()
    {
        if(onItemDrop != null)
        {
            onItemDrop(_ActiveObject);
        }
    }

    private void HandleTouchMove()
    {
        if (_BoundaryCalculator.IsAnyLimitCrossed(_TouchPosition) && onTouchLimitCrossed != null)
        {
            onTouchLimitCrossed(_ActiveObject);
        }
    }

    private void HandleTouchEnd()
    {
        if (onTouchDrop != null)
        {
            onTouchDrop(_ActiveObject);
        }
    }

    private bool IsObjectcatchEnabled()
    {
        return !_isObjectMovementHandlingEnabled && !_isTouchMovementHandlingEnabled;
    }

    private bool IsObjectMovementHandlingNeeded()
    {
        return _isObjectMovementHandlingEnabled;
    }

    private bool IsTouchMovementHandlingNeeded()
    {
        return _isTouchMovementHandlingEnabled;
    }
}

