using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
#region Global Variables
    [SerializeField]
    protected int _playerLevel;
    protected float _playerLevelProgression;
    protected float _amountUntilNextLevel;
    [SerializeField]
    protected float _levelProgressionMultiplier;
    [SerializeField]
    protected float _xpValue;
    [SerializeField]
    GameManager _gameManager;
    [SerializeField]
    private Youdied youDied;
    [SerializeField]
    private SpriteRenderer PlayerSprite;
    private SpriteRenderer WeaponSprite;
    #endregion
    #region Level
    public void IncrementPlayerLevel(int incrementAmount)
    {
        _playerLevelProgression += incrementAmount;

        if (_playerLevelProgression >= _amountUntilNextLevel)
        {
            _playerLevel++;
            _playerLevelProgression -= _amountUntilNextLevel;

            _amountUntilNextLevel *= _levelProgressionMultiplier;
        }
    }
    #endregion
#region Damage
    public override void TakeDamage(int damage)
    {
        IncrementHealth(damage);
        if (_health <= 0)
        {
            // Get the current color of the player's sprite
            Color spriteColor = PlayerSprite.color;

            // Set the alpha to 0
            spriteColor.a = 0;

            // Apply the new color to the player's sprite
            PlayerSprite.color = spriteColor;


            /*Color WeaponspriteColor = WeaponSprite.color; // re add when figure out how to assign weapon sprite renderer in game.


            WeaponspriteColor.a = 0;


            WeaponSprite.color = WeaponspriteColor;*/


            youDied.StartCoroutine(youDied.FadeImage());
        }
  
    }
}
#endregion
