using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class Tile : MonoBehaviour {

    /// <summary>
    /// The sprite buildable
    /// </summary>
    public Sprite SpriteBuildable;
    /// <summary>
    /// The sprite unbuildable
    /// </summary>
    public Sprite SpriteUnbuildable;

    /// <summary>
    /// The _current sprite
    /// </summary>
    private Sprite _currentSprite;
    /// <summary>
    /// The _buildable
    /// </summary>
    private bool _buildable;
    /// <summary>
    /// The _walkable
    /// </summary>
    private bool _walkable;
    /// <summary>
    /// The _color state
    /// </summary>
    private UnityEngine.Color _colorState;
    /// <summary>
    /// The map sprite
    /// </summary>
    public Sprite mapSprite = null;
    /// <summary>
    /// The trans color
    /// </summary>
    private UnityEngine.Color transColor;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="Tile"/> is buildable.
    /// </summary>
    /// <value>
    ///   <c>true</c> if buildable; otherwise, <c>false</c>.
    /// </value>
    public bool Buildable
    {
        set { _buildable = value; }
        get { return _buildable; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="Tile"/> is walkable.
    /// </summary>
    /// <value>
    ///   <c>true</c> if walkable; otherwise, <c>false</c>.
    /// </value>
    public bool Walkable
    {
        set { _walkable = value; }
        get { return _walkable; }
    }

    // Use this for initialization
    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        transColor = new UnityEngine.Color(0, 0, 0, 0);
	}

    // Update is called once per frame
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update () {
        if (mapSprite != null)
        {
            _currentSprite = mapSprite;
            GetComponent<SpriteRenderer>().sprite = _currentSprite;
            GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
        } else
        {
            GetComponent<SpriteRenderer>().color = transColor;
        }
        /*else if(_walkable && _currentSprite != SpriteBuildable)
        {
            //_currentSprite = SpriteBuildable;
            //GetComponent<SpriteRenderer>().sprite = _currentSprite;
        } else if (!_walkable && _currentSprite != SpriteUnbuildable)
        {
            //_currentSprite = SpriteUnbuildable;
            //GetComponent<SpriteRenderer>().sprite = _currentSprite;
        }

        if (_buildable && _colorState != UnityEngine.Color.green)
        {
            //_colorState = UnityEngine.Color.green;
            //GetComponent<SpriteRenderer>().color = _colorState;
        }
        else if (!_buildable && _colorState != UnityEngine.Color.red)
        {
            //_colorState = UnityEngine.Color.red;
            //GetComponent<SpriteRenderer>().color = _colorState;
        }*/
    }
}
