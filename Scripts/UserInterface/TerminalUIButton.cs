using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// These are the UI button that the player uses at terminals to hack a system
/// </summary>
[RequireComponent(typeof(AbstractRequester))]
public class TerminalUIButton : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// A reference to the hack request component
    /// </summary>
    AbstractRequester m_hackRequester;

    [SerializeField]
    KeyCode m_key;

    /// <summary>
    /// Set references
    /// </summary>
    void Awake()
    {
        m_hackRequester = GetComponent<AbstractRequester>();
    }

    void Update()
    {
        if (Input.GetKeyDown(m_key)) {
            RequestHack();
        }
    }

    /// <summary>
    /// Triggers a hack request
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        RequestHack();
    }

    /// <summary>
    /// Triggers the request to hack
    /// </summary>
    public void RequestHack()
    {
        if (m_hackRequester.IsEnabled) {
            AudioManager.instance.PlaySound(AudioName.Button);
            m_hackRequester.Hack();
        }
    }
}
