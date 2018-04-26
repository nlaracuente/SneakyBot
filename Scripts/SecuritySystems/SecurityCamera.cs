using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A security camera will spot the player when it enters its cone of vision
/// thus triggering a game oveer
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class SecurityCamera : SecuritySystem
{
    /// <summary>
    /// The smallest angle between current and destination before
    /// the camera snaps into the desired rotation
    /// </summary>
    [SerializeField, Tooltip("Angle before snapping into rotation")]
    float m_minRotationAngle = 5f;

    /// <summary>
    /// How fast to rotate the camera
    /// </summary>
    [SerializeField]
    float m_rotationSpeed = 2f;

    /// <summary>
    /// The rotation order in which the camera will rotate
    /// </summary>
    [SerializeField, Tooltip("Angles in degrees in the order to rotate")]
    List<float> m_angles = new List<float>();

    /// <summary>
    /// At what angle can the camera see the player
    /// </summary>
    [SerializeField]
    float m_fovAngle = 25f;

    /// <summary>
    /// How far to cast the ray for calculating the field of vision
    /// </summary>
    [SerializeField]
    float m_fovDisatance = 100f;

    /// <summary>
    /// The Field of View point used to determine the line
    /// renderer's direction and length
    /// </summary>
    [SerializeField]
    GameObject m_fovStartPoint;

    /// <summary>
    /// A collection of all the Field of View points to use
    /// when doing a raycast to detect for player collision
    /// </summary>
    [SerializeField]
    List<GameObject> m_fovPoints;

    /// <summary>
    /// A reference to the line renderer component which servers as a field of vision
    /// </summary>
    LineRenderer m_fovLine;

    /// <summary>
    /// Keeps track of the next rotation the camera will turn towards
    /// </summary>
    Queue<float> m_rotations;

    /// <summary>
    /// A reference to instance of the player 
    /// </summary>
    PlayerController m_player;

    /// <summary>
    /// How far to draw the raycast
    /// This is updated when the FOV is updated
    /// </summary>
    [SerializeField]
    float m_rayLength = 1f;

    /// <summary>
    /// Where to start the ray from
    /// </summary>
    [SerializeField]
    Transform m_rayTransform;

    /// <summary>
    /// A list of all the cardinal points when checking surrounding tiles
    /// </summary>
    [SerializeField]
    List<Vector3> m_cardinalPoints = new List<Vector3>() {
        Vector3.forward,
        Vector3.left,
        Vector3.back,
        Vector3.right,
    };

    /// <summary>
    /// Audio info the when the player moves
    /// </summary>
    [SerializeField]
    AudioClipInfo m_moveAudioInfo;

    /// <summary>
    /// A reference to the soundclip that controls the audio for the player
    /// </summary>
    SoundClip m_soundClip;

    /// <summary>
    /// How far to draw the ray when calculting collisions with wall
    /// </summary>
    [SerializeField]
    float m_wallRayDistance = 6f;

    [SerializeField]
    Material m_alertFOV;
    
    /// <summary>
    /// Sets references
    /// </summary>
    protected override void OnAwake()
    {
        m_rotations = new Queue<float>(m_angles);
        m_player = FindObjectOfType<PlayerController>();
        m_fovLine = GetComponent<LineRenderer>();

        m_soundClip = GetComponent<SoundClip>();
        m_soundClip.Info = m_moveAudioInfo;
        base.OnAwake();
    }

    /// <summary>
    /// Allow this system to be hackable from the start
    /// </summary>
    protected override void OnStart()
    {
        base.OnStart();
        IsHackable = true;
        // QueueRotationAngles();
        if (m_player == null) {
            Debug.LogError("SecurityCamera Error: Missing Player Component");
        }
    }

    /// <summary>
    /// Checks if the player is within range and trigges a game over
    /// </summary>
    void Update()
    {
        if (m_LevelController.IsGameOver) {
            return;
        }

        UpdateFOV();
        //ScanForPlayer();
    }

    /// <summary>
    /// Calculates the angles of rotation this camera can rotate towards
    /// based on its current position and its surroundings
    /// </summary>
    //void QueueRotationAngles()
    //{
    //    Vector3 origin = m_rayTransform.position;
    //    Queue<float> angels = new Queue<float>();

    //    foreach(Vector3 dir in m_cardinalPoints) {
    //        Vector3 direction = transform.InverseTransformDirection(dir);
    //        Ray ray = new Ray(origin, direction * m_wallRayDistance);
    //        Debug.DrawRay(origin, direction * m_wallRayDistance, Color.cyan, 3f);
    //        RaycastHit hit;

    //        // Something is there, try a different direction
    //        if (Physics.Raycast(ray, out hit)) {
    //            Debug.Log("Collision: " + hit.collider.name);
    //            continue;
    //        }

    //        Quaternion rotation = Quaternion.LookRotation(direction);
    //        float angle = Quaternion.Angle(transform.rotation, rotation);

    //        Debug.LogFormat("Direction: {0}, Angle: {1}", direction, angle);
    //        angels.Enqueue(angle);
    //    }

    //    // Build the permanent queue ensureing the first item is not the current rotation
    //    float curRotation = transform.rotation.y;
    //    for (int i = 0; i < angels.Count; i++) {
    //        float angle = angels.Dequeue();

    //        // Prevents the first rotaiton from being the current one but also
    //        // avoid and infinite loop by checking this isn't the only one we got
    //        if (i == 0 && angle == curRotation && angels.Count > 1) {
    //            angels.Enqueue(angle);
    //            continue;
    //        }

    //        Debug.LogFormat("Angle Stored: {0}", angle);
    //        m_rotations.Enqueue(angle);
    //        m_angles.Add(angle);
    //    }
    //}

    /// <summary>
    /// Updates the Field of Vision by casting a line from the camera
    /// to the first object it hits using a ray cast
    /// </summary>
    void UpdateFOV()
    {
        // Update the start as the camera may have moved
        m_fovLine.SetPosition(0, m_fovStartPoint.transform.position);

        Vector3 startPos = m_fovStartPoint.transform.position;
        Vector3 direction = m_fovStartPoint.transform.forward;
        
        Ray ray = new Ray(startPos, direction * m_fovDisatance);
        Debug.DrawRay(m_fovStartPoint.transform.position, direction, Color.cyan);

        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) {
            return;
        }

        GameObject hitGo = hit.collider.gameObject;

        // Extend the line to reach the object it is hitting
        m_rayLength = Vector3.Distance(startPos, hitGo.transform.position);
        Vector3 destination = startPos + (direction * m_rayLength);
        m_fovLine.SetPosition(1, destination);

        if (hitGo.CompareTag("Player")) {
            AudioManager.instance.PlaySound(AudioName.DeathByCamera);
            m_fovLine.material = m_alertFOV;
            StopAllCoroutines();
            m_LevelController.TriggerGameOver();
        }
    }
    
    /// <summary>
    /// Uses all of its field of view points to raycast to see
    /// if the player is in sight and trigger a game over
    /// </summary>
    //void ScanForPlayer()
    //{
    //    // Then at least use the one used for the line renderer
    //    if(m_fovPoints.Count < 1) {
    //        m_fovPoints = new List<GameObject>() { m_fovStartPoint };
    //    }

    //    bool playerFound = false;
    //    foreach (GameObject point in m_fovPoints) {
    //        if(point == null) {
    //            continue;
    //        }
            
    //        if (IsPlayerInRayPath(point.transform)) {
    //            playerFound = true;
    //            break;
    //        }
    //    }

    //    if (playerFound) {
    //        m_LevelController.TriggerGameOver();
    //    }
    //}

    /// <summary>
    /// Returns true if at least one of the raycast fired from
    /// the FoV points connects with the player
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    //bool IsPlayerInRayPath(Transform origin)
    //{
    //    bool playerHit = false;
    //    Vector3 direction = origin.forward * m_rayLength;

    //    Ray ray = new Ray(origin.position, direction);
    //    Debug.DrawRay(origin.position, direction, Color.red, 1f);

    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit)) {
    //        playerHit = hit.collider.CompareTag("Player");
    //    }

    //    return playerHit;
    //}

    /// <summary>
    /// Sets the camera to not be hackable while the the rotation is still hapenning
    /// Smoothly rotates the camera to face the given direction
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    IEnumerator RotationRoutine(float angle)
    {
        IsHackable = false;
        Vector3 rotationVector = Vector3.up * angle;
        Quaternion targetRotation = Quaternion.Euler(Vector3.up * angle);

        m_soundClip.Play();
        while (Quaternion.Angle(transform.rotation, targetRotation) > m_minRotationAngle) {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                m_rotationSpeed * Time.deltaTime
            );
            yield return new WaitForFixedUpdate();
        }
        m_soundClip.Stop();

        transform.rotation = targetRotation;
        m_rotations.Enqueue(angle);
        IsHackable = true;
    }

    /// <summary>
    /// Ignores request
    /// </summary>
    public override void PowerHack(bool powerOn) { }

    /// <summary>
    /// Ignores request
    /// </summary>
    public override void ToggleHack() { }

    /// <summary>
    /// Triggers the camera to rotate towards the nexr available rotation angle
    /// </summary>
    public override void RotationHack()
    {
        // Can't process the rotation
        if (!IsHackable || m_rotations.Count < 1) {
            return;
        }

        StartCoroutine(RotationRoutine(m_rotations.Dequeue()));
    }
}
