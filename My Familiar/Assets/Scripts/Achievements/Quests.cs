using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quests : Observer
{
    // Quest UI
    public Toggle Quest1_Toggle;
    public Toggle Quest2_Toggle;
    public Toggle Quest3_Toggle;
    public Text Quest1_Text;
    public Text Quest2_Text;
    public Text Quest3_Text;

    // Quest lengths
    int Quest1Length = 1; // How many triggers it takes to complete
    int Quest2Length = 1;
    int Quest3Length = 1;

    // Quest progress
    int Quest1Progress = 0; // Current progress
    int Quest2Progress = 0;
    int Quest3Progress = 0;

    // Quest bools
    bool UsedFireball = false;

    // Enum of quests
    public enum QuestEnum { Empty, UseFireball, UseBoulder, UseVines, UseStormOrb, UseWaterfall, UseBalloon, UseTrampoline, UseFood, MakeHappy, MakeFull, WinBattle, Evolve }
    List<QuestEnum> AllQuests = new List<QuestEnum> { }; // Stores all quests (except empty)

    // Array of active quests
    QuestEnum[] ActiveQuestArray = new QuestEnum[3] { QuestEnum.Empty, QuestEnum.Empty, QuestEnum.Empty };

    // List of Tutorial quests
    List<QuestEnum> TutorialList = new List<QuestEnum> { };

    void Awake()
    {
        // Init tutorial quest list and all quests list
        foreach(QuestEnum e in System.Enum.GetValues(typeof(QuestEnum)))
        {
            // Dont add empty
            if(e == QuestEnum.Empty) { break; }

            // Add everything in quest enum
            TutorialList.Add(e);
            AllQuests.Add(e);
        }

        // Set quests
        SetNewQuest(1);
        SetNewQuest(2);
        SetNewQuest(3);     

        // Update quest UI
    }

    // Receives info on events by subjects
    public override void OnNotify(GameObject GO, Events _event)
    {
        // If all quests are empty, exit
        if (ActiveQuestArray[0] == QuestEnum.Empty && ActiveQuestArray[1] == QuestEnum.Empty && ActiveQuestArray[2] == QuestEnum.Empty)
        {
            return;
        }

        // Check event and trigger relevant quest
        switch (_event)
        {
            case Events.ItemUsed:
                // Used fireball
                TriggerQuest(QuestEnum.UseFireball);
                break;
            default:
                break;
        }
    }

    QuestEnum ChooseNextQuest()
    {
        QuestEnum chosenQuest = QuestEnum.Empty;

        // Are there tutorial quests left?
        if(TutorialList.Count > 0)
        {
            // Select a tutorial quest
            int rand = Random.Range(0, TutorialList.Count);
            chosenQuest = TutorialList[rand];
        }
        else
        {
            // Select any quest other than empty
            int rand = Random.Range(0, AllQuests.Count);
            chosenQuest = AllQuests[rand];
        }        

        return chosenQuest;
    }

    // Set a new quest to a slot
    void SetNewQuest(int questSlotNum)
    {        
        switch (questSlotNum)
        {
            case 1:
                ActiveQuestArray[0] = ChooseNextQuest();
                break;
            case 2:
                ActiveQuestArray[1] = ChooseNextQuest();
                break;
            case 3:
                ActiveQuestArray[2] = ChooseNextQuest();
                break;
            default:
                Debug.Log("ERROR! Quest num should be 1, 2 or 3");
                break;
        }
    }

    // Trigger quest
    void TriggerQuest(QuestEnum quest)
    {
        switch (quest)
        {
            case QuestEnum.UseFireball:
                UsedFireball = true;
                break;
            default:
                break;
        }
    }

    // isQuestComplete sets the toggle on/off, progressMade enables quest progress to be made or kept same
    void UpdateQuest1(bool isQuestComplete, int progressMade = 0)
    {
        // Update toggle
        Quest1_Toggle.isOn = isQuestComplete;
    }

    void UpdateQuest2(bool isQuestComplete, int progressMade = 0)
    {
        // Update toggle
        Quest2_Toggle.isOn = isQuestComplete;
    }

    void UpdateQuest3(bool isQuestComplete, int progressMade = 0)
    {
        // Update toggle
        Quest3_Toggle.isOn = isQuestComplete;
    }
}
