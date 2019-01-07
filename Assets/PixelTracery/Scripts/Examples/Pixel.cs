#region
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PixelTracery;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class Pixel : MonoBehaviour {
  public RawImage ScreenImage;
  private Texture2D texture;
  private Color32[] backColors;
  private Color32[] mainMask;

  private void Start () {
    ScreenImage.texture = new Texture2D (240, 136);
    texture = ScreenImage.texture as Texture2D;

    initMask ();
    drawBack ();
  }

  private void initMask () {
    Color32[] clearColors = null;
    texture.PixClear (ref clearColors, Color.black);
    texture.PixCircle (50, 50, 30, Color.white, true);
    texture.Apply ();

    mainMask = texture.GetPixels32 ();
  }

  private void drawBack () {
    // draw back
    var column_width = 240 / 16;
    for (var i = 0; i < 16; ++i) {
      texture.PixRectangle (i * column_width, 0, column_width, 136, Palettes.GetColor (i, Palettes.Palette.DB16), true);
    }
    texture.PixCircle (50, 50, 30, Color.white, true);
    texture.Apply ();

    // get back colors
    backColors = texture.GetPixels32 ();
    // draw back
    texture.PixClear (backColors);
  }

  void Update () {
    for (int i = 0; i < 1024; i++) {
      var color = Palettes.GetColor (Random.Range (8, 16), Palettes.Palette.DB16);
      texture.PixSetPixel (Random.Range (0, 240), Random.Range (0, 136), color, mask: mainMask, maskWidth: texture.width, maskHeight: texture.height);
    }

    texture.Apply ();
  }

}