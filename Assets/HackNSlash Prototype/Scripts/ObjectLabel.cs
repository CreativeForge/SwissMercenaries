using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUIText))]
public class ObjectLabel : MonoBehaviour
{

    public Transform target;  // Object that this label should follow
    public Vector3 offset = Vector3.up;    // Units in world space to offset; 1 unit above object by default
    public bool clampToScreen = false;  // If true, label will be visible even if object is off screen
    public float clampBorderSize = 0.05f;  // How much viewport space to leave at the borders when a label is being clamped
    public bool useMainCamera = true;   // Use the camera tagged MainCamera
    public Camera cameraToUse;   // Only use this if useMainCamera is false
    public bool DestroyAfterTime = false;
    public float TimeUntilDestruction = 5.0f;
    public bool isPicture = false;
    bool textActive = false;
    Vector3 lastposition=new Vector3(0,0,0);
    Camera cam;
    Transform thisTransform;
    Transform camTransform;

    float originalScaleX;
    float originalScaleY;
    float originalScaleZ;

    public float scaling = 2;

    void Start()
    {
        this.originalScaleX = this.gameObject.transform.localScale.x;
        this.originalScaleY = this.gameObject.transform.localScale.y;
        this.originalScaleZ = this.gameObject.transform.localScale.z;

        textActive = true;
        thisTransform = transform;
        if (useMainCamera)
            cam = Camera.main;
        else
            cam = cameraToUse;
        camTransform = cam.transform;
        if (DestroyAfterTime)
        {
            StartCoroutine(handleSpokenText(TimeUntilDestruction));
        }
    }


    void Update()
    {

        if (clampToScreen)
        {
            Vector3 relativePosition = camTransform.InverseTransformPoint(target.position + offset);
            relativePosition.z = Mathf.Max(relativePosition.z, 1.0f);
            thisTransform.position = cam.WorldToViewportPoint(camTransform.TransformPoint(relativePosition));
            thisTransform.position = new Vector3(Mathf.Clamp(thisTransform.position.x, clampBorderSize, 1.0f - clampBorderSize),
                                             Mathf.Clamp(thisTransform.position.y, clampBorderSize, 1.0f - clampBorderSize),
                                             thisTransform.position.z);

        }
        else
        {
            if (target != null)
            {
                Vector3 tmp = cam.WorldToViewportPoint(target.position + offset);
                if (tmp.z > 0)
                {
                    if (tmp.z>30)
                    {
                        this.transform.position= new Vector3(0,0,-100);
                        //lastposition = new Vector3(0, 0, 0);
                    }
                    else {
                        tmp = cam.WorldToViewportPoint(target.position + offset);
                       // tmp = tmp / Vector2.Distance(tmp, transform.position);
                       // print(tmp + " " + lastposition);
                        transform.position = tmp;
                            
                        //lastposition = transform.position;
                        if (isPicture)
                        {
                            tmp = new Vector3(this.transform.position.x * Screen.width, this.transform.position.y * Screen.height, this.transform.position.z);
                            this.transform.position = tmp;
                            float tmpscale = Mathf.Min(5 / tmp.z * (scaling));
                            this.transform.localScale = new Vector3(originalScaleX * tmpscale, originalScaleY * tmpscale, originalScaleZ * tmpscale);
                            //lastposition = transform.position;

                        }
                    }
                } else
                {
                    //lastposition = new Vector3(0, 0, 0);
                }
            }
        }
    }


    IEnumerator handleSpokenText(float time)
    {
        yield return new WaitForSeconds(time);
        //Destroy (displayedText);
        Destroy(this.gameObject);
    }
}