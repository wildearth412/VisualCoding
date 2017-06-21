using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shaderInitialize : MonoBehaviour {

	public GameObject go;
	private Renderer render;

	// Use this for initialization
	void Start () {
		this.render = go.GetComponent<Renderer>();

		float d;
		d = this.render.sharedMaterial.GetFloat ("_d");

		float[] vals = new float[100];
		for (int i = 1; i <= 100; i++) {
			vals [i] = 0;
		}
		this.render.sharedMaterial.SetFloatArray ("buf0",vals);
	}

//	// Update is called once per frame
//	void Update () {
//		// 経過時間に応じて値が変わるよう適当な値を入れる
//		float r = Mathf.Sin(Time.time);
//		float g = Mathf.Sin(Time.time) + Mathf.Cos(Time.time);
//		float b = Mathf.Cos(Time.time);
//
//		// Materialクラスの`Set****`メソッドを使ってシェーダに値を送信
//		this.material.SetColor("_Color", new Color(r, g, b, 1.0f));
//	}
}
