using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _prefabs;

    private GameUtil.PrefabParamDict _prefabDict;

    private static EffectManager s_instance = null;
    public static EffectManager Instance
    {
        get
        {
            return s_instance;
        }
    }

    void Awake()
    {
        s_instance = this;
        _prefabDict = new GameUtil.PrefabParamDict(_prefabs);

        Messenger.AddListener<string, Vector3>("PlayEffect", OnPlayEffect);
    }

    public void OnPlayEffect(string name, Vector3 position)
    {
        if (_prefabDict.ContainsKey(name))
        {
            var prefab = _prefabDict[name];
            var effect = Instantiate(prefab) as GameObject;
            effect.transform.parent = transform;
            effect.transform.position = new Vector3(position.x, position.y, effect.transform.position.z);

            var animator = effect.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Effect");
                var clips = animator.GetCurrentAnimatorClipInfo(0);
                StartCoroutine(AnimationCallback(animator, clips[0].clip.length));
            }
        }
    }

    IEnumerator AnimationCallback(Animator animator, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(animator.gameObject);
    }
}
