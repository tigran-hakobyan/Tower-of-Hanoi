using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float AspectRatio = 16.0f / 9.0f;

    // Use this for initialization
    void Start () {
        float currentWindowAspect = (float)Screen.width / (float)Screen.height;

        float scaleHeight = currentWindowAspect / AspectRatio;

        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) * 0.5f;

            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) * 0.5f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
