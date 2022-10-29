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
    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private AudioSource hitAudio;
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsOn && !isDead)
            return;
        if (new Vector2(targetPos.x * levelGenerator.delta - transform.position.x, targetPos.y * levelGenerator.delta - transform.position.y).magnitude <= 0.01f)
        {
            lastPos = targetPos;
            levelGenerator.SetVisit(targetPos.x, targetPos.y);
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
        {
            if (curentInput != InputKey.None)
            {
                hitAudio.Play();
                Instantiate(hitEffect, new Vector3(transform.position.x + curInputTarget.x * levelGenerator.delta, transform.position.y + curInputTarget.y * levelGenerator.delta, 0) * 0.5f, Quaternion.identity);
            }
            curentInput = InputKey.None;
        }
        PlayerMovingAudio(targetPos);
        Move(curentInput);
    }
    [SerializeField]
    private float speed = 0.1f;
    [SerializeField]
    private AudioSource movingAudioSrc;
    [SerializeField]
    private AudioClip[] audioClips;
    private void PlayerMovingAudio(Vector2Int curTatgetPos)
    {
        if (curentInput == InputKey.None)
        {
            movingAudioSrc.Pause();
            return;
        }
        int curInputTargetType = levelGenerator.GetTypeFromXY(curTatgetPos.x, curTatgetPos.y);
        if (!levelGenerator.GetVisit(curTatgetPos.x, curTatgetPos.y) && (curInputTargetType == 5 || curInputTargetType == 6))
            movingAudioSrc.clip = audioClips[1];
        else
            movingAudioSrc.clip = audioClips[0];
        if (!movingAudioSrc.isPlaying)
            movingAudioSrc.Play();
        else
            movingAudioSrc.UnPause();
    }
    private void Move(InputKey input)
    {
        // top
        if (input==InputKey.Up)
        {
            animator.SetFloat("dir", 0.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, speed * Time.deltaTime);
        }
        else if (input == InputKey.Right)
        {
            // right
            animator.SetFloat("dir", 3.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, speed * Time.deltaTime);
        }
        else if (input == InputKey.Down)
        {
            // down
            animator.SetFloat("dir", 2.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, speed * Time.deltaTime);
        }
        else if (input == InputKey.Left)
        {
            // left
            animator.SetFloat("dir", 1.0f);
            PacStudent.position = Vector3.Lerp(PacStudent.position, new Vector3(targetPos.x, targetPos.y, 0) * levelGenerator.delta, speed * Time.deltaTime);
        }
        //else
        //{
        //    animator.SetFloat("dir", 0.0f);
        //}
    }
    [SerializeField]
    private GameObject dustEffect;
    public void Teleport(Vector2Int pos)
    {
        dustEffect.SetActive(false);
        transform.position = new Vector3(pos.x*levelGenerator.delta, pos.y * levelGenerator.delta,0);
        targetPos = pos;
        dustEffect.SetActive(true);
    }
    private Vector2Int startPos = new Vector2Int(-12, 13);
    private int lifes = 3;
    [SerializeField]
    private GameObject deadEffect;
    private bool isDead = false;
    private void Respawn()
    {
        curentInput = InputKey.None;
        lastInput = InputKey.None;
        GetComponent<Collider2D>().enabled = false;
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        StartCoroutine(WaitForRespawn());
    }
    private IEnumerator WaitForRespawn()
    {
        isDead = true;
        yield return new WaitForSeconds(1);
        Teleport(startPos);
        animator.SetFloat("dir", 0.0f);
        GetComponent<Collider2D>().enabled = true;
        isDead = false;
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
