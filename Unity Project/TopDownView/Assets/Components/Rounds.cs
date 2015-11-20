using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class Rounds : MonoBehaviour {
    //int rounds;

    /// <summary>
    /// The enemies
    /// </summary>
    public EnemyAI[] enemies;
    /// <summary>
    /// The not parsed
    /// </summary>
    public bool notParsed;

    //ArrayList _waves;
    /// <summary>
    /// The _current wave
    /// </summary>
    ArrayList _currentWave;
    /// <summary>
    /// The _count
    /// </summary>
    int _count;

    // Use this for initialization
    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {        
        _currentWave = new ArrayList();
    }

    // Update is called once per frame
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update () {
	}

    /// <summary>
    /// Placeholder Wave Spawner for test purposes
    /// </summary>
    public void parseWave()
    {
        if (enemies.Length >= 2)
        {
            _currentWave.Add(enemies[0]);
            _currentWave.Add(enemies[1]);
            _currentWave.Add(enemies[2]);
            _currentWave.Add(enemies[3]);
            //_currentWave.Add(enemies[4]);         
        }

        notParsed = false;
    }
    /// <summary>
    /// Returns the Enemy Object that will be spawned in the game
    /// </summary>
    /// <returns></returns>
    public EnemyAI getEnemySpawn()
    {
        _count = (_count + 1) % _currentWave.Count;

        return (EnemyAI) _currentWave[_count];
    }
}
