#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PixelTracery;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class Sprites : MonoBehaviour {
  public Sprite Sprite;
  public RawImage ScreenImage;
  private Texture2D texture;
  private Color32[] backColors;
  private Color32[] mainMask;

  [Range (1, 6)] public int Scale = 1;
  [Range (0, 3)] public int Flip;
  [Range (0, 3)] public int Rotate;

  private int x;
  private int y;
  private Color alphaColor;

  private Color32[] heroColors;
  private int heroWidth;
  private int heroHeight;

  private void Start () {
    ScreenImage.texture = new Texture2D (240, 136);
    texture = ScreenImage.texture as Texture2D;
    x = 40;
    y = 40;
    alphaColor = Sprite.PixGetColors () [0];
    heroColors = Sprite.PixGetColors ();
    heroWidth = Sprite.PixGetWidth ();
    heroHeight = Sprite.PixGetHeight ();

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
  }

  void Update () {
    if (Input.GetKey (KeyCode.UpArrow)) y--;
    if (Input.GetKey (KeyCode.DownArrow)) y++;
    if (Input.GetKey (KeyCode.LeftArrow)) x--;
    if (Input.GetKey (KeyCode.RightArrow)) x++;

    texture.PixClear (backColors);
    texture.PixSprite (x, y, Sprite, Scale, Flip, Rotate, alphaColor : alphaColor, mask: mainMask, maskWidth: texture.width, maskHeight: texture.height);
    texture.PixSetPixels (heroColors, x + 25, y, heroWidth, heroHeight, Scale, Flip, Rotate, alphaColor : alphaColor, mask : mainMask, maskWidth: texture.width, maskHeight: texture.height);
    texture.Apply ();
  }

}