using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyelids : MonoBehaviour
{
    [Header("Top Eye Lid")]
    [SerializeField] private GameObject topEyeLid; 
    [SerializeField] private float topOpenPos;
    [SerializeField] private float topClosePos;

    [Header("Bottom Eye Lid")]
    [SerializeField] private GameObject bottomEyeLid;
    [SerializeField] private float bottomOpenPos;
    [SerializeField] private float bottomClosePos;

    [SerializeField] private float eyeLidSpeed = 3f;

    private SceneManager sceneManager;
    private bool eyesOpen = true;

    private void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        sceneManager.playerDeath += (context) => PlayerDied();
        sceneManager.resetLevel += ResetLevel;
    }

    private void PlayerDied()
    {
        topEyeLid.SetActive(true);
        bottomEyeLid.SetActive(true);
        StartCoroutine(ChangeEyeLidState(false));
    }

    private void ResetLevel()
    {
        StartCoroutine(ChangeEyeLidState(true));
    }

    private IEnumerator ChangeEyeLidState(bool isOpen)
    {
        float t = 0f;


        float topStartY = isOpen ? topClosePos : topOpenPos;
        float topEndY = isOpen ? topOpenPos : topClosePos;

        float botStartY = isOpen ? bottomClosePos : bottomOpenPos;
        float botEndY = isOpen ? bottomOpenPos : bottomClosePos;

        while (t < eyeLidSpeed)
        {
            float interp = t / eyeLidSpeed;
            float topNewY = Mathf.Lerp(topStartY, topEndY, interp);
            float botNewY = Mathf.Lerp(botStartY, botEndY, interp);

            Vector3 curTopPos = topEyeLid.transform.localPosition;
            curTopPos.y = topNewY;
            topEyeLid.transform.localPosition = curTopPos;

            Vector3 curBotPos = bottomEyeLid.transform.localPosition;
            curBotPos.y = botNewY;
            bottomEyeLid.transform.localPosition = curBotPos;

            yield return null;
            t += Time.deltaTime;
        }

        if(isOpen)
        {
            topEyeLid.SetActive(false);
            bottomEyeLid.SetActive(false);
        }

        eyesOpen = isOpen;
    }
}
