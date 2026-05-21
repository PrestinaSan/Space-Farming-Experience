using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject mainMenuObj;
    [SerializeField] private GameObject introSequenceObj;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isMenu;
    private string introText = "You wake up on an unfamiliar planet...";

    private void Start()
    {
        if (isMenu)
        {
            text.text = "";
            continueButton.SetActive(false);
            introSequenceObj.SetActive(false);
        }
        else
        {
            introSequenceObj.SetActive(true);
            StartCoroutine(PlayAnimationAndWait("Black Screen Animation Reverse"));
        }
    }
    public void OnStartButtonClick()
    {
        introSequenceObj.SetActive(true);
        StartCoroutine(PlayAnimationAndWait("Black Screen Animation"));
    }
    public void OnContinueClick()
    {

        SceneManager.LoadScene("Game");
    }

    private IEnumerator AddLetter()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < introText.Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            if (text.text != introText)
            {
                text.text += introText[i];
            }
            else
            {
                break;
            }
        }
        yield return new WaitForSeconds(0.05f);
        continueButton.SetActive(true);
    }

    private IEnumerator PlayAnimationAndWait(string animationName)
    {
        animator.Play(animationName);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animationName));

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        if (isMenu)
        {
            mainMenuObj.SetActive(false);
            StartCoroutine(AddLetter());
        }
        else
        {
            Destroy(introSequenceObj);
        }
    }
}
