using NUnit.Framework;
using UnityEngine;

public class ParallaxControllerScript : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;
    float distance;

    GameObject[] backgrounds;
    Material[]mat;
    float[]backSPeed;
    float farthesrBack;

    [UnityEngine.Range(0.01f, 0.05f)]
    public float parallaxSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam=Camera.main.transform;
        camStartPos=cam.position;
        int backCount=transform.childCount;
        mat=new Material[backCount];
        backSPeed=new float[backCount];
        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i]=transform.GetChild(i).gameObject;
            mat[i]=backgrounds[i].GetComponent<Renderer>().material;
            // distance=Vector3.Distance(cam.position,backgrounds[i].transform.position);
            // backSPeed[i]=distance/farthesrBack*parallaxSpeed;
        }
        BackSpeedCalculator(backCount);
    }
void BackSpeedCalculator(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
           if(backgrounds[i].transform.position.z>farthesrBack)
           {
               farthesrBack=backgrounds[i].transform.position.z;
           }
        }
        for (int i = 0; i < backCount; i++)
        {
           backSPeed[i]=1-(backgrounds[i].transform.position.z-cam.position.z)*parallaxSpeed/farthesrBack;
        }
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        distance=cam.position.x-camStartPos.x;
        transform.position=new Vector3(cam.position.x,transform.position.y,transform.position.z);
        for (int i = 0; i < backgrounds.Length; i++)
        {
           float speed=distance*backSPeed[i];
           mat[i].SetTextureOffset("_MainTex", new Vector2(speed, 0));
        }
    }
}
