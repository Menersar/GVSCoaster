// PlayerSave
public class PlayerSave
{
    public float[] times = new float[100];

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
    } = true;

    public int environmentLevel
    {
        get;
        set;
    } = 2;

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

    public bool muted
    {
        get;
        set;
    }

    public float volume
    {
        get;
        set;
    } = 0.5f;

    public int powerLevel
    {
        get;
        set;
    } = 10;
}