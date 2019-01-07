#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PixelTracery;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class Print : MonoBehaviour {
  [Header("Use arrow keys for controls")]
  public Sprite[] FontSprites;
  private Dictionary<char, Sprite> FontDict;

  public RawImage ScreenImage;
  private Texture2D texture;
  private Color32[] backColors;

  private Color32[] mainMask;

  public string Text;

  [Range (1, 6)] public int Scale = 1;
  [Range (0, 3)] public int Flip;
  [Range (0, 3)] public int Rotate;
  [Range (0, 100)] public int CircleShiftX;
  [Range (0, 100)] public int CircleShiftY;

  private int x;
  private int y;
  private int textWidth;
  private int textHeight;

  private void Start () {
    ScreenImage.texture = new Texture2D (240, 136);
    texture = ScreenImage.texture as Texture2D;
    FontDict = FontSprites.PixToFontDictionary ();
    x = 50;
    y = 50;
    textWidth = texture.PixGetTextWidth (Text, FontDict);
    textHeight = texture.PixGetTextHeight (FontDict);
    initMask ();
    drawBack ();
  }

  private void initMask () {
    Color32[] clearColors = null;
    texture.PixClear (ref clearColors, Color.black);
    texture.PixCircle (50, 50, 30, Color.white, true);
    texture.PixCircle (75, 70, 15, Color.white, true);
    texture.PixCircle (100, 70, 25, Color.white, true);
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
    texture.PixCircle (75, 70, 15, Color.white, true);
    texture.PixCircle (100, 70, 25, Color.white, true);
    texture.Apply ();

    // get back colors
    backColors = texture.GetPixels32 ();
  }

  void Update () {
    if (Input.GetKey (KeyCode.UpArrow)) y--;
    if (Input.GetKey (KeyCode.DownArrow)) y++;
    if (Input.GetKey (KeyCode.LeftArrow)) x--;
    if (Input.GetKey (KeyCode.RightArrow)) x++;

    Color32[] clearColors = null;
    texture.PixClear (ref clearColors, Color.black);

    texture.SetPixels32 (backColors);

    var scaledTextWidth = textWidth * Scale;
    var scaledTextHeight = textHeight * Scale;
    texture.PixLine (x, 0, x, 136, Color.green);
    texture.PixLine (0, y, 240, y, Color.green);
    texture.PixLine (x + scaledTextWidth, 0, x + scaledTextWidth, 136, Color.green);
    texture.PixLine (0, y + scaledTextHeight, 240, y + scaledTextHeight, Color.green);

    texture.PixPrintText (Text, FontDict, x, y, tintColor : Color.red, scale : Scale, flip : Flip, rotate : Rotate,
      circleShiftX : CircleShiftX, circleShiftY : CircleShiftY, mask: mainMask, maskWidth: texture.width, maskHeight: texture.height);

    texture.Apply ();
  }

}