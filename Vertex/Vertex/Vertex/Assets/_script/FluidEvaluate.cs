using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidEvaluate : MonoBehaviour {

	public GameObject go;
	private Renderer render;
    Vector3[] buf0 = new Vector3[10000];
    Vector3[] buf1 = new Vector3[10000];

    void Start () {
		this.render = go.GetComponent<Renderer>();

		float c,d,t,mu,k1,k2,k3;
        c = this.render.sharedMaterial.GetFloat("_c");
        d = this.render.sharedMaterial.GetFloat("_d");
        t = this.render.sharedMaterial.GetFloat("_t");
        mu = this.render.sharedMaterial.GetFloat("_mu");

        float f1 = c * c * t * t / (d * d);
        float f2 = 1.0f / (mu * t + 2.0f);
        k1 = (4.0f - 8.0f * f1) * f2;
        k2 = (mu * t - 2.0f) * f2;
        k3 = 2.0f * f1 * f2;

        this.render.sharedMaterial.SetFloat("k1", k1);
        this.render.sharedMaterial.SetFloat("k2", k2);
        this.render.sharedMaterial.SetFloat("k3", k3);

		for (int j = 0; j < 100; j++) {
            float y = d * j;
            for (int i = 0; i < 100; i++)
            {
                buf0[j*i] = buf1[j*i] = new Vector3(d * i, y, 0);
            }
		}
        float[] vals = new float[10000 * 3];
        for (int j = 0; j < 10000; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                //if (i == 0) { vals[j * i] = valsV[j].x; }
                //if (i == 1) { vals[j * i] = valsV[j].y; }
                //if (i == 2) { vals[j * i] = valsV[j].z; }
            }
        }
        this.render.sharedMaterial.SetFloatArray ("bufC", vals);
        this.render.sharedMaterial.SetFloatArray ("bufP", vals);
    }

    void Update()
    {

    }
}
