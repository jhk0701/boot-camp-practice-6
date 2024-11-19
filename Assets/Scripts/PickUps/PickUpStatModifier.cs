using System.Collections.Generic;
using UnityEngine;

public class PickUpStatModifier : PickUpItem
{
    [SerializeField] List<CharacterStat> statModifier = new List<CharacterStat>();

    protected override void OnPickedUp(GameObject go)
    {
        CharacterStatsHandler statsHandler = go.GetComponent<CharacterStatsHandler>();
        if(statsHandler != null)
        {
            foreach(CharacterStat stat in statModifier)
            {
                statsHandler.AddStatModifier(stat);
            }
        }

        // 최대 체력을 올리거나 회복하는 경우
        HealthSystem healthSystem = go.GetComponent<HealthSystem>();
        healthSystem.ChangeHealth(0);
    }
}
