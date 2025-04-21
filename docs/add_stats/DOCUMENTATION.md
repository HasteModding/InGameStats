# Introduction

Since In-Game Stats 2.0, adding your mod stats is now available.  
This documentation will explain how to do so correctly.  

# Import InGameStats

To plug your mod on mine, you will need to import it in your `*.csproj` file.

If you are using HP's great template on GitHub, you can simply use the `.steam.props` you can find [here](./.steam.props) (docs/add_stats/.steam.props).  
Make sure to enable the workshop mods you need (l.31 - l.36)!

Otherwise, you will need to locate the mod's workshop folder. It should be under:
`{SteamLibraryPath}/steamapps/workshop/content/1796470/3457771863`

One way to get the location easily is having the mod in-game, go to `F1` > `Steam Workshop` > `In-Game Stats` > `Open` (next to `SteamLibrary` line)

Once you have this path, you only need to include the dll the following way in your `*.csproj` file:
```xml
<ItemGroup>
    <Reference Include="{SteamLibraryPath}/steamapps/workshop/content/1796470/3457771863/*.dll" Private="false">
</ItemGroup>
```
You should place this code next to your Haste dll import line, in the `<Target>` part.

# Use InGameStats

Basically, you only need to create a class that inherits from `IG_Stat` class, and register your new class to `InGameStats`.

Basic example:
```cs
// MyStat.cs
public class MyStat : IG_Stat {
    private static bool _enabled = true;

    public override bool Enabled {
        get => _enabled;
        set => _enabled = value;
    }

    public static void SetEnabled(bool enabled) {
        _enabled = enabled;
    }

    public override LocalizedString DefaultText => new UnlocalizedString("My Stat: Loaded");

    // Called when InGame Stats updates its UI elements content
    public override void OnUpdate(TextMeshProUGUI? text, ColorizedMode colorized) {
        if (text == null || !Enabled) return; // In that case, our stat doesn't do anything because it isn't displayed.
        text.text = "My Stat: my super value"; // You can show anything you computed
        if (colorized == ColorizedMode.Colorized) text.color = Color.blue; // If you don't affect colors, it'll always be white.
    }

    /*
    You can add more functions that gets called on specific events! Here are the overridable functions you might want to use:
    - OnTextCreation(text, colorized) // The TextMeshProUGUI just got created
    - OnTextDestroy(text) // The TextMeshProUGUI is about to get detroyed (for example when the list of enabled stats is changed)
    - OnStartNewRun() // When a new run (shard) is started.
    - OnNewLevel() // When a new level (fragment) is started.
    - OnSpawnedInHub() // When player spawns in the hub.
    - OnMainMenuPlayButton() // When the Play Button is pressed.
    - OnLandAction(landingType, saved) // When a the player lands and triggers a landing score.
    - OnHitLandingObjectComputed(landing) // If you want to do something with the raw landing object. Be careful: it doesn't check if the player was in the air!
    - OnLandingScoreComputed(landingScore) // I compute the landing score for you ahah, the score is between 0 and 1.
    */
}
```
```cs
// Program.cs
[LandfallPlugin]
public class Program {
    static Program() {
        // Register the stat in the InGameStats UI!
        InGameStats.RegisterStat<MyStat>();
    }
}
```
And you should see a new line, dedicated to `MyStat`!

Now your job is to create the settings you need and all the computation you'll want to do to display your stats.  
If you want more stats examples, take a look at the `stats` folder. There, you'll find all implementation for built-in stats.  
For settings examples, you can have a look at `Settings.cs`, it contains all the settings objects used for InGameStats.  
You can also use the basic SettingTemplate in `Types.cs`, that creates a simple setting to enable / disable your stat.

Have fun!
