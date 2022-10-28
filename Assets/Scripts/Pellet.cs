using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pacman"))
        {
            PacStudentController pacStudent = collision.GetComponent<PacStudentController>();
            if (pacStudent)
            {
                GameManager.Instance.AddScore(10);
                Destroy(gameObject);
            }
        }
    }
}
