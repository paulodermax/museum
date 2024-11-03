using UnityEngine;

public class WerteBox : MonoBehaviour
{
    public string profileTag;  // Der Tag für den Wert, z.B. "waffen", "rüstung"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Holt das PlayerProfile-Skript des Spielers
            PlayerProfile playerProfile = other.GetComponent<PlayerProfile>();

            if (playerProfile != null)
            {
                // Erhöht den entsprechenden Wert basierend auf dem Tag der Box
                playerProfile.IncreaseValue(profileTag);
            }
            else
            {
                Debug.LogWarning("Kein PlayerProfile auf dem Spieler gefunden.");
            }
        }
    }
}
