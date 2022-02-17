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
    public TMP_Text buttonOpaqueTexturesText;

    private void OnEnable()
    {
        UpdateList(buttonPPText, GameState.Instance.GetGraphics());

       // Debug.Log(GameState.Instance.GetGraphics() + "_________");
        UpdateList2(buttonAAText, GameState.Instance.GetQuality());
        // UpdateList(shake, GameState.Instance.shake);








        // volumeS.value = GameState.Instance.GetVolume();
        //  musicS.value = GameState.Instance.GetMusic();

        ////     resolutionDropDown.value = GameState.Instance.GetResolution();

        //  quality.value = GameState.Instance.GetFov();
        //  MonoBehaviour.print(GameState.Instance.GetMusic());
        //  UpdateVolume();
    }

    public void ChangeGraphics()
    {
        bool _graphics = !GameState.Instance.GetGraphics();
        GameState.Instance.SetGraphics(_graphics);
        if (_graphics)
        {
            buttonPPText.text = "Post Processing:\nON";
        }
        else
        {
            buttonPPText.text = "Post Processing:\nOFF";
        }
        // UpdateList(graphics, b);
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
        if (_quality == 0)
        {
            buttonAAText.text = "Quality:\nLOW";
        }
        else if (_quality == 1)
        {
            buttonAAText.text = "Quality:\nMEDIUM";
        }
        else if (_quality == 2)
        {
            buttonAAText.text = "Quality:\nHIGH";
        }
    }








    /*
    public void ChangeShake(bool b)
    {
        GameState.Instance.SetShake(b);
        UpdateList(shake, b);
    }


    */

    /*
    public void UpdateVolume()
    {
        float num = AudioListener.volume = volumeS.value;
        GameState.Instance.SetVolume(num);
        volume.text = $"{num:F2}";
    }

    */


    private void UpdateList(TMP_Text text, bool b)
    {

        if (b)
        {
            buttonPPText.text = "Post Processing:\nON";
        }
        else
        {
            buttonPPText.text = "Post Processing:\nOFF";
        }
    }


    private void UpdateList2(TMP_Text text, int b)
    {
        if (b == 0)
        {
            buttonAAText.text = "Quality:\nLOW";
        }
        else if (b == 1)
        {
            buttonAAText.text = "Quality:\nMEDIUM";
        }
        else if (b == 2)
        {
            buttonAAText.text = "Quality:\nHIGH";
        }
    }







    private void UpdateList3(TMP_Dropdown dropD, int resindex)
    {

    }
}