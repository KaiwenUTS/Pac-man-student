using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    int[,] levelMap =
        {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
        };

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] manual_lvl = GameObject.FindGameObjectsWithTag("Manual-lvl");
        foreach(GameObject go in manual_lvl)
        {
            GameObject.Destroy(go);
        }

        // find template object
        GameObject[] templates = new GameObject[8];
        templates[1] = GameObject.Find("Outer Corner template");
        templates[2] = GameObject.Find("Outer Wall template");
        templates[3] = GameObject.Find("Inside Corner template");
        templates[4] = GameObject.Find("Inner Wall template");
        templates[5] = GameObject.Find("Pellet template");
        templates[6] = GameObject.Find("Power Pellet template");
        templates[7] = GameObject.Find("t conjunct template");

        float cur_row = 0f;
        float cur_col = 0f;
        GameObject topleft = new GameObject();
        topleft.name = "topleft";
        topleft.transform.position = Vector3.zero;
        for (int i = 0; i < levelMap.GetLength(0); ++i)
        {
            for(int j = 0; j < levelMap.GetLength(1); ++j)
            {
                int type = levelMap[i, j];
                if (type == 0)
                {
                    cur_col += 0.4f;
                    continue;
                }
                GameObject curobj = GameObject.Instantiate(templates[type]);
                curobj.transform.position = new Vector3(cur_col, cur_row, 0f);
                curobj.transform.parent = topleft.transform;
                if (type == 1)
                {
                    int[] types = new int[] { 2 };
                    Vector3 wall_check = checkRotation(i, j, types);
                    if(wall_check != Vector3.zero)
                    {
                        curobj.transform.rotation = Quaternion.Euler(wall_check);
                    }
                    else
                    {
                        types = new int[] { 1, 2 };
                        wall_check = checkRotation(i, j, types);
                        curobj.transform.rotation = Quaternion.Euler(wall_check);
                    }
                }
                else if(type == 3)
                {
                    int[] types = new int[] { 4 };
                    Vector3 wall_check = checkRotation(i, j, types);
                    if (wall_check != Vector3.zero)
                    {
                        curobj.transform.rotation = Quaternion.Euler(wall_check);
                    }
                    else
                    {
                        types = new int[] { 3, 4 };
                        wall_check = checkRotation(i, j, types);
                        curobj.transform.rotation = Quaternion.Euler(wall_check);
                    }
                }
                curobj.GetComponent<SpriteRenderer>().enabled = true;
                if (curobj.GetComponent<Animator>())
                {
                    curobj.GetComponent<Animator>().enabled = true;
                }
                cur_col += 0.4f;
            }
            cur_col = 0f;
            cur_row -= 0.4f; 
        }
        // get top-right
        GameObject topright = GameObject.Instantiate(topleft);
        topright.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        topright.transform.position = new Vector3(topleft.transform.position.x + 2 * (0.4f * levelMap.GetLength(1)) - 0.2f, topleft.transform.position.y, topleft.transform.position.z);
        GameObject top = new GameObject("top");
        topleft.transform.parent = top.transform;
        topright.transform.parent = top.transform;
        GameObject bottom = GameObject.Instantiate(top);
        bottom.transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, 0f));
        bottom.transform.position = new Vector3(top.transform.position.x, top.transform.position.y - 2 * (0.4f * levelMap.GetLength(1)) + 0.2f, top  .transform.position.z);
        GameObject level = new GameObject("level");
        top.transform.parent = level.transform;
        bottom.transform.parent = level.transform;
        level.transform.position = new Vector3(-10f, 4.5f, 0f );
    
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
