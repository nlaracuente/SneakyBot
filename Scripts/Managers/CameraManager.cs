using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Design to manage all camera controls
/// Currently set to find and track the player or whatever target it is assigned
/// </summary>
[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// The transform to track
    /// </summary>
    [SerializeField]
    Transform m_target;
    public Transform Target { set { m_target = value; } }

    /// <summary>
    /// The camera to use to track the target
    /// </summary>
    [SerializeField]
    Camera m_camera;
    public Camera CurrentCamera { set { m_camera = value; } }

    /// <summary>
    /// How fast to track the target
    /// </summary>
    [SerializeField]
    float m_trackingSpeed = 5f;
    public float TrackingSpeed { get { return m_trackingSpeed; }  set { m_trackingSpeed = value; } }

    /// <summary>
    /// How far away from the target to stay at when tracking
    /// </summary>
    [SerializeField]
    Vector3 m_trackingDistance = new Vector3(0f, 10f, 0f);

    /// <summary>
    /// How close to the target before to consider it as "close enough"
    /// </summary>
    [SerializeField]
    float m_destinationProximity = .01f;

    /// <summary>
    /// Initialize references
    /// </summary>
    void Awake()
    {
        SetPlayerAsTarget();
        if (m_camera == null) {
            m_camera = Camera.main;
        }
    }

    /// <summary>
    /// Smoothly tracks the target
    /// </summary>
    void LateUpdate()
    {
        // This is so that in the editor it can find the player whenever the
        // level is rebuilt and the player is destroyed and re-added
        SetPlayerAsTarget();

        // Can't track unless we know what to track or with that to track it with
        if (m_target == null || m_camera == null) {
            return;
        }

        Vector3 targetPosition = m_target.position + m_trackingDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, m_trackingSpeed * Time.deltaTime);
    }

    /// <summary>
    /// If a target is not already assigned it will attempt to located the 
    /// player and set it as the target
    /// </summary>
    void SetPlayerAsTarget()
    {
        if (m_target == null) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) {
                m_target = player.GetComponent<Transform>();
            }
        }
    }

    /// <summary>
    /// Changes the camera target to track
    /// Waits until the camera is close enough to the target before finishing
    /// This is triggered when an event to have the camera focus on something
    /// is triggered and we need to wait for the camera to finish tracking
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public IEnumerator ChangeTargetRoutine(Transform target)
    {
        if (target != null) {
            m_target = target;
            while (Vector3.Distance(transform.position, target.position) > m_destinationProximity) {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
