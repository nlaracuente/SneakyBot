using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A NavPoint represents a destination where a guard can go to
/// </summary>
public class NavPoint : MonoBehaviour
{
    /// <summary>
    /// What color to use when drawing gizmos
    /// </summary>
    [SerializeField, Tooltip("What color to draw the cube")]
    Color m_color = new Color(1f, 0f, 0f, .5f);

    /// <summary>
    /// What size to draw the gizmo at
    /// </summary>
    [SerializeField, Tooltip("How large to draw the cube")]
    Vector3 m_size = Vector3.one;

    /// <summary>
    /// Returns this navpoint's current position
    /// </summary>
    public Vector3 Position { get { return transform.position; } }

    /// <summary>
    /// Helps visualization in the editor as these are empty objects
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = m_color;
        Gizmos.DrawCube(transform.position, m_size);
    }
}
