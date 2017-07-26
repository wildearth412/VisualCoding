using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLayerColorModeChanel : MonoBehaviour
{
    [SerializeField]
    private Renderer layerColorRender;


    // Use this for initialization
    void Start() {

    }

    //void SetColor()
    //{
    //    this.layerColorRender.sharedMaterial.SetColor();
    //}

    // Update is called once per frame
    void Update() {

    }

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

    // normalize
    float Normalize(float i)
    {
        float n = i / 255.0f;
        return n;
    }

    //float Normalize2(float b)
    //{
    //    float n = lerp(i / 255.0f;
    //    return n;
    //}

    // result check
    float ResultCheck(float c)
    {
        c = Mathf.Floor(c);
        if (c < 0) c = 0;
        if (c > 255f) c = 255f;
        return c;
    }

    // ~~~~~~~~~~~~~~~~~~~~~~~~~ Layer Color Mode ~~~~~~~~~~~~~~~~~~~~~~~~~
    // a 0-255 down layer;  b 0-255 up layer;  d 0-1 transparency
    // opacity
    float Opacity(float a, float b, float d)
    {
        float c = d * a + (1f - d) * b;
        return c;
    }

    // darken
    float Darken(float a, float b)
    {
        float c = Mathf.Min(a, b);
        return c;
    }

    // lighten
    float Lighten(float a, float b)
    {
        float c = Mathf.Max(a, b);
        return c;
    }

    // multiply
    float Multiply(float a, float b)
    {
        float c = a * b / 255f;
        return c;
    }

    // screen
    float Screen(float a, float b)
    {
        float c = 255f - (255f - a) * (255f - b) / 255f;
        return c;
    }

    // color burn
    float ColorBurn(float a, float b)
    {
        if (b == 0) b = 1f;
        float c = a - (255f - a) * (255f - b) / b;
        return c;
    }

    // color dodge
    float ColorDodge(float a, float b)
    {
        if (b == 255f) b = 254f;
        float c = a + a * b / (255f - b);
        return c;
    }

    // linear burn
    float LinearBurn(float a, float b)
    {
        float c = a + b - 255f;
        return c;
    }

    // linear dodge
    float LinearDodge(float a , float b)
    {
        float c = a + b;
        return c;
    }

    // add
    float Add(float a, float b)
    {
        float c = (a + b) / 4f + 128f;
        return c;
    }

    // remove
    float Remove(float a, float b)
    {
        float c = (a - b) / 2f + 128f;
        return c;
    }

    // overlay
    float Overlay(float a, float b)
    {
        float c;
        if (a <= 128f) c = a * b / 128f;
        else c = 255f - (255f - a) * (255f - b) / 128f;
        return c;
    }

    // hard light
    float HardLight(float a, float b)
    {
        float c;
        if (b <= 128f) c = a * b / 128f;
        else c = 255f - (255f - a) * (255f - b) / 128f;
        return c;
    }

    // soft light
    float SoftLight(float a, float b)
    {
        float c;
        if (b <= 128f) c = a * b / 128f + (a / 255f) * (a / 255f) * (255f - 2 * b);
        else c = a * (255f - b) / 128f + Mathf.Pow((a / 255f), 0.5f) * (2 * b - 255f);
        return c;
    }

    // vivid light
    float VividLight(float a, float b)
    {
        float c;
        if (b <= 128f) c = a - (255f - a) * (255f - 2 * b) / (2 * b);
        else c = a + a * (2 * b - 255f) / (2 * (255f - b));
        return c;
    }

    // linear light
    float LinearLight(float a,float b)
    {
        float c = a + 2 * b - 255f;
        return c;
    }

    // pin light
    float PinLight(float a, float b)
    {
        float c;
        if (b <= 128f) c = Mathf.Min(a, 2 * b);
        else c = Mathf.Min(a, 2 * b - 255f);
        return c;
    }

    // hard mix
    float HardMix(float a,float b)
    {
        float c;
        if (a + b >= 255f) c = 255;
        else c = 0;
        return c;
    }

    // difference
    float Difference(float a, float b)
    {
        float c = Mathf.Abs(a - b);
        return c;
    }

    // exclusion
    float Exclusion(float a, float b)
    {
        float c = a + b - a * b / 128f;
        return c;
    }

    // hue
    Vector3 Hue(Vector3 a, Vector3 b)
    {
        Vector3 aa = RGB2HSV(a);
        Vector3 bb = RGB2HSV(b);
        Vector3 c = new Vector3(bb.x, aa.y, aa.z);
        c = HSV2RGB(c);
        return c;
    }

    // saturation
    Vector3 Saturation(Vector3 a, Vector3 b)
    {
        Vector3 aa = RGB2HSV(a);
        Vector3 bb = RGB2HSV(b);
        Vector3 c = new Vector3(aa.x, bb.y, aa.z);
        c = HSV2RGB(c);
        return c;
    }

    // color
    Vector3 Color(Vector3 a, Vector3 b)
    {
        Vector3 aa = RGB2HSV(a);
        Vector3 bb = RGB2HSV(b);
        Vector3 c = new Vector3(bb.x, bb.y, aa.z);
        c = HSV2RGB(c);
        return c;
    }

    // luminosity
    Vector3 Luminosity(Vector3 a, Vector3 b)
    {
        Vector3 aa = RGB2HSV(a);
        Vector3 bb = RGB2HSV(b);
        Vector3 c = new Vector3(aa.x, aa.y, bb.z);
        c = HSV2RGB(c);
        return c;
    }

    // dissolve

    // normal
}
