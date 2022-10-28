using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public float delta = 0.4f;
    int[,] levelMap =
        {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,4},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,4,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,4,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {0,0,0,0,0,2,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,2,5,0,0,0,4,0,0,0},
        };

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] manual_lvl = GameObject.FindGameObjectsWithTag("Manual-lvl");
        foreach(GameObject go in manual_lvl)
            GameObject.Destroy(go);

        // find template object
        GameObject[] templates = new GameObject[8];
        templates[1] = GameObject.Find("Outer Corner template");
        templates[2] = GameObject.Find("Outer Wall template");
        templates[3] = GameObject.Find("Inside Corner template");
        templates[4] = GameObject.Find("Inner Wall template");
        templates[5] = GameObject.Find("Pellet template");
        templates[6] = GameObject.Find("Power Pellet template");
        templates[7] = GameObject.Find("t conjunct template");

        //float cur_row = 0f;
        //float cur_col = 0f;
        GameObject level = new GameObject();
        level.name = "Level";
        level.transform.position = Vector3.zero;
        for (int x = 1 - levelMap.GetLength(1); x <= levelMap.GetLength(1) - 1; ++x)//
        {
            for(int y = 1 - levelMap.GetLength(0); y <= levelMap.GetLength(0) - 1; ++y)//
            {
                
                int type = GetTypeFromXY(x, y);
                if (type == 0)
                    continue;
                GameObject curobj = GameObject.Instantiate(templates[type]);
                curobj.transform.position = new Vector3(x * delta, y * delta, 0f);
                curobj.transform.parent = level.transform;
                if (type == 1)
                {
                    if (GetTypeFromXY(x - 1,y) == 2)
                        curobj.transform.Rotate(Vector3.up, 180);
                    if (GetTypeFromXY(x, y + 1) == 2)
                        curobj.transform.Rotate(Vector3.right, 180);
                }
                if (type == 3)
                {
                    if (GetTypeFromXY(x - 1, y) == 4 || GetTypeFromXY(x - 1, y) == 3)
                        curobj.transform.Rotate(Vector3.up, 180);
                    if (GetTypeFromXY(x, y + 1) == 4 || GetTypeFromXY(x, y + 1) == 3)
                        curobj.transform.Rotate(Vector3.right, 180);
                }
                curobj.GetComponent<SpriteRenderer>().enabled = true;
                if (curobj.GetComponent<Animator>())
                {
                    curobj.GetComponent<Animator>().enabled = true;
                }
            }
        }
    }
    public int GetTypeFromXY(int x,int y)
    {
        int absI = levelMap.GetLength(0) - 1 - (y < 0 ? -y : y), absJ = levelMap.GetLength(1) - 1 - (x < 0 ? -x : x);
        if (absI < 0 || absJ < 0 || absI >= levelMap.GetLength(0) || absJ >= levelMap.GetLength(1))
            return 0;
        return levelMap[absI, absJ];
    }
    Vector3 checkRotation(int i, int j, int[] targetType)
    {
        var rotationVector = Vector3.zero;
        int right = (j == levelMap.GetLength(1) - 1) ? -1 : levelMap[i, j + 1];
        int left = (j == 0) ? -1 : levelMap[i, j - 1];
        int top = (i == 0) ? -1 : levelMap[i - 1, j];
        int bottom = (i == levelMap.GetLength(0) - 1) ? -1 : levelMap[i + 1, j];

        if(targetType.Contains(right) && targetType.Contains(bottom))
        {
            return rotationVector;
        }else if(targetType.Contains(left) && targetType.Contains(bottom))
        {
            rotationVector.y = 180f;
            return rotationVector;
        }else if(targetType.Contains(left) && targetType.Contains(top))
        {
            rotationVector.x = 180f;
            rotationVector.y = 180f;
            return rotationVector;
        }
        else if(targetType.Contains(right) && targetType.Contains(top))
        {
            rotationVector.x = 180f;
            return rotationVector;
        }
        return rotationVector;
    }
}
