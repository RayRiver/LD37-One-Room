using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveTextBehaviour : MonoBehaviour
{
    private Text _text;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void Start()
    {
        Messenger.AddListener<int>("WaveStart", OnWaveStart);
        Messenger.AddListener<bool>("GameResult", OnGameResult);
    }

    void OnWaveStart(int wave)
    {
        if (_text != null)
        {
            _text.text = "Wave " + wave.ToString();
        }
    }

    void OnGameResult(bool win)
    {
        if (_text != null)
        {
            _text.text = "";
        }
    }
}
