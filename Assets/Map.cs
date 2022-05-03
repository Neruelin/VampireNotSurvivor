using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Point Top Maps
public class Map : MonoBehaviour
{

    private System.Random rnd = new System.Random();
    private GameObject[] prefab = new GameObject[3] {null, null, null};
    private const int defaultTile = 0;
    private List<GameObject> rendered;
    private HashSet<string> renderedSet;
    public GameObject player = null;
    public Dictionary<string, int> tiles;   
    public float tileSize;
    public Material m1;
    public Material m2;
    public Material m3;

    public bool localRender = false;

    int Get(int q, int r) {
        string key = "" + q + "," + r;
        if (!tiles.ContainsKey(key)) {
            this.GenerateTile(q, r);
        }
        return tiles[key];
    }

    void GenerateTile(int q,  int r) {
        string key = "" + q + "," + r;
        // tiles.Add(key, defaultTile);
        tiles.Add(key, rnd.Next() % prefab.Length);
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
        Debug.Log("" + axials[0] + " " + axials[1]);
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
            subMap = GetMapAroundPixel(x, y, 15);
            foreach (var go in rendered) {
                DestroyImmediate(go);
            }
            rendered.Clear();
            renderedSet.Clear();
        } else {
            GenerateAroundPixel(x, y, 15);
        }
        string output = "\n";
        int i = 0;
        foreach (var item in subMap) {
            if (!localRender && renderedSet.Contains(item.Key)) continue;
            string[] coords = item.Key.Split(',');
            int q = Int32.Parse(coords[0]);
            int r = Int32.Parse(coords[1]);
            double[] xy = Axial2Pixel(q, r);
            output += "#" + i + " q:" + q + " r:" + r + " x:" + xy[0] + " y:" + xy[1] + "\n";
            i++;
            int tileType = item.Value;
            GameObject go = (GameObject) Instantiate(prefab[tileType], new Vector3((float) xy[0],(float) xy[1],0), Quaternion.identity);
            go.SetActive(true);
            rendered.Add(go);
            renderedSet.Add(item.Key);
        }
        // Debug.Log(output);
    }

    void Awake() {
        tiles = new Dictionary<string, int>();
        Material[] mats = new Material[3] {m1, m2, m3};
        prefab = new GameObject[3] {new GameObject("Tile"), new GameObject("Tile"), new GameObject("Tile")};
        for (int j = 0; j < 3; j++) {
            GameObject go = prefab[j];
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            go.AddComponent<Renderer>();
            var mesh = new Mesh();
            int i = 0;
            float increment = (float) (2 * Math.PI/6);
            float offset = (float) (Math.PI/6);
            mesh.vertices = new Vector3[] {
                new Vector3((float) Math.Cos( (i * increment) + offset ),(float) Math.Sin( (i++ * increment) + offset ),0), 
                new Vector3((float) Math.Cos( (i * increment) + offset ),(float) Math.Sin( (i++ * increment) + offset ),0), 
                new Vector3((float) Math.Cos( (i * increment) + offset ),(float) Math.Sin( (i++ * increment) + offset ),0), 
                new Vector3((float) Math.Cos( (i * increment) + offset ),(float) Math.Sin( (i++ * increment) + offset ),0), 
                new Vector3((float) Math.Cos( (i * increment) + offset ),(float) Math.Sin( (i++ * increment) + offset ),0), 
                new Vector3((float) Math.Cos( (i * increment) + offset ),(float) Math.Sin( (i++ * increment) + offset ),0)
            };
            mesh.triangles = new int[] {
                0, 5, 1,
                1, 5, 2,
                2, 5, 4,
                2, 4, 3
            };
            go.GetComponent<MeshFilter>().mesh = mesh;
            go.transform.localScale = new Vector3(1,1,1) * tileSize;
            go.GetComponent<Renderer>().material = mats[j];
            go.SetActive(false);
        }

        rendered = new List<GameObject>();
        renderedSet = new HashSet<string>();
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
