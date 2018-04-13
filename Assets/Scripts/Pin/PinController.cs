using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour {

    public delegate void OnPinSelect(GameObject activeRing, Transform selectedPin);
    public event OnPinSelect onPinSelect;

    public Transform StartPin;
    public Transform MiddlePin;
    public Transform WinPin;

    public GameObject ActiveRing { get; set; }

    private Bounds _StartPinColliderBounds;
    private Bounds _MiddlePinColliderBounds;
    private Bounds _WinPinColliderBounds;

    private Bounds _ActiveRingBounds;

    // Use this for initialization
    void Start () {
        _StartPinColliderBounds = StartPin.GetComponent<BoxCollider2D>().bounds;
        _MiddlePinColliderBounds = MiddlePin.GetComponent<BoxCollider2D>().bounds;
        _WinPinColliderBounds = WinPin.GetComponent<BoxCollider2D>().bounds;
    }
	
	// Update is called once per frame
	void Update () {
        CheckIntersectionWithPinColliders();
    }

    /// <summary>
    /// Returns the Transform component of the Pin if ring is on top of it
    /// </summary>
    /// <param name="ring">Ring to check</param>
    /// <returns>The Transform component or null if ring not on top</returns>
    public Transform GetPinIfRingOnTop(GameObject ring)
    {
        Transform pinAttached = null;
        if (StartPin.GetComponent<PinMonitor>().IsRingOnTop(ring.name))
        {
            pinAttached = StartPin;
        }
        else if (MiddlePin.GetComponent<PinMonitor>().IsRingOnTop(ring.name))
        {
            pinAttached = MiddlePin;
        }
        else if (WinPin.GetComponent<PinMonitor>().IsRingOnTop(ring.name))
        {
            pinAttached = WinPin;
        }
        return pinAttached;
    }

    private void CheckIntersectionWithPinColliders()
    {
        if (onPinSelect != null)
        {
            _ActiveRingBounds = ActiveRing.GetComponent<BoxCollider2D>().bounds;
            if (_StartPinColliderBounds.Intersects(_ActiveRingBounds))
            {
                onPinSelect(ActiveRing, StartPin);
            }

            if (_MiddlePinColliderBounds.Intersects(_ActiveRingBounds))
            {
                onPinSelect(ActiveRing, MiddlePin);
            }
            if (_WinPinColliderBounds.Intersects(_ActiveRingBounds))
            {
                onPinSelect(ActiveRing, WinPin);
            }
        }
    }
}
