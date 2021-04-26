using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public OreGrid prefab;
    public PlayerController player;

    public OreGrid surface;
    public OreGrid topLayer;
    public OreGrid midLayer;
    public OreGrid lowerLayer;
    public OreGrid deepLayer;

    public Vector3 surfaceOffset;
    public Vector3 topOffset;
    public Vector3 midOffset;
    public Vector3 lowOffset;
    public Vector3 deepOffset;

    /*
        Top layer - 10x10
        Middle - 14x5x3
        Lower - 14x7x5
        Deepest - 10x5x10

         */

    int surfaceX = 14;
    int surfaceY = 3;
    int surfaceZ = 1;

    int topX = 10;
    int topY = 10;
    int topZ = 2;

    int midX = 14;
    int midY = 5;
    int midZ = 3;

    int lowX = 14;
    int lowY = 7;
    int lowZ = 5;

    int deepX = 10;
    int deepY = 5;
    int deepZ = 10; // not sure if it works with a higher value

    // Start is called before the first frame update
    void Start()
    {
        // spawn the surface layer
        // move the top layer to the offset
        // build top layer
        // etc.
        BuildWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsSolid(int x, int y, int z)
    {
        //return GetGrid(x, y, z).IsSolid(x, y, z);
        int g = GetGrid(x, y, z);
        switch (g)
        {
            case 0:
                return surface.IsSolid(x, y, z);

            case 1:
                return topLayer.IsSolid(x -1, y + 10, z);

            case 2:
                return midLayer.IsSolid(x + 1, y + 15, z);

            case 3:
                return lowerLayer.IsSolid(x + 1, y + 22, z);

            case 4:
                return deepLayer.IsSolid(x -1, y + 27, z);
        }
        return true;
    }

    public Vector3 GetPosition(int x, int y, int z)
    {
        int g = GetGrid(x, y, z);
        switch(g)
        {
            case 0:
                return surface.GetPosition(x, y, z);

            case 1:
                return topLayer.GetPosition(x-1, y+10, z);

            case 2:
                return midLayer.GetPosition(x+1, y+15, z);

            case 3:
                return lowerLayer.GetPosition(x + 1, y + 22, z);
                
            case 4:
                return deepLayer.GetPosition(x -1, y + 27, z);
                
        }
        return Vector3.zero;
    }

    public void Smash(int x, int y, int z, int power, Side hitFrom, bool obliterate = false)
    {
        int g = GetGrid(x, y, z);
        switch (g)
        {
            case 0:
                surface.Smash(x, y, z, power, hitFrom, obliterate);
                break;

            case 1:
                topLayer.Smash(x -1, y + 10, z, power, hitFrom, obliterate);
                break;

            case 2:
                midLayer.Smash(x +1, y + 10 + 5, z, power, hitFrom, obliterate);
                break;

            case 3:
                lowerLayer.Smash(x + 1, y + 15 + 7, z, power, hitFrom, obliterate);
                break;

            case 4:
                deepLayer.Smash(x -1, y + 22 + 5, z, power, hitFrom, obliterate);
                break;
        }
    }

    int GetGrid(int x, int y, int z)
    {
        y = -y;
        if (y < 1)
        {
            return 0;// surface;
        }
        else if (y < 1 + topY)
        {
            return 1;// topLayer;
        }
        else if (y < 1 + topY + midY)
        {
            return 2;// midLayer;
        }
        else if (y < 1 + topY + midY + lowY)
        {
            return 3; // low
        }
        else if (y < 1 + topY + midY + lowY + deepY)
        {
            return 4; // deep
        }
        return 4;// deep;
    }

    void BuildWorld()
    {
        surface = Instantiate(prefab, transform.position + surfaceOffset, transform.rotation, transform);
        topLayer = Instantiate(prefab, transform.position + topOffset, transform.rotation, transform);
        midLayer = Instantiate(prefab, transform.position + midOffset, transform.rotation, transform);
        lowerLayer = Instantiate(prefab, transform.position + lowOffset, transform.rotation, transform);
        deepLayer = Instantiate(prefab, transform.position + deepOffset, transform.rotation, transform);

        BuildSurface();
        BuildTopLayer();
        BuildMidLayer();
        BuildLowLayer();
        BuildDeepLayer();
        /*
        Top layer - 10x10
        Middle - 14x5x3
        Lower - 14x7x5
        Deepest - 10x5x10

         */
        //Debug.Log("Finish");
        FinishWorld();
    }

    void ResetWorld()
    {
        topLayer.DestroyRocks();
        midLayer.DestroyRocks();
        lowerLayer.DestroyRocks();
        deepLayer.DestroyRocks();

        surface.UnSmash(7, 0, 0);

        BuildTopLayer();
        BuildMidLayer();
        BuildLowLayer();
        BuildDeepLayer();

    }

    void BuildSurface()
    {
        surface.xSize = surfaceX;
        surface.ySize = surfaceY;
        surface.zSize = surfaceZ;

        surface.offset = surfaceOffset;

        surface.player = player;
        surface.BuildGrid();

        for (int i = 0; i < surface.xSize; i++)
        {
            for (int j = 0; j < surface.ySize; j++)
            {
                for (int k = 0; k < surface.zSize; k++)
                {
                    int r = 0;
                    int damage = 5;
                    if(j==0 && i != 7)
                    {
                        r = 1;
                    }
                    if(j==0 && i == 7)
                    {
                        damage = 0;
                    }
                    if(j==2 && i==surface.xSize-1)
                    {
                        r = 6;
                    }
                    if (j == 2 && i == surface.xSize - 2)
                    {
                        r = 7;
                    }
                    surface.Fill(i, j, k, r, damage, Side.Mid);
                    //surface.Smash(i, j, k, 5, Side.Mid, true);
                }
            }
        }
    }

    void BuildTopLayer()
    {
        topLayer.xSize = topX;
        topLayer.ySize = topY;
        topLayer.zSize = topZ; // bg is all unbreakable

        topLayer.offset = topOffset;

        topLayer.player = player;
        topLayer.BuildGrid();
        for (int i = 0; i < topLayer.xSize; i++)
        {
            for (int j = 0; j < topLayer.ySize; j++)
            {
                for (int k = 0; k < topLayer.zSize; k++)
                {
                    int r = 0;
                    int damage = 0;
                    Side side = Side.Mid;
                    if (i==0 || i==(topLayer.xSize-1) || k==1)
                    {
                        r = 1; // granicrete
                    }
                    else if(j > topLayer.ySize - 4)
                    {
                        r = 0; // dirt
                    }
                    else
                    {
                        r = GetRandomOre(j, 0, 8, 0, 19, 14, 0, 0);
                    }
                    if (r > 1)
                    {
                        damage = GetRandomDamage();
                        side = GetRandomSide();
                    }
                    topLayer.Fill(i, j, k, r, damage, side);
                }
            }
        }
    }

    void BuildMidLayer()
    {
        midLayer.xSize = midX;
        midLayer.ySize = midY;
        midLayer.zSize = midZ;

        midLayer.offset = midOffset;
        midLayer.player = player;
        midLayer.BuildGrid();
        for (int i = 0; i < midLayer.xSize; i++)
        {
            for (int j = 0; j < midLayer.ySize; j++)
            {
                for (int k = 0; k < midLayer.zSize; k++)
                {
                    int r = 0;
                    int damage = 0;
                    Side side = Side.Mid;
                    if (i == 0 || i == (midLayer.xSize - 1) || (j == midLayer.ySize - 2 && (k == 0 || k==1)) || ((k == midLayer.zSize - 1) && j==0))
                    {
                        r = 1;
                    }
                    else
                    {
                        r = GetRandomOre(j, k, 0, 0, 7, 5, 1, 0);
                        if (k==midLayer.zSize-2 && j==0)
                        {
                            // add some random granicrete
                            r = Random.Range(1, 3);
                        }
                    }
                    if (r > 1)
                    {
                        damage = GetRandomDamage();
                        side = GetRandomSide();
                    }
                    midLayer.Fill(i, j, k, r, damage, side);
                }
            }
        }
    }

    void BuildLowLayer()
    {
        lowerLayer.xSize = lowX;
        lowerLayer.ySize = lowY;
        lowerLayer.zSize = lowZ;

        lowerLayer.offset = lowOffset;
        lowerLayer.player = player;
        lowerLayer.BuildGrid();
        for (int i = 0; i < lowerLayer.xSize; i++)
        {
            for (int j = 0; j < lowerLayer.ySize; j++)
            {
                for (int k = 0; k < lowerLayer.zSize; k++)
                {
                    int r = 0;
                    int damage = 0;
                    Side side = Side.Mid;
                    if(i==0 || i == (lowerLayer.xSize-1))
                    {
                        r = 1;
                    }
                    else
                    {
                        r = GetRandomOre(j, k, 0, 0, 9, 6, 4, 3);
                    }
                    if(r > 1)
                    {
                        damage = GetRandomDamage();
                        side = GetRandomSide();
                    }
                    lowerLayer.Fill(i, j, k, r, damage, side);
                }
            }
        }
    }

    void BuildDeepLayer()
    {
        deepLayer.xSize = deepX;
        deepLayer.ySize = deepY;
        deepLayer.zSize = deepZ;

        deepLayer.offset = lowOffset;
        deepLayer.player = player;
        deepLayer.BuildGrid();
        for (int i = 0; i < deepLayer.xSize; i++)
        {
            for (int j = 0; j < deepLayer.ySize; j++)
            {
                for (int k = 0; k < deepLayer.zSize; k++)
                {
                    int r = 0;
                    int damage = 0;
                    Side side = Side.Mid;
                    if (i == 0 || i == (deepLayer.xSize - 1) || j == 0)
                    {
                        r = 1;
                    }
                    else
                    {
                        r = GetRandomOre(j, k, 0, 0, 3, 7, 5, 6);
                    }
                    if (r > 1)
                    {
                        damage = GetRandomDamage();
                        side = GetRandomSide();
                    }
                    deepLayer.Fill(i, j, k, r, damage, side);
                }
            }
        }
    }

    int GetRandomOre(int y, int z, int dRate, int gRate, int bRate, int cRate, int fRate, int mRate)
    {
        // rarer ore deeper down
        // 0 dirt
        // 1 granicrete
        // 2 bould
        // 3 cobb
        // 4 ferr
        // 5 mith
        int sum = dRate + gRate + bRate + cRate + fRate + mRate + z + y;
        int r = Random.Range(0, sum);

        r -= dRate;
        if (r < 0)
            return 0;
        r -= gRate;
        if (r < 0)
            return 1;
        r -= bRate;
        if (r < 0)
            return 2;
        r -= cRate;
        if (r < 0)
            return 3;
        r -= fRate + y;
        if (r < 0)
            return 4;
        r -= mRate + z;
        if (r < 0)
            return 5;

        return 2; // boulder is base
    }

    int GetRandomDamage()
    {
        int r = Random.Range(0, 95);

        // 10% 5 damage (air)
        // 2% 4 damage
        // 5% 3 damage
        // 5% 2 damage
        // 10% 1 damage

        int percent1 = 5;
        int percent2 = 3;
        int percent3 = 2;
        int percent4 = 1;
        int percent5 = 6;

        // 5% for 1 damage
        // means r= 0,1,2,3,4 is 1 damage
        // once I sub r by 5, if it's negative, I know it was one of those numbers.
        // 3% for 2 damage
        // is then r= 5,6,7
        // etc.
        // this way I can tweak each percentage individually without having to change number for everything else
        // like I had it before if(r>95):1 if(r>92):2 ... 

        r -= percent1;
        if(r < 0)
        {
            return 1;
        }
        r -= percent2;
        if(r<0)
        {
            return 2;
        }
        r -= percent3;
        if(r<0)
        {
            return 3;
        }
        r -= percent4;
        if(r<0)
        {
            return 4;
        }
        r -= percent5;
        if(r<0)
        {
            return 5;
        }
        return 0;
    }

    Side GetRandomSide()
    {
        int r = Random.Range(0, 5);
        return (Side)r;
    }

    void FinishWorld()
    {
        //Invoke("SpawnPlayer", 2);
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        player.xPos = 0;
        player.yPos = 1; // topLayer.ySize-1;
        player.zPos = 0;
        player.grid = surface;
        player.gameObject.SetActive(true);

        player.OnRopeEnd += ResetWorld;

        // sets up where the layers move to
        surface.SetupPlayer();
        topLayer.SetupPlayer();
        midLayer.SetupPlayer();
        lowerLayer.SetupPlayer();
        deepLayer.SetupPlayer();
        SmashStart();
        //Invoke("SmashStart", 0.1f);
    }

    void SmashStart()
    {
        // the problem was the ore was setting its hp at start
        Smash(player.xPos, player.yPos, player.zPos, 100, Side.Mid, true);

    }
}
