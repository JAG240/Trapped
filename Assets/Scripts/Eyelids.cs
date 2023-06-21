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
    private bool inTransition = false;

    private void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        sceneManager.playerDeath += (context) => PlayerDied();
        sceneManager.resetLevel += ResetLevel;
        sceneManager.playerEneteredPorch += EnterPorch;
    }

    private void PlayerDied()
    {
        StartCoroutine(ChangeEyeLidState(false));
    }

    private void ResetLevel()
    {
        StartCoroutine(ChangeEyeLidState(true));
    }

    private void EnterPorch()
    {
        if (inTransition)
            return;

        inTransition = true;
        StartCoroutine(PorchTansition());
    }

    private IEnumerator PorchTansition()
    {
        StartCoroutine(ChangeEyeLidState(false));
        yield return new WaitForSeconds(eyeLidSpeed);
        StartCoroutine(ChangeEyeLidState(true));
        yield return new WaitForSeconds(eyeLidSpeed);
        inTransition = false;
    }

    private IEnumerator ChangeEyeLidState(bool isOpen)
    {
        if (!isOpen)
        {
            topEyeLid.SetActive(true);
            bottomEyeLid.SetActive(true);
        }

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
    }
}
