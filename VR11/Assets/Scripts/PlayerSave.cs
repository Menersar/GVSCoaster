// PlayerSave
public class PlayerSave
{
    public float[] times = new float[100];

    public bool cameraShake
    {
        get;
        set;
    } = true;


    public bool motionBlur
    {
        get;
        set;
    } = true;

    public string MQTTAdress
    {
        get;
        set;
    } = "127.0.0.1";

    public string MQTTPort
    {
        get;
        set;
    } = "1883";

    public bool fullscreen
    {
        get;
        set;
    } = true;


    public bool slowmo
    {
        get;
        set;
    } = true;


    public bool graphics
    {
        get;
        set;
    } = true;

    public bool shadowsOn
    {
        get;
        set;
    } = true; 
    
    public bool movable
    {
        get;
        set;
    } = false;

    public bool environmentOn
    {
        get;
        set;
    } = true;

    public int quality
    {
        get;
        set;
    } = 2;

    public int viewingDistance
    {
        get;
        set;
    } = 2;   

    public int resolution
    {
        get;
        set;
    }

    public bool depth
    {
        get;
        set;
    }


    public bool muted
    {
        get;
        set;
    }

    public float sensitivity
    {
        get;
        set;
    } = 1f;


    public float fov
    {
        get;
        set;
    } = 80f;


    public float volume
    {
        get;
        set;
    } = 0.0f;


    public float music
    {
        get;
        set;
    } = 0.5f;

}