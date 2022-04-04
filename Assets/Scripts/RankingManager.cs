using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public Rank[] TopRanks;
    public Rank PersonalRank;

    void Start()
    {
        Leaderboard.GetLeaderboard().Then(res =>
        {
            for (var i = 0; i < res.Length; i++)
            {
                var entry = res[i];
                var minutes = (int)entry.score / 60;
                
                TopRanks[i].Name.text = entry.name;
                TopRanks[i].Time.text = $"{minutes}:{(entry.score - 60f * minutes).ToString("00.###", CultureInfo.InvariantCulture)}";
            }
        });

        Leaderboard.GetOwn(NameManager.Instance.Name).Then(res =>
        {
            var minutes = (int)res.score / 60;
            PersonalRank.Name.text = res.name;
            PersonalRank.Time.text = $"{minutes}:{(res.score - 60f * minutes).ToString("00.###", CultureInfo.InvariantCulture)}";
            PersonalRank.RankNr.text = $"{res.id + 1}";
            
            if (res.id == -1)
            {
                PersonalRank.RankNr.text = "-";
            }
        });
    }
}
