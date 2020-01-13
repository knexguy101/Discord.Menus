# Discord.Menus .NET Framework


Discord.Menus is an package that can be used with [Discord.NET Framework](https://github.com/discord-net/Discord.Net) in order to build a functioning, real time, UI.

  - Create multiple tabs of different UI Elements
  - Navigate, Exit, and Interact with the UI using Reactions
  - Asynchronous in order to best run with other functions within the bot

# UI Elements

  - Button
  - Field
  - Textbox
  - Checkbox
  - Dropdown
  - Tab
  - More coming soon!


Why use Discord.Menus?
  - Users only interact with, at a maximum, of two messages at a time, eliminating clutter
  - Developers can read and edit the values of the elements in real time outside of the original instance a UI was initialized.
  - It can be implemented in as little as 5 lines, making it one of the easiest packages to integrate with your bot.

# Packages

Discord.Menus is completely open source as well as all of its packages

* [discord-net](https://github.com/discord-net/Discord.Net) - Open source framework for c# discord bot creation

# Installation

Discord.Menus requires [.NET Framework in order to work](https://dotnet.microsoft.com/download/dotnet-framework) v4.5+ to run.

# Usage
### Usage in a command
```cs
    public class OpenGUI : ModuleBase
    {
        [Command("open", RunMode = RunMode.Async)]
        public async Task OpenAsync()
        {
            //init your main message, this will be the one that is edited with the GUI
            var message = await Context.Message.Channel.SendMessageAsync("Discord.Menus Framework by Knexguy101", false, null);

            //init your Tab element, this is used to store elements and place them in the UI
            Tab test = new Tab();

            //Add elements
            test.AddElement(new Field("Testing", "Testing Label"));
            test.AddElement(new Checkbox(true, "Testing checkbox"));
            test.AddElement(new Dropdown(new List<string>
            {
                "test 1",
                "test 2",
                "test 3"
            }.ToArray(), "Testing dropdown"));
            test.AddElement(new Textbox("Entry test", "Testing textbox"));
            test.AddElement(new Button("customemotename", "test", () => 
            {
                Console.WriteLine("test action");
            }));

            //init second tab
            Tab test2 = new Tab();

            //add elements to 2nd tab
            test2.AddElement(new Field("Testing 2", "Testing Label 2"));

            //create a list of tabs
            List<Tab> TabList = new List<Tab>()
            {
                test,
                test2
            };


            //init the main GUI Class
            GUI gui = new GUI(message, TabList, Context.User, Context.Client as DiscordSocketClient);

            //run it
            await gui.RunGUI();
        }
    }
```

