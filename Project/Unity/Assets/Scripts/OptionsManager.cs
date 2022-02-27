// Options
using TMPro;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public TMP_Text buttonPPText;
    public TMP_Text buttonAAText;
    public TMP_Text buttonShadowsText;
    public TMP_Text buttonViewingDistanceText;
    public TMP_Text buttonEnvironmentText;

    public TMP_Text buttonMuteText;
    public TMP_Text buttonCurrentVolumeText;

    public TMP_Text buttonCurrentPowerLevelText;

    public TMP_Text cameraOnRollerCoasterMovableText;

    public TMP_Text MQTTAdressText;
    public TMP_Text MQTTPortText;

    public bool settingsEnabled = false;

    private void OnEnable()
    {
        if (!settingsEnabled)
        {
            UpdateListBool(buttonPPText, GameState.Instance.GetGraphics(), "ON", "OFF");

            UpdateList3Int(buttonAAText, GameState.Instance.GetQuality(), "LOW", "MEDIUM", "HIGH");

            UpdateListBool(buttonShadowsText, GameState.Instance.GetShadowsOn(), "ON", "OFF");

            UpdateList3Int(buttonViewingDistanceText, GameState.Instance.GetViewingDistance(), "LOW", "MEDIUM", "HIGH");

            UpdateList3Int(buttonEnvironmentText, GameState.Instance.GetEnvironment(), "ONLY RELEVANT ITEMS", "SOME DECORATIVE ITEMS", "ALL ITEMS");

            UpdateListBool(buttonMuteText, !GameState.Instance.GetMuted(), "ON", "OFF");

            float _num = GameState.Instance.GetVolume();
            buttonCurrentVolumeText.text = $"{_num:F2}";

            UpdateListBool(cameraOnRollerCoasterMovableText, GameState.Instance.GetMovable(), "ROTATE + TRANSFORM", "ROTATE ONLY");

            UpdateText(buttonCurrentPowerLevelText, "Power Level: " + GameState.Instance.GetPowerLevel());
        }
        UpdateVolume();

        UpdateText(MQTTAdressText, GameState.Instance.GetMQTTAdress());
        UpdateText(MQTTPortText, GameState.Instance.GetMQTTPort());
    }

    public void refreshButtons()
    {
        if (settingsEnabled)
        {
            UpdateListBool(buttonPPText, GameState.Instance.GetGraphics(), "ON", "OFF");

            UpdateList3Int(buttonAAText, GameState.Instance.GetQuality(), "LOW", "MEDIUM", "HIGH");

            UpdateListBool(buttonShadowsText, GameState.Instance.GetShadowsOn(), "ON", "OFF");

            UpdateList3Int(buttonViewingDistanceText, GameState.Instance.GetViewingDistance(), "LOW", "MEDIUM", "HIGH");

            UpdateList3Int(buttonEnvironmentText, GameState.Instance.GetEnvironment(), "ONLY RELEVANT ITEMS", "SOME DECORATIVE ITEMS", "ALL ITEMS");

            UpdateListBool(buttonMuteText, !GameState.Instance.GetMuted(), "ON", "OFF");

            float _num = GameState.Instance.GetVolume();
            buttonCurrentVolumeText.text = $"{_num:F2}";

            UpdateListBool(cameraOnRollerCoasterMovableText, GameState.Instance.GetMovable(), "ROTATE + TRANSFORM", "ROTATE ONLY");

            UpdateText(buttonCurrentPowerLevelText, "Power Level: " + GameState.Instance.GetPowerLevel());

            UpdateVolume();
        }
    }

    public void ChangeGraphics()
    {
        bool _graphics = !GameState.Instance.GetGraphics();
        GameState.Instance.SetGraphics(_graphics);

        UpdateListBool(buttonPPText, _graphics, "ON", "OFF");
    }

    public void ToggleShadows()
    {
        bool _shadows = !GameState.Instance.GetShadowsOn();
        GameState.Instance.SetShadowsOn(_shadows);
        UpdateListBool(buttonShadowsText, _shadows, "ON", "OFF");
    }


    public void ToggleMovable()
    {
        bool _movable = !GameState.Instance.GetMovable();
        GameState.Instance.SetMovable(_movable);
        UpdateListBool(cameraOnRollerCoasterMovableText, GameState.Instance.GetMovable(), "ROTATE + TRANSFORM", "ROTATE ONLY");

    }

    public void ChangeQuality()
    {
        int _quality = GameState.Instance.GetQuality();
        _quality++;
        if (_quality > 2)
        {
            _quality = 0;
        }
        GameState.Instance.SetQuality(_quality);

        UpdateList3Int(buttonAAText, _quality, "LOW", "MEDIUM", "HIGH");
    }

    public void ChangeMQTTSettings()
    {
        GameState.Instance.SetMQTTAdress(MQTTAdressText.text);
        GameState.Instance.SetMQTTPort(MQTTPortText.text);
        UpdateText(MQTTAdressText, GameState.Instance.GetMQTTAdress());
        UpdateText(MQTTPortText, GameState.Instance.GetMQTTPort());
    }

    public void ChangeViewingDistance()
    {
        int _viewingDistance = GameState.Instance.GetViewingDistance();
        _viewingDistance++;
        if (_viewingDistance > 2)
        {
            _viewingDistance = 0;
        }
        GameState.Instance.SetViewingDistance(_viewingDistance);

        UpdateList3Int(buttonViewingDistanceText, _viewingDistance, "LOW", "MEDIUM", "HIGH");
    }

    public void ChangeEnvironment()
    {
        int _environment = GameState.Instance.GetEnvironment();
        _environment++;
        if (_environment > 2)
        {
            _environment = 0;
        }
        GameState.Instance.SetEnvironment(_environment);
        UpdateList3Int(buttonEnvironmentText, _environment, "RELEVANTS ITEMS", "SOME DECORATIVES", "ALL ITEMS");

    }

    public void ToggleMute()
    {
        bool _muted = !GameState.Instance.GetMuted();
        GameState.Instance.SetMuted(_muted);
        UpdateListBool(buttonMuteText, !_muted, "ON", "OFF");

        if (!_muted)
        {
            UpdateVolume();
        }
    }

    public void UpdateVolume()
    {
        float num = GameState.Instance.GetVolume();
        GameState.Instance.SetVolume(num);
        buttonCurrentVolumeText.text = $"{num:F2}";
    }

    public void reduceVolume()
    {
        if (GameState.Instance.GetVolume() >= 0.2f)
        {
            float num = GameState.Instance.GetVolume();
            GameState.Instance.SetVolume(num - .1f);
            buttonCurrentVolumeText.text = $"{GameState.Instance.GetVolume():F2}";
        }
    }

    public void increaseVolume()
    {
        float num = GameState.Instance.GetVolume();
        GameState.Instance.SetVolume(num + .1f);
        buttonCurrentVolumeText.text = $"{GameState.Instance.GetVolume():F2}";
    }


    public void increasePowerLevel()
    {
        int powlevel = GameState.Instance.GetPowerLevel();
        GameState.Instance.SetPowerLevel(powlevel + 1);
        buttonCurrentPowerLevelText.text = "Power Level: " + GameState.Instance.GetPowerLevel();
    }

    public void decreasePowerLevel()
    {
        int powlevel = GameState.Instance.GetPowerLevel();
        GameState.Instance.SetPowerLevel(powlevel - 1);
        buttonCurrentPowerLevelText.text = "Power Level: " + GameState.Instance.GetPowerLevel();
    }

    private void UpdateListBool(TMP_Text text, bool b, string textTrue, string textFalse)
    {
        if (b)
        {
            text.text = textTrue;
        }
        else
        {
            text.text = textFalse;
        }
    }

    private void UpdateList3Int(TMP_Text text, int b, string bezeichnung1, string bezeichnung2, string bezeichnung3)
    {
        if (b == 0)
        {
            text.text = bezeichnung1;
        }
        else if (b == 1)
        {
            text.text = bezeichnung2;
        }
        else if (b == 2)
        {
            text.text = bezeichnung3;
        }
    }

    private void UpdateText(TMP_Text _textObject, string _text)
    {
        _textObject.text = _text;
    }
}