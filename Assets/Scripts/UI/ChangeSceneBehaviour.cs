using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatMode
{
    static public bool On = false;
}

public class ChangeSceneBehaviour : MonoBehaviour
{
    public GameObject _cheatText;
    public string _nextScene;

    private AudioSource _audioSource;
    private KeyCode[] _cheatCode;
    private int _index = 0;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _cheatCode = new KeyCode[] {
            KeyCode.R,
            KeyCode.A,
            KeyCode.Y,
            KeyCode.R,
            KeyCode.I,
            KeyCode.V,
            KeyCode.E,
            KeyCode.R,
        };
        _index = 0;

        if (_cheatText)
        {
            DontDestroyOnLoad(_cheatText);
        }
    }

    void Update()
    {
        CheckCheat();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(_nextScene);
        }
    }

    void CheckCheat()
    {
        // cheat on!
        if (_index >= _cheatCode.Length - 1)
        {
            CheatMode.On = true;

            if (_cheatText != null)
            {
                _cheatText.SetActive(true);
            }

            return;
        }

        if (Input.GetKeyDown(_cheatCode[_index]))
        {
            ++_index;

            // play sfx;
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
        }
    }
}
