using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaiseTheFlagWinForms
{
    public partial class Form1 : Form
    {
        Image image;

        private bool IsFlagMoving { get; set; } = false;

        public Form1()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            //Load an image in from a file
            image = new Bitmap("flag.png");

            //Set our picture box to that image
            flag.Image = (Bitmap)image.Clone();

            button.Click += OnButtonClicked;
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            await RotateFlag();
        }

        private async Task RotateFlag()
        {
            if (IsFlagMoving) return;
            IsFlagMoving = true;

            var change = -10;

            for (int i = 0; i < 18; i++)
            {
                if (i == 9)
                {
                    change *= -1;
                    await Task.Delay(1000);
                }

                Image oldImage = flag.Image;
                flag.Image = RotateImage(flag.Image, new PointF(15, image.Height / 2 + 15), change);
                if (oldImage != null)
                {
                    oldImage.Dispose();
                }
                await Task.Delay(50);
            }
            IsFlagMoving = false;
        }

        public static Bitmap RotateImage(Image image, PointF offset, float angle)
        {
            var newImage = new Bitmap(image.Width, image.Height);
            var g = Graphics.FromImage(newImage);
            g.TranslateTransform(offset.X, offset.Y);
            g.RotateTransform(angle);
            g.TranslateTransform(-offset.X, -offset.Y);
            g.DrawImage(image, new PointF(0, 0));

            return newImage;
        }
    }
}
