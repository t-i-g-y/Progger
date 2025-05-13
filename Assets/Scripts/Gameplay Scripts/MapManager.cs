using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public List<ZoneImageEntry> zones;

    private Dictionary<ProgrammingLanguage, ZoneImageEntry> zoneLookup;

    private void Awake()
    {
        zoneLookup = new Dictionary<ProgrammingLanguage, ZoneImageEntry>();
        foreach (var zone in zones)
        {
            zone.zoneImage.sprite = zone.unknownSprite;
            zone.zoneImage.GetComponent<Button>().onClick.AddListener(() => OnZoneClicked(zone.language));
            if (zone.zoomPage != null)
                zone.zoomPage.SetActive(false);

            zoneLookup.Add(zone.language, zone);
        }
    }

    public void RevealZone(ProgrammingLanguage lang)
    {
        if (zoneLookup.TryGetValue(lang, out var entry))
        {
            entry.zoneImage.sprite = entry.revealedSprite;
        }
    }

    private void OnZoneClicked(ProgrammingLanguage lang)
    {
        if (zoneLookup.TryGetValue(lang, out var entry))
        {
            if (entry.zoneImage.sprite == entry.revealedSprite && entry.zoomPage != null)
            {
                entry.zoomPage.SetActive(true);
            }
        }
    }

    public void CloseAllZoomPages()
    {
        foreach (var entry in zones)
        {
            if (entry.zoomPage != null)
                entry.zoomPage.SetActive(false);
        }
    }
}
