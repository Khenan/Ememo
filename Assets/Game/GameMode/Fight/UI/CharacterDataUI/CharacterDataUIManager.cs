using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDataUIManager : Singleton<CharacterDataUIManager>
{
    [SerializeField] private Transform apPanel;
    [SerializeField] private TextMeshProUGUI apValue;
    [SerializeField] private Transform mpPanel;
    [SerializeField] private TextMeshProUGUI mpValue;
    [SerializeField] private Transform hpPanel;
    [SerializeField] private TextMeshProUGUI hpValue;

    internal void SetHudValues(bool _onFight, int _hp, int _ap, int _mp)
    {
        apPanel.gameObject.SetActive(_onFight);
        mpPanel.gameObject.SetActive(_onFight);
        apValue.text = _ap.ToString();
        mpValue.text = _mp.ToString();
        hpValue.text = _hp.ToString();
    }

}
