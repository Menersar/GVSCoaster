// Options
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


public class OptionsManager : MonoBehaviour
{

    public TextMeshProUGUI volume;


    public TextMeshProUGUI[] sounds;

    // public TextMeshProUGUI[] graphics;
    // public TextMeshProUGUI[] quality;

    public TextMeshProUGUI[] shake;




    public Slider volumeS;

    public Slider musicS;



    Resolution[] resolutions;


    public TMP_Text buttonPPText;
    public TMP_Text buttonAAText;
    public TMP_Text buttonShadowsText;
    public TMP_Text buttonViewingDistanceText;
    public TMP_Text buttonEnvironmentText;
    
    public TMP_Text buttonMuteText;
    public TMP_Text buttonCurrentVolumeText;
    
    public TMP_Text cameraOnRollerCoasterMovableText;
   
    public TMP_Text MQTTAdressText;
    public TMP_Text MQTTPortText;


    public bool settingsEnabled = false;


    //  public TMP_Text buttonOpaqueTexturesText;
    //   public TMP_Text buttonShadowDistanceText;

    private void OnEnable()
    {
        if (!settingsEnabled)
        {
            UpdateListBool(buttonPPText, GameState.Instance.GetGraphics(), "ON", "OFF");


            // Debug.Log(GameState.Instance.GetGraphics() + "_________");
            UpdateList3Int(buttonAAText, GameState.Instance.GetQuality());
            // UpdateList(shake, GameState.Instance.shake);

            UpdateListBool(buttonShadowsText, GameState.Instance.GetShadowsOn(), "ON", "OFF");


            UpdateList3Int(buttonViewingDistanceText, GameState.Instance.GetViewingDistance());

            UpdateListBool(buttonEnvironmentText, GameState.Instance.GetEnvironmentOn(), "ON", "OFF");

            UpdateListBool(buttonMuteText, !GameState.Instance.GetMuted(), "ON", "OFF");

            //UpdateListBool(buttonMuteText, !GameState.Instance.GetMuted());

            float _num = GameState.Instance.GetVolume();
            buttonCurrentVolumeText.text = $"{_num:F2}";

            UpdateListBool(cameraOnRollerCoasterMovableText, GameState.Instance.GetMovable(), "ROTATE + TRANSFORM", "ROTATE ONLY");
            
           


        }
        UpdateVolume();

        // volumeS.value = GameState.Instance.GetVolume();
        //  musicS.value = GameState.Instance.GetMusic();

        ////     resolutionDropDown.value = GameState.Instance.GetResolution();

        //  quality.value = GameState.Instance.GetFov();
        //  MonoBehaviour.print(GameState.Instance.GetMusic());
        //  UpdateVolume();

        // this.enabled = false;

        UpdateMQTTText(MQTTAdressText, MQTTPortText, GameState.Instance.GetMQTTAdress(), GameState.Instance.GetMQTTPort());

    }

    public void refreshButtons()
    {
        if (settingsEnabled)
        {
            UpdateListBool(buttonPPText, GameState.Instance.GetGraphics(), "ON", "OFF");


            // Debug.Log(GameState.Instance.GetGraphics() + "_________");
            UpdateList3Int(buttonAAText, GameState.Instance.GetQuality());
            // UpdateList(shake, GameState.Instance.shake);

            UpdateListBool(buttonShadowsText, GameState.Instance.GetShadowsOn(), "ON", "OFF");


            UpdateList3Int(buttonViewingDistanceText, GameState.Instance.GetViewingDistance());

            UpdateListBool(buttonEnvironmentText, GameState.Instance.GetEnvironmentOn(), "ON", "OFF");

            UpdateListBool(buttonMuteText, !GameState.Instance.GetMuted(), "ON", "OFF");

            //UpdateListBool(buttonMuteText, !GameState.Instance.GetMuted());

            float _num = GameState.Instance.GetVolume();
            buttonCurrentVolumeText.text = $"{_num:F2}";

            UpdateListBool(cameraOnRollerCoasterMovableText, GameState.Instance.GetMovable(), "ROTATE + TRANSFORM", "ROTATE ONLY");

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

        UpdateList3Int(buttonAAText, _quality);
    }

    public void ChangeMQTTSettings()
    {
       // string _MQTTadress = GameState.Instance.GetMQTTAdress();
       // string _MQTTport = GameState.Instance.GetMQTTPort();
        
        GameState.Instance.SetMQTTAdress(MQTTAdressText.text);
        GameState.Instance.SetMQTTPort(MQTTPortText.text);
        UpdateMQTTText(MQTTAdressText, MQTTPortText, MQTTAdressText.text, MQTTPortText.text);

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

          UpdateList3Int(buttonViewingDistanceText, _viewingDistance);


    }


    public void ToggleEnvironment()
    {
        bool _environment = !GameState.Instance.GetEnvironmentOn();
        GameState.Instance.SetEnvironmentOn(_environment);
        UpdateListBool(buttonEnvironmentText, _environment, "ON", "OFF");

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

    /*
    public void ChangeShake(bool b)
    {
        GameState.Instance.SetShake(b);
        UpdateList(shake, b);
    }


    */

    
    public void UpdateVolume()
    {
        //float num = AudioListener.volume = GameState.Instance.GetVolume();
        //  GameState.Instance.SetVolume(num);
        // buttonCurrentVolumeText.text = $"{num:F2}";

        //float num = AudioListener.volume = volumeS.value;
        //float num = AudioListener.volume = GameState.Instance.GetVolume();
        float num = GameState.Instance.GetVolume();
        GameState.Instance.SetVolume(num);
        //volume.text = $"{num:F2}";
        buttonCurrentVolumeText.text = $"{num:F2}";
    }

    public void reduceVolume()
    {
        if (GameState.Instance.GetVolume() >= 0.2f) {
            float num = GameState.Instance.GetVolume() ;
            GameState.Instance.SetVolume(num - .1f);
            buttonCurrentVolumeText.text = $"{GameState.Instance.GetVolume():F2}";
        }
    }




    public void increaseVolume()
    {
       // if (GameState.Instance.GetVolume() <= .9f)
       // {
            float num = GameState.Instance.GetVolume() ;
            GameState.Instance.SetVolume(num + .1f);
            buttonCurrentVolumeText.text = $"{GameState.Instance.GetVolume():F2}";
       // }
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


    private void UpdateList3Int(TMP_Text text, int b)
    {
        if (b == 0)
        {
            text.text = "LOW";
        }
        else if (b == 1)
        {
            text.text = "MEDIUM";
        }
        else if (b == 2)
        {
            text.text = "HIGH";
        }
    }



    private void UpdateMQTTText(TMP_Text _adress, TMP_Text _port, string _adressText, string _portText)
    {
        _adress.text = _adressText;
        _port.text = _portText;
    }


    /*
        private void UpdateList3(TMP_Dropdown dropD, int resindex)
        {

        }
    */
}