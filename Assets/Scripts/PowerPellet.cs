using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pacman"))
        {
            PacStudentController pacStudent = collision.GetComponent<PacStudentController>();
            if (pacStudent)
            {
                GameManager.Instance.PowerStart();
                Destroy(gameObject);
            }
        }
    }
}
