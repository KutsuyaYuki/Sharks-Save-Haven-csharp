using System;
using Eto.Forms;
using Eto.Drawing;
using Sharks_Save_Haven.Models;
using System.Data;
using System.Linq;
using System.Collections.ObjectModel;

namespace Sharks_Save_Haven;
public partial class MainForm : Form
{
    private ObservableCollection<Game> games = new ObservableCollection<Game>();

    private SharkDbContext context = new SharkDbContext();

    private Game selectedGame;
    public MainForm()
    {
        ClientSize = new Size(1280, 720);
        Title = "Shark's Save Haven";
        context.Database.EnsureCreated();
        //context.Games.Add(new Game { Title = "Ham" });
        context.SaveChangesAsync();

        base.Menu = CreateMenu();
        context.Games.ToList().ForEach(game => games.Add(game));
        //a grid in which the database gets displayed
        var grid = new GridView { DataStore = games };
        grid.SelectionChanged += (sender, e) =>
        {
            selectedGame = (Game)grid.SelectedItem;
        };
        grid.Columns.Add(new GridColumn
        {
            DataCell = new TextBoxCell("Id"),
            HeaderText = "Id"
        });
        grid.Columns.Add(new GridColumn
        {
            DataCell = new TextBoxCell("Title"),
            HeaderText = "Title"
        });

        Content = grid;
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
        Command gamesAddCommand = new Command((sender, e) =>
        {
            var gameAddForm = new GameAdd();
            gameAddForm.GameAdded += (sender, e) =>
            {
                var t = context.Games.Add(e);
                context.SaveChanges();
                games.Add(t.Entity);
            };
            gameAddForm.Show();
        })
        {
            MenuText = "Add Game"
        };
        Command gamesDeleteCommand = new Command((sender, e) =>
        {
            TableRow buttonRow = new TableRow(
                                    new TableCell(),
                                    new TableCell(new Button { Text = "Yes" }, true),
                                    new TableCell(new Button { Text = "No" }, true)
                                );
            buttonRow.ScaleHeight = true;
            new Dialog
            {
                Title = $"Deleting {selectedGame?.Title}",
                //content with a label and yes and no buttons
                Content = new TableLayout
                {
                    Padding = new Padding(10),
                    Spacing = new Size(5, 5),
                    Rows =
                {
                    new TableRow(
                        new TableCell(new Label { Text = $"You are about to delete {selectedGame?.Title}. Are you sure?\nThis will also remove all the save games" }, true)
                    ),
                    buttonRow
                }
                },

                //Content = new Label { Text = $"You are about to delete {selectedGame?.Title}. Are you sure?" },
            }.ShowModal(this);
        })
        {
            MenuText = "Delete Game"
        };
        return new MenuBar
        {
            Items =
            {
                // application menu
                new ButtonMenuItem { Text = "&Games", Items = { gamesAddCommand, gamesDeleteCommand} }
            },
            // quit item (goes in Application menu on OS X, File menu for others)
            QuitItem = quitCommand,
            // about command (goes in Application menu on OS X, Help menu for others)
            AboutItem = aboutCommand
        };
    }
}