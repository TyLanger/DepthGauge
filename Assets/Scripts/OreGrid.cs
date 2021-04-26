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

    public Vector3 offset;

    Ore[,,] grid;
    bool built = false;
    bool canMove = false;

    public float layerMoveSpeed = 1;
    Transform[] layers;
    Vector3[] layerTargets;

    public GameObject[] orePrefabs;
    public PlayerController player;

    private void Start()
    {

        //BuildGrid();
        //HideMiddleTester();

        //Debug.Log($"Pos 5,5,0: {GetPosition(5, 5, 0)}");
    }

    private void Update()
    {
        if (canMove)
        {
            if (layerTargets != null && (layerTargets.Length > 0))
            {
                for (int i = 0; i < layerTargets.Length; i++)
                {
                    layers[i].transform.position = Vector3.MoveTowards(layers[i].transform.position, layerTargets[i], Time.deltaTime * layerMoveSpeed);
                }
            }
        }
    }

    public void BuildGrid()
    {
        if (!built)
        {
            grid = new Ore[xSize, ySize, zSize];
            layers = new Transform[zSize];
            layerTargets = new Vector3[layers.Length];
            built = true; 
            // check this so I can call build to rebuild without doing these steps again
            // although, these steps are probably the easiest
        }
        BuildLayers();
        canMove = true;
        //Randomize();
    }

    void BuildLayers()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            GameObject copy = new GameObject($"Layer {i}");
            copy.transform.position = transform.position + new Vector3(0, 0, i * depthSpacing);
            copy.transform.parent = transform;
            layers[i] = copy.transform;
        }
    }

    public void DestroyRocks()
    {
        canMove = false;
        for (int i = 0; i < layers.Length; i++)
        {
            Destroy(layers[i].gameObject);
        }
        //BuildLayers();
    }

    public void SetupPlayer()
    {
        player.OnPlayerChangeLayer += AdjustLayers;
        AdjustLayers(0);
    }

    public void UnSmash(int x, int y, int z)
    {
        grid[x, y, z].UnSmash();
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

    public void Fill(int x, int y, int z, int r, int damage, Side side)
    {
        GameObject copy = Instantiate(orePrefabs[r], transform.position + new Vector3(x * spacing, y * spacing, z * depthSpacing), transform.rotation, layers[z]);
        Ore o = copy.GetComponent<Ore>();
        o.SetSortOrder(-z * 10);
        if (damage > 0)
        {
            //Debug.Log($"Random Damage {damage}");
            o.Smash(damage, side, true);
        }
        grid[x, y, z] = o;
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
        //Debug.Log($"Solid {x}, {y}, {z}");
        if (isWithinBounds(x, y, z))
        {
            return grid[x, y, z] && grid[x, y, z].IsSolid();
        }
        return true;
    }

    public void Smash(int x, int y, int z, int power, Side sideHitFrom, bool obliterate = false)
    {
        //Debug.Log($"smash? {x}, {y}, {z}");
        if (isWithinBounds(x, y, z))
        {
            grid[x, y, z].Smash(power, sideHitFrom, obliterate);
        }
    }

    bool isWithinBounds(int x, int y, int z)
    {
        if(x>=0 && y >= 0 && z>=0 && x<xSize && y<ySize && z<zSize)
        {
            return true;
        }
        else
        {
            Debug.Log($"Out of bounds {x}, {y}, {z}");
            return false;
        }
    }

    void AdjustLayers(int playerZ)
    {
        // what if the player moves back and forth?
        // can they move quick enough to have 2 coroutines?
        // playerZ is between 0 and 4
        for (int i = 0; i < layers.Length; i++)
        {
            int diff = playerZ - i;
            if (diff > 0)
            {
                // move towards camera
                //layers[i].transform.position = transform.position + new Vector3(0, 0, -i * 0.5f);
                layerTargets[i] = transform.position + new Vector3(0, 0, i * depthSpacing - diff * 0.5f);
            }
            else
            {
                layerTargets[i] = transform.position + new Vector3(0, 0, i * depthSpacing);
            }
        }
    }

    IEnumerator MoveLayersSmooth(int playerZ)
    {
        yield return null;
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
