using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Quest[] quests;
    public int currentQuestIndex = 0;

    private bool questActive = false;

    private PlayerResourceCollector player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerResourceCollector>();
        UIManager.instance.StartStory();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !questActive)
        {
            questActive = true;
            UIManager.instance.ShowQuestPanel();
            ShowQuest(quests[currentQuestIndex]);
        }
        else
        {
            Quest currentQuest = quests[currentQuestIndex];
            if (currentQuest.IsCompleted(player.GetWood(), player.GetMoney(), player.GetMeat()))
            {
                AdvanceQuest();
            }
            else
            {
                ShowQuest(currentQuest);
            }
        }
    }

    public void ShowQuest(Quest quest)
    {
        UIManager.instance.UpdateRequiredResourcesQuestAndName(quest.moneyRequired, quest.woodRequired, quest.meatRequired, quest.questName);
    }

    public void AdvanceQuest()
    {
        player.SetMoney(player.GetMoney() - quests[currentQuestIndex].moneyRequired);
        player.SetWood(player.GetWood() - quests[currentQuestIndex].woodRequired);
        player.SetMeat(player.GetMeat() - quests[currentQuestIndex].meatRequired);

        player.UpdateAllResources();

        if (currentQuestIndex < quests.Length - 1)
        {
            currentQuestIndex++;
            ShowQuest(quests[currentQuestIndex]);
        }
        else
        {
            UIManager.instance.HideQuestPanel();
            questActive = false;
            UIManager.instance.EndStory();
        }
    }
}
