using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreGrid : MonoBehaviour
{

    public int xSize = 10;
    public int ySize = 10;
    public int zSize = 1;

    public float spacing;
    public float depthSpacing;

    Ore[,,] grid;

    Transform[] layers;
    public GameObject[] orePrefabs;

    private void Start()
    {
        BuildGrid();
        HideMiddleTester();

        //Debug.Log($"Pos 5,5,0: {GetPosition(5, 5, 0)}");
    }

    public void BuildGrid()
    {
        grid = new Ore[xSize, ySize, zSize];
        layers = new Transform[zSize];
        for (int i = 0; i < layers.Length; i++)
        {
            GameObject copy = new GameObject($"Layer {i}");
            copy.transform.position = transform.position + new Vector3(0, 0, i*depthSpacing);
            copy.transform.parent = transform;
            layers[i] = copy.transform;
        }
        Randomize();
    }

    void Randomize()
    {
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                for (int k = 0; k < zSize; k++)
                {
                    int r = Random.Range(0, orePrefabs.Length);
                    GameObject copy = Instantiate(orePrefabs[r], transform.position + new Vector3(i * spacing, j * spacing, k * depthSpacing), transform.rotation, layers[k]);
                    Ore o = copy.GetComponent<Ore>();
                    o.SetSortOrder(-k * 10);
                    grid[i, j, k] = o;
                    // add to the appropriate events
                    // allows me to change the alpha of a bunch of them when the player changes depth
                }
            }
        }
    }

    public Vector3 GetPosition(int x, int y, int z)
    {
        x = Mathf.Clamp(x, 0, xSize);
        y = Mathf.Clamp(y, 0, ySize);
        z = Mathf.Clamp(z, 0, zSize);

        return transform.position + new Vector3(x * spacing, y * spacing, z * depthSpacing);
    }

    public bool IsSolid(int x, int y, int z)
    {
        return grid[x,y,z] && grid[x, y, z].IsSolid();
    }

    public void Smash(int x, int y, int z, int power, Side sideHitFrom)
    {
        grid[x, y, z].Smash(power, sideHitFrom);
    }

    public void HideMiddleTester()
    {
        for (int i = 3; i < 6; i++)
        {
            for (int j = 3; j < 6; j++)
            {
                grid[i, j, 0].gameObject.SetActive(false);
            }
        }
    }
}
