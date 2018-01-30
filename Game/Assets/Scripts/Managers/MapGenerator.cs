using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour {

    int clearance = 3;
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
    public NavMeshSurface floor;
    public Vector3[] SpawnPoints;

    int [,] map;
    
    List<GameObject> trees = new List<GameObject>();
    string[] treetypes = {"Tree_1", "Tree_2", "Tree_3"};

    void Start(){
        GenerateMap();
        floor.BuildNavMesh();
    }

    void Update(){
        //regenerate with right mouse click
        if (Input.GetMouseButtonDown(1)){
            foreach(GameObject o in trees){
                GameObject.Destroy(o.gameObject); 
            }
            trees.Clear();
            GenerateMap();
            floor.BuildNavMesh();
        }
    }

    void GenerateMap(){
        map = new int[width,height];
        randomFillMap();

        int midx = width/2;
        int midy = height/2;

        //clear space for the player at 0,0,0
        for(int i = -clearance; i < clearance; i++){
            for(int j = -clearance; j < clearance; j++){
                map[i+midx,j+midy] = 0;
            }
        }

        //clear space for the enemy spawn points
        int spx, spy, spz;
        for(int k = 0; k < SpawnPoints.GetLength(0); k++){
            spx = (int)SpawnPoints[k].x;
            spy = (int)SpawnPoints[k].z; // z axis is y in this 2d coordinate
            //spz = SpawnPoints[j].z;
            //
            for(int i = -clearance; i < clearance; i++){
                for(int j = -clearance; j < clearance; j++){
                    map[spx+midx,spy+midy] = 0;
                }
            }
            GameObject sp = Instantiate(Resources.Load("Rock_1"),
                        new Vector3(spx, 0, spy), Quaternion.identity) as GameObject;
        }

        for (int i = 0; i < smoothFactor; i++){
            SmoothMap();
        }
        

        //add border to the map
        int[,] borderMap = new int[width + borderSize*2, height +borderSize*2];
        for(int x = 0; x < borderMap.GetLength(0); x++){
            for(int y = 0; y < borderMap.GetLength(1); y++){
                if(x>=borderSize && x < width && y >=borderSize && y < height){
                    borderMap[x,y] = map[x-borderSize,y-borderSize];
                } else {
                    borderMap[x,y] = 1;
                }
            }
        }

        // randomly set trees
        int other=0;
        int xloc;
        int yloc;
        for(int x = 0; x < borderMap.GetLength(0); x++){
            for(int y = 0; y < borderMap.GetLength(1); y++){
                xloc = (-width/2+x-borderSize)*sz;
                yloc = (-height/2+y-borderSize)*sz;
                if (borderMap[x,y] ==1){
                    other++;
                    if (other%17==0){
                        GameObject myTreeInstance =
                            Instantiate(Resources.Load(
                                        treetypes[randomGen.Next(0,3)]),
                                    new Vector3(xloc, 0, yloc),
                                    Quaternion.identity) as GameObject;
                        trees.Add(myTreeInstance);
                        myTreeInstance.transform.SetParent(floor.transform);
                    } else if (other%2==0){
                        GameObject terrainInst =
                            Instantiate(Resources.Load("Log_3"), 
                                    new Vector3(xloc, 0, yloc),
                                    Quaternion.identity) as GameObject;
                        trees.Add(terrainInst);
                        terrainInst.transform.SetParent(floor.transform);
                    }
				}
			}
		}
        

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(borderMap, sz);
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
                    /* Mark all the edges as wall */
                    map[x,y]  = 1;
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
