using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public float RingMovementTime 							= 0.5f;
    private RingMovementController _RingMovementController  = null;
    private PinController _PinController                    = null;
    private GameObject _ActiveRingInTheAir                  = null;
    private Transform _ActivePinForAirRing                  = null;
    private float _verticalCrossedLimit;

    void Start () {
        Init();
    }

    /// <summary>
    /// Ring selection handler
    /// </summary>
    /// <param name="ring">Touched ring</param>
    public void RingSelectHandler(GameObject ring)
    {
        if (IsActiveRingInTheAirSelected(ring))
        {
            ProcessRingInTheAirSelected(ring);
        }
        else if(false == IsActiveRingInTheAir() && IsTopRingOnPinSelected(ring))
        {
            ProcessTopRingOnThePinSelected(ring);
        }
        else
        {
            ProcessNotTopRingOnThePinSelected(ring);
        }
    }

    /// <summary>
    /// Ring "Touch End" handler
    /// </summary>
    /// <param name="ring">Ring to handle</param>
    public void RingDropHandler(GameObject ring)
    {
        _RingMovementController.StopObjectMovementHandling();

        if (IsTopRingOnPinSelected(ring))
        {
            Transform holderPin = _PinController.GetPinIfRingOnTop(ring);
            ring.SendMessage("SetCurrentStateNeutral");
            _RingMovementController.enabled = false;
            PinMonitor pinMonitor = holderPin.GetComponent<PinMonitor>();
            RingMonitor ringMonitor = ring.GetComponent<RingMonitor>();
            Vector3 topRingSlotPosition = pinMonitor.GetCurrentTopRingSlotPosition();
            ringMonitor.onMoveComplete += MoveToPositionOnPinHandler;
            ringMonitor.MoveToPosition(topRingSlotPosition, RingMovementTime / ringMonitor.GetRingSize(), true);
        }
        else
        {
            ring.SendMessage("SetCurrentStateSurprised");
        }
    }

    /// <summary>
    /// Touch end handler
    /// </summary>
    /// <param name="ring">Current active object</param>
    public void TouchDropHandler(GameObject ring)
    {
        ring.SendMessage("SetCurrentStateNeutral");
        _RingMovementController.StopTouchPointMovementHandling();
    }

    /// <summary>
    /// Pin collider touched handler
    /// </summary>
    /// <param name="activeRing">Active ring</param>
    /// <param name="selectedPin">Collided pin</param>
    public void PinSelectHandler(GameObject activeRing, Transform selectedPin)
    {
        _PinController.enabled = false;
        _RingMovementController.enabled = false;
        _RingMovementController.StopObjectMovementHandling();
        PinMonitor pinMonitor = selectedPin.GetComponent<PinMonitor>();
        RingMonitor ringMonitor = activeRing.GetComponent<RingMonitor>();
        Vector3 moveToPosition = pinMonitor.GetAvailableEmptySlotPosition();
        if (pinMonitor.CanAddRing(activeRing))
        {
            activeRing.SendMessage("SetCurrentStateActive");
            _ActivePinForAirRing = selectedPin;
            activeRing.GetComponent<RingMonitor>().onMoveComplete += MoveToPositionOnPinFromAirHandler;
            activeRing.GetComponent<RingMonitor>().MoveToPosition(moveToPosition, RingMovementTime / ringMonitor.GetRingSize(), true);
        }
        else
        {
            activeRing.SendMessage("SetCurrentStateSad");
            moveToPosition = pinMonitor.GetOverPinPosition();
            activeRing.GetComponent<RingMonitor>().onMoveComplete += MoveToPositionOverPinHandler;
            activeRing.GetComponent<RingMonitor>().MoveToPosition(moveToPosition, RingMovementTime, false);
        }
    }

    /// <summary>
    /// Vertcial movement boundary crossing handler
    /// </summary>
    /// <param name="ring">Active ring</param>
    /// <param name="verticalCrossedLimit">Crossed vertical limit value</param>
    public void RingVerticalMovementLimitCrossedHandler(GameObject ring, float verticalCrossedLimit)
    {
        if (false == IsActiveRingInTheAir())
        {
            _verticalCrossedLimit = verticalCrossedLimit;
            OverPinMovementBoundaryCalculator movementStrategy = new OverPinMovementBoundaryCalculator(verticalCrossedLimit);
            _RingMovementController.SetBoundaryCalculator(movementStrategy);
            Transform holderPin = _PinController.GetPinIfRingOnTop(ring);
            holderPin.SendMessage("RemoveTopRing");
            _ActiveRingInTheAir = ring;
            _PinController.ActiveRing = ring;
            _PinController.enabled = true;
        }
    }

    /// <summary>
    /// Handler for touch limit crossed for ring
    /// </summary>
    /// <param name="touchedRing">Touched ring</param>
    public void RingTouchLimitCrossedHandler(GameObject touchedRing)
    {
        touchedRing.SendMessage("SetCurrentStateSad");
    }

    private void Init()
    {
        _RingMovementController = this.GetComponentInChildren<RingMovementController>();
        _RingMovementController.onItemSelect += RingSelectHandler;
        _RingMovementController.onItemDrop += RingDropHandler;
        _RingMovementController.onVerticalMovementLimitCrossed += RingVerticalMovementLimitCrossedHandler;
        _RingMovementController.onTouchLimitCrossed += RingTouchLimitCrossedHandler;
        _RingMovementController.onTouchDrop += TouchDropHandler;

        _PinController = this.GetComponentInChildren<PinController>();
        _PinController.onPinSelect += PinSelectHandler;
        _PinController.enabled = false;
    }

    private void ProcessRingInTheAirSelected(GameObject ring)
    {
        IRingMovementBondaryCalculationStrategy boundaryCalculator;
        ring.SendMessage("SetCurrentStateActive");
        boundaryCalculator = new OverPinMovementBoundaryCalculator(_verticalCrossedLimit);
        _RingMovementController.StartObjectMovementHandling(ring, boundaryCalculator);
    }

    private void ProcessTopRingOnThePinSelected(GameObject ring)
    {
        IRingMovementBondaryCalculationStrategy boundaryCalculator;
        ring.SendMessage("SetCurrentStateActive");
        Transform holderPin = _PinController.GetPinIfRingOnTop(ring);
        boundaryCalculator = new PinStickedMovementBoundaryCalculator(ring, holderPin);
        _RingMovementController.StartObjectMovementHandling(ring, boundaryCalculator);
    }

    private void ProcessNotTopRingOnThePinSelected(GameObject ring)
    {
        IRingMovementBondaryCalculationStrategy boundaryCalculator;
        ring.SendMessage("SetCurrentStateConfused");
        Vector2 touchMoveDelta = new Vector2(1.5f, 0.5f);
        boundaryCalculator = new RingTouchRectangularBoundary(_RingMovementController.GetTouchPosition(), touchMoveDelta);
        _RingMovementController.StartTouchPointMovementHandling(ring, boundaryCalculator);
        if (IsActiveRingInTheAir())
        {
            _ActiveRingInTheAir.SendMessage("SetCurrentStateWaving");
        }
    }

    private void MoveToPositionOnPinHandler(GameObject ring)
    {
        ring.GetComponent<RingMonitor>().onMoveComplete -= MoveToPositionOnPinHandler;
        _RingMovementController.enabled = true;
        ring.SendMessage("SetCurrentStateNeutral");
    }

    private void MoveToPositionOnPinFromAirHandler(GameObject ring)
    {
        ring.GetComponent<RingMonitor>().onMoveComplete -= MoveToPositionOnPinFromAirHandler;
        PinMonitor pinMonitor = _ActivePinForAirRing.GetComponent<PinMonitor>();
        pinMonitor.AddRing(ring);
        ring.SendMessage("SetCurrentStateNeutral");
        _ActiveRingInTheAir = null;
        _RingMovementController.enabled = true;
    }

    private void MoveToPositionOverPinHandler(GameObject ring)
    {
        ring.GetComponent<RingMonitor>().onMoveComplete -= MoveToPositionOverPinHandler;
        _RingMovementController.enabled = true;
        _PinController.enabled = true;
        ring.SendMessage("SetCurrentStateSurprised");
    }

    private bool IsActiveRingInTheAir()
    {
        return _ActiveRingInTheAir != null;
    }

    private bool IsActiveRingInTheAirSelected(GameObject ring)
    {
        bool retVal = false;
        if(_ActiveRingInTheAir != null && _ActiveRingInTheAir.name == ring.name)
        {
            retVal = true;
        }
        return retVal;
    }

    private bool IsTopRingOnPinSelected(GameObject ring)
    {
        bool retVal = false;
        Transform holderPin = _PinController.GetPinIfRingOnTop(ring);
        if(holderPin != null)
        {
            retVal = true;
        }
        return retVal;
    }
}
