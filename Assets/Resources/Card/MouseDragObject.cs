using UnityEngine;

namespace WebXR.Interactions
{
  [RequireComponent(typeof(Rigidbody))]
  public class MouseDragObject : MonoBehaviour
  {
    [Tooltip("If true, clicks on child colliders will also trigger dragging")]
    public bool includeChildColliders = false;

    private Camera m_currentCamera;
    private Rigidbody m_rigidbody;
    private Vector3 m_screenPoint;
    private Vector3 m_offset;
    private Vector3 m_currentVelocity;
    private Vector3 m_previousPos;
    private bool m_isDragging = false;

    void Awake()
    {
      m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
      // Handle mouse input for dragging
      if (Input.GetMouseButtonDown(0))
      {
        if (IsMouseOverObject())
        {
          StartDragging();
        }
      }
      else if (Input.GetMouseButtonUp(0) && m_isDragging)
      {
        StopDragging();
      }
    }

    void OnMouseDown()
    {
      // Keep this for backward compatibility when includeChildColliders is false
      if (!includeChildColliders)
      {
        StartDragging();
      }
    }

    void OnMouseUp()
    {
      // Keep this for backward compatibility when includeChildColliders is false
      if (!includeChildColliders && m_isDragging)
      {
        StopDragging();
      }
    }

    private void StartDragging()
    {
      m_currentCamera = FindCamera();
      if (m_currentCamera != null)
      {
        m_screenPoint = m_currentCamera.WorldToScreenPoint(gameObject.transform.position);
        m_offset = gameObject.transform.position - m_currentCamera.ScreenToWorldPoint(GetMousePosWithScreenZ(m_screenPoint.z));
        m_isDragging = true;
      }
    }

    private void StopDragging()
    {
      m_rigidbody.linearVelocity = m_currentVelocity;
      m_currentCamera = null;
      m_isDragging = false;
    }

    private bool IsMouseOverObject()
    {
      if (!includeChildColliders)
      {
        return false; // Use OnMouseDown/OnMouseUp instead
      }

      Camera camera = FindCamera();
      if (camera == null)
      {
        return false;
      }

      Ray ray = camera.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        // Check if the hit object is this object or a child
        Transform hitTransform = hit.transform;
        while (hitTransform != null)
        {
          if (hitTransform == transform)
          {
            return true;
          }
          hitTransform = hitTransform.parent;
        }
      }

      return false;
    }

    void FixedUpdate()
    {
      if (m_isDragging && m_currentCamera != null)
      {
        Vector3 currentScreenPoint = GetMousePosWithScreenZ(m_screenPoint.z);
        m_rigidbody.linearVelocity = Vector3.zero;
        m_rigidbody.MovePosition(m_currentCamera.ScreenToWorldPoint(currentScreenPoint) + m_offset);
        m_currentVelocity = (transform.position - m_previousPos) / Time.deltaTime;
        m_previousPos = transform.position;
      }
    }

    Vector3 GetMousePosWithScreenZ(float screenZ)
    {
      return new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenZ);
    }

    Camera FindCamera()
    {
#if UNITY_2023_1_OR_NEWER
      Camera[] cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
#else
      Camera[] cameras = FindObjectsOfType<Camera>();
#endif
      Camera result = null;
      int camerasSum = 0;
      foreach (var camera in cameras)
      {
        if (camera.enabled)
        {
          result = camera;
          camerasSum++;
        }
      }
      if (camerasSum > 1)
      {
        result = null;
      }
      return result;
    }
  }
}
