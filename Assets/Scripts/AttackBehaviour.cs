using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleUnit))]
public class AttackBehaviour : MonoBehaviour
{
    public Transform _gunPoint;
    public GameObject _bulletPrefab;

    private BattleUnit _unit;

    void Awake()
    {
        _unit = GetComponent<BattleUnit>();
    }

    public void Attack()
    {
        var bullet = Instantiate(_bulletPrefab, _gunPoint.position, _gunPoint.rotation) as GameObject;

        var com = bullet.GetComponent<BulletBehaviour>();
        if (com != null)
        {
            com.Owner = _unit;
            com.Direction = Vector2.up;
        }
    }
}
