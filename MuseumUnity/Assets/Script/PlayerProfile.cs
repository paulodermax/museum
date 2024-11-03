using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProfileValue
{
    public string tag;  // Der Name des Wertes (z. B. "waffen")
    public int value;   // Der aktuelle Wert (zwischen 0 und 10)
}

public class PlayerProfile : MonoBehaviour
{
    // Liste, die alle Tags und Werte speichert und im Inspector sichtbar ist
    public List<ProfileValue> values = new List<ProfileValue>();

    private void Start()
    {
        InitializeValues();
    }

    // Methode zur Initialisierung der Werte mit 0 aus allen WerteBoxen in der Szene
    private void InitializeValues()
    {
        // Finde alle WerteBoxen in der Szene
        WerteBox[] allWerteBoxes = FindObjectsOfType<WerteBox>();

        foreach (WerteBox box in allWerteBoxes)
        {
            // Überprüfe, ob der Tag bereits existiert
            ProfileValue existingValue = values.Find(v => v.tag == box.profileTag);

            if (existingValue == null)
            {
                // Füge einen neuen Wert mit dem Tag und dem Wert 0 hinzu
                ProfileValue newValue = new ProfileValue { tag = box.profileTag, value = 0 };
                values.Add(newValue);
            }
        }
    }

    // Methode zum Erhöhen eines Wertes basierend auf dem Tag
    public void IncreaseValue(string tag)
    {
        // Prüfen, ob der Tag bereits in der Liste existiert
        ProfileValue existingValue = values.Find(v => v.tag == tag);

        if (existingValue != null)
        {
            // Erhöhe den Wert nur, wenn er noch 0 ist
            if (existingValue.value == 0)
            {
                existingValue.value = 1;
                Debug.Log($"Wert für '{tag}' wurde auf 1 gesetzt.");
            }
        }
        else
        {
            // Füge neuen Wert hinzu, wenn er nicht existiert
            ProfileValue newValue = new ProfileValue { tag = tag, value = 1 };
            values.Add(newValue);
            Debug.Log($"Neuer Wert '{tag}' hinzugefügt und auf 1 gesetzt.");
        }
    }

    // Methode zur Anzeige der aktuellen Werte im Inspector (optional für Debugging)
    public void ShowValues()
    {
        foreach (var profileValue in values)
        {
            Debug.Log($"{profileValue.tag}: {profileValue.value}");
        }
    }
}
