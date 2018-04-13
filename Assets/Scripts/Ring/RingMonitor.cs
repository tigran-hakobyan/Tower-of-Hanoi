using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMonitor : MonoBehaviour {

    public delegate void OnMoveComplete(GameObject ring);
    public event OnMoveComplete onMoveComplete;

    public int RingSize = 0;

    private const string _STATE_NEUTRAL_NAME    = "StateNeutral";
    private const string _STATE_ACTIVE_NAME     = "StateActive";
    private const string _STATE_SURPRISED_NAME  = "StateSurprised";
    private const string _STATE_CONFUSED_NAME   = "StateConfused";    
    private const string _STATE_WAVING_NAME     = "StateWaving";
    private const string _STATE_SAD_NAME        = "StateSad";

    private Transform _StateNeutral     = null;
    private Transform _StateActive      = null;
    private Transform _StateSurprised   = null;
    private Transform _StateConfused    = null;
    private Transform _StateWaving      = null;
    private Transform _StateSad         = null;
    private Transform _CurrentState     = null;

    private Vector3 _MoveInitialPosition;
    private Vector3 _MoveTargetPosition;
    private float _MovementTime     = 0.0f;
    private float _CurrentMoveStep  = 0.0f;
    private bool _MovementEnabled   = false;

    // Use this for initialization
    void Start () {
        InitStates();
        SetCurrentStateNeutral();
	}
	
	// Update is called once per frame
	void Update () {
        MoveIfNeeded();
    }

    /// <summary>
    /// Returns the size of the ring
    /// </summary>
    /// <returns>Ring size</returns>
    public int GetRingSize()
    {
        return RingSize;
    }

    /// <summary>
    /// Set the current state Neutral
    /// </summary>
    public void SetCurrentStateNeutral()
    {   
        SetCurrentState(ref _StateNeutral);
    }

    /// <summary>
    /// Set the current state Active
    /// </summary>
    public void SetCurrentStateActive()
    {
        SetCurrentState(ref _StateActive);
    }

    /// <summary>
    /// Set the current state Surprised
    /// </summary>
    public void SetCurrentStateSurprised()
    {
        SetCurrentState(ref _StateSurprised);
    }

    /// <summary>
    /// Set the current state Confused
    /// </summary>
    public void SetCurrentStateConfused()
    {
        SetCurrentState(ref _StateConfused);
    }

    /// <summary>
    /// Set the current state Waving
    /// </summary>
    public void SetCurrentStateWaving()
    {
        SetCurrentState(ref _StateWaving);
    }

    /// <summary>
    /// Set the current state Sad
    /// </summary>
    public void SetCurrentStateSad()
    {
        SetCurrentState(ref _StateSad);
    }

    /// <summary>
    /// Move to target position
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="time">needs to be in range [0.0, 1.0]</param>
    /// <param name="correctWithTargetPositionX">If true before moving the object X position will be corrected to target position's x value</param>
    public void MoveToPosition(Vector3 targetPosition, float time, bool correctWithTargetPositionX)
    {
        if(correctWithTargetPositionX)
        {
            this.transform.position = new Vector3(targetPosition.x, this.transform.position.y, this.transform.position.z);
        }
        
        _MoveInitialPosition = this.transform.position;
        _MoveTargetPosition = targetPosition;
        _MovementTime = time;
        _MovementEnabled = true;
    }

    private void MoveIfNeeded()
    {
        if (_MovementEnabled)
        {
            float rate = 1.0f / _MovementTime;
            if (_CurrentMoveStep < 1.0f)
            {
                _CurrentMoveStep += Time.deltaTime * rate;
                this.transform.position = Vector3.Lerp(_MoveInitialPosition, _MoveTargetPosition, _CurrentMoveStep);
            }
            else
            {
                _MovementEnabled = false;
               _CurrentMoveStep = 0.0f;
                MoveCompleteHandler();
            }
        }
    }

    private void MoveCompleteHandler()
    {
        if (onMoveComplete != null)
        {
            onMoveComplete(this.transform.gameObject);
        }
    }

    private void InitStates()
    {
        _StateNeutral = this.transform.Find(_STATE_NEUTRAL_NAME);
        EnableDisableState(ref _StateNeutral, false);

        _StateActive = this.transform.Find(_STATE_ACTIVE_NAME);
        EnableDisableState(ref _StateActive, false);

        _StateSurprised = this.transform.Find(_STATE_SURPRISED_NAME);
        EnableDisableState(ref _StateSurprised, false);

        _StateConfused = this.transform.Find(_STATE_CONFUSED_NAME);
        EnableDisableState(ref _StateConfused, false);

        _StateWaving = this.transform.Find(_STATE_WAVING_NAME);
        EnableDisableState(ref _StateWaving, false);

        _StateSad = this.transform.Find(_STATE_SAD_NAME);
        EnableDisableState(ref _StateSad, false);
    }

    private void EnableDisableState(ref Transform stateToEnableDisable, bool isActive)
    {
        if(stateToEnableDisable != null)
        {
            stateToEnableDisable.gameObject.SetActive(isActive);
        }
    }

    private void SetCurrentState(ref Transform state)
    {
        EnableDisableState(ref _CurrentState, false);
        _CurrentState = state;
        EnableDisableState(ref _CurrentState, true);
    }
}
