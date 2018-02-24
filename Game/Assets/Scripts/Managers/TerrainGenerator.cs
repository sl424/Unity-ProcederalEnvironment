using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.heparo.terrain.toolkit;
using UnityEngine.AI;

public class TerrainGenerator : MonoBehaviour {

        public Texture2D cliffTexture;
        public Texture2D sandTexture;
        public Texture2D grassTexture;
        public Texture2D rockTexture;// = (Texture2D)Resources.LoadAssetAtPath("Assets/com/heparo/terrain/toolkit/editor/resources/Dirt.jpg", typeof(Texture2D));
        public NavMeshSurface floor;
        public GameObject mm;
        public float normalizeFactor = 0.02f;

        //float[] slopeStops = new float[] { 20.0f, 50.0f };
        //float[] heightStops = new float[] { .2f, .4f, .6f, .8f };

        float[] slopeStops = new float[] { 30.0f, 45.0f };
        float[] heightStops = new float[] { .025f, .05f, .06f, .07f };

	// Use this for initialization
	void Start () {
        GameManager ss = mm.GetComponent<GameManager>();

        Texture2D[] textures = new Texture2D[] { cliffTexture, sandTexture, grassTexture, rockTexture};
		TerrainToolkit toolkit = GetComponent<TerrainToolkit>();
        toolkit.resetTerrain();

        /*********
		//PerlinGenerator(int frequency, float amplitude, int octaves, float blend)
        *********/
		//toolkit.PerlinGenerator(2, 0.5f, 9, 1.0f);

        /********
        VoronoiGenerator(FeatureType featureType, int cells, float features, float scale, float blend)
        **********/
        toolkit.VoronoiGenerator(com.heparo.terrain.toolkit.TerrainToolkit.FeatureType.Hills, 20, 1, 0.75f, 0.25f);

		//TextureTerrain(float[] slopeStops, float[] heightStops, Texture2D[] textures)
        toolkit.TextureTerrain(slopeStops, heightStops, textures);

		//NormaliseTerrain(float minHeight, float maxHeight, float blend)
		toolkit.NormaliseTerrain(0f, normalizeFactor, 1.0f);
        floor.BuildNavMesh();
        ss.beginGame();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
