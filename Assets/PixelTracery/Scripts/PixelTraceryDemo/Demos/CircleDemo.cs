using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelTracery;

public class CircleDemo : BaseDemo {

	private float a;
	private int space = 10;

	void Update () {
		texture.PixClear (ref clearColors, Color.black);

		for (var i = 0; i < 200; i += space) {
			texture.PixCircle (120 + 80 * Mathf.Sin (a), 68 + 40 * Mathf.Cos (a), i + Time.time / 40 % space, Color.green, false);
			texture.PixCircle (120 + 80 * Mathf.Sin (a / 2), 68 + 40 * Mathf.Cos (a / 2), i + Time.time / 40 % space, Color.green, false);
		}
		a += Mathf.PI / 240;

		texture.Apply ();
	}

}