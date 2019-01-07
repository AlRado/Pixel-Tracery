using System.Collections;
using System.Collections.Generic;
using PixelTracery;
using UnityEngine;

public class PixelTraceryScene : MonoBehaviour {

  public const string ZX_FONT_PATH = "SpriteFonts/ZxSpectrumFont";
  public const string UBBLE_FONT_PATH = "SpriteFonts/Ubble";
  public const string MARKSMAN_FONT_PATH = "SpriteFonts/marksman-v1";

  private const string TITLE = "Pixel Tracery v.0.3";
  private const string DESC_1 = "* dynamic inscriptions on the walls";
  private const string DESC_2 = "* drawings on the 3d-objects";
  private const string DESC_3 = "* old-school 2d-games inside your unity 3d-game!";
  private const string DESC_4 = "Simple, fast, reliable.";

  private const string BEST_ERASER = "Best eraser";
  private const string PENCIL_DESC_1 = "Pencil for creativity";
  private const string PENCIL_DESC_2 = "Magic pencil";
  private const string BEST = "BEST";
  private const string RETRO = "RETRO";
  private const string MACHINE = "MACHINE";
  private const string ARCADE = "ARCADE";
  private const string FOREVER = "FOREVER";

  public Material EraserMaterial;
  public Material PaperMaterial;
  public Material Pencil1Material;
  public Material Pencil2Material;
  public Material MiniArcadeMachineBodyMaterial;

  private Texture2D defaultEraserTexture;
  private Texture2D defaultPaperTexture;
  private Texture2D defaultPencil1Texture;
  private Texture2D defaultPencil2Texture;
  private Texture2D defaultMiniArcadeMachineBodyTexture;

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

