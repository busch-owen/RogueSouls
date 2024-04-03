using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairFade : MonoBehaviour
{
    [SerializeField]
    Vector3 _beginFadeOutBounds;

    [SerializeField]
    Transform _crosshairPosition;

    [SerializeField]
    SpriteRenderer _crosshairSprite;

    [SerializeField]
    float _fadeOutSpeed;

    public void CheckCrosshairPosition()
    {
        if (_crosshairPosition.localPosition == Vector3.zero)
        {
            FadeOutCrosshair();
        }
        else
        {
            FadeInCrosshair();
        }
    }

    public void FadeOutCrosshair()
    {
        var crosshairColor = _crosshairSprite.color;
        if (crosshairColor.a > 0)
        {
            crosshairColor.a = 0;
            _crosshairSprite.color = crosshairColor;
        }
    }

    public void FadeInCrosshair()
    {
        var crosshairColor = _crosshairSprite.color;
        if(crosshairColor.a < 1)
        {
            crosshairColor.a = 1;
            _crosshairSprite.color = crosshairColor;
        }
    }
}
