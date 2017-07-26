using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLayerColorMode : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    //RGB 2 HSV
    Vector3 RGB2HSV(Vector3 rgbV)
    {
        float r = rgbV.x;
        float g = rgbV.y;
        float b = rgbV.z;
        float maxc = r > g ? r : g;
        maxc = maxc > b ? maxc : b;
        float minc = r < g ? r : g;
        minc = minc < b ? minc : b;
        float h = maxc - minc;
        if (h > 0.0f)
        {
            if (maxc == r)
            {
                h = (g - b) / h;
                if (h < 0.0f) { h += 6.0f; }
            }
            else if (maxc == g)
            {
                h = 2.0f + (b - r) / h;
            }
            else
            {
                h = 4.0f + (r - g) / h;
            }
        }
        h /= 6.0f;
        float s = (maxc - minc);
        if (maxc != 0.0f) { s /= maxc; }
        float v = maxc;
        Vector3 RESULT = new Vector3(h, s, v);
        return (RESULT);
    }

    //HSV 2 RGB
    Vector3 HSV2RGB(Vector3 hsvV)
    {
        float h = hsvV.x;
        float s = hsvV.y;
        float v = hsvV.z;
        float r = v;
        float g = v;
        float b = v;
        if (s > 0.0f)
        {
            h *= 6.0f;
            int i = (int)h;
            float f = h - (float)i;
            if (i == 0)
            {
                g *= 1 - s * (1 - f);
                b *= 1 - s;
            }
            else if (i == 1)
            {
                r *= 1 - s * f;
                b *= 1 - s;
            }
            else if (i == 2)
            {
                r *= 1 - s;
                b *= 1 - s * (1 - f);
            }
            else if (i == 3)
            {
                r *= 1 - s;
                g *= 1 - s * f;
            }
            else if (i == 4)
            {
                r *= 1 - s * (1 - f);
                g *= 1 - s;
            }
            else if (i == 5)
            {
                g *= 1 - s;
                b *= 1 - s * f;
            }
        }
        Vector3 RESULT = new Vector3(r, g, b);
        return (RESULT);
    }

    // Invert Hue
    Vector3 Invert(Vector3 i)
    {
        Vector3 invert = new Vector3(255f - RGB2HSV(i).x, RGB2HSV(i).y, RGB2HSV(i).z);
        invert = HSV2RGB(invert);
        return invert;
    }

    // normalize
    Vector3 Normalize(Vector3 i)
    {
        Vector3 n = i / 255.0f;
        return n;
    }

    //float Normalize2(float b)
    //{
    //    float n = lerp(i / 255.0f;
    //    return n;
    //}

    // ~~~~~~~~~~~~~~~~~~~~~~~~~ Layer Color Mode ~~~~~~~~~~~~~~~~~~~~~~~~~
    // a 0-255 down layer;  b 0-255 up layer;  d 0-1 transparency
    // opacity
    Vector3 Opacity(Vector3 a, Vector3 b, float d)
    {
        Vector3 c = d * a + (1f - d) * b;
        return c;
    }

    // darken
    Vector3 Darken(Vector3 a, Vector3 b)
    {
        Vector3 c = Vector3.Min(a, b);
        return c;
    }

    // lighten
    Vector3 Lighten(Vector3 a, Vector3 b)
    {
        Vector3 c = Vector3.Max(a, b);
        return c;
    }

    // multiply
    Vector3 Multiply(Vector3 a, Vector3 b)
    {
        Vector3 c = Vector3.Scale(a, b) / 255f;
        return c;
    }

    // screen
    Vector3 Screen(Vector3 a, Vector3 b)
    {
        Vector3 w = new Vector3(255f, 255f, 255f);
        Vector3 c = w - Vector3.Scale(Invert(a), Invert(b)) / 255f;
        return c;
    }

    // color burn
    Vector3 ColorBurn(Vector3 a, Vector3 b)
    {
        Vector3 i = Vector3.Scale(Invert(a), Invert(b));
        if (b.x == 0) b.x = 1f;
        if (b.y == 0) b.y = 1f;
        if (b.z == 0) b.z = 1f;
        Vector3 c = new Vector3(i.x / b.x, i.y / b.y, i.z / b.z);
        c = a - c;
        if (c.x < 0) c.x = 0;
        if (c.y < 0) c.y = 0;
        if (c.z < 0) c.z = 0;
        return c;
    }

    // color dodge
    Vector3 ColorDodge(Vector3 a, Vector3 b)
    {
        Vector3 i = Invert(b);
        Vector3 c = Vector3.Scale(a, b);
        c = new Vector3(c.x / i.x, c.y / i.y, c.z / i.z);
        c = a + c;
        if (c.x > 255f) c.x = 255f;
        if (c.y > 255f) c.y = 255f;
        if (c.z > 255f) c.z = 255f;
        return c;
    }

    // linear burn
    Vector3 LinearBurn(Vector3 a, Vector3 b)
    {
        Vector3 c = new Vector3(255f,255f,255f);
        c = a + b - c;
        if (c.x < 0) c.x = 0;
        if (c.y < 0) c.y = 0;
        if (c.z < 0) c.z = 0;
        return c;
    }

    // linear dodge
    Vector3 LinearDodge(Vector3 a , Vector3 b)
    {
        Vector3 c = a + b;
        if (c.x > 255f) c.x = 255f;
        if (c.y > 255f) c.y = 255f;
        if (c.z > 255f) c.z = 255f;
        return c;
    }

    // overlay
    //Vector3 Overlay(Vector3 a, Vector3 b)
    //{
    //    Vector3 c;
    //    if()
    //}
}
