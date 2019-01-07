using System.Collections;
using System.Collections.Generic;
using PixelTracery;
using UnityEngine;
using UnityEngine.UI;

public class Formula1 : MonoBehaviour {
  [Header("Use arrow keys for controls")]
  public RawImage ScreenImage;
  private Texture2D texture;
  private Color[] backColors;

  const int FRAMES = 31;
  const int REAL_FRAMES = 15;
  const int SHIFT_FRAMES = -8;
  const float CONTROLLABILITY = 0.3f;
  const float MAX_SPEED = 2;
  const float REVERSE_SPEED = 0.5f;
  const float FRICTION = -0.07f;

  public Sprite[] Sprites;

  float x = 120;
  float y = 70;
  float speed = 0;
  float frame = 0;
  int speedSign = 1;

  void Start () {
    ScreenImage.texture = new Texture2D (240, 136);
    texture = ScreenImage.texture as Texture2D;

    // draw back
    var clearColors = new Color32[texture.width * texture.height];
    texture.PixClear (ref clearColors, Color.grey);
    for (int i = 0; i < 1024; i++) {
      var color = Palettes.GetColor (Random.Range (8, 16), Palettes.Palette.DB16);
      texture.PixSetPixel (Random.Range (0, 240), Random.Range (0, 136), color);
    }

    // get back colors
    backColors = texture.GetPixels ();
  }

  void Update () {
    // control move
    if (Input.GetKey (KeyCode.UpArrow)) {
      speedSign = 1;
      speed = MAX_SPEED;
    }
    if (Input.GetKey (KeyCode.DownArrow)) {
      speedSign = -1;
      speed = REVERSE_SPEED;
    }

    // control direction 
    if (Input.GetKey (KeyCode.LeftArrow) && speed != 0) {
      frame -= CONTROLLABILITY;
      if (frame < 0) frame = FRAMES;
    }
    if (Input.GetKey (KeyCode.RightArrow) && speed != 0) {
      frame += CONTROLLABILITY;
      if (frame > FRAMES) frame = 0;
    }

    // friction
    speed += FRICTION;
    if (speed < 0) speed = 0;

    // movement
    var val = 2f * Mathf.PI * ((float) (frame + SHIFT_FRAMES) / FRAMES);
    x += (Mathf.Cos (val)) * speed * speedSign;
    y += (Mathf.Sin (val)) * speed * speedSign;

    // bounds 
    x = Mathf.Clamp (x, 0, 220);
    y = Mathf.Clamp (y, 0, 120);

    // draw
    var flip = 0;
    var flippedFrame = (int) frame;
    if (frame > REAL_FRAMES) {
      flip = 1;
      flippedFrame = FRAMES - (int) frame;
    }

    texture.SetPixels (backColors);
    texture.PixSprite (x, y, Sprites[flippedFrame], 1, flip, 0);
    texture.Apply ();
  }

}