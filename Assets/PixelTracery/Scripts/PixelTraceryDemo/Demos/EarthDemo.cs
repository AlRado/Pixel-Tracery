#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PixelTracery;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class EarthDemo : BaseDemo {
  public const float FRAME_SHOW_SECONDS = 0.12f;

  private Sprite eath2;
  private Color32[] eath2Mask;

  private int shiftX;

  private int x;
  private int y;

  private int maskWidth;
  private int maskHeight;

  private void Start () {
    x = 90;
    y = 35;
    eath2 = Resources.Load<Sprite> ("Sprites/Earth2");

    initMask ();

    StartCoroutine (animationCoroutine ());
  }

  private void initMask () {
    // drawing mask: black color will hide pixels, white color will show pixels
    texture.PixClear (ref clearColors, Color.black);
    texture.PixCircle (30, 30, 30, Color.white, true);
    texture.Apply ();
    // get individual mask for sprite
    maskWidth = eath2.PixGetWidth();
    maskHeight = eath2.PixGetHeight();
    eath2Mask = texture.PixGetPixels (0, 0, maskWidth, maskHeight);
  }

  void Update () {
    if (Input.GetKey (KeyCode.UpArrow)) y--;
    if (Input.GetKey (KeyCode.DownArrow)) y++;
    if (Input.GetKey (KeyCode.LeftArrow)) x--;
    if (Input.GetKey (KeyCode.RightArrow)) x++;

    texture.PixClear (ref clearColors, Color.black);
    texture.PixSprite (x, y, eath2, scale:1, circleShiftX : shiftX, mask : eath2Mask, maskWidth: this.maskWidth, maskHeight: this.maskHeight, maskX:x, maskY:y);
    texture.Apply ();
  }

  private IEnumerator animationCoroutine () {
    yield return new WaitForSeconds (FRAME_SHOW_SECONDS);

    shiftX--;

    StartCoroutine (animationCoroutine ());
  }

}