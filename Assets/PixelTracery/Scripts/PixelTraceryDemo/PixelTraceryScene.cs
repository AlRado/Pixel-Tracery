using System.Collections;
using System.Collections.Generic;
using PixelTracery;
using UnityEngine;

public class PixelTraceryScene : MonoBehaviour {

  public RandomDemoLauncher HandheldGameConsoleDemo;
  public RandomDemoLauncher MiniArcadeMachineDemo;

  public const float SHOW_DELAY = 0.2f;

  public const string ZX_FONT_PATH = "SpriteFonts/ZxSpectrumFont";
  public const string UBBLE_FONT_PATH = "SpriteFonts/Ubble";
  public const string MARKSMAN_FONT_PATH = "SpriteFonts/marksman-v1";

  private const string TITLE = "Pixel Tracery v.0.3";
  private const string DESC_1 = "* dynamic inscriptions on the walls";
  private const string DESC_2 = "* drawings on the 3d-objects";
  private const string DESC_3 = "* old-school 2d-games inside your unity 3d-game!";
  private const string DESC_4 = "Simple, fast, reliable.";

  private const string ERASER_TEXT = "Best eraser";
  private const string PENCIL_TEXT_1 = "Pencil for creativity";
  private const string PENCIL_TEXT_2 = "Magic pencil";
  private const string BEST_TEXT = "BEST";
  private const string RETRO_TEXT = "RETRO";
  private const string MACHINE_TEXT = "MACHINE";
  private const string ARCADE_TEXT = "ARCADE";
  private const string FOREVER_TEXT = "FOREVER";

  public Material EraserMaterial;
  public Material PaperMaterial;
  public Material Pencil1Material;
  public Material Pencil2Material;
  public Material MiniArcadeMachineBodyMaterial;

  private Texture2D defaultEraserTexture;
  private Texture2D defaultPaperTexture;
  private Texture2D defaultPencil1Texture;
  private Texture2D defaultPencil2Texture;
  private Texture2D defaultArcadeMachineBodyTexture;

  private Texture2D eraserTexture;
  private Texture2D paperTexture;
  private Texture2D pencil1Texture;
  private Texture2D pencil2Texture;
  private Texture2D arcadeMachineBodyTexture;

  private Dictionary<char, Sprite> zxFont;
  private Color32 zxFontAlpha;
  private int zxFontHeight;

  private Dictionary<char, Sprite> ubbleFont;
  private Color32 ubbleFontAlpha;

  private Dictionary<char, Sprite> marksmanFont;
  private Color32 marksmanFontAlpha;

  void Start () {
    var zxFontSprites = Resources.LoadAll<Sprite> (ZX_FONT_PATH);
    zxFont = zxFontSprites.PixToFontDictionary ();
    zxFontAlpha = zxFontSprites[0].PixGetColors () [0];
    zxFontHeight = zxFont['A'].PixGetHeight ();

    var ubbleFontSprites = Resources.LoadAll<Sprite> (UBBLE_FONT_PATH);
    ubbleFont = ubbleFontSprites.PixToFontDictionary ();
    ubbleFontAlpha = ubbleFontSprites[0].PixGetColors () [0];

    var marksmanFontSprites = Resources.LoadAll<Sprite> (MARKSMAN_FONT_PATH);
    marksmanFont = marksmanFontSprites.PixToFontDictionary ();
    marksmanFontAlpha = marksmanFontSprites[0].PixGetColors () [0];

    HandheldGameConsoleDemo.enabled = false;
    MiniArcadeMachineDemo.enabled = false;

    defaultEraserTexture = (Texture2D) EraserMaterial.mainTexture;
    defaultArcadeMachineBodyTexture = (Texture2D) MiniArcadeMachineBodyMaterial.mainTexture;
    defaultPencil1Texture = (Texture2D) Pencil1Material.mainTexture;
    defaultPencil2Texture = (Texture2D) Pencil2Material.mainTexture;
    defaultPaperTexture = (Texture2D) PaperMaterial.mainTexture;

    StartCoroutine (printCoroutine());
  }

