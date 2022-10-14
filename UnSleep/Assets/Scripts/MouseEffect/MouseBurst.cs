using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseBurst : MonoBehaviour
{
    private static MouseBurst instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private ParticleSystem ps;
    private Ray ray;
    private Camera mainCam;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        distance = -mainCam.transform.position.z;
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.Equals(SceneChanger.GetSceneName(SceneType.Mental)) ||
                scene.Equals(SceneChanger.GetSceneName(SceneType.Nightmare)) ||
                scene.Equals(SceneChanger.GetSceneName(SceneType.Nightmare27))) return;

            if (mainCam == null)
            {
                mainCam = Camera.main;
                distance = -mainCam.transform.position.z;
            }
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            transform.position = ray.GetPoint(distance);
            ps.Play();
        }
    }
}
