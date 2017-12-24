using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject PlayerPrefab;
    public GameObject CameraBundlePrefab;

    Movement player1;
    Movement player2;

    CameraDivisionEffect mainCamera;
    Transform camera1;
    Transform camera2;

    bool initialized = false;



    private void Start()
    {
        RunGame();
    }

    void RunGame()
    {
        StartCoroutine(RunGameCoroutine());
    }

    IEnumerator RunGameCoroutine()
    {
        ClearGame();
        InitializePrefabs();
        player1.transform.position = new Vector3(-1.8f, .5f, -12f);
        player2.transform.position = new Vector3(2.2f, .5f, -12f);

        yield return null;

    }

    void ClearGame()
    {
        //nothing yet
    }

    void InitializePrefabs()
    {
        if(!initialized)
        {
            GameObject playerClone = Instantiate(PlayerPrefab);
            playerClone.transform.SetParent(transform, false);
            player1 = playerClone.GetComponent<Movement>();

            playerClone = Instantiate(PlayerPrefab);
            playerClone.transform.SetParent(transform, false);
            player2 = playerClone.GetComponent<Movement>();
            player2.id = 2;

            GameObject cameraClone = Instantiate(CameraBundlePrefab);
            cameraClone.transform.SetParent(transform, false);
            camera1 = cameraClone.transform.GetChild(1);
            camera2 = cameraClone.transform.GetChild(2);
            mainCamera = cameraClone.transform.GetChild(0).GetComponent<CameraDivisionEffect>();
            mainCamera.Init(player1.transform, player2.transform, camera1, camera2);

            initialized = true;
        }
    }
}
