using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock
{
    List<Boid> boids;

    public Flock()
    {
        boids = new List<Boid>();
    }

	public void Input ()
    {
		foreach(Boid b in boids)
        {
            b.SetInput();
        }
	}

    public void Initialize(GameObject prefab)
    {
        foreach (Boid b in boids)
        {
            b.InitializeBoid(prefab);
        }
    }
	
	public void Run (Vector3 v)
    {
		foreach(Boid b in boids)
        {
            b.RunBoid(boids,v);
        }
	}

    public void AddBoid(Boid b)
    {
        boids.Add(b);
    }
}
