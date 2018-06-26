using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour {

    public static UIController instance=null;
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DontDestroyOnLoad (gameObject);
	}

  

    public void Fade( RawImage img, float speedRate, int inOut)
	{
		// in = 1 - fade in
		// in= 0 fade out
		inOut=Mathf.Clamp (inOut, 0, 1);
		float _alphaValue = img.color.a;
		_alphaValue = Mathf.Lerp(_alphaValue, inOut, Time.deltaTime*speedRate);
		img.color = new Color(img.color.r, img.color.g, img.color.b, _alphaValue);
	}
    public void Fade(Image img, float speedRate, int inOut)
    {
        // in = 1 - fade in
        // in= 0 fade out
        inOut = Mathf.Clamp(inOut, 0, 1);
        float _alphaValue = img.color.a;
        _alphaValue = Mathf.Lerp(_alphaValue, inOut, Time.deltaTime * speedRate);
        img.color = new Color(img.color.r, img.color.g, img.color.b, _alphaValue);
    }
    public void Fade( Text text, float speedRate, int inOut)
	{
		// in = 1 - fade in
		// in= 0 fade out
		inOut=Mathf.Clamp (inOut, 0, 1);
		float _alphaValue = text.color.a;
		_alphaValue = Mathf.Lerp(_alphaValue, inOut, Time.deltaTime*speedRate);
		text.color = new Color(text.color.r, text.color.g, text.color.b, _alphaValue);
	}

	IEnumerator HitScreen(RawImage img)
	{
		int counter = 30;
		while (counter>0) {
			UIController.instance.Fade (img, 4, 1);
			yield return new WaitForEndOfFrame ();
			counter--;
		}
		yield return null;
		while (counter!=60) {
			UIController.instance.Fade (img, 5, 0);
			yield return new WaitForEndOfFrame ();
			counter++;
		}

	}

	public void SetTransparent(RawImage img)
	{
		img.color = new Color (img.color.r, img.color.g, img.color.b, 0);
	}


}
