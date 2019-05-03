using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : Observer
{
    // Achievement bools
    bool UsedFireball = false;

    // Enum of achievements
    public enum AchievementEnum { UseFireball, HappyHappy }

    // Receives info on events by subjects
    public override void OnNotify(GameObject GO, Events _event)
    {
        switch (_event)
        {
            case Events.ItemUsed:
                break;
            default:
                break;
        }
    }

    // Unlock achievements and do the related code i.e. reward player
    void UnlockAchievement(AchievementEnum Achievement)
    {
        switch (Achievement)
        {
            case AchievementEnum.UseFireball:
                UsedFireball = true;
                break;
            default:
                break;
        }
    }
}
