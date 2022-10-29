using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource[] m_AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponents<AudioSource>();
        m_AudioSource[0].Play();
        m_AudioSource[1].PlayDelayed(m_AudioSource[0].clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