  private IEnumerator printCoroutine () {
    // printOnEraser
    var step = 5;
    var shift = 20;
    for (int i = -shift*step; i < 0; i+=step) {
        printOnEraser(0, i);
        yield return new WaitForSeconds(SHOW_DELAY/step);
    }
    yield return new WaitForSeconds(SHOW_DELAY);

    // printOnArcadeMachine
    for (int i = -shift*step; i < 0; i+=step) {
        printOnArcadeMachine(0, i);
        yield return new WaitForSeconds(SHOW_DELAY/step);
    }
    yield return new WaitForSeconds(SHOW_DELAY);

    // printOnFirstPencil  
    for (int i = -shift*step; i < 0; i+=step) {
        printOnFirstPencil(0, i);
        yield return new WaitForSeconds(SHOW_DELAY/step);
    }
    yield return new WaitForSeconds(SHOW_DELAY);

    // printOnSecondPencil
    for (int i = -shift*step; i < 0; i+=step) {
        printOnSecondPencil(0, i);
        yield return new WaitForSeconds(SHOW_DELAY/step);
    }

    // printOnPaper
    for (int i = -shift*step; i < 0; i+=step) {
        printOnPaper(0, i);
        yield return new WaitForSeconds(SHOW_DELAY/step);
    }

    // start HandheldGameConsoleDemo
    HandheldGameConsoleDemo.enabled = true;
    yield return new WaitForSeconds(SHOW_DELAY);

    // start MiniArcadeMachineDemo
    MiniArcadeMachineDemo.enabled = true;
  }

  private void printOnEraser(int shiftX, int shiftY) {
    if (eraserTexture == null) {
      eraserTexture = cloneTexture2D (defaultEraserTexture);
    } else {
      eraserTexture.SetPixels (defaultEraserTexture.GetPixels ());
    }

    EraserMaterial.mainTexture = eraserTexture;
    var eraserTextWidth = eraserTexture.PixGetTextWidth (ERASER_TEXT, zxFont, 2);
    var eraserTextHeight = zxFontHeight * 2;
    eraserTexture.PixRectangle (124 + shiftX, 78 + shiftY, eraserTextHeight + 4, eraserTextWidth + 4, Color.white, true);
    eraserTexture.PixPrintText (ERASER_TEXT, zxFont, 126 + shiftX, 80 + shiftY, alphaColor : zxFontAlpha, tintColor : Color.grey, scale : 2, rotate : 1);
    eraserTexture.Apply ();
  }

  private void printOnArcadeMachine(int shiftX, int shiftY) {
    if (arcadeMachineBodyTexture == null) {
      arcadeMachineBodyTexture = cloneTexture2D (defaultArcadeMachineBodyTexture);
    } else {
      arcadeMachineBodyTexture.SetPixels (defaultArcadeMachineBodyTexture.GetPixels ());
    }

    MiniArcadeMachineBodyMaterial.mainTexture = arcadeMachineBodyTexture;
    arcadeMachineBodyTexture.PixPrintText (BEST_TEXT, ubbleFont, 70 + shiftX, 770 + shiftY, alphaColor : ubbleFontAlpha, tintColor : Color.black, scale : 4, rotate : 0);
    arcadeMachineBodyTexture.PixPrintText (BEST_TEXT, ubbleFont, 70 + 5 + shiftX, 770 - 5 + shiftY, alphaColor : ubbleFontAlpha, tintColor : Color.yellow, scale : 4, rotate : 0);
    arcadeMachineBodyTexture.PixPrintText (RETRO_TEXT, ubbleFont, 330 + shiftX, 750 + shiftY, alphaColor : ubbleFontAlpha, tintColor : Color.black, scale : 4, rotate : 0);
    arcadeMachineBodyTexture.PixPrintText (RETRO_TEXT, ubbleFont, 330 + 5 + shiftX, 750 - 5 + shiftY, alphaColor : ubbleFontAlpha, tintColor : Color.yellow, scale : 4, rotate : 0);
    arcadeMachineBodyTexture.PixPrintText (MACHINE_TEXT, ubbleFont, 305 + shiftX, 830 + shiftY, alphaColor : ubbleFontAlpha, tintColor : Color.black, scale : 4, rotate : 0);
    arcadeMachineBodyTexture.PixPrintText (MACHINE_TEXT, ubbleFont, 305 + 5 + shiftX, 830 - 5 + shiftY, alphaColor : ubbleFontAlpha, tintColor : Color.red, scale : 4, rotate : 0);
    var arcadeTextWidth = arcadeMachineBodyTexture.PixGetTextWidth (ARCADE_TEXT, ubbleFont, 4);
    arcadeMachineBodyTexture.PixPrintText (ARCADE_TEXT, ubbleFont, 750 + 5 + shiftX, 60 + arcadeTextWidth - 5 + shiftY, alphaColor : ubbleFontAlpha, tintColor : Color.black, scale : 4, rotate : 3);
    arcadeMachineBodyTexture.PixPrintText (ARCADE_TEXT, ubbleFont, 750 + shiftX, 60 + arcadeTextWidth + shiftY, alphaColor : ubbleFontAlpha, tintColor : Color.yellow, scale : 4, rotate : 3);
    var foreverTextWidth = arcadeMachineBodyTexture.PixGetTextWidth (FOREVER_TEXT, ubbleFont, 4);
    arcadeMachineBodyTexture.PixPrintText (FOREVER_TEXT, ubbleFont, 920 - 5 + shiftX, 50 + foreverTextWidth - 5 + shiftY, alphaColor : ubbleFontAlpha, tintColor : Color.black, scale : 4, rotate : 3);
    arcadeMachineBodyTexture.PixPrintText (FOREVER_TEXT, ubbleFont, 920 + shiftX, 50 + foreverTextWidth + shiftY, alphaColor : ubbleFontAlpha, tintColor : Color.yellow, scale : 4, rotate : 3);
    arcadeMachineBodyTexture.Apply ();
  }

