
using UnityEngine;
using UnityEngine.Video;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private AudioSource m_source;
    [SerializeField] private Collider m_collider;
    [SerializeField] private VideoPlayer m_player;

    private void OnTriggerEnter(Collider other)
    {
        Camera cam = other.GetComponent<Camera>();
        if (cam == null) return;

        OpenDoor();
    }

    public void OpenDoor()
    {
        m_animator.SetTrigger("Open");
        if(m_source != null) m_source.Play();
        if (m_collider != null) m_collider.enabled = false;

        Invoke("StartVideo", 0.3f);
    }

    private void StartVideo()
    {
        m_player.Play();
    }
}
