using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Youdied : MonoBehaviour
{

    [SerializeField]
     private float fadeDuration = 2.0f;
    [SerializeField]
    private Image Image;
    private WaitForSeconds waitForSeconds;
    private PlayerStats _playerStats;

    CanvasGroup _canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    public void Died()
    {
        StartCoroutine(FadeImage());
        _canvasGroup.alpha = 0;
    }

    private IEnumerator FadeImage()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / fadeDuration)//FadeIn
        {
            _canvasGroup.alpha = i;
            yield return null; // Wait for the next frame
        }
        _canvasGroup.alpha = 1.0f;
        _playerStats.Respawn();
        yield return new WaitForSeconds(fadeDuration * 2);
        for (float i = 1; i >= 0; i -= Time.deltaTime / (fadeDuration / 2))//FadeOut
        {
            _canvasGroup.alpha = i;
            yield return null;
        }
        _canvasGroup.alpha = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