  private void printOnFirstPencil(int shiftX, int shiftY) {
    if (pencil1Texture == null) {
      pencil1Texture = cloneTexture2D (defaultPencil1Texture);
    } else {
      pencil1Texture.SetPixels (defaultPencil1Texture.GetPixels ());
    }

    Pencil1Material.mainTexture = pencil1Texture;
    pencil1Texture.PixPrintText (PENCIL_TEXT_1, zxFont, 54 + shiftX, 350 + shiftY, alphaColor : zxFontAlpha, tintColor : Color.black, scale : 4, rotate : 1);
    pencil1Texture.PixPrintText (PENCIL_TEXT_1, zxFont, 54 + 2 + shiftX, 350 + 2 + shiftY, alphaColor : zxFontAlpha, tintColor : Color.white, scale : 4, rotate : 1);
    pencil1Texture.Apply ();
  }

  private void printOnSecondPencil(int shiftX, int shiftY) {
    if (pencil2Texture == null) {
      pencil2Texture = cloneTexture2D (defaultPencil2Texture);
    } else {
      pencil2Texture.SetPixels (defaultPencil2Texture.GetPixels ());
    }

    Pencil2Material.mainTexture = pencil2Texture;
    pencil2Texture.PixPrintText (PENCIL_TEXT_2, zxFont, 54 + shiftX, 390 + shiftY, alphaColor : zxFontAlpha, tintColor : Color.gray, scale : 4, rotate : 1);
    pencil2Texture.PixPrintText (PENCIL_TEXT_2, zxFont, 54 + 2 + shiftX, 390 + 2 + shiftY, alphaColor : zxFontAlpha, tintColor : Color.cyan, scale : 4, rotate : 1);
    pencil2Texture.Apply ();
  }

  private void printOnPaper(int shiftX, int shiftY) {
    if (paperTexture == null) {
      paperTexture = cloneTexture2D (defaultPaperTexture);
    } else {
      paperTexture.SetPixels (defaultPaperTexture.GetPixels ());
    }

    PaperMaterial.mainTexture = paperTexture;
    paperTexture.PixPrintText (TITLE, zxFont, 20+2 + shiftX, 80+2 + shiftY, alphaColor : zxFontAlpha, tintColor : Color.gray, scale : 4, rotate : 0);
    paperTexture.PixPrintText (TITLE, zxFont, 20 + shiftX, 80 + shiftY, alphaColor : zxFontAlpha, tintColor : Color.red, scale : 4, rotate : 0);
    var descTextHeight = zxFontHeight * 3 + 4;
    paperTexture.PixPrintText (DESC_1, marksmanFont, 17 + shiftX, 150 + shiftY, alphaColor : marksmanFontAlpha, tintColor : Color.gray, scale : 2, rotate : 0);
    paperTexture.PixPrintText (DESC_2, marksmanFont, 17 + shiftX, 150 + descTextHeight + shiftY, alphaColor : marksmanFontAlpha, tintColor : Color.gray, scale : 2, rotate : 0);
    paperTexture.PixPrintText (DESC_3, marksmanFont, 17 + shiftX, 150 + descTextHeight * 2 + shiftY, alphaColor : marksmanFontAlpha, tintColor : Color.gray, scale : 2, rotate : 0);
    paperTexture.PixPrintText (DESC_4, marksmanFont, 17 + 250 + shiftX, 150 + descTextHeight * 6 + shiftY, alphaColor : marksmanFontAlpha, tintColor : Color.gray, scale : 2, rotate : 0);
    paperTexture.Apply ();
  }

  private Texture2D cloneTexture2D (Texture2D source) {
    Texture2D texture = new Texture2D (source.width, source.height);
    texture.SetPixels (source.GetPixels ());

    return texture;
  }

  private void OnDisable () {
    EraserMaterial.mainTexture = defaultEraserTexture;
    PaperMaterial.mainTexture = defaultPaperTexture;
    Pencil1Material.mainTexture = defaultPencil1Texture;
    Pencil2Material.mainTexture = defaultPencil2Texture;
    MiniArcadeMachineBodyMaterial.mainTexture = defaultArcadeMachineBodyTexture;
  }

}