using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToMousePosition : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float sensitivity = 1f;

    private Transform targetTransform;

    private void Update()
    {
        DetectTargetTransform();
        transform.position = MousePosition();
    }

    private void DetectTargetTransform()
    {
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity))
        {
            targetTransform = hit.transform;
        }
    }

    private Vector3 MousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        // mousePosition.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, targetTransform == null ? camera.nearClipPlane : targetTransform.position.z));
    }
}
