using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum InputKey
{
    None, Up, Down,Left,Right
}
public class PacStudentController : MonoBehaviour
{
    private Transform PacStudent;
    private Animator animator;

    [SerializeField]
    private LevelGenerator levelGenerator;
    [SerializeField]
    private Vector2Int lastPos = new Vector2Int(-12, 13), targetPos = new Vector2Int(-12, 13);
    [SerializeField]
    private InputKey lastInput = InputKey.Up;
    [SerializeField]
    private InputKey curentInput = InputKey.Up;
    // Start is called before the first frame update
    void Start()
    {
        PacStudent = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        #region
        //int targetType;
        ////到达目标点时，更新位置，如果lastInput目标可达，更新curentInput
        //if (new Vector2(targetPos.x*levelGenerator.delta-transform.position.x, targetPos.y * levelGenerator.delta - transform.position.y).magnitude<=0.0001f)
        //{
        //    lastPos = targetPos;
        //    transform.position = new Vector3(targetPos.x * levelGenerator.delta, targetPos.y * levelGenerator.delta, 0);
        //    Vector2Int lastInputTarget = GetTargetPos(lastInput);
        //    targetType = levelGenerator.GetTypeFromXY(lastInputTarget.x, lastInputTarget.y);
        //    if (targetType == 0 || targetType == 5 || targetType == 6)
        //    {
        //        curentInput = lastInput;
        //        targetPos = lastInputTarget;
        //    }
        //}
        ////未到达目标点时
        ////else
        //{
        //    //基于curentInput的目标点
        //    Vector2Int curInputTarget = GetTargetPos(curentInput);
        //    targetType = levelGenerator.GetTypeFromXY(curInputTarget.x, curInputTarget.y);
        //    //判断目标点可达与否，来更新curentInput
        //    if (targetType == 0 || targetType == 5 || targetType == 6)
        //    {
        //        targetPos = curInputTarget;
        //        curentInput = lastInput;
        //    }
        //    Move(curentInput);
        //}
        #endregion
        if (new Vector2(targetPos.x * levelGenerator.delta - transform.position.x, targetPos.y * levelGenerator.delta - transform.position.y).magnitude <= 0.0001f)
        {
            lastPos = targetPos;
            Vector2Int lastInputTarget = GetTargetPos(lastInput);
            if (IsWalkable(lastInputTarget.x, lastInputTarget.y))
                curentInput = lastInput;
        }
        //输入更新lastInput
        if (Input.GetKeyDown(KeyCode.W))
            lastInput = InputKey.Up;
        else if (Input.GetKeyDown(KeyCode.S))
            lastInput = InputKey.Down;
        else if (Input.GetKeyDown(KeyCode.A))
            lastInput = InputKey.Left;
        else if (Input.GetKeyDown(KeyCode.D))
            lastInput = InputKey.Right;
        //curentInput = lastInput;
        Vector2Int curInputTarget = GetTargetPos(curentInput);
        if (IsWalkable(curInputTarget.x, curInputTarget.y))
            targetPos = curInputTarget;
        else
            curentInput = InputKey.None;
        Move(curentInput);
    }
    private void Move(InputKey input)
    {
        // top
        if (input==InputKey.Up)
        {
            animator.SetFloat("dir", 0.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, 0.1f);
        }
        else if (input == InputKey.Right)
        {
            // right
            animator.SetFloat("dir", 3.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, 0.1f);
        }
        else if (input == InputKey.Down)
        {
            // down
            animator.SetFloat("dir", 2.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, 0.1f);
        }
        else if (input == InputKey.Left)
        {
            // left
            animator.SetFloat("dir", 1.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, 0.1f);
        }
        //else
        //{
        //    animator.SetFloat("dir", 0.0f);
        //}
    }
    //基于输入求解目标点
    private Vector2Int GetTargetPos(InputKey input)
    {
        Vector2Int targetPos=lastPos;
        if (input == InputKey.Up)
            targetPos = lastPos + Vector2Int.up;
        else if (input == InputKey.Down)
            targetPos = lastPos + Vector2Int.down;
        else if (input == InputKey.Left)
            targetPos = lastPos + Vector2Int.left;
        else if (input == InputKey.Right)
            targetPos = lastPos + Vector2Int.right;
        return targetPos;
    }
    private bool IsWalkable(int x,int y)
    {
        int targetType = levelGenerator.GetTypeFromXY(x, y);
        if (targetType == 0 || targetType == 5 || targetType == 6)
            return true;
        return false;
    }
}
