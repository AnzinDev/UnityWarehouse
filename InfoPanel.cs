using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour, Assets.IObserver
{
    public Text[] place;

    void Assets.IObserver.Update(object o)
    {
        UpdateUI(o);
    }

    internal void UpdateUI(object o)
    {
        string note = o as string;
        string subNote = note.Substring(5);
        if (note.StartsWith("info:") && place[0].text != subNote)
        {
            for (int i = 0; i < place.Length - 1; i++)
            {
                place[place.Length - 1 - i].text = place[place.Length - 1 - (i + 1)].text;
            }
            place[0].text = subNote;
        }
    }
}
