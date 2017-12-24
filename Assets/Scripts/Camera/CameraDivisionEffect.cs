using UnityEngine;
using System.Collections;

public class CameraDivisionEffect : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public Transform camera1;
    public Transform camera2;

    RenderTexture camTex1;
    RenderTexture camTex2;

    Vector3 direction;

    bool renderMainCamera = true;
    bool renderMainCameraState = true;


    bool subCamsActive = false;
    bool placingSubCams = false;
    float placingCounter = 0;
    float placingTime = .25f;

    public float cameraVelocity = 10f;
    private Vector2 screenSize = new Vector2(0, 0);

    Vector3 initPos1;
    Vector3 initPos2;

    public void Init(Transform p1, Transform p2, Transform c1, Transform c2)
    {
        player1 = p1;
        player2 = p2;
        camera1 = c1;
        camera2 = c2;
    }

    void Start()
    {
        updateTextures();
    }

    void updateTextures()
    {
        camTex1 = new RenderTexture(Screen.width, Screen.height, 32);
        camera1.GetComponent<Camera>().targetTexture = camTex1;

        camTex2 = new RenderTexture(Screen.width, Screen.height, 32);
        camera2.GetComponent<Camera>().targetTexture = camTex2;
        screenSize = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);
        if (currentScreenSize != screenSize) updateTextures();

        Vector3 dir = (player2.position - player1.position);
        direction = new Vector3(dir.x, dir.z, 0);

        Vector3 cameraPos = player1.position + dir / 2f;// * (direction.magnitude / 2f);
        cameraPos.y = transform.position.y;
        transform.position = cameraPos;

        if (renderMainCameraState)
        {
            if ((Mathf.Abs(direction.x) > Camera.main.aspect * 2f * Camera.main.orthographicSize * 0.5f) ||
            (Mathf.Abs(direction.y) > 2f * Camera.main.orthographicSize * 0.5f))
            {
                subCamsActive = true;
                placingSubCams = true;
                placingCounter = 0;
                renderMainCamera = false;
                renderMainCameraState = false;
                initPos1 = transform.position;
                initPos2 = transform.position;
            }
        } else
        {
            if ((Mathf.Abs(direction.x) < Camera.main.aspect * 2f * Camera.main.orthographicSize * 0.5f) &&
            (Mathf.Abs(direction.y) < 2f * Camera.main.orthographicSize * 0.5f))
            {
                subCamsActive = false;
                placingSubCams = true;
                placingCounter = 0;
                //renderMainCamera = true;
                renderMainCameraState = true;
                initPos1 = camera1.position;
                initPos2 = camera2.position;
            }
        }
        dir.y = 0;
        dir.Normalize();

        Vector3 cam1Pos = cameraPos;
        Vector3 cam2Pos = cameraPos;

        if (placingSubCams)
        {
            if(subCamsActive)
            {
                cam1Pos = player1.position + new Vector3(dir.x * Camera.main.aspect * 2f * Camera.main.orthographicSize, 0, dir.z * 2f * Camera.main.orthographicSize) * 0.25f;
                cam2Pos = player2.position - new Vector3(dir.x * Camera.main.aspect * 2f * Camera.main.orthographicSize, 0, dir.z * 2f * Camera.main.orthographicSize) * 0.25f;
                cam1Pos.y = camera1.position.y;
                cam2Pos.y = camera2.position.y;
            } else
            {
                cam1Pos = cameraPos;
                cam2Pos = cameraPos;
            }

            camera1.position = Vector3.Lerp(initPos1, cam1Pos, placingCounter/placingTime);
            camera2.position = Vector3.Lerp(initPos2, cam2Pos, placingCounter/placingTime);

            placingCounter += Time.deltaTime;
            if (placingCounter >= placingTime)
            {
                placingSubCams = false;
                renderMainCamera = renderMainCameraState;
            }
        }

        if (placingSubCams) return;

        if (subCamsActive)
        {   
            cam1Pos = player1.position + new Vector3(dir.x * Camera.main.aspect * 2f * Camera.main.orthographicSize, 0, dir.z * 2f * Camera.main.orthographicSize) * 0.25f;
            cam2Pos = player2.position - new Vector3(dir.x * Camera.main.aspect * 2f * Camera.main.orthographicSize, 0, dir.z * 2f * Camera.main.orthographicSize) * 0.25f;
            
            cam1Pos.y = camera1.position.y;
            camera1.position = cam1Pos;

            cam2Pos.y = camera2.position.y;
            camera2.position = cam2Pos;
        }
        else
        {
            camera1.position = cameraPos;
            camera2.position = cameraPos;            
        }
    }

    public Shader shader;

    static Material m_Material = null;
    protected Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(shader);
                m_Material.hideFlags = HideFlags.DontSave;
            }
            return m_Material;
        }
    }


    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {

        if (renderMainCamera && !subCamsActive)
        {
            Graphics.Blit(src, dest);
        }
        else
        {
            material.SetTexture("_Cam1", camTex1);
            material.SetTexture("_Cam2", camTex2);

            material.SetVector("_Direction", direction);
            material.SetFloat("_Width", Screen.width);
            material.SetFloat("_Heigth", Screen.height);

            Graphics.Blit(src, dest, material);
        }
    }

    public Transform GetCamera(int id)
    {
        if (renderMainCamera) return transform;
        if (id == 1) return camera1;
        else return camera2;
    }
}