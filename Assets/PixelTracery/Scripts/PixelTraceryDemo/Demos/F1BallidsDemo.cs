using System.Collections;
using System.Collections.Generic;
using PixelTracery;
using UnityEngine;
using UnityEngine.UI;

public class F1BallidsDemo : BaseDemo {

  const int FRAMES = 31;
  const int REAL_FRAMES = 15;

  private Sprite[] sprites;

  float x = 80;
  float y = 70;
  float speed = 2;
  float frame = 0;

  void Start () {
    sprites = Resources.LoadAll<Sprite>("Sprites/CarFormula1");
  }

  void Update () {
    // direction 
    frame += 0.3f;
    frame %= FRAMES;

    // movement
    var val = 2f * Mathf.PI * ((float) (frame + REAL_FRAMES) / FRAMES);
    x = (Mathf.Cos (val)) * speed * 20;
    y = (Mathf.Sin (val)) * speed * 20;

    // draw
    var flip = 0;
    var flippedFrame = (int) frame;
    if (frame > REAL_FRAMES) {
      flip = 1;
      flippedFrame = FRAMES - (int) frame;
    }
    var carSprite = sprites[flippedFrame];
    texture.PixClear (ref clearColors, Color.grey);
    texture.PixSprite (110 + x, 60 + y, carSprite, 1, flip, 0);
    texture.PixSprite (100, 50, carSprite, 2, flip, 0);

    texture.Apply ();
  }

}