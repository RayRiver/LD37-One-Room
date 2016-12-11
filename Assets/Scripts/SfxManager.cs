using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public GameObject _audioSourcePrefab;
    public AudioClip[] _audios;

    private GameUtil.AudioParamDict _audioDict;

    void Awake()
    {
        _audioDict = new GameUtil.AudioParamDict(_audios);
    }

    void Start()
    {
        Messenger.AddListener<string, Vector3>("PlaySfx", OnPlaySfx);
    }

    void OnPlaySfx(string name, Vector3 pos)
    {
        AudioClip clip = null;
        var exists = _audioDict.TryGetValue(name, out clip);
        if (!exists) return;

        if (_audioSourcePrefab != null)
        {
            var go = Instantiate(_audioSourcePrefab, pos, Quaternion.identity);
            go.transform.parent = transform;

            var audioSource = go.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
                StartCoroutine(AudioPlayCallback(audioSource, clip.length));
            }
        }
    }

    IEnumerator AudioPlayCallback(AudioSource audioSource, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(audioSource.gameObject);
    }
}
