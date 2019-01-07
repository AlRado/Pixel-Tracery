#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PixelTracery;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class AnimationExample : MonoBehaviour {

  public const float FRAME_SHOW_SECONDS = 0.12f;

  [Header("Use arrow keys for controls")]
  public Sprite[] AnimSprites;

  public RawImage ScreenImage;
  [HideInInspector] public Texture2D Texture;

  private Color32[] backColors;
  private Color32[] mainMask;

  private int spriteId;
  private int x;
  private int y;
  private int lastX;
  private int horizontalDir;
  private bool isMove;

  private void Start () {
    ScreenImage.texture = new Texture2D (240, 136);
    Texture = ScreenImage.texture as Texture2D;
    x = 50;
    y = 50;
    initMask ();
    drawBack ();

    AnimSprites = Resources.LoadAll<Sprite> ("Sprites/soldier");
    StartCoroutine (animationCoroutine ());
  }

  private void initMask () {
    Color32[] clearColors = null;
    Texture.PixClear (ref clearColors, Color.black);
    Texture.PixCircle (50, 50, 30, Color.white, true);
    Texture.PixCircle (75, 70, 15, Color.white, true);
    Texture.PixCircle (100, 70, 25, Color.white, true);
    Texture.Apply ();

    mainMask = Texture.GetPixels32 ();
  }

  private void drawBack () {
    // draw back
    var column_width = 240 / 16;
    for (var i = 0; i < 16; ++i) {
      Texture.PixRectangle (i * column_width, 0, column_width, 136, Palettes.GetColor (i, Palettes.Palette.DB16), true);
    }
    Texture.PixCircle (50, 50, 30, Color.white, true);
    Texture.PixCircle (75, 70, 15, Color.white, true);
    Texture.PixCircle (100, 70, 25, Color.white, true);
    Texture.Apply ();

    // get back colors
    backColors = Texture.GetPixels32 ();
  }

  void Update () {
    isMove = false;
    if (Input.GetKey (KeyCode.UpArrow)) { y--; isMove = true; }
    if (Input.GetKey (KeyCode.DownArrow)) { y++; isMove = true; }
    if (Input.GetKey (KeyCode.LeftArrow)) { x--; isMove = true; }
    if (Input.GetKey (KeyCode.RightArrow)) { x++; isMove = true; }

    Texture.PixClear (backColors);

    if (horizontalDir != x - lastX) horizontalDir = x - lastX >= 0 ? 1 : 0;
    // draw shadow
    Texture.PixSprite (x + 2, y, sprite : AnimSprites[spriteId], flip : horizontalDir, alphaColor : Color.white, tintColor : Color.gray, mask: mainMask, maskWidth: Texture.width, maskHeight: Texture.height);
    // draw man
    Texture.PixSprite (x, y, sprite : AnimSprites[spriteId], flip : horizontalDir, alphaColor : Color.white, mask: mainMask, maskWidth: Texture.width, maskHeight: Texture.height);
    Texture.Apply ();

    lastX = x;
  }

  private IEnumerator animationCoroutine () {
    yield return new WaitForSeconds (FRAME_SHOW_SECONDS);

    if (isMove) {
      if (++spriteId >= AnimSprites.Length) spriteId = 0;
    } else {
      spriteId = 0;
    }
    StartCoroutine (animationCoroutine ());
  }

}