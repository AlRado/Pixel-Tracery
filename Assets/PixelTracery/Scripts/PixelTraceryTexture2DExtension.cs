using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelTracery {

    /**
     * The MIT License (MIT)
     * 
     *  Copyright © 2018 Alexey Radyuk
     * 
     * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
     * associated documentation files (the «Software»), to deal in the Software without restriction, including 
     * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
     * of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
     * 
     * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
     * 
     * THE SOFTWARE IS PROVIDED «AS IS», WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
     * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
     * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
     * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR 
     * THE USE OR OTHER DEALINGS IN THE SOFTWARE.
     */

    /**
     * Pixel Tracery
     * Texture2D extension class thath realized draw into Texture2D.
     * Start position (0,0) is the left top corner.
     * Graphics API used Bresenham's algorithms.
     * All API methods start from "Pix" prefix.
     */

    /**
     * Pixel Tracery v.0.3.1
     * Main functions:
     * - print sprites with parameters: scale, flip, rotate, alpha color, tint color, circle shift, mask with position 
     * - print text with sprite fonts with parameters: scale, flip, rotate, alpha color, tint color, fixed width, auto width
     * - print geometry:
     *    Clear, Pixel, Line,
     *    Circle, Filled Circle,
     *    Ellipse, Filled Ellipse,
     *    Rectangle, Filled Rectangle,
     *    Triangle, Filled Triangle,
     *    Fill, Quad Bezier
     */

    public static class PixelTraceryTexture2DExtension {

    #region Public

    public static void PixSetPixel (this Texture2D texture, float x, float y, Color32 color) {
      _setPixel (texture, x, y, color, null, 0, 0, 0, 0);
    }

    public static void PixSetPixel (this Texture2D texture, float x, float y, Color32 color,
      Color32[] mask, int maskWidth, int maskHeight, int maskX = 0, int maskY = 0) {

      _setPixel (texture, x, y, color, mask, maskWidth, maskHeight, maskX, maskY);
    }

    public static Color32 PixGetPixel (this Texture2D texture, float x, float y) {
      int _x = (int) x;
      int _y = (int) y;
      if (_x < 0 || _x >= texture.width || _y < 0 || _y >= texture.height) return Color.clear;

      return texture.GetPixel (_x, transformY (_y, texture.height));
    }

    public static Color32[] PixGetPixels (this Texture2D texture, float x, float y, float width, float height) {
      int _x = (int) x;
      int _y = (int) y;
      if (_x < 0 || _x >= texture.width || _y < 0 || _y >= texture.height) return null;

      int newY = transformY (_y + (int) height - 1, texture.height); 

      var rawColors = texture.GetPixels (_x, newY, (int) width, (int) height);
      var colors = new Color32[rawColors.Length];
      for (int i = 0; i < rawColors.Length; i++) {
        colors[i] = rawColors[i];
      }

      return colors;
    }

    public static void PixSetPixels (this Texture2D texture, Color32[] rawData, float x, float y,  int blockWidth, int blockHeight, int scale = 1, 
      int flip = 0, int rotate = 0, float circleShiftX = 0, float circleShiftY = 0) { 

      PixSetPixels (texture, rawData, x, y, blockWidth, blockHeight, scale, flip, rotate, circleShiftX, circleShiftY,
       default (Color32) , default (Color32));
    }

    public static void PixSetPixels (this Texture2D texture, Color32[] rawData, float x, float y,  int blockWidth, int blockHeight, int scale = 1, 
      int flip = 0, int rotate = 0, float circleShiftX = 0, float circleShiftY = 0,
      Color32 alphaColor = default (Color32), Color32 tintColor = default (Color32)) { 

      PixSetPixels (texture, rawData, x, y, blockWidth, blockHeight, scale, flip, rotate, circleShiftX, circleShiftY,
       alphaColor , tintColor, null, 0, 0, 0, 0);
    }

    public static void PixSetPixels (this Texture2D texture, Color32[] rawData, float x, float y,  int blockWidth, int blockHeight, int scale = 1, 
      int flip = 0, int rotate = 0, float circleShiftX = 0, float circleShiftY = 0,
      Color32 alphaColor = default (Color32), Color32 tintColor = default (Color32),
      Color32[] mask = null, int maskWidth = 0, int maskHeight = 0, int maskX = 0, int maskY = 0) {

      if (rawData == null) return;

      var clonedData = rawData.Clone () as Color32[];

      var data = SpriteExtension.PixGetColors (clonedData, blockWidth, blockHeight, flip, rotate, circleShiftX, circleShiftY);
      var width = rotate == 1 || rotate == 3 ? blockHeight : blockWidth;
      var height = rotate == 1 || rotate == 3 ? blockWidth : blockHeight;

      PixSetPixels (texture, data, x, y, width, height, scale, alphaColor, tintColor, mask, maskWidth, maskHeight, maskX, maskY);
    }

    // Can be used for the most economical graphics output, pre-cached data
    public static void PixSetPixels (this Texture2D texture, Color32[] data, float x, float y, float blockWidth, float blockHeight, int scale = 1) {
        PixSetPixels (texture, data, x, y, blockWidth, blockHeight, scale, default (Color32), default (Color32));
    }

    // Can be used for the most economical graphics output, pre-cached data
    public static void PixSetPixels (this Texture2D texture, Color32[] data, float x, float y, float blockWidth, float blockHeight, int scale = 1, 
      Color32 alphaColor = default (Color32), Color32 tintColor = default (Color32)) {
        PixSetPixels (texture, data, x, y, blockWidth, blockHeight, scale, alphaColor, tintColor, null, 0, 0, 0, 0);
    }

    // Can be used for the most economical graphics output, pre-cached data
    public static void PixSetPixels (this Texture2D texture, Color32[] data, float x, float y, float blockWidth, float blockHeight, int scale = 1, 
      Color32 alphaColor = default (Color32), Color32 tintColor = default (Color32), 
      Color32[] mask = null, int maskWidth = 0, int maskHeight = 0, int maskX = 0, int maskY = 0) {

      var height = (int) blockHeight * scale;
      var width = (int) blockWidth * scale;
      int _x = (int) x;
      int _y = transformY ((int) y + height - 1, texture.height); // HAKA
      int i = 0;
      int scale2 = scale * scale;
      for (int yy = 0; yy < height; yy += scale) {
        for (int xx = 0; xx < width; xx += scale) {
          Color32 color = data[i];
          if (!(color.r == alphaColor.r && color.g == alphaColor.g && color.b == alphaColor.b && color.a == alphaColor.a)) {
            for (int n = 0; n < scale2; n++) {
              var posX = _x + xx + n % scale;
              var posY = _y + yy + n / scale;
              if (posX < 0 || posX >= texture.width || posY < 0 || posY >= texture.height) continue;

              if (isInMask (posX, posY, texture.height, mask, maskWidth, maskHeight, maskX, maskY)) continue;

              var defCol = default (Color32);
              var col = tintColor.r == defCol.r && tintColor.g == defCol.g && tintColor.b == defCol.b && tintColor.a == defCol.a ? color : tintColor;
              texture.SetPixel (posX, posY, col);
            }
          }
          i++;
        }
      }
    }

    public static void PixLine (this Texture2D texture, float x0, float y0, float x1, float y1, Color32 color) {
        _line (texture, x0, y0, x1, y1, color, null, 0, 0, 0, 0);
    }

    public static void PixLine (this Texture2D texture, float x0, float y0, float x1, float y1, Color32 color,
      Color32[] mask, int maskWidth, int maskHeight, int maskX = 0, int maskY = 0) {

        _line (texture, x0, y0, x1, y1, color, mask, maskWidth, maskHeight, maskX, maskY);
    }

    public static void PixCircle (this Texture2D texture, float x, float y, float radius, Color32 color, bool filled) {
        PixCircle (texture, x, y, radius, color, filled, null, 0, 0, 0, 0);
    }

    public static void PixCircle (this Texture2D texture, float x, float y, float radius, Color32 color, bool filled,
      Color32[] mask, int maskWidth, int maskHeight, int maskX = 0, int maskY = 0) {

      int cx = (int) radius;
      int cy = 0;
      int radiusError = 1 - cx;

      while (cx >= cy) {
        if (filled) {
          _line(texture, cx + x, cy + y, -cx + x, cy + y, color, mask, maskWidth, maskHeight, maskX, maskY);
          _line(texture, cy + x, cx + y, -cy + x, cx + y, color, mask, maskWidth, maskHeight, maskX, maskY);
          _line(texture, -cx + x, -cy + y, cx + x, -cy + y, color, mask, maskWidth, maskHeight, maskX, maskY);
          _line(texture, -cy + x, -cx + y, cy + x, -cx + y, color, mask, maskWidth, maskHeight, maskX, maskY);

        } else {
          _setPixel(texture, cx + x, cy + y, color, mask, maskWidth, maskHeight, maskX, maskY);
          _setPixel(texture, cy + x, cx + y, color, mask, maskWidth, maskHeight, maskX, maskY);
          _setPixel(texture, -cx + x, cy + y, color, mask, maskWidth, maskHeight, maskX, maskY);
          _setPixel(texture, -cy + x, cx + y, color, mask, maskWidth, maskHeight, maskX, maskY);
          _setPixel(texture, -cx + x, -cy + y, color, mask, maskWidth, maskHeight, maskX, maskY);
          _setPixel(texture, -cy + x, -cx + y, color, mask, maskWidth, maskHeight, maskX, maskY);
          _setPixel(texture, cx + x, -cy + y, color, mask, maskWidth, maskHeight, maskX, maskY);
          _setPixel(texture, cy + x, -cx + y, color, mask, maskWidth, maskHeight, maskX, maskY);
        }
        cy++;

        if (radiusError < 0) {
          radiusError += 2 * cy + 1;
        } else {
          cx--;
          radiusError += 2 * (cy - cx + 1);
        }
      }
    }

    // x,y : position of the ellipse
    // a,b : major and minor radius
    // angle : angle of the ellipse in degrees
    // color : color of the filled ellipse
    public static void PixEllipseFilled (this Texture2D texture, float x, float y, float a, float b, float angle, Color32 color) {
        PixEllipseFilled (texture, x, y, a, b, angle, color, null, 0, 0, 0, 0);
    }

    // x,y : position of the ellipse
    // a,b : major and minor radius
    // angle : angle of the ellipse in degrees
    // color : color of the filled ellipse
    public static void PixEllipseFilled (this Texture2D texture, float x, float y, float a, float b, float angle, Color32 color,
      Color32[] mask, int maskWidth, int maskHeight, int maskX = 0, int maskY = 0) {

      angle *= Mathf.Deg2Rad;
      var cos = Mathf.Cos (angle);
      var sin = Mathf.Sin (angle);
      var cos2 = Mathf.Cos (angle) + Mathf.PI * 0.5f;
      var sin2 = Mathf.Sin (angle) + Mathf.PI * 0.5f;
      var ux = a * cos;
      var uy = a * sin;
      var vx = b * cos2;
      var vy = b * sin2;
      var w = Mathf.Sqrt (ux * ux + vx * vx);
      var h = Mathf.Sqrt (uy * uy + vy * vy);
      for (var x0 = -w; x0 < w; x0++) {
        for (var y0 = -h; y0 < h; y0++) {
          var x1 = (cos * x0 + sin * y0) / a;
          var y1 = (-sin * x0 + cos * y0) / b;
          if (x1 * x1 + y1 * y1 < 1) pixel (texture, x + x0, y + y0, texture.width, texture.height, color, mask, maskWidth, maskHeight, maskX, maskY);
        }
      }
    }

    public static void PixEllipseRect (this Texture2D texture, float x0, float y0, float x1, float y1, Color32 color) {
        PixEllipseRect (texture, x0, y0, x1, y1, color, null, 0, 0, 0, 0);
    }

    public static void PixEllipseRect (this Texture2D texture, float x0, float y0, float x1, float y1, Color32 color,
      Color32[] mask, int maskWidth, int maskHeight, int maskX = 0, int maskY = 0) {

      float a = Mathf.Abs (x1 - x0), b = Mathf.Abs (y1 - y0), b1 = (int) b & 1;
      float dx = 4 * (1 - a) * b * b, dy = 4 * (b1 + 1) * a * a;
      float err = dx + dy + b1 * a * a, e2;

      if (x0 > x1) { x0 = x1; x1 += a; }
      if (y0 > y1) y0 = y1;
      y0 += (b + 1) / 2;
      y1 = y0 - b1;
      a *= 8 * a;
      b1 = 8 * b * b;

      do {
        pixel (texture, x1, y0, texture.width, texture.height, color, mask, maskWidth, maskHeight, maskX, maskY);
        pixel (texture, x0, y0, texture.width, texture.height, color, mask, maskWidth, maskHeight, maskX, maskY);
        pixel (texture, x0, y1, texture.width, texture.height, color, mask, maskWidth, maskHeight, maskX, maskY);
        pixel (texture, x1, y1, texture.width, texture.height, color, mask, maskWidth, maskHeight, maskX, maskY);
        e2 = 2 * err;
        if (e2 <= dy) { y0++; y1--; err += dy += a; }
        if (e2 >= dx || 2 * err > dy) { x0++; x1--; err += dx += b1; }
      } while (x0 <= x1);

      while (y0 - y1 < b) {
        pixel (texture, x0 - 1, y0, texture.width, texture.height, color, mask, maskWidth, maskHeight, maskX, maskY);
        pixel (texture, x1 + 1, y0++, texture.width, texture.height, color, mask, maskWidth, maskHeight, maskX, maskY);
        pixel (texture, x0 - 1, y1, texture.width, texture.height, color, mask, maskWidth, maskHeight, maskX, maskY);
        pixel (texture, x1 + 1, y1--, texture.width, texture.height, color, mask, maskWidth, maskHeight, maskX, maskY);
      }
    }

    public static void PixQuadBezier (this Texture2D texture, float x0, float y0, float x1, float y1, float x2, float y2, Color32 color) {
        PixQuadBezier (texture, x0, y0, x1, y1, x2, y2, color, null, 0, 0, 0, 0);
    }  

    public static void PixQuadBezier (this Texture2D texture, float x0, float y0, float x1, float y1, float x2, float y2, Color32 color,
      Color32[] mask, int maskWidth, int maskHeight, int maskX = 0, int maskY = 0) {

      float sx = x2 - x1, sy = y2 - y1;
      float xx = x0 - x1, yy = y0 - y1, xy;
      double dx, dy, err, cur = xx * sy - yy * sx;

      if (!(xx * sx <= 0 && yy * sy <= 0)) return;

      if (sx * (long) sx + sy * (long) sy > xx * xx + yy * yy) {
        x2 = x0;
        x0 = sx + x1;
        y2 = y0;
        y0 = sy + y1;
        cur = -cur;
      }
      if (cur != 0) {
        xx += sx;
        xx *= sx = x0 < x2 ? 1 : -1;
        yy += sy;
        yy *= sy = y0 < y2 ? 1 : -1;
        xy = 2 * xx * yy;
        xx *= xx;
        yy *= yy;
        if (cur * sx * sy < 0) {
          xx = -xx;
          yy = -yy;
          xy = -xy;
          cur = -cur;
        }
        dx = 4.0 * sy * cur * (x1 - x0) + xx - xy;
        dy = 4.0 * sx * cur * (y0 - y1) + yy - xy;
        xx += xx;
        yy += yy;
        err = dx + dy + xy;
        do {
          pixel (texture, x0, y0, texture.width, texture.height, color, mask, maskWidth, maskHeight, maskX, maskY);
          if (x0 == x2 && y0 == y2) return;
          y1 = (int) (2 * err);
          if (2 * err > dy) { x0 += sx; dx -= xy; err += dy += yy; }
          if (y1 < dx) { y0 += sy; dy -= xy; err += dx += xx; }
        } while (dy < dx);
      }
      PixLine (texture, x0, y0, x2, y2, color, mask, maskWidth, maskHeight, maskX, maskY);
    }

    public static void PixFill (this Texture2D texture, float x, float y, Color32 color) {
      int width = texture.width;
      int height = texture.height;
      Point start = new Point ((int) x, transformY ((int) y, texture.height));
      TwoDimArrColors colorsCopy = new TwoDimArrColors (texture.width, texture.GetPixels32 ());
      Color32 originalColor = texture.GetPixel (start.x, start.y);

      if (System.Object.Equals (originalColor, color)) return;

      colorsCopy[start.x, start.y] = color;

      Queue<Point> nodes = new Queue<Point> ();
      nodes.Enqueue (start);

      int i = 0;
      int emergency = width * height;

      while (nodes.Count > 0) {
        i++;

        if (i > emergency) return;

        Point current = nodes.Dequeue ();
        int _x = current.x;
        int _y = current.y;

        if (_x > 0) {
          if (System.Object.Equals (colorsCopy[_x - 1, _y], originalColor)) {
            colorsCopy[_x - 1, _y] = color;
            nodes.Enqueue (new Point (_x - 1, _y));
          }
        }
        if (_x < width - 1) {
          if (System.Object.Equals (colorsCopy[_x + 1, _y], originalColor)) {
            colorsCopy[_x + 1, _y] = color;
            nodes.Enqueue (new Point (_x + 1, _y));
          }
        }
        if (_y > 0) {
          if (System.Object.Equals (colorsCopy[_x, _y - 1], originalColor)) {
            colorsCopy[_x, _y - 1] = color;
            nodes.Enqueue (new Point (_x, _y - 1));
          }
        }
        if (_y < height - 1) {
          if (System.Object.Equals (colorsCopy[_x, _y + 1], originalColor)) {
            colorsCopy[_x, _y + 1] = color;
            nodes.Enqueue (new Point (_x, _y + 1));
          }
        }
      }

      texture.SetPixels32 (colorsCopy.data);
    }

    public static void PixRectangle (this Texture2D texture, float x, float y, float width, float height, Color32 color, bool filled) {
      PixRectangle (texture, x, y, width, height, color, filled, null, 0, 0, 0, 0);
    }

    public static void PixRectangle (this Texture2D texture, float x, float y, float width, float height, Color32 color, bool filled,
      Color32[] mask, int maskWidth, int maskHeight, int maskX = 0, int maskY = 0) {

      if (filled) {
        int _width = (int) width;
        int _height = (int) height;

        int _x = (int) x;
        if (_x < 0) {
          _width += _x;
          _x = 0;
        }

        int _y = (int) y;
        if (_y < 0) {
          _height += _y;
          _y = 0;
        }

        if (_x + _width < 0 || _y + _height < 0) return;

        for (int py = 0; py < _height; py++) {
          for (int px = 0; px < _width; px++) {
            _setPixel(texture, _x + px, _y + py, color, mask, maskWidth, maskHeight, maskX, maskY);
          }
        }

      } else {
        _line(texture, x, y, x, y + height, color, mask, maskWidth, maskHeight, maskX, maskY);
        _line(texture, x, y + height, x + width, y + height, color, mask, maskWidth, maskHeight, maskX, maskY);
        _line(texture, x + width, y + height, x + width, y, color, mask, maskWidth, maskHeight, maskX, maskY);
        _line(texture, x + width, y, x, y, color, mask, maskWidth, maskHeight, maskX, maskY);
      }
    }

    public static void PixTriangle (this Texture2D texture, float x1, float y1, float x2, float y2, float x3, float y3, Color32 color, bool filled) {
      PixTriangle (texture, x1, y1, x2, y2, x3, y3, color, filled, null, 0, 0, 0, 0);
    }

    public static void PixTriangle (this Texture2D texture, float x1, float y1, float x2, float y2, float x3, float y3, Color32 color, bool filled,
      Color32[] mask, int maskWidth, int maskHeight, int maskX = 0, int maskY = 0) {

      if (filled) {
        Point vt1 = new Point ((int) x1, (int) y1);
        Point vt2 = new Point ((int) x2, (int) y2);
        Point vt3 = new Point ((int) x3, (int) y3);

        // sort vertices ascending by y 
        if (vt1.y > vt2.y) Swap (ref vt1, ref vt2);
        if (vt1.y > vt3.y) Swap (ref vt1, ref vt3);
        if (vt2.y > vt3.y) Swap (ref vt2, ref vt3);

        if (vt2.y == vt3.y) {
          fillFlatSideTriangle (texture, vt1, vt2, vt3, color, mask, maskWidth, maskHeight, maskX, maskY);
        } else if (vt1.y == vt2.y) {
          fillFlatSideTriangle (texture, vt3, vt1, vt2, color, mask, maskWidth, maskHeight, maskX, maskY);
        } else {
          Point vTmp = new Point ((int) (vt1.x + ((float) (vt2.y - vt1.y) / (float) (vt3.y - vt1.y)) * (vt3.x - vt1.x)), vt2.y);
          fillFlatSideTriangle (texture, vt1, vt2, vTmp, color, mask, maskWidth, maskHeight, maskX, maskY);
          fillFlatSideTriangle (texture, vt3, vt2, vTmp, color, mask, maskWidth, maskHeight, maskX, maskY);
        }

      } else {
        _line(texture, x1, y1, x2, y2, color, mask, maskWidth, maskHeight, maskX, maskY);
        _line(texture, x2, y2, x3, y3, color, mask, maskWidth, maskHeight, maskX, maskY);
        _line(texture, x3, y3, x1, y1, color, mask, maskWidth, maskHeight, maskX, maskY);
      }
    }

    public static void PixSprite (this Texture2D texture, float x, float y, Sprite sprite, int scale = 1, int flip = 0, int rotate = 0,
      Color32 alphaColor = default (Color32), Color32 tintColor = default (Color32), float circleShiftX = 0, float circleShiftY = 0,
      Color32[] mask = null, int maskWidth = 0, int maskHeight = 0, int maskX = 0, int maskY = 0) {

      if (sprite == null) return;

      var data = sprite.PixGetColors (flip, rotate, circleShiftX, circleShiftY);
      var width = rotate == 1 || rotate == 3 ? sprite.PixGetHeight () : sprite.PixGetWidth ();
      var height = rotate == 1 || rotate == 3 ? sprite.PixGetWidth () : sprite.PixGetHeight ();

      PixSetPixels (texture, data, x, y, width, height, scale, alphaColor, tintColor, mask, maskWidth, maskHeight, maskX, maskY);
    }

    public static int PixPrintText (this Texture2D texture, object msg, Dictionary<char, Sprite> font, float x = 0, float y = 0,
      Color32 alphaColor = default (Color32), Color32 tintColor = default (Color32), int scale = 1, int flip = 0, int rotate = 0,
      int fixedWidth = -1, float circleShiftX = 0, float circleShiftY = 0, Color32[] mask = null, int maskWidth = 0, int maskHeight = 0, int maskX = 0, int maskY = 0) {

      string text = msg.ToString ();
      if (String.IsNullOrEmpty (text)) return 0;

      int defaultCharWidth = 0;
      int textWidth = 0;
      int fullWidth = _getTextWidth (texture, msg, font, scale);
      foreach (var key in text) {
        if (!font.ContainsKey (key)) {
          textWidth += fixedWidth != -1 ? fixedWidth * scale : defaultCharWidth * scale;
          continue;
        }

        // TODO to do it well and cached variant {data, charWidth, charHeight}
        Sprite sprite = font[key];
        var data = sprite.PixGetColors ();
        var charWidth = sprite.PixGetWidth ();
        var charHeight = sprite.PixGetHeight ();

        Color32 defaultColor = data[0];
        defaultCharWidth = defaultCharWidth == 0 ? charWidth : defaultCharWidth;

        tintColor = tintColor.Equals (default (Color32)) ? defaultColor : tintColor;

        var pos = getCharPosition (x, y, scale, flip, rotate, fullWidth, textWidth, charWidth);

        PixSetPixels (texture, data, pos.x, pos.y, charWidth, charHeight, scale, flip, rotate, circleShiftX, circleShiftY,
          alphaColor, tintColor, mask, maskWidth, maskHeight, maskX, maskY);

        var delta = fixedWidth != -1 ? fixedWidth * scale : charWidth * scale;
        textWidth += delta;
      }
      return textWidth;
    }

    // for better performance, use this method once, such as in Awake or Start
    public static int PixGetTextWidth (this Texture2D texture, object msg, Dictionary<char, Sprite> font, int scale = 1, int fixedWidth = -1) {
      return _getTextWidth (texture, msg, font, scale, fixedWidth);
    } 
    
    // we believe that the height of all characters of the font is the same and equal to the height of the first character
    // for better performance, use this method once, such as in Awake or Start
    public static int PixGetTextHeight (this Texture2D texture, Dictionary<char, Sprite> font, int scale = 1) {
      return font.ToArray () [0].Value.PixGetHeight () * scale;;
    }

    public static void PixClear (this Texture2D texture, ref Color32[] clearColors, Color32 color) {
      if (clearColors == null) clearColors = new Color32[texture.width * texture.height];

      if (!Color.Equals (clearColors[0], color)) {
        for (int i = 0; i < clearColors.Length; i++) {
          clearColors[i] = color;
        }
      }
      PixClear (texture, clearColors);
    }

    public static void PixClear (this Texture2D texture, Color32[] clearColors) {
      if (clearColors == null) {
        Debug.LogError ("ClearColors is null");
        return;
      }

      texture.SetPixels32 (clearColors);
    }

    #endregion

    #region Private

    private static void _setPixel (this Texture2D texture, float x, float y, Color32 color,
      Color32[] mask, int maskWidth, int maskHeight, int maskX = 0, int maskY = 0) {

      pixel (texture, x, y, texture.width, texture.height, color, mask, maskWidth, maskHeight, maskX, maskY);
    }

    private static void _line (this Texture2D texture, float x0, float y0, float x1, float y1, Color32 color,
      Color32[] mask, int maskWidth, int maskHeight, int maskX = 0, int maskY = 0) {

      int width = texture.width;
      int height = texture.height;
      int _x0 = (int) x0;
      int _y0 = (int) y0;
      int _x1 = (int) x1;
      int _y1 = (int) y1;

      bool isSteep = Math.Abs (_y1 - _y0) > Math.Abs (_x1 - _x0);
      if (isSteep) {
        Swap (ref _x0, ref _y0);
        Swap (ref _x1, ref _y1);
      }
      if (_x0 > _x1) {
        Swap (ref _x0, ref _x1);
        Swap (ref _y0, ref _y1);
      }

      int deltaX = _x1 - _x0;
      int deltaY = Math.Abs (_y1 - _y0);

      int correction = deltaX / 2;
      int y = _y0;
      int yStep = _y0 < _y1 ? 1 : -1;

      for (int x = _x0; x <= _x1; x++) {
        if (isSteep) {
          pixel (texture, y, x, width, height, color, mask, maskWidth, maskHeight, maskX, maskY);
        } else {
          pixel (texture, x, y, width, height, color, mask, maskWidth, maskHeight, maskX, maskY);
        }
        correction = correction - deltaY;
        if (correction < 0) {
          y = y + yStep;
          correction = correction + deltaX;
        }
      }
    }

    private static int _getTextWidth (this Texture2D texture, object msg, Dictionary<char, Sprite> font, int scale = 1, int fixedWidth = -1) {
      string text = msg.ToString ();
      int defaultCharWidth = 0;
      int fullWidth = 0;
      foreach (var key in text) {
        if (!font.ContainsKey (key)) {
          fullWidth += fixedWidth != -1 ? fixedWidth * scale : defaultCharWidth * scale;
          continue;
        }
        fullWidth += fixedWidth != -1 ? fixedWidth * scale : font[key].PixGetWidth () * scale;
      }

      return fullWidth;
    }

    private static void pixel (Texture2D texture, float x, float y, float width, float height, Color32 color,
      Color32[] mask = null, int maskWidth = 0, int maskHeight = 0, int maskX = 0, int maskY = 0) {

      int _x = (int) x;
      int _y = (int) y;
      int _width = (int) width;
      int _height = (int) height;

      if (_x < 0 || _x >= _width || _y < 0 || _y >= _height) return;

      var newY = transformY (_y, _height);
      if (isInMask (_x, newY, texture.height, mask, maskWidth, maskHeight, maskX, maskY)) return;

      texture.SetPixel (_x, newY, color);
    }

    private static bool isInMask (int posX, int posY, int height, Color32[] mask, int maskWidth, int maskHeight, int maskX, int maskY) {
      if (mask != null && maskWidth == 0 || mask != null && maskHeight == 0) Debug.LogError ("Mask width or height is incorrect!");
      if (mask == null) return false;    

      var _maskY = transformY (maskY + maskHeight - 1, height);
      if (posX < maskX || posX > maskX + maskWidth || posY < _maskY || posY > _maskY + maskHeight) return false;

      var inMaskX = posX - maskX;
      var inMaskY = posY - _maskY;   
      if (inMaskX < 0 || inMaskX >= maskWidth || inMaskY < 0 || inMaskY >= maskHeight) return false;

      var maskColor = mask[inMaskX + inMaskY * maskWidth];
      return maskColor.r == 0 && maskColor.g == 0 && maskColor.b == 0;
    }

    private static void fillFlatSideTriangle (Texture2D texture, Point v1, Point v2, Point v3, Color32 color, 
    Color32[] mask, int maskWidth, int maskHeight, int maskX, int maskY) {

      Point vTmp1 = new Point (v1.x, v1.y);
      Point vTmp2 = new Point (v1.x, v1.y);

      bool changed1 = false;
      bool changed2 = false;

      int dx1 = Mathf.Abs (v2.x - v1.x);
      int dy1 = Mathf.Abs (v2.y - v1.y);

      int dx2 = Mathf.Abs (v3.x - v1.x);
      int dy2 = Mathf.Abs (v3.y - v1.y);

      int xSign1 = Math.Sign (v2.x - v1.x);
      int xSign2 = Math.Sign (v3.x - v1.x);

      int ySign1 = Math.Sign (v2.y - v1.y);
      int ySign2 = Math.Sign (v3.y - v1.y);

      if (dy1 > dx1) {
        Swap (ref dx1, ref dy1);
        changed1 = true;
      }

      if (dy2 > dx2) {
        Swap (ref dx2, ref dy2);
        changed2 = true;
      }

      int e1 = 2 * dy1 - dx1;
      int e2 = 2 * dy2 - dx2;

      for (int i = 0; i <= dx1; i++) {
        PixLine (texture, vTmp1.x, vTmp1.y, vTmp2.x, vTmp2.y, color, mask, maskWidth, maskHeight, maskX, maskY);

        while (e1 >= 0) {
          if (changed1)
            vTmp1.x += xSign1;
          else
            vTmp1.y += ySign1;
          e1 = e1 - 2 * dx1;
        }

        if (changed1)
          vTmp1.y += ySign1;
        else
          vTmp1.x += xSign1;

        e1 = e1 + 2 * dy1;

        while (vTmp2.y != vTmp1.y) {
          while (e2 >= 0) {
            if (changed2)
              vTmp2.x += xSign2;
            else
              vTmp2.y += ySign2;
            e2 = e2 - 2 * dx2;
          }

          if (changed2)
            vTmp2.y += ySign2;
          else
            vTmp2.x += xSign2;

          e2 = e2 + 2 * dy2;
        }
      }
    }

    private static Point getCharPosition (float x, float y, int scale, int flip, int rotate, int fullWidth, int textWidth, int charWidth) {
      var startX = x;
      var signX = 1;
      if (flip == 1 || flip == 3) {
        signX = -1;
        if (rotate != 1 && rotate != 3) {
          startX += fullWidth - charWidth * scale;
        }
      }

      var startY = y;
      var signY = 0;
      if (rotate == 1) {
        signX = 0;
        signY = 1;

      } else if (rotate == 2) {
        startX = x - charWidth * scale;
        signX = -1;

      } else if (rotate == 3) {
        startY = y - charWidth * scale;
        signX = 0;
        signY = -1;
      }

      var newX = (int) startX + textWidth * signX;
      var newY = (int) startY + textWidth * signY;

      return new Point (newX, newY);
    }

    private static int transformY (int y, int height) {
      return height - y - 1;
    }

    #endregion

    #region Helpers 

    public static void Swap<T> (ref T a, ref T b) {
      var temp = a;
      a = b;
      b = temp;
    }

    private class TwoDimArrColors {
      private int width;
      public Color32[] data;

      public TwoDimArrColors (int width, Color32[] data) {
        this.width = width;
        this.data = data;
      }

      public Color32 this [int x, int y] {
        get {
          return data[x + y * width];
        }
        set {
          data[x + y * width] = value;
        }
      }
    }

    private struct Point {
      public int x;
      public int y;

      public Point (int x, int y) {
        this.x = x;
        this.y = y;
      }
    }

    #endregion

  }

    /**
     * Sprite extension class
     */
    public static class SpriteExtension {

    public static int PixGetWidth (this Sprite sprite) { return (int) sprite.textureRect.width; }
    public static int PixGetHeight (this Sprite sprite) { return (int) sprite.textureRect.height; }

    public static Color32[] PixGetColors (this Sprite sprite) {
      return PixGetColors (sprite, 0, 0);
    }

    public static Color32[] PixGetColors (this Sprite sprite, int flip = 0, int rotate = 0) {
      return PixGetColors (sprite, flip, rotate, 0, 0);
    }

    public static Color32[] PixGetColors (this Sprite sprite, int flip, int rotate, float dx = 0, float dy = 0) {
      var x = (int) sprite.textureRect.x;
      var y = (int) sprite.textureRect.y;
      var width = PixGetWidth (sprite);
      var height = PixGetHeight (sprite);
      var rawColors = sprite.texture.GetPixels (x, y, width, height);

      return PixGetColors (rawColors, width, height, flip, rotate, dx, dy);
    }

    public static Color32[] PixGetColors (Color[] rawColors, int width, int height, int flip, int rotate, float dx = 0, float dy = 0) {
      var colors = new Color32[rawColors.Length];
      for (int i = 0; i < rawColors.Length; i++) {
        colors[i] = rawColors[i];
      }

      return PixGetColors (colors, width, height, flip, rotate, dx, dy);;
    }

    public static Color32[] PixGetColors (Color32[] colors, int width, int height, int flip, int rotate, float dx = 0, float dy = 0) {
      doFlip (ref colors, flip, width, height);
      colors = doRotate (colors, rotate, width, height);
      doCircularShift (ref colors, width, height, (int) dx, (int) dy);

      return colors;
    }

    #region CircularShift

    private static void doCircularShift (ref Color32[] data, int width, int height, int dx, int dy) {
      dx %= width;
      dy %= height;

      if (dx == 0 && dy == 0) return;

      if (dx < 0) dx = width + dx;
      if (dy < 0) dy = height + dy;

      var oldData = data.Clone () as Color32[];
      for (int r = 0; r < width; r++) {
        for (int c = 0; c < height; c++)
          data[(r + dx) % +width + ((c + dy) % height) * width] = oldData[r + c * width];
      }
    }

    #endregion

    #region Flip

    private static void doFlip (ref Color32[] data, int flip, int width, int height) {
      if (flip == 0) return;

      if (flip == 1 || flip == 3) flipHorizontal (ref data, width, height);
      if (flip == 2 || flip == 3) flipVertical (ref data, width, height);
    }

    private static void flipHorizontal (ref Color32[] data, int width, int height) {
      for (int i = 0; i < height; i++) {
        for (int lo = 0, hi = width - 1; lo < hi; lo++, hi--) {
          PixelTraceryTexture2DExtension.Swap (ref data[i * width + lo], ref data[i * width + hi]);
        }
      }
    }

    private static void flipVertical (ref Color32[] data, int width, int height) {
      for (int i = 0; i < width; i++) {
        for (int lo = 0, hi = height - 1; lo < hi; lo++, hi--) {
          PixelTraceryTexture2DExtension.Swap (ref data[lo * width + i], ref data[hi * width + i]);
        }
      }
    }

    #endregion

    #region Rotate

    private static Color32[] doRotate (Color32[] data, int rotate, int width, int height) {
      rotate = Math.Min (rotate, 3);

      return rotateCW (data, width, height, rotate);
    }

    private static Color32[] rotateCW (Color32[] data, int width, int height, int rotate) {
      if (rotate <= 0) return data;

      var result = data.Clone () as Color32[];
      for (int r = 0; r < width; r++) {
        for (int c = 0; c < height; c++) {
          result[c + (width - 1 - r) * height] = data[r + c * width];
        }
      }
      return rotateCW (result, height, width, --rotate);
    }

    #endregion

  }

    /**
     * Helper class for getting sprites font 
     */
    public static class SpriteArrayExtension {

    // See all codes http://www.asciitable.com
    // Please use ASCII character codes in decimal form for characters that can not be assigned as names for sprites in Unity, e.g:
    // 34 "  
    // 42 *  
    // 47 /  
    // 58 :
    // 60 <
    // 62 >
    // 63 ?
    // 92 \
    // 124 |
    public static Dictionary<char, Sprite> PixToFontDictionary (this Sprite[] fontSprites) {
      return fontSprites.ToDictionary (keySelector);
    }

    private static Char keySelector(Sprite x) {
      try {
          return x.name.Length > 1 ?
            Convert.ToChar (int.Parse (x.name)) :
            Convert.ToChar (x.name);

      } catch (System.Exception) {
          throw new Exception("Try parse NOT valid font char name: " + x);
      }
    }
  }
}