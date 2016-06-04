using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [Header("Canvases")]
    public GameObject splashCanvas;
    public GameObject menuCanvas;
    public GameObject helpCanvas;
    [Header("Fade in settings")]
    public float delay = 0.05f;
    public float logoFullTime = 1.0f;
    Image logo;
    CanvasGroup menuCanvasGroup;

    void Start()
    {
        Debug.Assert(splashCanvas != null && menuCanvas != null && helpCanvas != null, "a canvas is missing");
        ShowCanvases(true, false, false);
        logo = splashCanvas.GetComponentInChildren<Image>();
        menuCanvasGroup = menuCanvas.GetComponent<CanvasGroup>();
        menuCanvasGroup.alpha = 0f;
        // Fade in/out the logo, show menu canvas
        StartCoroutine("Splash");
    }

    void ShowCanvases(bool splash, bool menu, bool help)
    {
        splashCanvas.SetActive(splash);
        menuCanvas.SetActive(menu);
        helpCanvas.SetActive(help);
    }

    public void GetHelp() { ShowCanvases(false, false, true); }

    public void LeaveHelp() { ShowCanvases(false, true, false); }

    IEnumerator Splash()
    {
        Color startColor = Color.white;
        Color endColor = startColor;
        startColor.a = 0;

        float increment = 0.01f;
        float percentage = 0;

        // Fade in
        while (logo.color != endColor)
        {
            logo.color = Color.Lerp(startColor, endColor, percentage);
            percentage += increment;
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(logoFullTime);

        // Fade out
        percentage = 0;
        increment = 0.02f;
        while (logo.color != startColor)
        {
            logo.color = Color.Lerp(endColor, startColor, percentage);
            percentage += increment;
            yield return new WaitForSeconds(delay);
        }

        ShowCanvases(false, true, false);

        percentage = 0;
        increment = 0.01f;
        while (menuCanvasGroup.alpha != 1.0f)
        {
            menuCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, percentage);
            percentage += increment;
            yield return new WaitForSeconds(delay);
        }
    }
}
