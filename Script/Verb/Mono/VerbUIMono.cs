using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VerbUIMono : MonoBehaviour
{
    public VerbMono verbMono;
    public AbstractVerb verb => verbMono.verb;
    public GameObject countdownCanvasGameobject, completionBadgeGameobject, dumpButtonGameobject;
    public RecipeThresholdsDominionMono recipeTimerMono;
    public WithNumberIconComponentMono numberIcon;
    public Button dumpButton;

    private void Start()
    {
        verbMono = GetComponent<VerbMono>();
        dumpButton.onClick.AddListener(verbMono.situationWindowMono.CollectCurrentRecipe);
    }
    // Update is called once per frame
    void Update()
    {
        CloseAllUI();
        UpdateCountDownCanvas();
        UpdateCompletionBadge();
        UpdateDumpButton();
    }
    public void CloseAllUI()
    {
        countdownCanvasGameobject.gameObject.SetActive(false);
        completionBadgeGameobject.gameObject.SetActive(false);
        dumpButtonGameobject.gameObject.SetActive(false);
    }
    public void UpdateCountDownCanvas()
    {
        if(verb.situation.situationState == AbstractSituation.SituationState.WaitingForCollect)
        {
            completionBadgeGameobject.gameObject.SetActive(true);
            numberIcon.SetValue(verb.verbMono.situationWindowMono.rewardStorageDominionManager.gameObject.transform.childCount);
        }
    }
    public void UpdateCompletionBadge()
    {
        if (verb.situation.situationState == AbstractSituation.SituationState.Excuting)
        {
            countdownCanvasGameobject.gameObject.SetActive(true);
            recipeTimerMono.SetTimer(verb.situation.currentRecipe);
        }
    }
    public void UpdateDumpButton()
    {
        if (verb.situation.situationState == AbstractSituation.SituationState.WaitingForCollect)
        {
            dumpButtonGameobject.gameObject.SetActive(true);

        }
    }
}
