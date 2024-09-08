
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource m_Source;
    [SerializeField] AudioSource SfXsource;
    public AudioClip background;
    private void Start()
    {
        m_Source.clip = background;
        m_Source.Play();
    }
}
