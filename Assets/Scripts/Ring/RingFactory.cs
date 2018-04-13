using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingFactory : MonoBehaviour {

    public Transform Ring_1;
    public Transform Ring_2;
    public Transform Ring_3;
    public Transform Ring_4;
    public Transform StartingPin;

    void Awake()
    {
        CreateRingsAndAttachToPin();
    }

    private void CreateRingsAndAttachToPin()
    {
        if (Ring_1 != null && Ring_2 != null && Ring_3 != null && Ring_4 != null && StartingPin != null)
        {
            CreateRingAndAttachToPin(Ring_4);
            CreateRingAndAttachToPin(Ring_3);
            CreateRingAndAttachToPin(Ring_2);
            CreateRingAndAttachToPin(Ring_1);
        }
    }

    private void CreateRingAndAttachToPin(Transform ring)
    {
        PinMonitor pinMonitor = StartingPin.GetComponent<PinMonitor>();
        Vector3 ringPosition = pinMonitor.GetAvailableEmptySlotPosition();
        Transform instance = Instantiate(ring, ringPosition, Quaternion.identity);
        instance.parent = transform;
        pinMonitor.AddRing(instance.gameObject);
    }
}
