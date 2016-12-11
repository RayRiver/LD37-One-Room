using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleUnit))]
public class AttackBehaviour : MonoBehaviour
{
    public Transform[] _gunPoints;
    public GameObject _bulletPrefab;

    private BattleUnit _unit;

    void Awake()
    {
        _unit = GetComponent<BattleUnit>();
    }

    public void Attack()
    {
        foreach (var _gunPoint in _gunPoints)
        {
            var bullet = Instantiate(_bulletPrefab, _gunPoint.position, _gunPoint.rotation) as GameObject;

            if (tag == "Player")
            {
                float multiple = 1 + GameManager.Instance.GetSkillEffect("power");
                var scale = bullet.transform.localScale;
                bullet.transform.localScale = new Vector3(scale.x * multiple, scale.y * multiple, scale.z);
            }

            var com = bullet.GetComponent<BulletBehaviour>();
            if (com != null)
            {
                com.OwnerTag = _unit.tag;
                com.OwnerAtk = _unit.Atk;
                com.Direction = Vector2.up;
            }

            string audioName = "";
            if (tag == "Player")
            {
                audioName = "Laser";
            }
            else
            {
                audioName = "Laser2";
            }
            Messenger.Broadcast<string, Vector3>("PlaySfx", audioName, _gunPoint.position);
        }
    }
}
