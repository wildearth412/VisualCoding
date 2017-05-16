class Flock
{
  ArrayList<Boid> boids;
  
  Flock()
  {
    boids = new ArrayList<Boid>(); 
  }
  
  void Input()
  {
    for( Boid b : boids)
    {
      b.SetInput();  
    }
  }
  
  void Run()
  {
    for( Boid b : boids)
    {
      b.RunBoid(boids); 
    }
  }
  
  void AddBoid(Boid b)
  {
    boids.add(b); 
  }
}