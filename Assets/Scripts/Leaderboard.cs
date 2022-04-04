using Newtonsoft.Json;
using Proyecto26;
using RSG;

public class Leaderboard
{
    private static string api = "https://lboard.cocapasteque.tech/leaderboard";

    public static IPromise<BoardEntry[]> GetLeaderboard()
    {
        return RestClient.Get<BoardEntry[]>(api);
    }

    public static IPromise<BoardEntry> GetOwn(string name)
    {
        return RestClient.Get<BoardEntry>(api + "/" + name);
    }

    public static IPromise<BoardEntry> SendScore(BoardEntry entry)
    {
        return RestClient.Post<BoardEntry>(api, entry);
    }
}

public class BoardEntry
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("score")] public float Score { get; set; }
}