using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Transform PacStudent;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        PacStudent = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // top
        if (PacStudent.position.y < 4.06f && PacStudent.position.x < -4.4f && animator.GetFloat("dir") == 0.0f)
        {
            animator.SetFloat("dir", 0.0f);
            PacStudent.position += new Vector3(0.0f, 1.0f, 0.0f) * Time.deltaTime;
        }
        else if (PacStudent.position.x < -4.4f && PacStudent.position.y >= 4.06f)
        {
            // right
            animator.SetFloat("dir", 3.0f);
            PacStudent.position += new Vector3(1.0f, 0.0f, 0.0f) * Time.deltaTime;
        }
        else if (PacStudent.position.y > 1.2f)
        {
            // down
            animator.SetFloat("dir", 2.0f);
            PacStudent.position -= new Vector3(0.0f, 1.0f, 0.0f) * Time.deltaTime;
        }
        else if (PacStudent.position.x > -8.8f && animator.GetFloat("dir") >= 1.0f)
        {
            // left
            animator.SetFloat("dir", 1.0f);
            PacStudent.position -= new Vector3(1.0f, 0.0f, 0.0f) * Time.deltaTime;
        }
        else
        {
            animator.SetFloat("dir", 0.0f);
        }

    }
}
