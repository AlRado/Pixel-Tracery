using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelTracery;

public class BaseDemo : MonoBehaviour {
  [HideInInspector]
	public RawImage ScreenImage;
	protected Texture2D texture;
	protected Color32[] clearColors;
  
	public void Init (Texture2D texture) {
		this.texture = texture;
	}

}