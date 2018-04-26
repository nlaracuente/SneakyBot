using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Remains "OFF" until the player completes all objectives
/// It then turns on
/// </summary>
[RequireComponent(typeof(Renderer))]
public class GoalTile : MonoBehaviour
{
    /// <summary>
    /// Material for when active
    /// </summary>
    [SerializeField]
    Material m_activeMat;

    /// <summary>
    /// Material for when deactivated
    /// </summary>
    [SerializeField]
    Material m_deactivatedMat;

    /// <summary>
    /// A reference to the renderer component
    /// </summary>
    Renderer m_renderer;

    /// <summary>
    /// Changes the tile to the active state
    /// </summary>
    bool m_isActive = false;
    public bool Activate
    {
        get { return m_isActive; }
        set {
            m_isActive = value;

            // Triggers the alarm
            if (value) {
                AudioManager.instance.SetMusicVolume(0f);
                AudioManager.instance.PlaySound(AudioName.Alarm);
                m_renderer.material = m_activeMat;
            } else {
                m_renderer.material = m_deactivatedMat;
            }
        }
    }

    void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// Triggers the win
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (m_isActive && other.CompareTag("Player")) {
            FindObjectOfType<LevelController>().TriggerWin();
        }
    }
}
