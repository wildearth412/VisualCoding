
Flock flock;

void setup()
{
  size(1200,400);
  flock = new Flock();
  for(int i = 0;i < 100;i++)
  {
    Boid b = new Boid(random(width),random(height));
    flock.AddBoid(b);
  }
}

void draw()
{
  background(92,150,245);
  flock.Run();
}

void mouseClicked()
{
  flock.Input(); 
}