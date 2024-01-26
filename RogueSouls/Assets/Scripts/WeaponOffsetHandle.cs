using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponOffsetHandle : MonoBehaviour
{
    [SerializeField]
    float _weaponOffset;

    [SerializeField]
    GameObject _weapon;

    public void OffsetWeaponPos(float rotationAngle)
    {
        _weapon.transform.localPosition = new Vector2(this.transform.localPosition.x + _weaponOffset, this.transform.localPosition.y);

        if (rotationAngle is < 90 and > -180 && rotationAngle is not < -90 and > -180)
        {
            _weapon.transform.localScale = new Vector3(_weapon.transform.localScale.x, 1, _weapon.transform.localScale.z);
        }
        else
        {
            _weapon.transform.localScale = new Vector3(_weapon.transform.localScale.x, -1, _weapon.transform.localScale.z);
        }
    }
}
