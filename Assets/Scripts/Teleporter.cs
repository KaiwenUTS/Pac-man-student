using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private Vector2Int telePos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pacman"))
        {
            PacStudentController pacStudent = collision.GetComponent<PacStudentController>();
            if (pacStudent)
            {
                pacStudent.Teleport(telePos);
            }
        }
    }
}
