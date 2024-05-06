using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    Discord.Discord discord;
    void Start()
    {
        discord = new Discord.Discord(1237111564444766349, (ulong)Discord.CreateFlags.NoRequireDiscord);
        ChangeActivity();
    }

    private void OnDisable()
    {
        discord.Dispose();
    }

    public void ChangeActivity()
    {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = "Playing",
            Details = "Hello world!",
            Assets =
            {
                LargeImage = "game_logo",
                LargeText = "Hidden Library"
            }
        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            Debug.Log("Activity updated");
        });
    }
    void Update()
    {
        discord.RunCallbacks();
    }
}
