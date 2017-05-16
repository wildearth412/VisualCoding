class Boid
{
  PVector location;
  PVector velocity;
  PVector acceleration;
  
  PVector randomGoal;
  
  float r;     // for size
  float maxSpeed;
  float maxForce;
  
  int status;
  int timer;
  
  Boid(float x,float y)
  {
    acceleration = new PVector(0,0);
    velocity = new PVector(0,0);
    location = new PVector(x,y);
    randomGoal = new PVector(random(width),random(height));
    r = 4.5;
    maxSpeed = 4.;
    maxForce = .1;
    status = 0;
    timer = 0;
  }
  
  void Update()         // motion model
  {
    velocity.add(acceleration);
    velocity.limit(maxSpeed);
    location.add(velocity);
    acceleration.mult(0);
  }
  
  void ApplyForce(PVector force)
  {
    acceleration.add(force); 
  }
  
  void MouseBehavior(ArrayList<Boid> boids)
  {
    PVector separate = Separate(boids);
    PVector seek = Seek(new PVector(mouseX,mouseY));
    
    separate.mult(1.5);
    seek.mult(0.5);
    
    ApplyForce(separate);
    ApplyForce(seek);
    
    Arrive(new PVector(mouseX,mouseY));
  }
  
  void RandomSeek(ArrayList<Boid> boids)
  {
    if(timer >= 120)
    {
      randomGoal = new PVector(random(width),random(height));
      timer = 0;
      //println(randomGoal);
    }
    else
    {
      timer++; 
    }
    PVector separate = Separate(boids);
    PVector seek = Seek(randomGoal);
    
    separate.mult(2.);
    seek.mult(.5);
    
    ApplyForce(separate);
    ApplyForce(seek);
    
    Arrive(randomGoal);
  }
  
  void RandomSeek2(ArrayList<Boid> boids)
  {
    PVector separate = Separate(boids);
    PVector seek = Seek(new PVector(random(width),random(height)));
    
    separate.mult(1.5);
    seek.mult(noise(frameCount*0.05));
    
    ApplyForce(separate);
    ApplyForce(seek);
    
  }
  
  PVector Seek(PVector target)
  {
    PVector desired = PVector.sub(target,location);
    desired.normalize();
    desired.mult(maxSpeed);
    
    PVector steer = PVector.sub(desired,velocity);
    steer.limit(maxForce);  
    
    return steer;
  }
  
  void Arrive(PVector target)
  {
    PVector desired = PVector.sub(target,location);

    float d = desired.mag();
    desired.normalize();
    
    if(d<100)
    {
      float m = map(d,0,100,0,maxSpeed);
      desired.mult(m);
    }
    else
    {
      desired.mult(maxSpeed); 
    }
    
    PVector steer = PVector.sub(desired,velocity);
    steer.limit(maxForce);  
    
    ApplyForce(steer);     // apple steer to acceleration
  }
  
  void Flocking (ArrayList<Boid> boids)
  {
    PVector sep = Separate(boids); 
    PVector ali = Align(boids);
    PVector coh = Cohesion(boids);
    
    sep.mult(1.5);
    ali.mult(1.0);
    coh.mult(1.0);
    
    ApplyForce(sep);
    ApplyForce(ali);
    ApplyForce(coh);
  }
  
  PVector Separate(ArrayList<Boid> boids)
  {
     float desiredSeparate = r*5.;
     PVector sum = new PVector();
     PVector steer = new PVector(0,0);
     int count = 0;
     
     for(Boid other : boids)
     {
       float d = PVector.dist(location,other.location);
       if((d > 0) && (d < desiredSeparate))
       {
         PVector diff = PVector.sub(location,other.location);
         diff.normalize();
         sum.add(diff);
         count++;
       }
     }
     
     if(count > 0)
     {
       sum.div(count);
       
       sum.setMag(maxSpeed);
       
       steer = PVector.sub(sum,velocity);
       steer.limit(maxForce);     
     }
     
     return steer;
  }
  
  PVector Align(ArrayList<Boid> boids)
  {
    float neighborDist = 100.;
    PVector sum = new PVector(0,0);
    int count = 0;
    
    for(Boid other : boids)
    {
      float d = PVector.dist(location,other.location);
      if ((d > 0) && (d < neighborDist))
      {
        sum.add(other.velocity); 
        count++;
      }
    }
    
    if(count > 0)
    {
      sum.div(count);
      
      sum.setMag(maxSpeed);
      
      PVector steer = PVector.sub(sum,velocity);
      steer.limit(maxForce);
      return steer;
    }
    else
    {
      return new PVector(0,0); 
    }
  }
  
  PVector Cohesion(ArrayList<Boid> boids)
  {
    float neighborDist = 100.;
    PVector sum = new PVector(0,0);
    int count = 0;
    
    for(Boid other : boids)
    {
      float d = PVector.dist(location,other.location);
      if((d > 0)&&(d < neighborDist))
      {
        sum.add(other.location);
        count++;
      }
    }
    
    if(count > 0)
    {
      sum.div(count);
      return Seek(sum);
    }
    else
    {
      return new PVector(0,0);
    }
  }
  
  void Display()
  {
    float theta = velocity.heading() + PI/2.; 
    fill(255,255,255,100);
    noStroke();
    
    pushMatrix();
    translate(location.x,location.y);
    rotate(theta);
    beginShape();
    vertex(0,-r*1.25);
    vertex(-r,0);
    vertex(0,r*2.);
    vertex(-r*.75,r*3.);
    vertex(r*.75,r*3.);
    vertex(0,r*2.);
    vertex(r,0);
    endShape(CLOSE);
    popMatrix();
  }
  
  void SetInput()
  {
     status += 1;
     if(status > 2)
     {
       status = 0; 
     }
  }
  
  void RunBoid(ArrayList<Boid> boids)
  {
    //Flocking(boids);
    switch(status)
    {
      case 0:
        Flocking(boids);
        MouseBehavior(boids);
        break;
      case 1:
        RandomSeek(boids);
        break;
      case 2:
        RandomSeek2(boids);
        break;
    }
    Update();
    Display();
  }

}