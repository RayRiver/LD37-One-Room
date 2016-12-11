using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUtil
{
    public class PrefabParamDict : Dictionary<string, GameObject>
    {
        public PrefabParamDict(GameObject[] prefabs) : base()
        {
            if (prefabs != null)
            {
                foreach (var prefab in prefabs)
                {
                    this[prefab.name] = prefab;
                }
            }
        }
    }

    public class AudioParamDict : Dictionary<string, AudioClip>
    {
        public AudioParamDict(AudioClip[] audios) : base()
        {
            if (audios != null)
            {
                foreach (var audio in audios)
                {
                    this[audio.name] = audio;
                }
            }
        }
    }

    public class PrefabParamNameList : List<string>
    {
        public PrefabParamNameList(GameObject[] prefabs) : base()
        {
            if (prefabs != null)
            {
                foreach (var prefab in prefabs)
                {
                    this.Add(prefab.name);
                }
            }
        }
    }

}