using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public EnemySpawner _enemySpawner;

    private bool _gameOver = false;
    private bool _win = true;
    private int _wave = -1;
    private int _nextWave = 0;
    private bool _waitingForRestart = false;

    public delegate IEnumerator WaveProcess();
    private WaveProcess[] Waves;

    void Start()
    {
        Waves = new WaveProcess[]
        {
            Wave1,
            Wave1,
            Wave1,
        };

        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if (_waitingForRestart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Game");
                return;
            }
        }
    }

    void CheckWaveEnd()
    {
        if (_enemySpawner.GetEnemyCount() <= 0)
        {
            _wave++;
        }
    }

    IEnumerator Wave1()
    {
        yield return new WaitForSeconds(1);

        _enemySpawner.CreateEnemy("LeftTop");
        _enemySpawner.CreateEnemy("LeftBottom");
        _enemySpawner.CreateEnemy("RightTop");
        _enemySpawner.CreateEnemy("RightBottom");

        yield return new WaitForSeconds(1);

        _enemySpawner.CreateEnemy("LeftTop");
        _enemySpawner.CreateEnemy("LeftBottom");
        _enemySpawner.CreateEnemy("RightTop");
        _enemySpawner.CreateEnemy("RightBottom");

        yield return new WaitForSeconds(1);
    }

    IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(1);

        _gameOver = false;
        while (!_gameOver)
        {
            if (_nextWave > _wave)
            {
                _wave = _nextWave;

                Debug.Log("wave " + _wave);

                Messenger.Broadcast<int>("WaveStart", _wave + 1);

                yield return Waves[_wave]();
            }
            else
            {
                // check all enmey died;
                if (_enemySpawner.GetEnemyCount() <= 0)
                {
                    _nextWave++;
                    if (_nextWave >= Waves.Length)
                    {
                        _gameOver = true;
                        _win = true;
                        Debug.Log("wave over");
                        break;
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }

        Messenger.Broadcast<bool>("GameResult", _win);
        _waitingForRestart = true;

    }
}
