using UnityEngine;

/// <summary>
/// Helps translate a pixle color into a prefab
/// </summary>
[System.Serializable]
public struct PixleColorToPrefab
{
    /// <summary>
    /// The pixle color
    /// </summary>
    public Color32 color;

    /// <summary>
    /// The prefab associated with the pixle color
    /// </summary>
    public GameObject prefab;
}