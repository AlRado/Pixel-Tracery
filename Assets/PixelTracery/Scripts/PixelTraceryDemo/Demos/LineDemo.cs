using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelTracery;

public class LineDemo : BaseDemo {

	const float pi8 = Mathf.PI / 8;
	const float pi2 = Mathf.PI * 2;

	private float tt;

	void Awake () {
		tt = 0;
	}

	void Update () {
		texture.PixClear (ref clearColors, Color.black);

		//lines
		for (var i = tt % 8; i < 135; i += 8) {
			texture.PixLine (i, 0, 0, 135 - i, Color.green);
			texture.PixLine (i, 135, 135, 135 - i, Color.red);
			tt += 0.01f;
		}

		//prism
		for (var i = tt / 16 % pi8; i < pi2; i += pi8) {
			var x = 68 + 32 * Mathf.Cos (i);
			var y = 68 + 32 * Mathf.Cos (i);
			texture.PixLine (135, 0, x, y, Color.white);
			texture.PixLine (0, 135, x, y, Color.white);
		}

		// Border
		texture.PixLine (0, 0, 135, 0, Color.green);
		texture.PixLine (0, 0, 0, 135, Color.green);
		texture.PixLine (135, 0, 135, 135, Color.red);
		texture.PixLine (0, 135, 135, 135, Color.red);

		texture.Apply ();
	}

}