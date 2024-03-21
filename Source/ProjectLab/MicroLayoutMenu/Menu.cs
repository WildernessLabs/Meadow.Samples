using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;

namespace MicroLayoutMenu
{
    public class Menu
    {
        private Label[] _labels;
        private Box _highlightBox;

        private const int ItemHeight = 30;

        public int SelectedRow { get; set; } = 0;

        public readonly Color UnselectedTextColor = Color.AntiqueWhite;
        public readonly Color SelectedTextColor = Color.Black;
        public readonly Color SelectionColor = Color.Orange;
        public readonly IFont MenuFont = new Font12x20();

        public Menu(string[] items, DisplayScreen screen)
        {
            _labels = new Label[items.Length];

            var x = 2;
            var y = 0;
            var height = ItemHeight;

            // we compose the screen from the back forward, so put the box on first
            _highlightBox = new Box(0, -1, screen.Width, ItemHeight + 2)
            {
                ForeColor = SelectionColor,
                IsFilled = true,
            };

            screen.Controls.Add(_highlightBox);

            for (var i = 0; i < items.Length; i++)
            {
                _labels[i] = new Label(
                    left: x,
                    top: i * height,
                    width: screen.Width,
                    height: height)
                {
                    Text = items[i],
                    Font = MenuFont,
                    BackColor = Color.Transparent,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                // select the first item
                if (i == 0)
                {
                    _labels[i].TextColor = SelectedTextColor;
                }
                else
                {
                    _labels[i].TextColor = UnselectedTextColor;
                }

                screen.Controls.Add(_labels[i]);


                y += height;
            }
        }

        public void Down()
        {
            if (SelectedRow < _labels.Length - 1)
            {
                SelectedRow++;

                Resolver.Log.Info($"MENU SELECTED: {_labels[SelectedRow].Text}");

                Draw(SelectedRow - 1, SelectedRow);
            }
        }

        public void Up()
        {
            if (SelectedRow > 0)
            {
                SelectedRow--;

                Resolver.Log.Info($"MENU SELECTED: {_labels[SelectedRow].Text}");

                Draw(SelectedRow + 1, SelectedRow);
            }
        }

        public void Draw(int oldRow, int newRow)
        {
            _labels[oldRow].TextColor = UnselectedTextColor;
            _labels[newRow].TextColor = SelectedTextColor;

            _highlightBox.Top = _labels[newRow].Top - 1;
        }
    }
}