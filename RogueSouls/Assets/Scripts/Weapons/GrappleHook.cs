using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : RangedWeapon
{
    [SerializeField]
    Sprite _firedSprite, _notFiredSprite;

    SpriteRenderer _gunRenderer;

    public override void Awake()
    {
        base.Awake();
        _gunRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Shoot(Vector2 additionalVelocity = default)
    {
        base.Shoot(additionalVelocity);
        if (_uiHandler != null && !_uiHandler.IsPaused)
        {
            _gunRenderer.sprite = _firedSprite;
        }
    }

    public override void FinishReload()
    {
        base.FinishReload();
        _gunRenderer.sprite = _notFiredSprite;
        
    }
}
