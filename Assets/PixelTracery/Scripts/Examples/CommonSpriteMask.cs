#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PixelTracery;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class CommonSpriteMask : MonoBehaviour {
  public const float FRAME_SHOW_SECONDS = 0.12f;

  public RawImage ScreenImage;
  [HideInInspector] public Texture2D Texture;

  private Sprite eath1;
  private Sprite eath2;

  private int shiftX;

  private Color32[] commonMask;

  private void Start () {
    ScreenImage.texture = new Texture2D (240, 136);
    Texture = ScreenImage.texture as Texture2D;

    eath1 = Resources.Load<Sprite> ("Sprites/Earth1");
    eath2 = Resources.Load<Sprite> ("Sprites/Earth2");

    initCommonMask ();

    StartCoroutine (animationCoroutine ());
  }

  private void initCommonMask () {
    Color32[] clearColors = null;
    Texture.PixClear (ref clearColors, Color.black);
    Texture.PixCircle (90, 90, 78, Color.white, true);
    Texture.PixCircle (282-80, 52-6, 32, Color.white, true);
    Texture.Apply ();

    commonMask = Texture.GetPixels32 ();
  }

  void Update () {
    Texture.Apply ();
  }

  private IEnumerator animationCoroutine () {
    yield return new WaitForSeconds (FRAME_SHOW_SECONDS);

    Texture.PixSprite (9, 9, eath1, circleShiftX : shiftX, mask : commonMask, maskWidth : Texture.width, maskHeight : Texture.height);
    Texture.PixSprite (250-80, 15, eath2, circleShiftX : shiftX, mask : commonMask, maskWidth : Texture.width, maskHeight : Texture.height);
    shiftX--;

    StartCoroutine (animationCoroutine ());
  }

}