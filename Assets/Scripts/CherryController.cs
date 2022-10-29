using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = (-transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime);
        if (transform.position.magnitude > 16)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pacman"))
        {
            PacStudentController pacStudent = collision.GetComponent<PacStudentController>();
            if (pacStudent)
            {
                GameManager.Instance.AddScore(100);
                Destroy(gameObject);
            }
        }
    }
}
