using UnityEngine;

public class TransparentHome : MonoBehaviour
{
    private SpriteMask spriteMask;

    private void Start()
    {
        spriteMask = GetComponent<SpriteMask>();

        if (spriteMask == null)
        {
            Debug.LogWarning("SpriteMask component missing from this GameObject.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spriteMask == null) return;

        if (collision.CompareTag("Home"))
        {
            spriteMask.enabled = true;
            PlaySFX(AudioManager.Instance?.openDoor_sfx);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (spriteMask == null) return;

        if (collision.CompareTag("Home"))
        {
            spriteMask.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (spriteMask == null) return;

        if (collision.CompareTag("Home"))
        {
            PlaySFX(AudioManager.Instance?.closeDoor_sfx);
            spriteMask.enabled = false;
        }
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clip);
        }
        else if (clip == null)
        {
            Debug.LogWarning("Audio clip is missing.");
        }
        else
        {
            Debug.LogWarning("AudioManager instance is missing.");
        }
    }
}
