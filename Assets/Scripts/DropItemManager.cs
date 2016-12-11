using UnityEngine;
using System.Collections;

public class DropItemManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _prefabs;

    private GameUtil.PrefabParamDict _prefabDict;

    void Awake()
    {
        _prefabDict = new GameUtil.PrefabParamDict(_prefabs);

        Messenger.AddListener<string, Vector3>("DropItem", OnDropItem);
    }

    public void OnDropItem(string name, Vector3 position)
    {
        Debug.Log("drop item: " + name);
        if (_prefabDict.ContainsKey(name))
        {
            var prefab = _prefabDict[name];
            var effect = Instantiate(prefab) as GameObject;
            effect.transform.parent = transform;
            effect.transform.position = new Vector3(position.x, position.y, effect.transform.position.z);
        }
    }

}
