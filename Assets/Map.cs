using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Point Top Maps
public class Map : MonoBehaviour
{

    public Dictionary<string, int> tiles;   
    public const int defaultTile = 0;
    public float tileSize;
    private GameObject[] prefab = new GameObject[3] {null, null, null};

    public Material m1;
    public Material m2;
    public Material m3;

    private System.Random rnd = new System.Random();

    private List<GameObject> rendered;

    async void Awake() {
        tiles = new Dictionary<string, int>();
        Material[] mats = new Material[3] {m1, m2, m3};
        prefab = new GameObject[3] {new GameObject("Empty"), new GameObject("Empty"), new GameObject("Empty")};
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
    }

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

    int[] AxialRound(double q, double r) {
        int q_  = (int) Math.Round(q);
        int r_  = (int) Math.Round(r);
        double q_rem = q - q_;
        double r_rem = r - r_;
        if (Math.Abs(q_rem) >= Math.Abs(r_rem)) {
            return new int[] {q_ + (int) Math.Round(q_rem + 0.5*r_rem), r_};
        } else {
            return new int[] {q_, r_ + (int) Math.Round(r_rem + 0.5*q_)};
        }
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
                subMap.Add(key, Get(q, r));
            }
        }
        return subMap;
    }

    void RenderMap() {
        int x = (int) transform.position.x;
        int y = (int) transform.position.y;
        Dictionary<string, int> subMap = GetMapAroundPixel(x, y, 5);
        string output = "\n";
        int i = 0;
        foreach (var item in subMap) {
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
        }
        // Debug.Log(output);
    }

    void RenderFullMap() {
        int x = (int) transform.position.x;
        int y = (int) transform.position.y;
        Dictionary<string, int> subMap = GetMapAroundPixel(x, y, 5);
        string output = "\n";
        int i = 0;
        foreach (var item in subMap) {
            string[] coords = item.Key.Split(',');
            int q = Int32.Parse(coords[0]);
            int r = Int32.Parse(coords[1]);
            double[] xy = Axial2Pixel(q, r);
            output += "#" + i + " q:" + q + " r:" + r + " x:" + xy[0] + " y:" + xy[1] + "\n";
            i++;
            int tileType = item.Value;
            rendered.Add((GameObject) Instantiate(prefab[0], new Vector3((float) xy[0],(float) xy[1],0), Quaternion.identity));
        }
        Debug.Log(output);
    }

    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var go in rendered) {
            DestroyImmediate(go);
        }
        rendered.Clear();
        RenderMap();
    }
}
