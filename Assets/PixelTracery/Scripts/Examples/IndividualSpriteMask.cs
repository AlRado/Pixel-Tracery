#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PixelTracery;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class IndividualSpriteMask : MonoBehaviour {
  public const float FRAME_SHOW_SECONDS = 0.12f;

  public RawImage ScreenImage;
  [HideInInspector] public Texture2D Texture;

  private Sprite eath2;
  private Color32[] eath2Mask;

  private int shiftX;

  private Color32[] clearColors;

  private int x;
  private int y;

  private int maskWidth;
  private int maskHeight;

  private void Start () {
    ScreenImage.texture = new Texture2D (240, 136);
    Texture = ScreenImage.texture as Texture2D;

    x = 100;
    y = 20;
    eath2 = Resources.Load<Sprite> ("Sprites/Earth2");

    initMask ();

    StartCoroutine (animationCoroutine ());
  }

  private void initMask () {
    // drawing mask: black color will hide pixels, white color will show pixels
    Texture.PixClear (ref clearColors, Color.black);
    Texture.PixCircle (30, 30, 30, Color.white, true);
    Texture.Apply ();
    // get individual mask for sprite
    maskWidth = eath2.PixGetWidth();
    maskHeight = eath2.PixGetHeight();
    eath2Mask = Texture.PixGetPixels (0, 0, maskWidth, maskHeight);
  }

  void Update () {
    if (Input.GetKey (KeyCode.UpArrow)) y--;
    if (Input.GetKey (KeyCode.DownArrow)) y++;
    if (Input.GetKey (KeyCode.LeftArrow)) x--;
    if (Input.GetKey (KeyCode.RightArrow)) x++;

    Texture.PixClear (ref clearColors, Color.grey);
    // Texture.Sprite (x, y, eath2, scale:3, circleShiftX : shiftX, mask : eath2Mask, maskWidth: eath2.GetWidth (), maskX : x, maskY : y);
    Texture.PixSprite (x, y, eath2, scale:1, circleShiftX : shiftX, mask : eath2Mask, maskWidth: this.maskWidth, maskHeight: this.maskHeight, maskX:x, maskY:y);
    // for test
    // Texture.Pixels (x, y+100, eath2Mask, maskWidth, maskHeight);
    Texture.Apply ();
  }

  private IEnumerator animationCoroutine () {
    yield return new WaitForSeconds (FRAME_SHOW_SECONDS);

    shiftX--;

    StartCoroutine (animationCoroutine ());
  }

}