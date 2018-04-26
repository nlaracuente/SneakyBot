using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles receiving, interpreting, and storing user input
/// </summary>
public class InputManager : MonoBehaviour
{
    /// <summary>
    /// A reference to self
    /// </summary>
    public static InputManager instance;

    /// <summary>
    /// Holds the player's direction input vector 
    /// </summary>
    Vector3 m_inputVector = Vector3.zero;
    public Vector3 InputVector { get { return m_inputVector; } }

    /// <summary>
    /// True: causes the input manager to always set the input vector to Vector.zero
    /// </summary>
    bool m_inputDisabled = false;
    public bool DisableInput
    {
        get { return m_inputDisabled; }
        set {m_inputDisabled = value;}
    }

    /// <summary>
    /// Set references
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Store the player's input
    /// This uses a very rough and simple logic to switch between moving vertically vs horixontally
    /// This is because it designed to allow movement on one of this axis at a time
    /// To remove this restriction simply remove the conditions for m_pressedInput and 
    /// always store the values of h and v
    /// </summary>
    void Update()
    {
        // Ignore
        if (m_inputDisabled) {
            m_inputVector = Vector3.zero;
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");


        m_inputVector.Set(h, 0f, v);
    }
}
