// GameState
//using Audio;
using UnityEngine;
//!using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameState : MonoBehaviour
{
    public GameObject ppVolume;

    public VolumeProfile vp;


    public bool graphics = true;

    // 0 low, 1 medium, 2 high
    public int quality = 2;
    //  public int resolution;

    //  public bool muted;

    public OptionsManager optionsManager;
   // public bool shake = true;



  //  private float volume;

  //  private float music;


 //  public float cameraShake = 1f;

    public static GameState Instance
    {
        get;
        private set;
    }

    private void Start()
    {









        Instance = this;

        graphics = SaveManager.Instance.state.graphics;
        quality = SaveManager.Instance.state.quality;
    //    shake = SaveManager.Instance.state.cameraShake;
     //   muted = SaveManager.Instance.state.muted;
     //   music = SaveManager.Instance.state.music;
     //   volume = SaveManager.Instance.state.volume;
        UpdateSettings();

        optionsManager.enabled = true;
            }
    
    public void SetGraphics(bool b)
    {
        graphics = b;
        ppVolume.SetActive(b);
        SaveManager.Instance.state.graphics = b;
        SaveManager.Instance.Save();
    }









    
    public void SetQuality(int qual)
    {
        quality = qual;
        //  ppVolume.SetActive(b);
        QualitySettings.SetQualityLevel(qual);
        if (qual == 0)
        {
            Camera.main.GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.None;
        }
        else if (qual == 1 || qual == 2)
        {
            Camera.main.GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.FastApproximateAntialiasing;
        }
        SaveManager.Instance.state.quality = qual;
        SaveManager.Instance.Save();
    }

    




    /*
    public void SetShake(bool b)
    {
        shake = b;
        if (b)
        {
            cameraShake = 1f;
        }
        else
        {
            cameraShake = 0f;
        }
        SaveManager.Instance.state.cameraShake = b;
        SaveManager.Instance.Save();
    }
    */

/*
    public void SetMusic(float s)
    {
        float musicVolume = music = Mathf.Clamp(s, 0f, 1f);
        if ((bool)Music.Instance)
        {
            Music.Instance.SetMusicVolume(musicVolume);
        }
        SaveManager.Instance.state.music = musicVolume;
        SaveManager.Instance.Save();
        MonoBehaviour.print("music saved as: " + music);
    }
    */
/*
    public void SetVolume(float s)
    {
        float num2 = AudioListener.volume = (volume = Mathf.Clamp(s, 0f, 1f));
        SaveManager.Instance.state.volume = num2;
        SaveManager.Instance.Save();
    }
*/

    /*
    public void SetMuted(bool b)
    {
        AudioManager.Instance.MuteSounds(b);
        muted = b;
        SaveManager.Instance.state.muted = b;
        SaveManager.Instance.Save();
    }
    */
    private void UpdateSettings()
    {
        SetGraphics(graphics);
      //  SetMusic(music);
     //   SetVolume(volume);
      //  SetShake(shake);
     //   SetMuted(muted);
    }

    public bool GetGraphics()
    {
        return graphics;
    }

    
    public int GetQuality()
    {
        return quality;
    }


    /*
    public float GetVolume()
    {
        return volume;
    }

    public float GetMusic()
    {
        return music;
    }



    public bool GetMuted()
    {
        return muted;
    }

    */



}
