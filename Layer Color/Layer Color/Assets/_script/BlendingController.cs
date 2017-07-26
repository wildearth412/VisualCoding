using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlendingController : MonoBehaviour
{
    [SerializeField]
    private Renderer layerColorRender;
    private float visibility1;
    private float visibility2;
    [SerializeField]
    private Toggle tog1;
    [SerializeField]
    private Toggle tog2;
    [SerializeField]
    private Dropdown blendModeDropdown;


    void Start()
    {
        this.layerColorRender.sharedMaterial.SetFloat("_Transparency1", 1.0f);
        this.layerColorRender.sharedMaterial.SetFloat("_Transparency2", 1.0f);
        this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 0);
    }

    void Update()
    {

    }

    float GetMat(string m)
    {
        float val = this.layerColorRender.sharedMaterial.GetFloat(m);
        return val;
    }

    void SetMat(string m,float n)
    {
        this.layerColorRender.sharedMaterial.SetFloat(m,n);
    }

    public void SetTransparency1(float t)
    {
        SetMat("_Transparency1", t);
        visibility1 = GetMat("_Transparency1");
        if(!tog1.isOn) SetMat("_Transparency1", 0);
    }

    public void SetTransparency2(float t)
    {
        SetMat("_Transparency2", t);
        visibility2 = GetMat("_Transparency2");
        if (!tog2.isOn) SetMat("_Transparency2", 0);
    }

    public void SetVisibility1(bool b)
    {
        if (b)
        {
            SetMat("_Transparency1", visibility1);
        }
        else
        {
            visibility1 = GetMat("_Transparency1");
            SetMat("_Transparency1", 0);
        }
    }

    public void SetVisibility2(bool b)
    {
        if (b)
        {
            SetMat("_Transparency2", visibility2);
        }
        else
        {
            visibility2 = GetMat("_Transparency2");
            SetMat("_Transparency2", 0);
        }
    }

    /*
 #region BlendMode

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

 // normalize
 Vector3 Normalize(Vector3 i)
 {
     Vector3 n = i / 255.0f;
     return n;
 }

 // result check
 Vector3 ResultCheck(Vector3 c)
 {
     if (c.x < 0) c.x = 0;
     if (c.y < 0) c.y = 0;
     if (c.z < 0) c.z = 0;
     if (c.x > 225.0f) c.x = 225.0f;
     if (c.y > 225.0f) c.y = 225.0f;
     if (c.z > 225.0f) c.z = 225.0f;
     return c;
 }

 // ~~~~~~~~~~~~~~~~~~~~~~~~~ Layer Color Mode ~~~~~~~~~~~~~~~~~~~~~~~~~
 // a 0-255 down layer;  b 0-255 up layer;  d 0-1 transparency
 // opacity
 Vector3 Opacity(Vector3 a, Vector3 b, float d)
 {
     Vector3 c = d * a + (1f - d) * b;
     return c;
 }

 // normal
 //Vector3 Normal(Vector3 a, Vector3 b)
 //{	}

 // dissolve
 Vector3 Dissolve(Vector3 a, Vector3 b, Vector3 i)
 {
     Vector3 c;
     c.x = a.x * (Mathf.Abs(Mathf.Sin(i.x))) + b.x * (1.0f - (Mathf.Abs(Mathf.Sin(i.x))));
     c.y = a.y * (Mathf.Abs(Mathf.Sin(i.x + i.y))) + b.y * (1.0f - (Mathf.Abs(Mathf.Sin(i.x + i.y))));
     c.z = a.z * (Mathf.Abs(Mathf.Sin(i.y))) + b.z * (1.0f - (Mathf.Abs(Mathf.Sin(i.y))));
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
     Vector3 c = Vector3.one * 255f - Vector3.Scale( (Vector3.one * 255f - a), (Vector3.one * 255f - b) )/ 255f;
     return c;
 }

 // color burn
 Vector3 ColorBurn(Vector3 a, Vector3 b)
 {
     if (b.x == 0) { b.x = 1.0f; }
     if (b.y == 0) { b.y = 1.0f; }
     if (b.z == 0) { b.z = 1.0f; }
     Vector3 c = a - Vector3.Scale( Vector3.Scale( (Vector3.one * 255f - a), (Vector3.one * 255f - b) ) , new Vector3(1.0f / b.x, 1.0f / b.y, 1.0f / b.z));
     return c;
 }

 // color dodge
 Vector3 ColorDodge(Vector3 a, Vector3 b)
 {
     if (b.x == 255f) b.x = 254f;
     if (b.y == 255f) b.y = 254f;
     if (b.z == 255f) b.z = 254f;
     Vector3 c = a + Vector3.Scale( Vector3.Scale(a, b), new Vector3(1.0f / (255.0f - b.x), 1.0f / (255.0f - b.y), 1.0f / (255.0f - b.z) ) );
     return c;
 }

 // linear burn
 Vector3 LinearBurn(Vector3 a, Vector3 b)
 {
     Vector3 c = a + b - Vector3.one * 255f;
     return c;
 }

 // linear dodge
 Vector3 LinearDodge(Vector3 a , Vector3 b)
 {
     Vector3 c = a + b;
     return c;
 }

 // add
 Vector3 Add(Vector3 a, Vector3 b)
 {
     Vector3 c = (a + b) / 4.0f + Vector3.one * 128f;
     return c;
 }

 // remove
 Vector3 Remove(Vector3 a, Vector3 b)
 {
     Vector3 c = (a - b) / 2.0f + Vector3.one * 128f;
     return c;
 }

 // overlay
 Vector3 Overlay(Vector3 a, Vector3 b)
 {
     Vector3 c = Vector3.zero;
     if (a.x <= 128.0f) c.x = a.x * b.x / 128.0f;
     else if (a.x > 128.0f) c.x = 225.0f - (225.0f - a.x) * (225.0f - b.x) / 128.0f;
     if (a.y <= 128.0f) c.y = a.y * b.y / 128.0f;
     else if (a.y > 128.0f) c.y = 225.0f - (225.0f - a.y) * (225.0f - b.y) / 128.0f;
     if (a.z <= 128.0f) c.z = a.z * b.z / 128.0f;
     else if (a.z > 128.0f) c.z = 225.0f - (225.0f - a.z) * (225.0f - b.z) / 128.0f;
     return c;
 }

 // hard light
 Vector3 HardLight(Vector3 a, Vector3 b)
 {
     Vector3 c = Vector3.zero;
     if (b.x <= 128.0f) { c.x = a.x * b.x / 128.0f; }
     else if (b.x > 128.0f) { c.x = 225.0f - (225.0f - a.x) * (225.0f - b.x) / 128.0f; }
     if (b.y <= 128.0f) { c.y = a.y * b.y / 128.0f; }
     else if (b.y > 128.0f) { c.y = 225.0f - (225.0f - a.y) * (225.0f - b.y) / 128.0f; }
     if (b.z <= 128.0f) { c.z = a.z * b.z / 128.0f; }
     else if (b.z > 128.0f) { c.z = 225.0f - (225.0f - a.z) * (225.0f - b.z) / 128.0f; }
     return c;
 }

 // soft light
 Vector3 SoftLight(Vector3 a, Vector3 b)
 {
     Vector3 c = Vector3.zero;
     if (b.x <= 128.0f) { c.x = a.x * b.x / 128.0f + (a.x / 225.0f) * (a.x / 225.0f) * (225.0f - 2.0f * b.x); }
     else if (b.x > 128.0f) { c.x = a.x * (225.0f - b.x) / 128.0f + Mathf.Pow((a.x / 225.0f), 0.5f) * (2.0f * b.x - 225.0f); }
     if (b.y <= 128.0f) { c.y = a.y * b.y / 128.0f + (a.y / 225.0f) * (a.y / 225.0f) * (225.0f - 2.0f * b.y); }
     else if (b.y > 128.0f) { c.y = a.y * (225.0f - b.y) / 128.0f + Mathf.Pow((a.y / 225.0f), 0.5f) * (2.0f * b.y - 225.0f); }
     if (b.z <= 128.0f) { c.z = a.z * b.z / 128.0f + (a.z / 225.0f) * (a.z / 225.0f) * (225.0f - 2.0f * b.z); }
     else if (b.z > 128.0f) { c.z = a.z * (225.0f - b.z) / 128.0f + Mathf.Pow((a.z / 225.0f), 0.5f) * (2.0f * b.z - 225.0f); }
     return c;
 }

 // vivid light
 Vector3 VividLight(Vector3 a, Vector3 b)
 {
     Vector3 c = Vector3.zero;
     if (b.x <= 128.0f) c.x = a.x - (225.0f - a.x) * (225.0f - 2.0f * b.x) / (2.0f * b.x);
     else if (b.x > 128.0f) c.x = a.x + a.x * (2.0f * b.x - 225.0f) / (2.0f * (225.0f - b.x));
     if (b.y <= 128.0f) c.y = a.y - (225.0f - a.y) * (225.0f - 2.0f * b.y) / (2.0f * b.y);
     else if (b.y > 128.0f) c.y = a.y + a.y * (2.0f * b.y - 225.0f) / (2.0f * (225.0f - b.y));
     if (b.z <= 128.0f) c.z = a.z - (225.0f - a.z) * (225.0f - 2.0f * b.z) / (2.0f * b.z);
     else if (b.z > 128.0f) c.z = a.z + a.z * (2.0f * b.z - 225.0f) / (2.0f * (225.0f - b.z));
     return c;
 }

 // linear light
 Vector3 LinearLight(Vector3 a,Vector3 b)
 {
     Vector3 c = a + 2 * b - Vector3.one * 255.0f;
     return c;
 }

 // pin light
 Vector3 PinLight(Vector3 a, Vector3 b)
 {
     Vector3 c = Vector3.zero;
     if (b.x <= 128.0f) c.x = Mathf.Min(a.x, 2.0f * b.x);
     else if (b.x > 128.0f) c.x = Mathf.Min(a.x, 2.0f * b.x - 225.0f);
     if (b.y <= 128.0f) c.y = Mathf.Min(a.y, 2.0f * b.y);
     else if (b.y > 128.0f) c.y = Mathf.Min(a.y, 2.0f * b.y - 225.0f);
     if (b.z <= 128.0f) c.z = Mathf.Min(a.z, 2.0f * b.z);
     else if (b.z > 128.0f) c.z = Mathf.Min(a.z, 2.0f * b.z - 225.0f);
     return c;
 }

 // hard mix
 Vector3 HardMix(Vector3 a,Vector3 b)
 {
     Vector3 c = Vector3.zero;
     if (a.x + b.x >= 255.0f) { c.x = 255; }
     else if (a.x + b.x < 255.0f) { c.x = 0; }
     if (a.y + b.y >= 255.0f) { c.y = 255; }
     else if (a.y + b.y < 255.0f) { c.y = 0; }
     if (a.z + b.z >= 255.0f) { c.z = 255; }
     else if (a.z + b.z < 255.0f) { c.z = 0; }
     return c;
 }

 // difference
 Vector3 Difference(Vector3 a, Vector3 b)
 {
     Vector3 c = a - b;
     c = new Vector3(Mathf.Abs(c.x), Mathf.Abs(c.y), Mathf.Abs(c.z));
     return c;
 }

 // exclusion
 Vector3 Exclusion(Vector3 a, Vector3 b)
 {
     Vector3 c = a + b - (Vector3.Scale(a , b) / 128.0f);
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
 #endregion
 */

    public void SetBlendMode()
    {
        switch (blendModeDropdown.value)
        {
            case 0:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 0);
                break;
            case 1:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 1.0f);
                break;
            case 2:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 2.0f);
                break;
            case 3:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 3.0f);
                break;
            case 4:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 4.0f);
                break;
            case 5:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 5.0f);
                break;
            case 6:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 6.0f);
                break;
            case 7:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 7.0f);
                break;
            case 8:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 8.0f);
                break;
            case 9:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 9.0f);
                break;
            case 10:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 10.0f);
                break;
            case 11:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 11.0f);
                break;
            case 12:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 12.0f);
                break;
            case 13:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 13.0f);
                break;
            case 14:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 14.0f);
                break;
            case 15:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 15.0f);
                break;
            case 16:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 16.0f);
                break;
            case 17:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 17.0f);
                break;
            case 18:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 18.0f);
                break;
            case 19:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 19.0f);
                break;
            case 20:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 20.0f);
                break;
            case 21:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 21.0f);
                break;
            case 22:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 22.0f);
                break;
            case 23:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 23.0f);
                break;
            case 24:
                this.layerColorRender.sharedMaterial.SetFloat("_BlendMode", 24.0f);
                break;
        }
     }
}
