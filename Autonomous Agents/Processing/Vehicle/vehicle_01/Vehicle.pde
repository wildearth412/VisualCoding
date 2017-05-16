class Vehicle
{
  PVector location;
  PVector velocity;
  PVector acceleration;
  
  float r;     // for size
  float maxSpeed;
  float maxForce;
  
  Vehicle(float x,float y)
  {
    acceleration = new PVector(0,0);
    velocity = new PVector(0,0);
    location = new PVector(x,y);
    r = 3.;
    maxSpeed = 4.;
    maxForce = .1;
  }
  
  void Update()         // motion model
  {
    velocity.add(acceleration);
    velocity.limit(maxSpeed);
    location.add(velocity);
    acceleration.mult(0);
  }
  
  void applyForce(PVector force)
  {
    acceleration.add(force); 
  }
  
  void Seek(PVector target)
  {
    PVector desired = PVector.sub(target,location);
    desired.normalize();
    desired.mult(maxSpeed);
    
    PVector steer = PVector.sub(desired,velocity);
    steer.limit(maxForce);
    
    applyForce(steer);
  }
  
  void Display()
  {
    float theta = velocity.heading() + PI/2.; 
    fill(175);
    stroke(0);
    
    pushMatrix();
    translate(location.x,location.y);
    rotate(theta);
    beginShape();
    vertex(0,-r*2.);
    vertex(-r,r*2.);
    vertex(r,r*2.);
    endShape(CLOSE);
    popMatrix();
  }

}