using System;
using Eto.Forms;
using Eto.Drawing;
using Sharks_Save_Haven.Models;
using System.Data;
using System.Linq;

namespace Sharks_Save_Haven;
public partial class GameAdd : Form
{
    public event EventHandler<Game> GameAdded;

    public GameAdd()
    {
        ClientSize = new Size(480, 300);
        Title = "Shark's Save Haven";
        var context = new SharkDbContext();
        context.Database.EnsureCreated();
        //context.Games.Add(new Game { Title = "Ham" });
        context.SaveChangesAsync();

        // A textbox with a button underneath it in a table layout
        var title = new TextBox();
        var titleButton = new Button { Text = "Add Game" };

        Content = new TableLayout
        {
            Padding = new Padding(10),
            Spacing = new Size(5, 5),
            Rows =
            {
                new TableRow(
                    new TableCell(new Label { Text = "Title" }, true),
                    new TableCell(title, true)
                ),
                new TableRow(
                    new TableCell(),
                    new TableCell(titleButton, true)
                )
            }
        };
        titleButton.Click += (sender, e) =>
        {
            GameAdded?.Invoke(this, new Game { Title = title.Text });
            Close();
        };
    }
}