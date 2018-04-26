using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level Generator is a container for both the information about the level
/// and the prefabs of the level. The generation happens in the Editor script
/// to keep the link to the prefab
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    /// <summary>
    /// The image to use to create the map
    /// </summary>
    public Texture2D m_tilesTexture;

    /// <summary>
    /// The image to use when adding the security systems to the
    /// </summary>
    public Texture2D m_objectsTexture;

    /// <summary>
    /// Contains how many units a single tile is (width, height)
    /// </summary>
    [SerializeField, Tooltip("X: Width, Y: Height")]
    Vector2 m_tileSize;
    public Vector2 TileSize { get { return m_tileSize; } }

    /// <summary>
    /// A list of the definitions for pixles to tiles definition
    /// </summary>
    public List<PixleColorToPrefab> m_tilesPixleMap;

    /// <summary>
    /// A list of definitions for pixles to object definition
    /// </summary>
    public List<PixleColorToPrefab> m_objectsPixleMap;    

    /// <summary>
    /// Where to parent all the tiles
    /// </summary>
    public GameObject m_tilesParentGO;

    /// <summary>
    /// Where to parent all the objects
    /// </summary>
    public GameObject m_objectsParentGO;  
}
