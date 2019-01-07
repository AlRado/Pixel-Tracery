using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PixelTracery;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DrawWithMaskTest : MonoBehaviour {

  public const int FULL_SCREEN_WIDTH = 240;
  public const int FULL_SCREEN_HEIGHT = 136;
  public const int SCREEN_WIDTH = 100;
  public const int SCREEN_HEIGHT = 100;
  public const int BORDER_WIDTH = 32;
  public const int BORDER_HEIGHT = 24;

  public RawImage ScreenImage;
  private Texture2D texture;
  private Color32[] clearColors;

  private Color32[] borderMask;

  void Start () {
    ScreenImage.texture = new Texture2D (FULL_SCREEN_WIDTH, FULL_SCREEN_HEIGHT);
    ScreenImage.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, FULL_SCREEN_WIDTH);
    ScreenImage.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, FULL_SCREEN_HEIGHT);
    texture = ScreenImage.texture as Texture2D;

    initBorderMask ();
  }

  private void initBorderMask () {
    // draw mask
    texture.PixClear (ref clearColors, Color.white);
    texture.PixRectangle (BORDER_WIDTH, BORDER_HEIGHT, SCREEN_WIDTH, SCREEN_HEIGHT, Color.black, true);
    texture.Apply ();
    // get mask
    borderMask = texture.PixGetPixels (0, 0, FULL_SCREEN_WIDTH, FULL_SCREEN_HEIGHT);
  }

  private Color32 getColor (int colorIx) {
    return Palettes.GetColor (colorIx, Palettes.Palette.ZX_Spectrum_Orthodox);
  }

  private void border (int colorIx) {
    texture.PixRectangle (0, 0, FULL_SCREEN_WIDTH, FULL_SCREEN_HEIGHT, getColor (colorIx), true, borderMask, FULL_SCREEN_WIDTH, FULL_SCREEN_HEIGHT);

    var color = Palettes.GetColor (UnityEngine.Random.Range (8, 16), Palettes.Palette.ZX_Spectrum_Orthodox);
    var y = UnityEngine.Random.Range (0, 192);
    texture.PixLine (0, y, FULL_SCREEN_WIDTH, y, color, borderMask, FULL_SCREEN_WIDTH, FULL_SCREEN_HEIGHT);

    var x = UnityEngine.Random.Range (0, 256);
    var r = UnityEngine.Random.Range (0, 50);
    texture.PixCircle (x, y, r, color, true, borderMask, FULL_SCREEN_WIDTH, FULL_SCREEN_HEIGHT);

    x += 30;
    y += 30;
    texture.PixEllipseFilled (x, y, 50, 25, 45, color, borderMask, FULL_SCREEN_WIDTH, FULL_SCREEN_HEIGHT);

    x += 30;
    y += 30;
    texture.PixEllipseRect (x, y, x + 50, y + 25, color, borderMask, FULL_SCREEN_WIDTH, FULL_SCREEN_HEIGHT);

    var X0 = 50;
    var Y0 = 0;
    var X1 = 30;
    var Y1 = 0;
    var X2 = 30;
    var Y2 = 20;
    texture.PixQuadBezier (x + X0, y + Y0, x + X1, y + Y1, x + X2, y + Y2, color, borderMask, FULL_SCREEN_WIDTH, FULL_SCREEN_HEIGHT);

    x += 30;
    y += 30;

    texture.PixTriangle (x + X0, y + Y0, x + X1, y + Y1, x + X2, y + Y2, color, true, borderMask, FULL_SCREEN_WIDTH, FULL_SCREEN_HEIGHT);
    // texture.PixTriangle (x + X0, y + Y0, x + X1, y + Y1, x + X2, y + Y2, color, true);
  }

  private void Update () {
    border (Random.Range (0, 8));
    texture.Apply ();

  }

}