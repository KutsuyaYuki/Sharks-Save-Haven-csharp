using System;
using Eto.Forms;
using Eto.Drawing;
using Sharks_Save_Haven.Models;

namespace Sharks_Save_Haven;

public class MyCommand : Command
{
    public MyCommand()
    {
        MenuText = "C&lick Me, Command";
        ToolBarText = "Click Me";
        ToolTip = "This shows a dialog for no reason";
        //Image = Icon.FromResource ("MyResourceName.ico");
        //Image = Bitmap.FromResource ("MyResourceName.png");
        Shortcut = Application.Instance.CommonModifier | Keys.M;  // control+M or cmd+M
    }

    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);

        MessageBox.Show(Application.Instance.MainForm, "You clicked me!", "Tutorial 2", MessageBoxButtons.OK);
    }
}

public partial class MainForm : Form
{
    public MainForm()
    {
        ClientSize = new Size(1280, 720);
        Title = "Shark's Save Haven";
        var context = new SharkDbContext();
        context.Database.EnsureCreated();
        context.Games.Add(new Game { Title = "Ham" });
        context.SaveChangesAsync();

        base.Menu = CreateMenu();
    }

    private MenuBar CreateMenu()
    {
        // create menu
        Command aboutCommand = new Command((sender, e) => new Dialog { Content = new Label { Text = "About my app..." }, ClientSize = new Size(200, 200) }.ShowModal(this))
        {
            MenuText = "About my app"
        };
        Command quitCommand = new Command((sender, e) => Application.Instance.Quit())
        {
            MenuText = "Quit",
            Shortcut = Application.Instance.CommonModifier | Keys.Q
        };

        return new MenuBar
        {
            Items = {
                new ButtonMenuItem
                {
                    Text = "&File",
                    Items = { 
                        // you can add commands or menu items
                        new MyCommand(),
                        new ButtonMenuItem { Text = "Click Me, MenuItem" }
                    }
                }
            },
            // quit item (goes in Application menu on OS X, File menu for others)
            QuitItem = quitCommand,
            // about command (goes in Application menu on OS X, Help menu for others)
            AboutItem = aboutCommand
        };
    }
}