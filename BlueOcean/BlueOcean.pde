
// Blue Ocean       Genetic Algorithm

color c1 = color(0,174,239);
color c2 = color(255,255,255);
color c3 = color(255,242,0);
color c4 = color(0,114,188);

int perfectNum = 0;
float perfectRate = 0;

DNA[] pop;
//DNA[] PA;
//DNA[] PB;
//DNA[] T;

//int[] isRep;
int[] tx;
int[] ty;

ArrayList<DNA> matingPool;
int totalPop = 16;          // it's an even number
float mutationRate;
int target = 48;     // (5+1) * 8

void setup(){
  size(768,768);
  background(0,174,239);
  noStroke();
  
  frameRate(10);
  
  mutationRate = 0.01;
  
  pop = new DNA[totalPop];             // *** intiliazation for group
  tx = new int[totalPop]; 
  ty = new int[totalPop]; 
  for(int i=0;i<pop.length;i++){
    pop[i] = new DNA(); 
    //PA[i] = new DNA();
    //PB[i] = new DNA();
    //T[i] = new DNA();
    //isRep[i] = 0;
    tx[i] = 0;
    ty[i] = 0;
  }
}

void draw(){
  fill(c1);
  rect(0,0,width,height);
  
  perfectNum = 0;
  for(int i=0; i<pop.length; i++){             // ~~ Visualization
    for(int j=0; j<pop[i].genes.length; j++){  
      int g = pop[i].getGenes(j);
      switch(g){
        case 1:
          fill(c1);
          break;
        case 2:
          fill(c2);
          break;
        case 3:
          fill(c3);
          break;
        case 5:
          perfectNum += 1;
          fill(c4);
          break;
      }
      rect(pop[i].getPosX(j)+tx[i],pop[i].getPosY(j)+ty[i],24,24);
    }
    perfectRate = float(perfectNum) / float(pop.length * pop[i].genes.length);
  }
  println("Perfect Num :",perfectNum,"  Perfect Rate :",perfectRate );
  if (perfectRate > 0.99){ noLoop();}              // [STOP]
  
  for(int i=0; i<pop.length; i++){     // *** selection
   pop[i].fitness();                      // * mating rate
  }
  
  ArrayList<DNA> matingPool = new ArrayList<DNA>();
  
  for ( int i = 0; i < pop.length; i++){  // * mating pool
    int n = int(pop[i].fitness * 100);
    pop[i].setID(i);
    for ( int j = 0; j < n; j++){
     matingPool.add(pop[i]); 
    }
  }
  
  for(int i= 0;i < pop.length; i++){      // *** reproduction
    int a = int(random(matingPool.size()));
    int b = 0;
    int m = int(random(matingPool.size()-1));
    if(m < a){
      b = int(random(a));
    }else{
      b = int(random(matingPool.size()-a)) + a;
    }

    DNA parentA = matingPool.get(a);          // * crossover
    DNA parentB = matingPool.get(b);
    
    int tpax = parentA.getPosX(0);
    int tpay = parentA.getPosY(0);
    int tpbx = parentB.getPosX(0);
    int tpby = parentB.getPosY(0);
    
    if(tpax != tpbx || tpay != tpby){
      int tmpIdA = parentA.getID();
      int tmpIdB = parentB.getID();
      if(pop[tmpIdA].getPosX(0) < pop[tmpIdB].getPosX(0)){
       tx[tmpIdA] += 24;
       if(pop[tmpIdA].getPosX(0) < pop[tmpIdB].getPosX(0)){
          tx[tmpIdB] -= 24;
       }
      }else if(pop[tmpIdA].getPosX(0) > pop[tmpIdB].getPosX(0)){
       tx[tmpIdA] -= 24;
       if(pop[tmpIdA].getPosX(0) > pop[tmpIdB].getPosX(0)){
          tx[tmpIdB] += 24;
       }
      }
      if(pop[tmpIdA].getPosY(0) < pop[tmpIdB].getPosY(0)){
       ty[tmpIdA] += 24;
       if(pop[tmpIdA].getPosY(0) < pop[tmpIdB].getPosY(0)){
          ty[tmpIdB] -= 24;
       }
      }else if(pop[tmpIdA].getPosY(0) > pop[tmpIdB].getPosY(0)){
       ty[tmpIdA] -= 24;
       if(pop[tmpIdA].getPosY(0) > pop[tmpIdB].getPosY(0)){
          ty[tmpIdB] += 24;
       }
      }
    }else{
      DNA child = parentA.crossover(parentB);
      child.mutate(mutationRate);               // * mutation
      pop[i] = child;            // update population
      tx[i] = 0;
      ty[i] = 0;
      //isRep[i] = 1;
    }
  }
}