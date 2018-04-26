using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for moving the player in the desire direction
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    /// <summary>
    /// How fast the player moves
    /// </summary>
    [SerializeField]
    float m_moveSpeed = 5f;

    /// <summary>
    /// How close to the destination vector when moving is allowed
    /// before considering the movement to be done
    /// </summary>
    [SerializeField]
    float m_destinationProximity = .01f;

    /// <summary>
    /// How fast to rotate
    /// </summary>
    [SerializeField]
    float m_rotationSpeed = 12f;

    /// <summary>
    /// How close to the angle of rotation must the transform be before considering the transform as complete
    /// </summary>
    [SerializeField]
    float m_rotationAngleProximity = .01f;

    /// <summary>
    /// A reference to the rigidbody component
    /// </summary>
    Rigidbody m_rigidbody;

    /// <summary>
    /// True while the player is still moving
    /// </summary>
    bool m_isMoving = false;
    public bool IsMoving { get { return m_isMoving; } }

    /// <summary>
    /// Sets references 
    /// </summary>
    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();

        if (m_rigidbody == null) {
            Debug.LogErrorFormat("PlayerController Error: Missing Component! " +
                "Rigidbody: {0}",
                m_rigidbody
            );
        }
    }

    /// <summary>
    /// Triggers the movement routine
    /// </summary>
    /// <param name="direction"></param>
    public void TriggerMovement(Vector3 direction)
    {
        Move(direction);
        Rotate(direction);
    }

    /// <summary>
    /// Moves the player
    /// </summary>
    /// <param name="direction"></param>
    void Move(Vector3 direction)
    {
        Vector3 movement = direction.normalized * m_moveSpeed * Time.deltaTime;
        m_rigidbody.MovePosition(transform.position + movement);

    }

    /// <summary>
    /// Rotates the player to face the given direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    void Rotate(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        m_rigidbody.MoveRotation(
            Quaternion.Lerp(transform.rotation,
                            targetRotation, 
                            m_rotationSpeed * Time.deltaTime)
        );
    }
}
