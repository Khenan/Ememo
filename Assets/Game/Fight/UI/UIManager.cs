using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TMP_Text apValue;
    [SerializeField] private TMP_Text mpValue;
    [SerializeField] private TMP_Text hpValue;

    internal void SetHudValues(int _hp, int _ap, int _mp)
    {
        apValue.text = _ap.ToString();
        mpValue.text = _mp.ToString();
        hpValue.text = _hp.ToString();
    }

}
