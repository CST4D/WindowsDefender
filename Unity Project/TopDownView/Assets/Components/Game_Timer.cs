using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class Game_Timer : MonoBehaviour {
    /// <summary>
    /// The TXT_ timer
    /// </summary>
    public UnityEngine.UI.Text txt_Timer;

    /// <summary>
    /// The _ time
    /// </summary>
    float _Time;

    /// <summary>
    /// Sets the time.
    /// </summary>
    /// <param name="time">The time.</param>
    public void setTime(float time)
    {
        _Time = time;
    }

    /// <summary>
    /// Ticks this instance.
    /// </summary>
    public void tick()
    {
        _Time -= 1 * Time.deltaTime;
        txt_Timer.text = ((int)_Time).ToString();
    }

    /// <summary>
    /// Times up.
    /// </summary>
    /// <returns></returns>
    public bool timeUp()
    {
        if (_Time <= 0)
            txt_Timer.text = "";

        return _Time <= 0;
    }

    // Use this for initialization
    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
	
	}

    // Update is called once per frame
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update () {
	
	}
}
