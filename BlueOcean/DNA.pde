
class DNA{
  
  int[] genes = new int[8];
  float fitness;
  int[] posX = new int[8];
  int[] posY = new int[8];
  int id = 0;
  
  DNA(){                                  // create DNA
    posX[0] = int(random(4,29))*24;
    posY[0] = int(random(4,29))*24;
    
   for(int i=0;i<genes.length;i++){
    int sc = (int)random(0,4);
    switch(sc){
      case 0 :
       genes[i] = 1;
       break;
      case 1 :
       genes[i] = 2;
       break;
      case 2:
       genes[i] = 3;
       break;
      case 3:
       genes[i] = 5;
       break;
    }
    if(i!=0){
      int rr = (int)random(1,9);
      switch(rr){
        case 1:
          posX[i]= posX[i-1] + 24;
          posY[i]= posY[i-1] + 24;
          break;
        case 2:
          posX[i]= posX[i-1] + 24;
          posY[i]= posY[i-1] - 24;
          break;
        case 3:
          posX[i]= posX[i-1] + 24;
          posY[i]= posY[i-1];
          break;
        case 4:
          posX[i]= posX[i-1] - 24;
          posY[i]= posY[i-1] + 24;
          break;
        case 5:
          posX[i]= posX[i-1] - 24;
          posY[i]= posY[i-1] - 24;
          break;
        case 6:
          posX[i]= posX[i-1] - 24;
          posY[i]= posY[i-1];
          break;
        case 7:
          posX[i]= posX[i-1];
          posY[i]= posY[i-1] + 24;
          break;
        case 8:
          posX[i]= posX[i-1];
          posY[i]= posY[i-1] - 24;
          break;  
      }
    }
   }
  }
  
  void fitness(){                      // calculate fieness
    int score = 0;
    for( int i = 0; i < genes.length; i++){
      score += genes[i];
      if(genes[i] == 5) score += 1;
      //println(score);
    }
    fitness = (float)score/target;
  }
  
  //DNA crossover(DNA partner){           // crossover
  //  DNA child = new DNA();
  //  int midpoint = int(random(genes.length));
  //  for (int i= 0; i < genes.length; i++){
  //    if(i > midpoint) child.genes[i] = genes[i];
  //    else child.genes[i] = partner.genes[i];
  //  }
  //  return child;
  //}
  
  DNA crossover(DNA partner){              // crossover
  
     int seed1 = int(random(0,genes.length));
     int seed2 = int(random(0,genes.length));
     if (seed2 == seed1){ 
       seed2 += 1; 
       if(seed2 >= genes.length){ seed2 = 0;}
     }
     
     DNA child = new DNA();                     // interaction
     for (int i= 0; i < genes.length; i++){
       if(i == seed1 || i == seed2){
         int mat = genes[i] * partner.genes[i];
         switch (mat){
           case 2:
             child.genes[i] = 3; break;
           case 15:
             child.genes[i] = 5; break;
           case 10:
             child.genes[i] = 3; break;
           case 5:
             child.genes[i] = 2; break;
           case 6:
             child.genes[i] = 2; break;
           case 3:
             child.genes[i] = 1; break;
           case 1:
             child.genes[i] = 3; break;
           case 4:
             child.genes[i] = 3; break;
           case 9:
             child.genes[i] = 5; break;
           case 25:
             child.genes[i] = 5; break;
         }
       }else{    
         int midpoint = int(random(2,genes.length));       // inherit
         if(i > midpoint) child.genes[i] = genes[i];
         else child.genes[i] = partner.genes[i];
       }
     }
     
     return child;
  }
    
  void mutate(float mutationRate){          // mutation
    for (int i = 0;i < genes.length; i++){
      if (random(1) < mutationRate){
        int scr = (int)random(0,4);
        switch(scr){
          case 0 :
           genes[i] = 1;
           break;
          case 1 :
           genes[i] = 2;
           break;
          case 2:
           genes[i] = 3;
           break;
          case 3:
           genes[i] = 5;
           break;
        }
      }
    }
  }
  
  int getGenes(int idx){                 // visualization 
    return genes[idx];
  }
  
  int getPosX(int idx){
     return posX[idx];
  }
  
   int getPosY(int idx){
     return posY[idx];
  }
  
  void setID(int input){
   id =  input;
  }
  
  int getID(){
   return id;
  }
}