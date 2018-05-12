using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour {

    int clearance = 5;
    public string seed;
    public bool useRandomSeed;
    public int width;
    public int height;
    public int smoothFactor;
    public int nFactor;
    public int sz;
    public int borderSize;

    [Range(0,100)]
    public int fillPercent;
    System.Random randomGen;
    //public NavMeshSurface floor;
    Transform[] SpawnPoints;
    public GameObject gm;
    public GameObject treegroup;

    int [,] map;
    List<GameObject> trees = new List<GameObject>();
    string[] treetypes = {};
    string treelog;
    string extra = "";

    int midx;
    int midy;
    int [,] borderMap;


    void Awake(){
        Scene scene = SceneManager.GetActiveScene();
        switch((int)scene.buildIndex){                                         
            case 1:                                                            
                string[] obj  = {"Tree_1", "Tree_2", "Tree_3"};
                treelog = "Log_3";
                treetypes = obj;
                break;
            case 3:                                                             
                string[] obj3 = {"Mounting_1", "Mounting_2", "Mounting_3"};
                treetypes = obj3;
                treelog = "Flat_Rock_01";
                extra = "Flat_Cactus_02";
                //treelog = "Log_3";
                break;

            case 5:                                                             
                string[] obj5 = {"SnowStone1", "SnowStone2", "SnowStone3"};
                treetypes = obj5;
                extra = "Pine_Snowy1";
                treelog = "SnowStone10";
                //treelog = "Log_3";
                break;
        }                                                                                                                                                                                      

        SpawnPoints = gm.GetComponentInChildren<GameManager>().spawnPoints;
        GenerateMap();
        //floor.BuildNavMesh();
    }

    void Update(){
        //regenerate with right mouse click
        if (Input.GetMouseButtonDown(1)){
            foreach(GameObject o in trees){
                GameObject.Destroy(o.gameObject); 
            }
            trees.Clear();
            GenerateMap();
            //floor.BuildNavMesh();
        }
    }

    void GenerateMap(){
        map = new int[width,height];
        randomFillMap();

        midx = width/2;
        midy = height/2;

        //clear space for the player at 0,0,0
        for(int i = -clearance; i < clearance; i++){
            for(int j = -clearance; j < clearance; j++){
                map[i+midx,j+midy] = 0;
            }
        }

        //clear space for the enemy spawn points
        int spx, spy;// spz;
        int path_margin = 2;

        for(int k = 0; k < SpawnPoints.GetLength(0); k++){
            spx = (int)SpawnPoints[k].position.x;
            spy = (int)SpawnPoints[k].position.z; // z axis is y in this 2d coordinate
            //spz = SpawnPoints[j].z;
            //
            for(int i = -clearance; i < clearance; i++){
                for(int j = -clearance; j < clearance; j++){
                    map[spx+midx+i,spy+midy+j] = 0;
                }
            }
            /*
               GameObject sp = Instantiate(Resources.Load("Rock_1"),
               new Vector3(spx, 0, spy), Quaternion.identity) as GameObject;
               */

            //connect path enemy spawn points to the player
            int flipx;
            int flipy;
            //int tempmid;

            if (spx+midx < midx ) {flipx = 1;} else { flipx = -1;}
            if (spy+midy < midy ) {flipy = 1;} else { flipy = -1;}

            for (int j = spx+midx; j != midx; j += flipx){
                for (int l = -path_margin; l < path_margin; l++){
                    map[j, spy+midy+l] = 0;
                }
            }
            
            for (int j = spy+midy; j != midy; j += flipy){
                for (int l = -path_margin; l < path_margin; l++){
                    map[midx+l, j] = 0;
                }
            }
        }


        


        for (int i = 0; i < smoothFactor; i++){
            SmoothMap();
        }


        //add border to the map
        borderMap = new int[width + borderSize*2, height +borderSize*2];
        for(int x = 0; x < borderMap.GetLength(0); x++){
            for(int y = 0; y < borderMap.GetLength(1); y++){
                if(x>=borderSize && x < width && y >=borderSize && y < height){
                    borderMap[x,y] = map[x-borderSize,y-borderSize];
                } else {
                    borderMap[x,y] = 1;
                }
            }
        }

        //setobj();

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(borderMap, sz);
    }

    Vector3 returnHeight(Vector3 pos){                                         
        RaycastHit hit;                                                        
        if (Physics.Raycast(pos+Vector3.up*10, Vector3.down, out hit,100f)){
            //Debug.Log(hit.point);                                            
            return hit.point;                                                  
        }                                                                      
        return pos;
    }

    public void setobj(){

        // randomly set trees
        int other=0;
        float xloc;
        float yloc;
        for(int x = 0; x < borderMap.GetLength(0); x++){
            for(int y = 0; y < borderMap.GetLength(1); y++){
                xloc = (-width/2+x-borderSize)*sz + 0.5f;
                yloc = (-height/2+y-borderSize)*sz + 0.5f;
                if (borderMap[x,y] ==1){
                    other++;
                    if (other%17==0 && GetNeighborCount(x,y) >= 4){
                        GameObject myTreeInstance =
                            Instantiate(Resources.Load(
                                        treetypes[randomGen.Next(0,3)]),
                                    returnHeight(new Vector3(xloc, 2, yloc)),
                                    Quaternion.identity) as GameObject;
                        trees.Add(myTreeInstance);
                        myTreeInstance.transform.SetParent(treegroup.transform);
                    } else if (other%2==0 && other%17!=0) {
                        //} if (GetNeighborCount(x,y) >= 3) {
                        GameObject terrainInst =
                            Instantiate(Resources.Load(treelog), 
                                    returnHeight(new Vector3(xloc, 2, yloc)),
                                    Quaternion.identity) as GameObject;
                    trees.Add(terrainInst);
                    terrainInst.transform.SetParent(treegroup.transform);
                }
                    else if (other%11==0 && extra!=""){
                        GameObject terrainInst =
                            Instantiate(Resources.Load(extra), 
                                    returnHeight(new Vector3(xloc, 2, yloc)),
                                    Quaternion.identity) as GameObject;
                    trees.Add(terrainInst);
                    terrainInst.transform.SetParent(treegroup.transform);
                    }
                }
            }
        }

    }

    void randomFillMap(){
        if (useRandomSeed){
            //seed = Time.time.ToString();
            seed = DateTime.Now.Ticks.ToString();
        }
        //System.Random randomGen = new System.Random(seed.GetHashCode());
        randomGen = new System.Random(seed.GetHashCode());

        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                if (x==0 || x==width-1 || y==0 || y==height-1){
                    map[x,y]  = 1; /* Mark all the edges as wall */
                } else {
                    map[x,y]  = (randomGen.Next(0,100) < fillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap(){
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                int nCount = GetNeighborCount(x,y);
                if (nCount > nFactor) {
                    map[x,y] = 1;
                } else if ( nCount < nFactor){
                    map[x,y] = 0;
                }
            }
        }
    }

    int GetNeighborCount(int xpos, int ypos){
        int count = 0;
        for (int nx = xpos-1; nx <= xpos+1; nx++){
            for(int ny = ypos-1; ny <= ypos+1; ny++){
                if (nx >= 0 && nx < width && ny >=0 && ny < height){
                    if (nx != xpos || ny != ypos){
                        count += map[nx,ny];
                    }
                } else {
                    count++;
                }
            }
        }
        return count;
    }


}
/*
   private IEnumerator coroutine;

// every 2 seconds perform the print()
private IEnumerator WaitAndPrint(float waitTime)
{
while (true)
{
yield return new WaitForSeconds(waitTime);
print("WaitAndPrint " + Time.time);
}
}
*/

/*
   print("Starting " + Time.time);
   coroutine = WaitAndPrint(2.0f);
   StartCoroutine(coroutine);
   print("Before WaitAndPrint Finishes " + Time.time);
   */
