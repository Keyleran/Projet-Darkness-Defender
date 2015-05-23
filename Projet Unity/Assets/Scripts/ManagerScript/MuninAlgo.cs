// --------------------------------------------------
// Project: Darkness Defender
// Script: MainManagerScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MuninAlgo 
{
    // String <=> Coordonnées Cases "xxx:yyy"
    // Char   <=> Disponibilité de la case (G = Ground, A = Allowed, W = Wall, T = Tower, L = LockPath)
    private Dictionary<string, char> _gridMapping;
    public Dictionary<string, char> GridMapping 
    {
        get { return _gridMapping; }
        set { _gridMapping = value; }
    }

    // String <=> Coordonnées Cases "xxx:yyy"
    // Ushort <=> Donne l'état des cases adjacentes
    /* 
    Fonctionnement du mapping, chaque tour adjacente ajoute le nombre indiqué:
    0x40 | 0x80 | 0x01
    0x20 | Tour | 0x02
    0x10 | 0x08 | 0x04
        
    Par exemple, une tour en bas à droite ajoute 0x04 au compteur et une tour en haut ajoute 0x80, ainsi, la valeur sera de 0x84
    */
    private Dictionary<string, ushort> _griState;
    public Dictionary<string, ushort> GridState 
    {
        get { return _griState; }
        set { _griState = value; }
    }


    public MuninAlgo()
    {
        GridMapping = new Dictionary<string, char>();
        GridState = new Dictionary<string, ushort>();
    }

    public void InitiateGrid()
    {
        for (int x = -48; x <= 48; x += 2)
        {
            for (int z = -38; z <= 38; z += 2)
            {
                string coord = x.ToString() + ":" + z.ToString();
                if (GridMapping[coord] == 'W')
                {
                    GridState[(x + 2).ToString() + ":" + (z + 2).ToString()] += 0x01;
                    GridState[(x + 2).ToString() + ":" + (z    ).ToString()] += 0x02;
                    GridState[(x + 2).ToString() + ":" + (z - 2).ToString()] += 0x04;
                    GridState[(x    ).ToString() + ":" + (z - 2).ToString()] += 0x08;
                    GridState[(x - 2).ToString() + ":" + (z - 2).ToString()] += 0x10;
                    GridState[(x - 2).ToString() + ":" + (z    ).ToString()] += 0x20;
                    GridState[(x - 2).ToString() + ":" + (z + 2).ToString()] += 0x40;
                    GridState[(x    ).ToString() + ":" + (z + 2).ToString()] += 0x80;
                }
            }
        }
    }

    public bool accessRequest(float x, float z)
    {
        bool access = false;
        string coor = x.ToString() + ":" + z.ToString();

        if ((GridMapping[coor] == 'G')||(GridMapping[coor] == 'A'))
            access = true;

        return access;
    }

    public void BuildTower(float x, float z)
    {
        string coor = x.ToString() + ":" + z.ToString();
        GridMapping[coor] = 'T';

        GridState[(x + 2).ToString() + ":" + (z + 2).ToString()] += 0x01;
        GridState[(x + 2).ToString() + ":" + (z    ).ToString()] += 0x02;
        GridState[(x + 2).ToString() + ":" + (z - 2).ToString()] += 0x04;
        GridState[(x    ).ToString() + ":" + (z - 2).ToString()] += 0x08;
        GridState[(x - 2).ToString() + ":" + (z - 2).ToString()] += 0x10;
        GridState[(x - 2).ToString() + ":" + (z    ).ToString()] += 0x20;
        GridState[(x - 2).ToString() + ":" + (z + 2).ToString()] += 0x40;
        GridState[(x    ).ToString() + ":" + (z + 2).ToString()] += 0x80;

        UpdateGrid(x + 2, z + 2);
        UpdateGrid(x + 2, z    );
        UpdateGrid(x + 2, z - 2);
        UpdateGrid(x    , z - 2);
        UpdateGrid(x - 2, z - 2);
        UpdateGrid(x - 2, z    );
        UpdateGrid(x - 2, z + 2);
        UpdateGrid(x    , z + 2);
    }

    public void UpdateGrid(float x, float z)
    {
        string coor = x.ToString() + ":" + z.ToString();

        if (SpecialCase(coor) && ((GridMapping[coor] == 'G') || (GridMapping[coor] == 'A'))) 
        {
            if (LockPath(x, z))
            {
                GridMapping[coor] = 'L';

                char stateR = GridMapping[(x + 2).ToString() + ":" + (z).ToString()];
                if ((stateR == 'G') || (stateR == 'A'))
                    GridMapping[(x + 2).ToString() + ":" + (z).ToString()] = 'L';

                char stateL = GridMapping[(x - 2).ToString() + ":" + (z).ToString()];
                if ((stateL == 'G') || (stateL == 'A'))
                    GridMapping[(x - 2).ToString() + ":" + (z).ToString()] = 'L';

                char stateU = GridMapping[(x).ToString() + ":" + (z + 2).ToString()];
                if ((stateU == 'G') || (stateU == 'A'))
                    GridMapping[(x).ToString() + ":" + (z + 2).ToString()] = 'L';

                char stateD = GridMapping[(x).ToString() + ":" + (z - 2).ToString()];
                if ((stateD == 'G') || (stateD == 'A'))
                    GridMapping[(x).ToString() + ":" + (z - 2).ToString()] = 'L';

                UpdateGrid(x + 2, z + 2);
                UpdateGrid(x + 2, z);
                UpdateGrid(x + 2, z - 2);
                UpdateGrid(x, z - 2);
                UpdateGrid(x - 2, z - 2);
                UpdateGrid(x - 2, z);
                UpdateGrid(x - 2, z + 2);
                UpdateGrid(x, z + 2);
            }
            else
            {
                GridMapping[coor] = 'A';
            }
        }
    }

    public bool SpecialCase(string coord)
    {
        bool result = false;

        if ((GridState[coord] & 0x05) == 0x05)
            result = true;
        else if ((GridState[coord] & 0x09) == 0x09)
            result = true;
        else if ((GridState[coord] & 0x11) == 0x11)
            result = true;
        else if ((GridState[coord] & 0x21) == 0x21)
            result = true;
        else if ((GridState[coord] & 0x12) == 0x12)
            result = true;
        else if ((GridState[coord] & 0x22) == 0x22)
            result = true;
        else if ((GridState[coord] & 0x14) == 0x14)
            result = true;
        else if ((GridState[coord] & 0x24) == 0x24)
            result = true;
        else if ((GridState[coord] & 0x41) == 0x41)
            result = true;
        else if ((GridState[coord] & 0x42) == 0x42)
            result = true;
        else if ((GridState[coord] & 0x44) == 0x44)
            result = true;
        else if ((GridState[coord] & 0x48) == 0x48)
            result = true;
        else if ((GridState[coord] & 0x50) == 0x50)
            result = true;
        else if ((GridState[coord] & 0x84) == 0x84)
            result = true;
        else if ((GridState[coord] & 0x88) == 0x88)
            result = true;
        else if ((GridState[coord] & 0x90) == 0x90)
            result = true;

        return result;
    }

    public bool LockPath(float x, float z)
    {
        bool noPath = true;

        bool accessR = false;
        bool accessL = false;
        bool accessU = false; 
        bool accessD = false;

        float i = 2;
        while (GridMapping[(x + i).ToString() + ":" + (z).ToString()] != 'W')
        {
            char state = GridMapping[(x + i).ToString() + ":" + (z).ToString()];
            if (state == 'A' || state == 'G' || state == 'L')
            {
                accessR = true;
                Debug.Log("trueR");
                break;
            }
            i += 2;
        }

        i = 2;
        while (GridMapping[(x - i).ToString() + ":" + (z).ToString()] != 'W')
        {
            char state = GridMapping[(x - i).ToString() + ":" + (z).ToString()];
            if (state == 'A' || state == 'G' || state == 'L')
            {
                accessL = true;
                Debug.Log("trueL");
                break;
            }
            i += 2;
        }

        i = 2;
        while (GridMapping[(x).ToString() + ":" + (z + i).ToString()] != 'W')
        {
            char state = GridMapping[(x).ToString() + ":" + (z + i).ToString()];
            if (state == 'A' || state == 'G' || state == 'L')
            {
                accessU = true;
                Debug.Log("trueU");
                break;
            }
            i += 2;
        }

        i = 2;
        while (GridMapping[(x).ToString() + ":" + (z - i).ToString()] != 'W')
        {
            char state = GridMapping[(x).ToString() + ":" + (z - i).ToString()];
            if (state == 'A' || state == 'G' || state == 'L')
            {
                accessD = true;
                Debug.Log("trueD");
                break;
            }
            i += 2;
        }

        if ((accessR || accessL) && (accessU || accessD))
            noPath = false;

        return noPath;
    }
}
