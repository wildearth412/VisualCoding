using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
	Vector3 location;
	Vector3 velocity;
	Vector3 acceleration;

	Vector3 randomGoal;

	float r;    // for size
	float maxSpeed;
	float maxForce;

	int status;
	int timer;

    GameObject go;


	public Boid(float x,float y,float z) 
	{
		acceleration = new Vector3(0,0,0);
		velocity = new Vector3(0,0,0);
		location = new Vector3 (x, y, z);
		randomGoal = new Vector3(Random.value*1200.0f,Random.value*1200.0f,Random.value*1200.0f);
		r = 4.5f;
		maxSpeed = 2.0f;
		maxForce = .1f;
		status = 0;
		timer = 0;
        go = null;
	}

	void UpdateBoidAttr ()       // motion model
	{
		velocity += acceleration;
		Vector3.ClampMagnitude (velocity, maxSpeed);
		location += velocity;
		acceleration *= 0;
	}

	void ApplyForce (Vector3 force)
	{
		acceleration += force;
	}

	void MouseBehavior(List <Boid> boids,Vector3 v)
	{
		Vector3 separate = Separate (boids);
        Vector3 seek = Seek(v);

		separate *= 1.5f;
		seek *= 0.5f;

		ApplyForce (separate);
		ApplyForce (seek);

		//Arrive (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
        Arrive(v);
    }

	void RandomSeek(List<Boid> boids)
	{
		if (timer >= 1200) {
			randomGoal = Random.insideUnitSphere * 100.0f;
			timer = 0;
		}
		else 
		{
			timer++;
		}

		Vector3 separate = Separate (boids);
		Vector3 seek = Seek(randomGoal);

		separate *= 2.0f;
		seek *= 0.5f;

		ApplyForce (separate);
		ApplyForce (seek);

		Arrive (randomGoal);
	}

	void RandomSeek2(List<Boid> boids)
	{
		Vector3 separate = Separate (boids);
		Vector3 seek = Seek(Random.insideUnitSphere * 100.0f);

		separate *= 1.5f;
		seek *= Mathf.PerlinNoise((float)Time.frameCount,(float)Time.frameCount);

		ApplyForce (separate);
		ApplyForce (seek);

	}

	Vector3 Seek(Vector3 target)
	{
		Vector3 desired = target - location;
		desired.Normalize ();
		desired *= maxSpeed;

		Vector3 steer = desired - velocity;
		Vector3.ClampMagnitude (steer, maxForce);

		return steer;
	}

	void Arrive(Vector3 target)
	{
		Vector3 desired = target - location;
		float d = desired.magnitude;
		desired.Normalize ();

		if (d < 100) {
			float m = Mathf.Lerp (0, maxSpeed, Mathf.InverseLerp (0, 100.0f, d));
			desired *= m;
		}
		else
		{
			desired *= maxSpeed;
		}

		Vector3 steer = desired - velocity;
		Vector3.ClampMagnitude (steer,maxForce);

		ApplyForce (steer);                  // apply steer to acceleration
	}

	void Flocking(List<Boid> boids)
	{
		Vector3 sep = Separate (boids);
		Vector3 ali = Align (boids);
		Vector3 coh = Cohesion (boids);

		sep *= 1.5f;
		ali *= 1.0f;
		coh *= 1.0f;

		ApplyForce (sep);
		ApplyForce (ali);
		ApplyForce (coh);
	}

	Vector3 Separate(List<Boid> boids)
	{
		float desiredSeparate = r * 5.0f;
		Vector3 sum = new Vector3 ();
		Vector3 steer = new Vector3 ();
		int count = 0;

		foreach (Boid other in boids) 
		{
			float d = Vector3.Distance (location, other.location);
			if ((d > 0) && (d < desiredSeparate)) 
			{
				Vector3 diff = location - other.location;
				diff.Normalize ();
				sum += diff;
				count++;
			}
		}

		if (count > 0) 
		{
			sum /= (float)count;
			sum.Normalize ();
			sum *= maxSpeed;

			steer = sum - velocity;
			Vector3.ClampMagnitude (steer, maxForce);
		}

		return steer;
	}

	Vector3 Align(List<Boid> boids)
	{
		float neighborDist = 100.0f;
		Vector3 sum = new Vector3 ();
		int count = 0;

		foreach (Boid other in boids) 
		{
			float d = Vector3.Distance (location, other.location);
			if ((d > 0) && (d < neighborDist)) 
			{
				sum += other.velocity;
				count++;
			}
		}

		if (count > 0) 
		{
			sum /= (float)count;
			sum.Normalize ();
			sum *= maxSpeed;

			Vector3 steer = sum - velocity;
			Vector3.ClampMagnitude (steer, maxForce);

			return steer;
		} 
		else 
		{
			return new Vector3 (0, 0, 0);
		}
	}

	Vector3 Cohesion(List<Boid> boids)
	{
		float neighborDist = 100.0f;
		Vector3 sum = new Vector3 ();
		int count = 0;

		foreach (Boid other in boids) 
		{
			float d = Vector3.Distance (location, other.location);
			if ((d > 0) && (d < neighborDist)) 
			{
				sum += other.location;
				count++;
			}
		}

		if (count > 0) 
		{
			sum /= (float)count;
			return Seek(sum);
		} 
		else 
		{
			return new Vector3 (0, 0, 0);
		}
	}

	public void SetInput()
	{
		status += 1;
		if (status > 2) 
		{
			status = 0;
		}
	}

    public void InitializeBoid(GameObject prefab)
    {
        go = (GameObject)Instantiate(prefab, location, Quaternion.FromToRotation(Vector3.up, velocity));
    }

    public void UpdateBoid()
    {
        go.transform.position = location;
        go.transform.rotation = Quaternion.FromToRotation(Vector3.up, velocity);
    }

    public void RunBoid(List<Boid> boids,Vector3 v)
	{
		switch (status) 
		{
			case 0:
				Flocking (boids);
				MouseBehavior (boids,v);
				break;
			case 1:
				RandomSeek (boids);
				break;
			case 2:
				RandomSeek2 (boids);
				break;
		}
		UpdateBoidAttr ();
        UpdateBoid();
	}

}
