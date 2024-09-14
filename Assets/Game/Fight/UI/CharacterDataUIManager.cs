using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDataUIManager : Singleton<CharacterDataUIManager>
{
    [SerializeField] private TextMeshProUGUI apValue;
    [SerializeField] private TextMeshProUGUI mpValue;
    [SerializeField] private TextMeshProUGUI hpValue;

    private void Awake() {
        
    }
    
    internal void SetHudValues(int _hp, int _ap, int _mp)
    {
        apValue.text = _ap.ToString();
        mpValue.text = _mp.ToString();
        hpValue.text = _hp.ToString();
    }

}
