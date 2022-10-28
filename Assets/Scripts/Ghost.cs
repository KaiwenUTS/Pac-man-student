using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostState
{
    Walking,Scared,Recovering, Dead
}
public class Ghost : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private GhostState state = GhostState.Walking;
    private void Awake()
    {
        startPos = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pacman"))
        {
            PacStudentController pacStudent = collision.GetComponent<PacStudentController>();
            if (pacStudent)
            {
                if(state==GhostState.Walking)
                {
                    pacStudent.Die();
                }
                else if(state==GhostState.Scared||state==GhostState.Recovering)
                {
                    GameManager.Instance.AddScore(300);
                    GetComponent<Collider2D>().enabled = false;
                    StartCoroutine(Respawn());
                }
            }
        }
    }
    public void SetGhostState(GhostState state)
    {
        if (this.state != state)
        {
            if (state == GhostState.Walking)
                animator.SetTrigger("WalkingTrigger");
            else if (state == GhostState.Scared)
                animator.SetTrigger("ScaredTrigger");
            else if (state == GhostState.Recovering)
                animator.SetTrigger("RecoveringTrigger");
            else if (state == GhostState.Dead)
                animator.SetTrigger("DeadTrigger");
            this.state = state;
        }
    }
    private Vector3 startPos;
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5);
        transform.position = startPos;
        GetComponent<Collider2D>().enabled = true;
        animator.SetTrigger("WalkingTrigger");
    }
}
