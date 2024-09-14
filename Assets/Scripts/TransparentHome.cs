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
        
        if (spriteMask != null && collision.CompareTag("Home"))
        {
            spriteMask.enabled = true;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.openDoor_sfx);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (spriteMask != null && collision.CompareTag("Home"))
        {
            spriteMask.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (spriteMask != null && collision.CompareTag("Home"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.closeDoor_sfx);
            spriteMask.enabled = false;
        }
    }
}
