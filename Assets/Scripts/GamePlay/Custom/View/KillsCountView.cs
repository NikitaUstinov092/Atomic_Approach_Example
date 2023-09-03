using TMPro;
using UnityEngine;
using Zenject;

public class KillsCountView : MonoBehaviour, IInitListener, IDisableListener
{
    [SerializeField]
    private TextMeshProUGUI _text;
    
    [Inject]
    private IKillCounterPM _pm;
    
    private const string TITLE = "KILLS: ";
    void IInitListener.OnInit()
    {
        _pm.OnValueChanged += UpdateGUI;
    }
    void IDisableListener.Disable()
    {
        _pm.OnValueChanged -= UpdateGUI;
    }
    private void UpdateGUI(int kills)
    {
        _text.text = TITLE + kills;
    }
}
