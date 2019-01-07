using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelTracery;

public class RectDemo : BaseDemo {

  private const float HALF_SCR_W = 240 / 2;
  private const float HALF_SCR_H = 136 / 2;
  private const float DEVIATION = 150;
  private const float RECT_COUNT = 70;
  private const float RECT_STEP = 4;

	void Update () {
		texture.PixClear (ref clearColors, Color.black);

    for (var i = 1; i < RECT_COUNT; i++) {
      var width = i * RECT_STEP;
      var height = width / 2;
      float x = Mathf.Sin (Time.time) * DEVIATION / i - width / 2;
      float y = Mathf.Cos (Time.time) * DEVIATION / i - height / 2;
      texture.PixRectangle (HALF_SCR_W + x, HALF_SCR_H + y, width, height, Color.green, false);
    }
		texture.Apply ();
	}

}