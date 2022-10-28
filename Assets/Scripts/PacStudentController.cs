using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum InputKey
{
    Up,Down,Left,Right
}
public class PacStudentController : MonoBehaviour
{
    private Transform PacStudent;
    private Animator animator;

    private InputKey lastInput = InputKey.Up;
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
        Move(lastInput);
        if (Input.GetKeyDown(KeyCode.W))
            lastInput = InputKey.Up;
        else if (Input.GetKeyDown(KeyCode.S))
            lastInput = InputKey.Down;
        else if (Input.GetKeyDown(KeyCode.A))
            lastInput = InputKey.Left;
        else if (Input.GetKeyDown(KeyCode.D))
            lastInput = InputKey.Right;

    }
    private void Move(InputKey input)
    {
        // top
        if (input==InputKey.Up)
        {
            animator.SetFloat("dir", 0.0f);
            PacStudent.position += Vector3.up * Time.deltaTime;
        }
        else if (input == InputKey.Right)
        {
            // right
            animator.SetFloat("dir", 3.0f);
            PacStudent.position += Vector3.right * Time.deltaTime;
        }
        else if (input == InputKey.Down)
        {
            // down
            animator.SetFloat("dir", 2.0f);
            PacStudent.position += Vector3.down * Time.deltaTime;
        }
        else if (input == InputKey.Left)
        {
            // left
            animator.SetFloat("dir", 1.0f);
            PacStudent.position += Vector3.left * Time.deltaTime;
        }
        else
        {
            animator.SetFloat("dir", 0.0f);
        }
    }
}
