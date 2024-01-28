using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SfxPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float volume = 1.0f;

    private void Start()
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to SfxPlayer.");
            return;
        }
    }

    public void PlayRandomSfx()
    {
        GameObject audioObject = new GameObject("AudioObject");
        audioObject.transform.position = transform.position;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.volume = volume;

        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[randomIndex]);
        Debug.Log("Playing Audio: " + audioClips[randomIndex]);

        StartCoroutine(DestroyAudioObject(audioObject, audioClips[randomIndex].length));
    }

    private IEnumerator DestroyAudioObject(GameObject audioObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(audioObject);
    }
}