using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TriggerAudioController : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSources;  // Liste der Audioquellen in der Trigger Box
    [SerializeField] private AudioSource speaker;             // Speaker, der die Audios abspielt
    [SerializeField] private float fadeDuration = 1f;         // Dauer für Fade-In und Fade-Out

    private Coroutine fadeCoroutine;                          // Speichert aktuelle Fade-Coroutine, um Konflikte zu vermeiden

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayRandomAudioWithFadeIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeOutAudio();
        }
    }

    private void PlayRandomAudioWithFadeIn()
    {
        if (audioSources.Count == 0 || speaker == null)
        {
            Debug.LogWarning("Keine Audioquellen oder Speaker zugewiesen.");
            return;
        }

        // Zufällige Audioquelle auswählen
        int randomIndex = Random.Range(0, audioSources.Count);
        AudioClip selectedClip = audioSources[randomIndex].clip;
        speaker.clip = selectedClip;
        
        // Falls ein Fade läuft, beenden, um Konflikte zu vermeiden
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Lautstärke auf 0 setzen, dann Clip starten und mit Fade-In erhöhen
        speaker.volume = 0;
        speaker.Play();
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    private void FadeOutAudio()
    {
        if (speaker != null && speaker.isPlaying)
        {
            // Falls ein Fade läuft, beenden, um Konflikte zu vermeiden
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            
            // Startet Fade-Out
            fadeCoroutine = StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        float targetVolume = 1f;  // Ziel-Lautstärke
        float startVolume = speaker.volume;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            speaker.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / fadeDuration);
            yield return null;
        }

        speaker.volume = targetVolume;  // Stellt sicher, dass die Lautstärke am Ende exakt erreicht wird
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

        speaker.volume = 0;  // Stellt sicher, dass die Lautstärke am Ende genau auf 0 ist
        speaker.Stop();      // Stoppt die Wiedergabe vollständig
    }
}
