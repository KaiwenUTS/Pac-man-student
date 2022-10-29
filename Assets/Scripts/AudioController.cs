using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    static public AudioController Instance;
    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }
    private AudioSource[] m_AudioSource;
    [SerializeField]
    private AudioClip[] audioClips;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponents<AudioSource>();
        m_AudioSource[0].Play();
        m_AudioSource[1].PlayDelayed(m_AudioSource[0].clip.length);
    }
    private int audioIndex = 0;
    public void SetAudio(int index)
    {
        audioIndex = index;
        m_AudioSource[1].clip = audioClips[index];
        if (!m_AudioSource[1].isPlaying)
            m_AudioSource[1].Play();
    }
    public int GetAudioIndex() => audioIndex;
}
