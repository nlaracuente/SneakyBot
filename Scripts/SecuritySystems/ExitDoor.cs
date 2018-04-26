using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Exit door blocks the player from exiting/winning the level
/// </summary>
[RequireComponent(typeof(Animator))]
public class ExitDoor : MonoBehaviour
{
    /// <summary>
    /// A reference to the level controller
    /// </summary>
    LevelController m_LevelController;

    /// <summary>
    /// A reference to the animator component
    /// </summary>
    Animator m_animator;

    /// <summary>
    /// Allows for child classes to override Awake()
    /// </summary>
    void Awake()
    {
        m_LevelController = FindObjectOfType<LevelController>();
        m_animator = GetComponent<Animator>();

        if (m_LevelController == null || m_animator == null) {
            Debug.LogErrorFormat("ExitDoor Error: Missing Component! " +
                "LevelController: {0}, Animator: {1}",
                m_LevelController,
                m_animator
            );
        }
    }

    /// <summary>
    /// Triggers the win sequence
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            m_LevelController.TriggerWin();
        }
    }

    /// <summary>
    /// Triggers the door to open
    /// </summary>
    public void Open()
    {
        m_animator.SetTrigger("Win");
    }
}
