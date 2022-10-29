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
        if (!GameManager.Instance.IsOn)
            return;
        if (new Vector2(targetPos.x * levelGenerator.delta - transform.position.x, targetPos.y * levelGenerator.delta - transform.position.y).magnitude <= 0.01f)
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
    [SerializeField]
    private float speed = 0.1f;
    private void Move(InputKey input)
    {
        // top
        if (input==InputKey.Up)
        {
            animator.SetFloat("dir", 0.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, speed);
        }
        else if (input == InputKey.Right)
        {
            // right
            animator.SetFloat("dir", 3.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, speed);
        }
        else if (input == InputKey.Down)
        {
            // down
            animator.SetFloat("dir", 2.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, speed);
        }
        else if (input == InputKey.Left)
        {
            // left
            animator.SetFloat("dir", 1.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, speed);
        }
        //else
        //{
        //    animator.SetFloat("dir", 0.0f);
        //}
    }
    public void Teleport(Vector2Int pos)
    {
        transform.position = new Vector3(pos.x*levelGenerator.delta, pos.y * levelGenerator.delta,0);
        targetPos = pos;
    }
    private Vector2Int startPos = new Vector2Int(-12, 13);
    private int lifes = 3;
    private void Respawn()
    {
        curentInput = InputKey.Up;
        lastInput = InputKey.Up;
        Teleport(startPos);
        animator.SetFloat("dir", 0.0f);
    }
    public void Die()
    {
        --lifes;
        GameManager.Instance.UpdateLife(lifes);
        if (lifes == 0)
            GameManager.Instance.GameOver();
        else
            Respawn();
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
