using System.Collections;
using System.Collections.Generic;
using PixelTracery;
using UnityEngine;
using UnityEngine.UI;

public class QuadBezier : MonoBehaviour {

  [Range (0, 200)] public int X0;
  [Range (0, 200)] public int Y0;
  [Range (0, 200)] public int X1;
  [Range (0, 200)] public int Y1;
  [Range (0, 200)] public int X2;
  [Range (0, 200)] public int Y2;

  public RawImage ScreenImage;
  private Texture2D texture;
  private Color32[] clearColors;

  void Start () {
    ScreenImage.texture = new Texture2D (240, 136);
    texture = ScreenImage.texture as Texture2D;

    X0 = 50;
    Y0 = 50;
    X1 = 30;
    Y1 = 50;
    X2 = 30;
    Y2 = 70;
  }

  void Update () {
    texture.PixClear (ref clearColors, Color.black);

    for (int i = 0; i < 9; i++) {
      var shiftX = 20 * i;
      texture.PixQuadBezier (X0+shiftX, Y0, X1+shiftX, Y1, X2+shiftX, Y2, Color.red);
    }

    texture.Apply ();
  }

}