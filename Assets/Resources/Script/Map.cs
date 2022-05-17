using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Point Top Maps
public class Map : MonoBehaviour
{

    private System.Random rnd = new System.Random();
    private GameObject[] prefab;
    private float[] distribution;
    private const int defaultTile = 0;
    private List<GameObject> rendered;
    private Dictionary<string, GameObject> renderedMap;
    public GameObject player = null;
    public Dictionary<string, int> tiles;   
    public float tileSize = 1;
    public bool localRender = false;
    public bool rat = false;
    public bool column = false;
    public int renderRadius = 15;
    [SerializeField]
    
    int Get(int q, int r) {
        string key = "" + q + "," + r;
        if (!tiles.ContainsKey(key)) {
            this.GenerateTile(q, r);
        }
        return tiles[key];
    }

    int PickWithDistribution(float[] probs) {
        int N = probs.Length;
		float sum = 0;

		foreach (var val in probs) { sum += val; }
		if (sum != 1.0f) { return -1; }

		int i = 0;
		float randomvalue = (float)rnd.NextDouble();
		while (randomvalue > 0)
		{
			randomvalue -= probs[i];
			i++;
		}

		return i - 1;
    }

    void GenerateTile(int q,  int r) {
        string key = "" + q + "," + r;
        tiles.Add(key, PickWithDistribution(distribution));
    }

    void SetTile(int q, int r, int tileType) {
        string key = "" + q + "," + r;
        tiles.Add(key, tileType);
    }

    double[] axial2Cube(double q, double r) {
        return new double[] {q, r, -q-r};
    }

    double[] cube2Axial(double q, double r, double s) {
        return new double[] {q, r};
    }

    int[] cube_round(double q, double r, double s) {
        int q_ = (int) Math.Round(q);
        int r_ = (int) Math.Round(r);
        int s_ = (int) Math.Round(s);

        double q_diff = Math.Abs(q_ - q);
        double r_diff = Math.Abs(r_ - r);
        double s_diff = Math.Abs(s_ - s);

        if (q_diff > r_diff && q_diff > s_diff) {
            q_ = -r_-s_;
        } else if (r_diff > s_diff) {
            r_ = -q_-s_;
        } else {
            s_ = -q_-r_;
        }

        return new int[] {q_, r_, s_};
    }

    int[] AxialRound(double q, double r) {
        double[] cubeCoords = axial2Cube(q, r);
        int[] roundedCube = cube_round(cubeCoords[0], cubeCoords[1], cubeCoords[2]);
        double[] roundedAxial = cube2Axial(roundedCube[0], roundedCube[1], roundedCube[2]);
        int[] integerRoundedAxial = new int[] {(int) roundedAxial[0], (int) roundedAxial[1]};
        return integerRoundedAxial;
    }

    double[] Axial2Pixel(int q, int r) {
        double x = tileSize * ((Math.Sqrt(3.0) * q) + ((Math.Sqrt(3.0)/2.0) * r));
        double y = tileSize * ((3.0/2.0) * r);
        return new double[] {x, y};
    }

    int[] Pixel2Axial(int x, int y) {
        double q = (((Math.Sqrt(3.0)/3.0) * x) + (-(1.0/3.0) * y)) / tileSize;
        double r = ((2.0/3.0) * y) / tileSize;
        return AxialRound(q, r);
    }

    Dictionary<string, int> GetMapAroundPixel(int x, int y, int rad) {
        Dictionary<string, int> subMap = new Dictionary<string, int>();
        int[] axials = Pixel2Axial(x, y);
        for (int q = -rad; q < rad; q++) {
            for (int r = Math.Max(-rad, -q-rad); r < Math.Min(rad, -q+rad); r++) {
                string key = "" + (q+axials[0]) + "," + (r+axials[1]);
                subMap.Add(key, Get(q + axials[0], r + axials[1]));
            }
        }
        return subMap;
    }

    void GenerateAroundPixel (int x, int y, int rad) {
        int[] axials = Pixel2Axial(x, y);
        for (int q = -rad; q < rad; q++) {
            for (int r = Math.Max(-rad, -q-rad); r < Math.Min(rad, -q+rad); r++) {
                Get(q + axials[0], r + axials[1]);
            }
        }
    }

    void RenderMap() {
        int x = (int) player.transform.position.x;
        int y = (int) player.transform.position.y;
        Dictionary<string, int> subMap = tiles;
        if (localRender) {
            subMap = GetMapAroundPixel(x, y, renderRadius);
            foreach (var go in rendered) {
                if (subMap.ContainsKey(go.name)) {
                    continue;
                }
                go.SetActive(false);
            }
            rendered.Clear();
        } else {
            GenerateAroundPixel(x, y, renderRadius);
        }
        string output = "\n";
        int i = 0;
        foreach (var item in subMap) {
            if (localRender && renderedMap.ContainsKey(item.Key)) {
                renderedMap[item.Key].SetActive(true);
                // if (item.Value == 4) {
                //     renderedMap[item.Key].transform.Find("GameObject").GetComponent<EnemySpawn>().Start();
                // }
                rendered.Add(renderedMap[item.Key]);
                continue;
            }
            if (renderedMap.ContainsKey(item.Key)) {
                continue;
            }
            string[] coords = item.Key.Split(',');
            int q = Int32.Parse(coords[0]);
            int r = Int32.Parse(coords[1]);
            double[] xy = Axial2Pixel(q, r);
            output += "#" + i + " q:" + q + " r:" + r + " x:" + xy[0] + " y:" + xy[1] + "\n";
            i++;
            int tileType = item.Value;
            GameObject go = (GameObject) Instantiate(prefab[tileType], new Vector3((float) xy[0],(float) xy[1],0), Quaternion.identity);
            go.name = item.Key;
            // go.SetActive(true);
            rendered.Add(go);
            renderedMap.Add(item.Key, go);
        }
        // Debug.Log(output);
    }

    void Awake() {        
        prefab = new GameObject[4];
        distribution = new float[4] {0.25f, 0.35f, 0.4f, 0.0f};
        if (rat) {
            distribution = new float[4] {0.25f, 0.35f, 0.39f, 0.01f};
        }
        string[] tileAddresses = new string[4] {"Prefabs/Tile_brown", "Prefabs/Tile_green", "Prefabs/Tile_grey", "Prefabs/Tile_enemyspawner"};
        if (column) {
            tileAddresses = new string[4] {"Prefabs/Tile_brown", "Prefabs/Tile_green", "Prefabs/HexTile", "Prefabs/Tile_enemyspawner"};
        }
        Vector3 scale = new Vector3(1,1,1);
        scale *= tileSize;
        int idx = 0;
        foreach (var str in tileAddresses) {
            prefab[idx] = Addressables.LoadAssetAsync<GameObject>(str).WaitForCompletion();
            prefab[idx].transform.localScale = scale;
            idx++;
        }

        tiles = new Dictionary<string, int>();
        rendered = new List<GameObject>();
        renderedMap = new Dictionary<string, GameObject>();

        int rad = 2;
        int[] axials = Pixel2Axial(0, 0);
        for (int q = -rad; q < rad; q++) {
            for (int r = Math.Max(-rad, -q-rad); r < Math.Min(rad, -q+rad); r++) {
                SetTile(q + axials[0], r + axials[1], 1);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            Debug.Log("No Player Added to Map");
            return;
        }

        RenderMap();
    }
}
