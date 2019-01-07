using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelTracery;

public class Ellipse : MonoBehaviour {

	[Range (0, 360)] public int Angle;

	[Space]
	[Range (0, 200)] public int X0;
	[Range (0, 200)] public int Y0;
	[Range (0, 200)] public int X1;
	[Range (0, 200)] public int Y1;

	public RawImage ScreenImage;
	private Texture2D texture;
	private Color32[] clearColors;

	void Start () {
		ScreenImage.texture = new Texture2D (240, 136);
		texture = ScreenImage.texture as Texture2D;

		X0 = 100;
		Y0 = 50;
		X1 = 200;
		Y1 = 100;
	}

	void Update () {
		texture.PixClear (ref clearColors, Color.black);

		texture.PixEllipseFilled (50, 50, 60, 30, Angle, Color.red);

		texture.PixEllipseRect (X0, Y0, X1, Y1, Color.red);

		texture.Apply ();
	}

}