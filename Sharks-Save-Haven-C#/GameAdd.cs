using System;
using Eto.Forms;
using Eto.Drawing;
using Sharks_Save_Haven.Models;
using System.Data;
using System.Linq;
using Sharks_Save_Haven.Api;
using System.Threading.Tasks;

namespace Sharks_Save_Haven;
public partial class GameAdd : Form
{
    public event EventHandler<Game> GameAdded;
    private ImageView headerImage;

    public GameAdd()
    {
        ClientSize = new Size(600, 350);
        Title = "Shark's Save Haven";
        var context = new SharkDbContext();
        context.Database.EnsureCreated();
        //context.Games.Add(new Game { Title = "Ham" });
        context.SaveChangesAsync();

        // A textbox with a button underneath it in a table layout
        var titleTextbox = new TextBox();
        var publisherTextbox = new TextBox();
        var platformTextbox = new TextBox();
        var releaseDateTextbox = new TextBox();
        var saveLocationTextbox = new TextBox();
        var steamDBTextbox = new TextBox();

        var lookupButton = new Button { Text = "Lookup" };
        var addGameButton = new Button { Text = "Add Game" };
        var cancelButton = new Button { Text = "Cancel" };

        var inputStacklayout = new StackLayout();
        var contentStacklayout = new StackLayout();
        contentStacklayout.Orientation = Orientation.Horizontal;
        contentStacklayout.Padding = new Padding(5, 5, 5, 5);

        var imageStacklayout = new StackLayout();
        imageStacklayout.Padding = new Padding(5, 5, 5, 5);
        
        inputStacklayout.Items.Add(new Label { Text = "Title" });
        inputStacklayout.Items.Add(titleTextbox);

        inputStacklayout.Items.Add(new Label { Text = "Publisher" });
        inputStacklayout.Items.Add(publisherTextbox);

        inputStacklayout.Items.Add(new Label { Text = "Platform" });
        inputStacklayout.Items.Add(platformTextbox);

        inputStacklayout.Items.Add(new Label { Text = "Release Date" });
        inputStacklayout.Items.Add(releaseDateTextbox);

        inputStacklayout.Items.Add(new Label { Text = "Save Location" });
        inputStacklayout.Items.Add(saveLocationTextbox);

        inputStacklayout.Items.Add(new Label { Text = "SteamDB" });
        inputStacklayout.Items.Add(steamDBTextbox);
        inputStacklayout.Items.Add(lookupButton);

        lookupButton.Click += async (sender, e) =>
        {
            if (int.TryParse(steamDBTextbox.Text, out int appId))
            {
                var gameData = await Steam.GetSteamGame(appId);
                await SetHeaderImage(appId);
            }
        };

        inputStacklayout.Items.Add(addGameButton);
        inputStacklayout.Items.Add(cancelButton);

        imageStacklayout.Items.Add(new Label { Text = "Image" });
        headerImage = new ImageView();
        imageStacklayout.Items.Add(headerImage);
        
        contentStacklayout.Items.Add(inputStacklayout);
        contentStacklayout.Items.Add(imageStacklayout);

        Content = contentStacklayout;
        
        addGameButton.Click += (sender, e) =>
        {
            GameAdded?.Invoke(this, new Game { Title = titleTextbox.Text });
            Close();
        };
    }

    public async Task SetHeaderImage(int appId)
    {
        byte[] bytes = await Steam.GetSteamHeaderImage(appId);
        if (bytes != null)
        {
            headerImage.Image = new Bitmap(bytes);
        }
    }
}