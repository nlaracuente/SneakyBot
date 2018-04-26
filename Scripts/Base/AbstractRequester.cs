using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// A hack requestor is an object that can trigger a request to hack
/// </summary>
public abstract class AbstractRequester : MonoBehaviour
{
    /// <summary>
    /// A reference to the level controller
    /// </summary>
    protected LevelController m_LevelController;

    /// <summary>
    /// A reference to the UI button used to trigger the hack
    /// </summary>
    protected Button m_button;

    /// <summary>
    /// How long before re-enabling the button
    /// </summary>
    protected float m_coolDownDelay = 1f;

    /// <summary>
    /// True when the button is interactible
    /// </summary>
    public bool IsEnabled { get{ return m_button.interactable; } }

    /// <summary>
    /// Sets references
    /// </summary>
    void Awake()
    {
        OnAwake();
    }

    /// <summary>
    /// Allows for child classes to override Awake()
    /// </summary>
    protected virtual void OnAwake() {
        m_button = GetComponent<Button>();
        m_LevelController = FindObjectOfType<LevelController>();
        if (m_LevelController == null) {
            Debug.LogErrorFormat("SecuritySystem Error: Missing Component! " +
                "LevelController: {0}",
                m_LevelController
            );
        }
    }

    /// <summary>
    /// Triggered when the hack request is made
    /// </summary>
    public abstract void Hack();

    /// <summary>
    /// Enables the use of the button within the control panel
    /// </summary>
    public virtual void Enable()
    {
        m_button.interactable = true;
    }

    /// <summary>
    /// Disables the use of the button within the control panel
    /// Cancels the CoolDownRoutine routine to avoid re-enabling the button
    /// </summary>
    public virtual void Disable()
    {
        m_button.interactable = false;
        StopCoroutine(CoolDownRoutine());
    }

    /// <summary>
    /// Handles re-enabling the button after cool down delay
    /// </summary>
    /// <returns></returns>
    protected IEnumerator CoolDownRoutine()
    {
        yield return new WaitForSeconds(m_coolDownDelay);
        Enable();
    }
}