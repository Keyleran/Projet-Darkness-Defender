// --------------------------------------------------
// Project: Darkness Defender
// Script: MainManagerScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MuninAlgo : MonoBehaviour
{
    // String <=> Coordonnées Cases "xxx:yyy"
    // Char   <=> Disponibilité de la case (G = Ground, A = Attente, W = Wall, B = Barricace)
    private Dictionary<string, char> _gridMapping;
    public Dictionary<string, char> GridMapping 
    {
        get { return _gridMapping; }
        set { _gridMapping = value; }
    }



    NavMeshAgent _gridMaker;


    public MuninAlgo(NavMeshAgent gridMaker)
    {
        GridMapping = new Dictionary<string, char>();

        _gridMaker = gridMaker;
    }

    public void InitiateGrid()
    {
        // X: -36 -> 40
        // Z: -30 -> 30
        int i = 0;
        int j = 0;


        for (int x = -50; x <= 55; x += 2)
        {
            for (int z = -40; z <= 40; z += 2)
            {
                NavMeshPath path = new NavMeshPath();
                _gridMaker.CalculatePath(new Vector3(x, 0, z), path);
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    GridMapping.Add(x.ToString() + ":" + z.ToString(), 'G');
                    i++;
                }
                else
                {
                    GridMapping.Add(x.ToString() + ":" + z.ToString(), 'W');
                    j++;
                }
            }
        }
        Debug.Log("Access: " + i + ", Denied: " + j);
    }

    public bool accessRequest(float x, float z, Vector3 positionPlayer)
    {
        bool access = false;
        string coor = x.ToString() + ":" + z.ToString();

        GridMapping[coor] = 'A';
        access = AlgoAEtoile(positionPlayer);
        GridMapping[coor] = 'G';

        return access;
    }

    bool AlgoAEtoile(Vector3 cible)
    {
        int cibleX = (int) roundNumber(cible.x);
        int cibleY = (int) roundNumber(cible.z);

        int[]  departx = new int[2]  { -38, 42 };
        int[]  departy = new int[2]  {   0,  0 };
        bool[] acccess = new bool[2] { false, false };
        bool accessGranted = false;

        for(int i = 0; i < 2; i++)
        {
            Dictionary<string, int> GridOuvert = new Dictionary<string, int>();
            List<string> GridFermer = new List<string>();

            int dist = CalculDistance(departx[i], departy[i], cibleX, cibleY);
            GridOuvert.Add(departx[i].ToString() + ":" + departy[i].ToString(), dist);

            while (GridOuvert.Count != 0)
            {
                string closeCase = SelectCloser(GridOuvert, cibleX, cibleY);
                string cibleCoor = cibleX.ToString() + ":" + cibleY.ToString();

                if( closeCase == cibleCoor)
                {
                    acccess[i] = true;
                    break;
                }
                 
                string[] coor = new string[2];
                coor = closeCase.Split(':');

                int[] coordonate = new int[2];
                coordonate[0] = (int) roundNumber(float.Parse(coor[0]));
                coordonate[1] = (int) roundNumber(float.Parse(coor[1]));
                
                string up = (coordonate[0] + 0).ToString() + ":" + (coordonate[1] + 2).ToString();
                if (GridMapping.ContainsKey(up) && GridMapping[up] == 'G')
                {
                    dist = CalculDistance((coordonate[0] + 0), (coordonate[1] + 2), cibleX, cibleY);
                    if (!GridFermer.Contains(up) && !GridOuvert.ContainsKey(up))
                    {
                        GridOuvert.Add(up, dist);
                    }
                    else if(GridOuvert.ContainsKey(up) && GridOuvert[up] > dist)
                    {
                        GridOuvert[up] = dist;
                    }
                }

                string down = (coordonate[0] + 0).ToString() + ":" + (coordonate[1] - 2).ToString();
                if (GridMapping.ContainsKey(down) && GridMapping[down] == 'G')
                {
                    dist = CalculDistance((coordonate[0] + 0), (coordonate[1] - 2), cibleX, cibleY);
                    if (!GridFermer.Contains(down) && !GridOuvert.ContainsKey(down))
                    {
                        GridOuvert.Add(down, dist);
                    }
                    else if (GridOuvert.ContainsKey(down) && GridOuvert[down] > dist)
                    {
                        GridOuvert[down] = dist;
                    }
                }

                string left = (coordonate[0] - 2).ToString() + ":" + (coordonate[1] + 0).ToString();
                if (GridMapping.ContainsKey(left) && GridMapping[left] == 'G')
                {
                    dist = CalculDistance((coordonate[0] - 2), (coordonate[1] + 0), cibleX, cibleY);
                    if (!GridFermer.Contains(left) && !GridOuvert.ContainsKey(left))
                    {
                        GridOuvert.Add(left, dist);
                    }
                    else if (GridOuvert.ContainsKey(left) && GridOuvert[left] > dist)
                    {
                        GridOuvert[left] = dist;
                    }
                }

                string right = (coordonate[0] + 2).ToString() + ":" + (coordonate[1] + 0).ToString();
                if (GridMapping.ContainsKey(right) && GridMapping[right] == 'G')
                {
                    dist = CalculDistance((coordonate[0] + 2), (coordonate[1] + 0), cibleX, cibleY);
                    if (!GridFermer.Contains(right) && !GridOuvert.ContainsKey(right))
                    {
                        GridOuvert.Add(right, dist);
                    }
                    else if (GridOuvert.ContainsKey(right) && GridOuvert[right] > dist)
                    {
                        GridOuvert[right] = dist;
                    }
                }

                GridFermer.Add(closeCase);
                GridOuvert.Remove(closeCase);
            }
        }
        if (acccess[0] && acccess[1])
            accessGranted = true;

        return accessGranted;
    }

    int CalculDistance(int posX, int posY, int endX, int endY)
    {
        int result = 0;
        result += posX > endX ? posX - endX : endX - posX;
        result += posY > endY ? posY - endY : endY - posY;
        return result;
    }

    string SelectCloser(Dictionary<string, int> GridOuvert, int endX, int endY)
    {
        int pos = 1000;
        string resultCloser = "50:50";
         
        foreach (KeyValuePair<string, int> Case in GridOuvert)
        {
            if (Case.Value < pos)
            {
                resultCloser = Case.Key;
                pos = Case.Value;
            }
            else if (Case.Value == pos)
            {
                string[] coor = new string[2];
                coor = resultCloser.Split(':');

                int[] coordonateClose = new int[2];
                coordonateClose[0] = (int)roundNumber(float.Parse(coor[0]));
                coordonateClose[1] = (int)roundNumber(float.Parse(coor[1]));


                string[] coorAct = new string[2];
                coorAct = Case.Key.Split(':');

                int[] coordonateActClose = new int[2];
                coordonateActClose[0] = (int)roundNumber(float.Parse(coorAct[0]));
                coordonateActClose[1] = (int)roundNumber(float.Parse(coorAct[1]));

                int diffCloseX    = coordonateClose[0]    > endX ? coordonateClose[0]    - endX : endX - coordonateClose[0];
                int diffCloseActX = coordonateActClose[0] > endX ? coordonateActClose[0] - endX : endX - coordonateActClose[0];
                int diffCloseY    = coordonateClose[1]    > endY ? coordonateClose[1]    - endY : endY - coordonateClose[1];
                int diffCloseActY = coordonateActClose[1] > endY ? coordonateActClose[1] - endY : endY - coordonateActClose[1];
                if ((diffCloseY > diffCloseActY) || (diffCloseY == diffCloseActY) && (diffCloseX > diffCloseActX))
                {
                    resultCloser = Case.Key;
                    pos = Case.Value;
                }
            }
        }

        return resultCloser;
    }


    public void BuildTower(float x, float z)
    {
        string coor = x.ToString() + ":" + z.ToString();
        GridMapping[coor] = 'T';
    }

    float roundNumber(float value)
    {
        float new_value = 0;

        if (value >= 0)
        {
            new_value = Mathf.Ceil(value);
            if (new_value % 2 != 0)
                new_value--;
        }
        else // Value < 0
        {
            new_value = Mathf.Floor(value);
            if (new_value % 2 != 0)
                new_value++;
        }

        return new_value;
    }
}
