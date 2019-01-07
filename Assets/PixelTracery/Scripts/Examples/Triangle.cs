using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelTracery;

public class Triangle : MonoBehaviour {

	public RawImage ScreenImage;
	private Texture2D texture;
	private Color32[] clearColors;

	void Start () {
		ScreenImage.texture = new Texture2D (240, 136);
		texture = ScreenImage.texture as Texture2D;
	}

	void Update () {
		texture.PixClear (ref clearColors, Color.black);

    for (var x = 0; x < 240; x += 28) {
      for (var y = 0; y < 136; y += 28) {
        var cx = 12 * Mathf.Sin (Time.time / 30 * (x + y + 1));
        var cy = 12 * Mathf.Cos (Time.time / 30 * (x + y + 1));
        Pir (x, y, 25, 25, x + cx, y + cy);
      }
    }
		texture.Apply();
	}

	private void Pir (float x, float y, float w, float h, float cx, float cy) {
		texture.PixTriangle (x, y, w / 2 + cx, h / 2 + cy, x + w, y, Color.gray, true);
		texture.PixTriangle (x + w, y, w / 2 + cx, h / 2 + cy, x + w, y + h, Color.green, true);
		texture.PixTriangle (x, y, w / 2 + cx, h / 2 + cy, x, y + h, Color.blue, true);
		texture.PixTriangle (x, y + h, w / 2 + cx, h / 2 + cy, x + w, y + h, Color.white, true);
	}
}