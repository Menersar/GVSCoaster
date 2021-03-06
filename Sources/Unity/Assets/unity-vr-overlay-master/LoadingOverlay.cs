using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class LoadingOverlay : MonoBehaviour {
    public bool fading = false;
    public float fade_timer = 0;

    public float in_alpha = 1.0f;
    public float out_alpha = 0.0f;

    public Color from_color;
    public Color to_color;

    private float from_float;
    private float to_float;
    public Material material;

    public CanvasGroup canvasGroup;
    public Material materialGuardianCircle;

    private float floatNow;
    private Color colorNow;

    //void Start(){
      //  LoadingOverlay.ReverseNormals(this.gameObject);
       // this.fading = false;
       // this.fade_timer = 0;
       // this.floatNow = 0;

       // this.material = this.gameObject.GetComponent<Renderer>().material;
       // this.from_color = this.material.color;
        //this.to_color = this.material.color;
   // }
    
    void Update(){
        if(this.fading == false)
            return;

        this.fade_timer += Time.deltaTime;
        //this.material.color = Color.Lerp(this.from_color, this.to_color, this.fade_timer);
        this.floatNow = Mathf.Lerp(this.from_float, this.to_float, this.fade_timer);
        this.colorNow = Color.Lerp(this.from_color, this.to_color, this.fade_timer);
        this.material.SetColor("_BaseColor", this.colorNow);
        materialGuardianCircle.SetFloat("_AlphaRamp", this.floatNow);
        this.canvasGroup.alpha = this.floatNow;
      //  this.canvasGroup.alpha = this.canvasGroup2.alpha = Mathf.Lerp(this.from_float, this.to_float, this.fade_timer);
        //this.canvasGroup.alpha = Mathf.Lerp(this.from_float, this.to_float, this.fade_timer);
        if (this.material.color == this.to_color && this.canvasGroup.alpha == this.to_float)
        {
            this.fading = false;
            this.fade_timer = 0;
        }
    }

    public void FadeOut(){
        // Fade the overlay to `out_alpha`.
        this.from_color.a = this.in_alpha;
        this.to_color.a = this.out_alpha;

        this.from_float = this.in_alpha;
        this.to_float = this.out_alpha;
        if (this.to_color != this.material.color){
            this.fading = true;
        }
    }

    public void FadeIn(){
        // Fade the overlay to `in_alpha`.
        this.from_color.a = this.out_alpha;
        this.to_color.a = this.in_alpha;

        this.from_float = this.out_alpha;
        this.to_float = this.in_alpha;
        if (this.to_color != this.material.color){
            this.fading = true;
        }
    }

    public void FadeOutStart()
    {
        // Fade the overlay to `out_alpha`.
        this.from_color.a = this.in_alpha;
        this.to_color.a = this.out_alpha;

        this.from_float = this.in_alpha;
        this.to_float = this.in_alpha;
        if (this.to_color != this.material.color)
        {
            this.fading = true;
        }
    }

    public static void ReverseNormals(GameObject gameObject){
        // Renders interior of the overlay instead of exterior.
        // Included for ease-of-use. 
        // Public so you can use it, too.
        MeshFilter filter = gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
        if(filter != null){
            Mesh mesh = filter.mesh;
            Vector3[] normals = mesh.normals;
            for(int i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            mesh.normals = normals;

            for(int m = 0; m < mesh.subMeshCount; m++){
                int[] triangles = mesh.GetTriangles(m);
                for(int i = 0; i < triangles.Length; i += 3){
                    int temp = triangles[i + 0];
                    triangles[i + 0] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }
        }
    }
}
