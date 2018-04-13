using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinMonitor : MonoBehaviour {

    public float RingOffset = 1.2f;
    public float RingInitialPositionOffset = 0.5f;

    private Stack<GameObject> _ringStack = new Stack<GameObject>();
    private Vector3 _CurrentTopRingSlotPosition;

    private const float _OVER_PIN_Y_DELTA = 1.5f;

    public Vector3 GetCurrentTopRingSlotPosition()
    {
        return _CurrentTopRingSlotPosition;
    }

    public Vector3 GetAvailableEmptySlotPosition()
    {
        Vector3 availablePosition = GetInitialEmptySlotPosition();
        GameObject topRing = PeekTopRing();
        if (topRing != null)
        {   
            availablePosition.y = topRing.transform.position.y + RingOffset;
        }
        return availablePosition;
    }

    public Vector3 GetOverPinPosition()
    {
        Vector3 initialPosition = this.transform.position;
        initialPosition.y += _OVER_PIN_Y_DELTA;
        return initialPosition;
    }

    public bool CanAddRing(GameObject ringToCheck)
    {
        bool retVal = false;
        GameObject topRing = PeekTopRing();
        if (topRing != null)
        {
            RingMonitor topRingComponent = topRing.GetComponent<RingMonitor>();
            RingMonitor ringToCheckComponent = ringToCheck.GetComponent<RingMonitor>();
            if (topRingComponent.GetRingSize() > ringToCheckComponent.GetRingSize())
            {
                retVal = true;
            }
        }
        else
        {
            retVal = true;
        }
        return retVal;
    }

    public bool AddRing(GameObject ringToAdd)
    {
        bool retVal = CanAddRing(ringToAdd);
        if(retVal)
        {
            AddRingInternal(ringToAdd);
        }
        return retVal;
    }

    public bool RemoveTopRing()
    {
        bool retVal = false;
        GameObject topRing = PeekTopRing();
        if (topRing != null)
        {
            _ringStack.Pop();
            UpdateCurrentTopRingSlotPosition();
            retVal = true;
        }
        return retVal;
    }

    public bool IsRingOnTop(string nameToCheck)
    {
        bool retVal = false;
        GameObject topRing = PeekTopRing();
        if (topRing != null && topRing.name.Equals(nameToCheck))
        {
            retVal = true;
        }
        return retVal;
    }

    private void UpdateCurrentTopRingSlotPosition()
    {
        GameObject topRing = PeekTopRing();
        if (topRing != null)
        {
            _CurrentTopRingSlotPosition = topRing.transform.position;
        }
        else
        {
            _CurrentTopRingSlotPosition = new Vector3(-1.0f, -1.0f, -1.0f);
        }
    }

    private void AddRingInternal(GameObject ringToAdd)
    {
        _ringStack.Push(ringToAdd);
        UpdateCurrentTopRingSlotPosition();
    }

    private Vector3 GetInitialEmptySlotPosition()
    {
        Vector3 initialPosition = this.transform.position;
        initialPosition.y -= this.GetComponent<SpriteRenderer>().bounds.size.y - RingInitialPositionOffset;
        return initialPosition;
    }

    private GameObject PeekTopRing()
    {
        if(_ringStack.Count > 0)
        {
            return _ringStack.Peek();
        }
        return null;
    }
}
