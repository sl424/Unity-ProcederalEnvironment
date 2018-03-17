using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.heparo.terrain.toolkit;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class TerrainGenerator : MonoBehaviour {

        public Texture2D cliffTexture;
        public Texture2D sandTexture;
        public Texture2D grassTexture;
        public Texture2D rockTexture;// = (Texture2D)Resources.LoadAssetAtPath("Assets/com/heparo/terrain/toolkit/editor/resources/Dirt.jpg", typeof(Texture2D));
        public Texture2D[] Textures;
        public NavMeshSurface floor;
        public GameObject mm;
        //public GameObject nn;
        float normalizeFactor;// = 0.02f;

        //float[] slopeStops = new float[] { 20.0f, 50.0f };
        //float[] heightStops = new float[] { .2f, .4f, .6f, .8f };

        float[] slopeStops; //= new float[] { 30.0f, 45.0f };
        float[] heightStops; //= new float[] { .025f, .05f, .06f, .07f };
        Texture2D[] textures;

	// Use this for initialization
	void Start () {
        GameManager ss = mm.GetComponent<GameManager>();
        //MapGenerator tt = nn.GetComponent<MapGenerator>();
        MapGenerator tt = transform.parent.GetComponent<MapGenerator>();

		TerrainToolkit toolkit = GetComponent<TerrainToolkit>();
        toolkit.resetTerrain();
        Scene scene = SceneManager.GetActiveScene();                           
        switch((int)scene.buildIndex){           
            case 1: 
                textures = new Texture2D[] { cliffTexture, sandTexture, grassTexture, rockTexture};
                slopeStops = new float[] { 30.0f, 45.0f };
                heightStops = new float[] { .025f, .05f, .06f, .07f };
                normalizeFactor = 0.02f;
                toolkit.VoronoiGenerator(com.heparo.terrain.toolkit.TerrainToolkit.FeatureType.Hills, 20, 1, 0.75f, 0.25f);
                break;
            case 3:
                textures = new Texture2D[] {Textures[18], Textures[19], Textures[20], Textures[19]};
                slopeStops = new float[] {30.025f, 30.05f};
                heightStops = new float[] { .025f, .05f, .06f, .07f };
                toolkit.VoronoiGenerator(com.heparo.terrain.toolkit.TerrainToolkit.FeatureType.Plateaus, 5, 1, 0.0f, 0.25f);
		        //toolkit.PerlinGenerator(2, 0.5f, 9, 1.0f);
                normalizeFactor = 0.02f;
                break;
            case 5:
                textures = new Texture2D[] {Textures[21], Textures[21], Textures[20], Textures[21]};
                slopeStops = new float[] {30.025f, 30.05f};
                heightStops = new float[] { .025f, .05f, .06f, .07f };
                toolkit.VoronoiGenerator(com.heparo.terrain.toolkit.TerrainToolkit.FeatureType.Mountains, 5, 1, 0.0f, 0.25f);
		        //toolkit.PerlinGenerator(2, 0.5f, 9, 1.0f);
                normalizeFactor = 0.04f;
                break;
        }

        /*********
		//PerlinGenerator(int frequency, float amplitude, int octaves, float blend)
        *********/
		//toolkit.PerlinGenerator(2, 0.5f, 9, 1.0f);

        /********
        VoronoiGenerator(FeatureType featureType, int cells, float features, float scale, float blend)
        **********/

		//TextureTerrain(float[] slopeStops, float[] heightStops, Texture2D[] textures)

        toolkit.TextureTerrain(slopeStops, heightStops, textures);
		//NormaliseTerrain(float minHeight, float maxHeight, float blend)
		toolkit.NormaliseTerrain(0f, normalizeFactor, 1.0f);
        floor.BuildNavMesh();
        tt.setobj();
        ss.beginGame();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
