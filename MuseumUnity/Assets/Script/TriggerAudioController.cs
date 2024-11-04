using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioCondition
{
    public AudioSource audioSource;  // Die Audioquelle, die abgespielt werden soll
    public string requiredTag;       // Der Tag im PlayerProfile, der überprüft wird
    public int requiredValue = 1;    // Der Wert, der im PlayerProfile mindestens erreicht sein muss
}

public class TriggerAudioController : MonoBehaviour
{
    [SerializeField] private List<AudioCondition> audioConditions; // Liste der Audio-Bedingungen
    [SerializeField] private AudioSource speaker;                 // Speaker, der die Audios abspielt
    [SerializeField] private float fadeDuration = 1f;             // Dauer für Fade-In und Fade-Out

    private Coroutine fadeCoroutine;                              // Speichert die aktuelle Fade-Coroutine

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerProfile playerProfile = other.GetComponent<PlayerProfile>();
            if (playerProfile != null)
            {
                PlayAudioBasedOnConditions(playerProfile);
            }
            else
            {
                Debug.LogWarning("Kein PlayerProfile auf dem Spieler gefunden.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeOutAudio();
        }
    }

    private void PlayAudioBasedOnConditions(PlayerProfile profile)
    {
        if (audioConditions.Count == 0 || speaker == null)
        {
            Debug.LogWarning("Keine Audioquellen oder Speaker zugewiesen.");
            return;
        }

        foreach (AudioCondition condition in audioConditions)
        {
            // Überprüfen, ob der Spieler den erforderlichen Wert für den Tag erreicht hat
            ProfileValue value = profile.values.Find(v => v.tag == condition.requiredTag);

            if (value != null && value.value >= condition.requiredValue)
            {
                // Bedingung erfüllt, setze die Audioquelle
                speaker.clip = condition.audioSource.clip;

                // Falls ein Fade läuft, beenden, um Konflikte zu vermeiden
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }

                speaker.volume = 0;
                speaker.Play();
                fadeCoroutine = StartCoroutine(FadeIn());
                return;  // Nur eine Audioquelle abspielen, daher hier abbrechen
            }
        }

        Debug.Log("Keine passende Audioquelle für die gegebenen Bedingungen gefunden.");
    }

    private void FadeOutAudio()
    {
        if (speaker != null && speaker.isPlaying)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        float targetVolume = 1f;
        float startVolume = speaker.volume;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            speaker.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / fadeDuration);
            yield return null;
        }

        speaker.volume = targetVolume;
    }

    private IEnumerator FadeOut()
    {
        float startVolume = speaker.volume;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            speaker.volume = Mathf.Lerp(startVolume, 0, elapsed / fadeDuration);
            yield return null;
        }

        speaker.volume = 0;
        speaker.Stop();
    }
}