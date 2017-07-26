using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HSVController : MonoBehaviour
{
    [SerializeField]
    private Renderer layerColorRender;
    [SerializeField]
    private Slider sliderH;
    [SerializeField]
    private Slider sliderS;
    [SerializeField]
    private Slider sliderV;


    void Start () {
        this.layerColorRender.sharedMaterial.SetInt("_Hfix", 0);
        this.layerColorRender.sharedMaterial.SetInt("_Sfix", 0);
        this.layerColorRender.sharedMaterial.SetInt("_Vfix", 0);
    }
	
	void Update () {

    }

    float GetMat(string m)
    {
        float val = this.layerColorRender.sharedMaterial.GetFloat(m);
        return val;
    }

    void SetMat(string m, float n)
    {
        this.layerColorRender.sharedMaterial.SetFloat(m, n);
    }

    public void SetHue()
    {
        if (sliderH.isActiveAndEnabled) SetMat("_Hfix", sliderH.value);
    }

    public void SetSaturation()
    {
        if (sliderH.isActiveAndEnabled) SetMat("_Sfix", sliderS.value);
    }

    public void SetBrightness()
    {
        if (sliderH.isActiveAndEnabled) SetMat("_Vfix", sliderV.value);
    }
}
