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
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 0);
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void Died()
    {
        StartCoroutine(FadeImage());
    }

    public IEnumerator FadeImage()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / fadeDuration)
        {
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, i);
            yield return null; // Wait for the next frame
        }
        _gameManager.Restart();
        for (float i = 1; i >= 0; i -= Time.deltaTime / fadeDuration)
        {
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, i);
            yield return null;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
