using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_01 : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 birthRange;
    Flock flock;

	void Start ()
    {
        flock = new Flock();
        for(int i=0; i<100; i++)
        {
            Boid b = new Boid(Random.value * birthRange.x, Random.value * birthRange.y, Random.value * birthRange.z);
            flock.AddBoid(b);
        }
        flock.Initialize(prefab);
	}
	
	void Update ()
    {
        Camera camera = GetComponent<Camera>();
        Vector3 v = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 500.0f);
        v = camera.ScreenToWorldPoint(v);
        flock.Run(v);

        if(Input.GetMouseButtonDown(0))
        {
            flock.Input();
        }
	}
}
