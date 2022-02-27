// GameState
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameState : MonoBehaviour
{
    public GameObject ppVolume;
    public GameObject environmentDisableGameObjectsAll;
    public GameObject environmentDisableGameObjectsSomeToDisable;
    public GameObject terrainWithShadows;
    public GameObject terrainWithOutShadows;

    public VolumeProfile vp;

    public bool graphics = true;
    public bool shadowsOn = true;

    public bool movable = false;

    // 0 low, 1 medium, 2 high
    public int quality = 2;
    public int viewingDistance = 2;

    public int environmentLevel = 2;

    public bool muted;

    public OptionsManager optionsManager;

    private float volume;

    private int powerLevel;

    private string MQTTAdress;
    private string MQTTPort;

    public static GameState Instance
    {
        get;
        private set;
    }

    private void Start()
    {
        Instance = this;

        graphics = SaveManager.Instance.state.graphics;
        shadowsOn = SaveManager.Instance.state.shadowsOn;

        movable = SaveManager.Instance.state.movable;

        quality = SaveManager.Instance.state.quality;
        viewingDistance = SaveManager.Instance.state.viewingDistance;

        muted = SaveManager.Instance.state.muted;

        volume = SaveManager.Instance.state.volume;
        powerLevel = SaveManager.Instance.state.powerLevel;

        environmentLevel = SaveManager.Instance.state.environmentLevel;

        MQTTAdress = SaveManager.Instance.state.MQTTAdress;
        MQTTPort = SaveManager.Instance.state.MQTTPort;

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

    public void SetMQTTAdress(string s)
    {
        MQTTAdress = s;
        SaveManager.Instance.state.MQTTAdress = s;
        SaveManager.Instance.Save();
    }

    public void SetMQTTPort(string s)
    {
        MQTTPort = s;
        SaveManager.Instance.state.MQTTPort = s;
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

    public void SetShadowsOn(bool b)
    {
        shadowsOn = b;
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderShadows = b;
        SaveManager.Instance.state.shadowsOn = b;
        SaveManager.Instance.Save();
    }

    public void SetMovable(bool b)
    {
        movable = b;
        SaveManager.Instance.state.movable = b;
        SaveManager.Instance.Save();
    }

    public void SetViewingDistance(int view)
    {
        viewingDistance = view;

        if (view == 0)
        {
            Camera.main.farClipPlane = 200;
        }
        else if (view == 1)
        {
            Camera.main.farClipPlane = 2000;
        }
        else if (view == 2)
        {
            Camera.main.farClipPlane = 10000;

        }
        SaveManager.Instance.state.viewingDistance = view;
        SaveManager.Instance.Save();
    }

    public void SetEnvironment(int env)
    {
        environmentLevel = env;

        if (env == 0)
        {
            environmentDisableGameObjectsSomeToDisable.SetActive(false);
            terrainWithShadows.SetActive(false);
            environmentDisableGameObjectsAll.SetActive(false);
        }
        else if (env == 1)
        {
            environmentDisableGameObjectsAll.SetActive(true);
            environmentDisableGameObjectsSomeToDisable.SetActive(false);
            terrainWithOutShadows.SetActive(true);
        }
        else if (env == 2)
        {
            environmentDisableGameObjectsAll.SetActive(true);
            terrainWithOutShadows.SetActive(false);

            environmentDisableGameObjectsSomeToDisable.SetActive(true);
            terrainWithShadows.SetActive(true);
        }
        SaveManager.Instance.state.environmentLevel = env;
        SaveManager.Instance.Save();
    }

    public void SetVolume(float s)
    {
        float num2 = AudioListener.volume = (volume = Mathf.Clamp(s, 0f, 1f));
        SaveManager.Instance.state.volume = num2;
        SaveManager.Instance.Save();
    }

    public void SetPowerLevel(int _powerLevel)
    {
        int clampedPowerLevel = Mathf.Clamp(_powerLevel, 1, 10);
        powerLevel = clampedPowerLevel;
        SaveManager.Instance.state.powerLevel = powerLevel;
        SaveManager.Instance.Save();
    }

    public void SetMuted(bool b)
    {
        AudioManager.Instance.MuteSounds(b);
        muted = b;
        SaveManager.Instance.state.muted = b;
        SaveManager.Instance.Save();
    }

    private void UpdateSettings()
    {
        SetGraphics(graphics);
        SetShadowsOn(shadowsOn);
        SetMovable(movable);
        SetViewingDistance(viewingDistance);
        SetEnvironment(environmentLevel);
        SetVolume(volume);
        SetPowerLevel(powerLevel);
        SetMuted(muted);
    }

    public bool GetGraphics()
    {
        return graphics;
    }

    public int GetQuality()
    {
        return quality;
    }
    public bool GetShadowsOn()
    {
        return shadowsOn;
    }

    public bool GetMovable()
    {
        return movable;
    }

    public int GetViewingDistance()
    {
        return viewingDistance;
    }

    public int GetEnvironment()
    {
        return environmentLevel;
    }
    public string GetMQTTAdress()
    {
        return MQTTAdress;
    }

    public string GetMQTTPort()
    {
        return MQTTPort;
    }

    public float GetVolume()
    {
        return volume;
    }

    public int GetPowerLevel()
    {
        return powerLevel;
    }

    public bool GetMuted()
    {
        return muted;
    }
}
