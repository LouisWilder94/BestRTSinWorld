using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour {

    public Image image;
    public float fadeInRate;

    public AnimationCurve fadeCurve;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * fadeInRate;
            float a = fadeCurve.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }
    }

    IEnumerator FadeOut(string _scene)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeInRate;
            float a = fadeCurve.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }

        SceneManager.LoadScene(_scene);
    }
}
