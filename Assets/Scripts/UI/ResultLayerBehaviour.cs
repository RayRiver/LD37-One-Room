using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultLayerBehaviour : MonoBehaviour
{
    public GameObject _content;
    public GameObject _winText;
    public GameObject _loseText;

    void Start()
    {
        Messenger.AddListener<bool>("GameResult", OnGameResult);
    }

    void OnGameResult(bool win)
    {
        if (_content != null)
        {
            _content.SetActive(true);
        }

        if (win)
        {
            _winText.SetActive(true);
            _loseText.SetActive(false);
        }
        else
        {
            _winText.SetActive(false);
            _loseText.SetActive(true);
        }
    }
}
