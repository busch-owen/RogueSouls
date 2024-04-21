using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject _preFinalBossAssets;
    [SerializeField] private GameObject _postFinalBossAssets;
    [SerializeField] private GameObject _forestBlockageAssets;
    
    private void Awake()
    {
        Time.timeScale = 1.0f;
        _postFinalBossAssets.SetActive(false);
        _forestBlockageAssets.SetActive(true);
    }
    
    public void CheckForHealthSouls(int amountOfSouls)
    {
        if (amountOfSouls >= 1)
        {
            _forestBlockageAssets.SetActive(false);
        }
        
        if (amountOfSouls >= 3)
        {
            if(!_preFinalBossAssets && !_postFinalBossAssets) return;
            
            _preFinalBossAssets.SetActive(false);
            _postFinalBossAssets.SetActive(true);
        }
        else
        {
            if(!_preFinalBossAssets && !_postFinalBossAssets) return;
            
            _preFinalBossAssets.SetActive(true);
            _postFinalBossAssets.SetActive(false);
        }
    }
    
    #region Restart
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}