    drawStatic ();
  }

  private void drawStatic () {
    defaultEraserTexture = (Texture2D) EraserMaterial.mainTexture;
    Texture2D eraserTexture = cloneTexture2D (defaultEraserTexture);
    EraserMaterial.mainTexture = eraserTexture;
    var eraserText = BEST_ERASER;
    var eraserTextX = 126;
    var eraserTextY = 80;
    var eraserTextScale = 2;
    var eraserTextBounds = 4;
    var eraserTextHalfBounds = eraserTextBounds / 2;
    var eraserTextWidth = eraserTexture.PixGetTextWidth (eraserText, zxFont, eraserTextScale);
    var eraserTextHeight = zxFontHeight * eraserTextScale;
    eraserTexture.PixRectangle (eraserTextX - eraserTextHalfBounds, eraserTextY - eraserTextHalfBounds, eraserTextHeight + eraserTextBounds, eraserTextWidth + eraserTextBounds, Color.white, true);
    eraserTexture.PixPrintText (eraserText, zxFont, eraserTextX, eraserTextY, alphaColor : zxFontAlpha, tintColor : Color.grey, scale : eraserTextScale, rotate : 1);
    eraserTexture.Apply ();

    defaultPaperTexture = (Texture2D) PaperMaterial.mainTexture;
    Texture2D paperTexture = cloneTexture2D (defaultPaperTexture);
    PaperMaterial.mainTexture = paperTexture;
    var title = TITLE;
    paperTexture.PixPrintText (title, zxFont, 20+2, 80+2, alphaColor : zxFontAlpha, tintColor : Color.gray, scale : 4, rotate : 0);
    paperTexture.PixPrintText (title, zxFont, 20, 80, alphaColor : zxFontAlpha, tintColor : Color.red, scale : 4, rotate : 0);
    var descColor = Color.gray;
    var descTextHeight = zxFontHeight * 3 + 4;
    var descX = 17;
    var descY = 150;
    paperTexture.PixPrintText (DESC_1, marksmanFont, descX, descY, alphaColor : marksmanFontAlpha, tintColor : descColor, scale : 2, rotate : 0);
    paperTexture.PixPrintText (DESC_2, marksmanFont, descX, descY + descTextHeight, alphaColor : marksmanFontAlpha, tintColor : descColor, scale : 2, rotate : 0);
    paperTexture.PixPrintText (DESC_3, marksmanFont, descX, descY + descTextHeight * 2, alphaColor : marksmanFontAlpha, tintColor : descColor, scale : 2, rotate : 0);
    paperTexture.PixPrintText (DESC_4, marksmanFont, descX+250, descY + descTextHeight * 6, alphaColor : marksmanFontAlpha, tintColor : descColor, scale : 2, rotate : 0);
    paperTexture.Apply ();

    defaultPencil1Texture = (Texture2D) Pencil1Material.mainTexture;
    Texture2D pencil1Texture = cloneTexture2D (defaultPencil1Texture);
    Pencil1Material.mainTexture = pencil1Texture;
    var pencilText = PENCIL_DESC_1;
    pencil1Texture.PixPrintText (pencilText, zxFont, 54, 350, alphaColor : zxFontAlpha, tintColor : Color.black, scale : 4, rotate : 1);
    pencil1Texture.PixPrintText (pencilText, zxFont, 54 + 2, 350 + 2, alphaColor : zxFontAlpha, tintColor : Color.white, scale : 4, rotate : 1);
    pencil1Texture.Apply ();

    defaultPencil2Texture = (Texture2D) Pencil2Material.mainTexture;
    Texture2D pencil2Texture = cloneTexture2D (defaultPencil2Texture);
    Pencil2Material.mainTexture = pencil2Texture;
    var pencilText2 = PENCIL_DESC_2;
    pencil2Texture.PixPrintText (pencilText2, zxFont, 54, 390, alphaColor : zxFontAlpha, tintColor : Color.gray, scale : 4, rotate : 1);
    pencil2Texture.PixPrintText (pencilText2, zxFont, 54 + 2, 390 + 2, alphaColor : zxFontAlpha, tintColor : Color.cyan, scale : 4, rotate : 1);
    pencil2Texture.Apply ();

    defaultMiniArcadeMachineBodyTexture = (Texture2D) MiniArcadeMachineBodyMaterial.mainTexture;
    Texture2D arcadeMachineBodyTexture = cloneTexture2D (defaultMiniArcadeMachineBodyTexture);
    MiniArcadeMachineBodyMaterial.mainTexture = arcadeMachineBodyTexture;
    var arcadeTextScale = 4;
    arcadeMachineBodyTexture.PixPrintText (BEST, ubbleFont, 70, 770, alphaColor : ubbleFontAlpha, tintColor : Color.black, scale : arcadeTextScale, rotate : 0);
    arcadeMachineBodyTexture.PixPrintText (BEST, ubbleFont, 70 + 5, 770 - 5, alphaColor : ubbleFontAlpha, tintColor : Color.yellow, scale : arcadeTextScale, rotate : 0);
    arcadeMachineBodyTexture.PixPrintText (RETRO, ubbleFont, 330, 750, alphaColor : ubbleFontAlpha, tintColor : Color.black, scale : arcadeTextScale, rotate : 0);
    arcadeMachineBodyTexture.PixPrintText (RETRO, ubbleFont, 330 + 5, 750 - 5, alphaColor : ubbleFontAlpha, tintColor : Color.yellow, scale : arcadeTextScale, rotate : 0);
    arcadeMachineBodyTexture.PixPrintText (MACHINE, ubbleFont, 305, 830, alphaColor : ubbleFontAlpha, tintColor : Color.black, scale : arcadeTextScale, rotate : 0);
    arcadeMachineBodyTexture.PixPrintText (MACHINE, ubbleFont, 305 + 5, 830 - 5, alphaColor : ubbleFontAlpha, tintColor : Color.red, scale : arcadeTextScale, rotate : 0);
    var arcadeText = ARCADE;
    var arcadeTextWidth = arcadeMachineBodyTexture.PixGetTextWidth (arcadeText, ubbleFont, arcadeTextScale);
    arcadeMachineBodyTexture.PixPrintText (arcadeText, ubbleFont, 750 + 5, 60 + arcadeTextWidth - 5, alphaColor : ubbleFontAlpha, tintColor : Color.black, scale : arcadeTextScale, rotate : 3);
    arcadeMachineBodyTexture.PixPrintText (arcadeText, ubbleFont, 750, 60 + arcadeTextWidth, alphaColor : ubbleFontAlpha, tintColor : Color.yellow, scale : arcadeTextScale, rotate : 3);
    var foreverText = FOREVER;
    var foreverTextWidth = arcadeMachineBodyTexture.PixGetTextWidth (foreverText, ubbleFont, arcadeTextScale);
    arcadeMachineBodyTexture.PixPrintText (foreverText, ubbleFont, 920 - 5, 50 + foreverTextWidth - 5, alphaColor : ubbleFontAlpha, tintColor : Color.black, scale : arcadeTextScale, rotate : 3);
    arcadeMachineBodyTexture.PixPrintText (foreverText, ubbleFont, 920, 50 + foreverTextWidth, alphaColor : ubbleFontAlpha, tintColor : Color.yellow, scale : arcadeTextScale, rotate : 3);
    arcadeMachineBodyTexture.Apply ();
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
    MiniArcadeMachineBodyMaterial.mainTexture = defaultMiniArcadeMachineBodyTexture;
  }

}