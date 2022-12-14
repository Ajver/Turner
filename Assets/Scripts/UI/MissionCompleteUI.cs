using TMPro;
using UnityEngine;

public class MissionCompleteUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI missionDescriptionText;
    [SerializeField] private string mainMenuSceneName;
    
    private void Start()
    {
        MissionSystem.Instance.OnMissionComplete += MissionSystem_OnMissionComplete;
        
        gameObject.SetActive(false);
    }

    public void OnContinueBtnClicked()
    {
        // Load in Mission Pick view
        MenuStateStore.inMissionPick = true;
        SceneFader.Instance.FadeToScene(mainMenuSceneName);
    }

    private void MissionSystem_OnMissionComplete(object sender, MissionSystem.MissionCompleteReason reason)
    {
        switch (reason)
        {
            case MissionSystem.MissionCompleteReason.CollectTreasure:
                missionDescriptionText.text = "You collected suitcase with a lot of money";
                break;
            case MissionSystem.MissionCompleteReason.KillEnemies:
                missionDescriptionText.text = "You killed all enemies";
                break;
        }
        
        gameObject.SetActive(true);
    }

}
