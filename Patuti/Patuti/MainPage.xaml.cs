using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Patuti
{
    public partial class MainPage : ContentPage
    {
        DisplayInfo mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
        private float xIncrement = 0;
        private float yIncrement = 0;
        private string patutiPath;
        private const string IMAGES_PATH = "Patuti.Moves.";
        private const int SPEED = 20;
        private const int JUMP_HEIGHT = -100;

        private string[] idle = new string[2]
            {
                "idle-1.png",
                "idle-2.png"
            };

        private string[] leftMoves = new string[5]
            {
                "left-1.png",
                "left-2.png",
                "left-3.png",
                "left-4.png",
                "left-5.png"
            };

        private string[] rightMoves = new string[5]
            {
                "right-1.png",
                "right-2.png",
                "right-3.png",
                "right-4.png",
                "right-5.png"
            };

        private string[] jumpMoves = new string[7]
            {
                "jump-1.png",
                "jump-2.png",
                "jump-3.png",
                "jump-4.png",
                "jump-5.png",
                "jump-6.png",
                "jump-7.png"
            };

        private string[] dockMoves = new string[5] 
            {
                "dock-4.png",
                "dock-4.png",
                "dock-4.png",
                "dock-4.png",
                "dock-4.png",
            };

        public MainPage()
        {
            InitializeComponent();
            patutiPath = IMAGES_PATH + idle[0];
            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                canvasView.InvalidateSurface();
                return true;
            });
        }

        [Obsolete]
        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            SKBitmap patuti;
            canvas.Clear(SKColors.CornflowerBlue);

            using (Stream stream = assembly.GetManifestResourceStream(patutiPath))
            {
                patuti = SKBitmap.Decode(stream);
                SKImageInfo dstInfo = new SKImageInfo((int)(mainDisplayInfo.Width / 5),
                    (int)(patuti.Height * (mainDisplayInfo.Width / 5)) / patuti.Width);
                patuti = patuti.Resize(dstInfo, SKBitmapResizeMethod.Hamming);
            }

            float xPosition = e.Info.Width / 2 + xIncrement;
            float yPosition = e.Info.Height / 2 + yIncrement;
            canvas.DrawBitmap(patuti, xPosition, yPosition);
        }

        public async void OnUpClicked(object sender, EventArgs e)
        {
            foreach (string move in jumpMoves)
            {
                patutiPath = IMAGES_PATH + move;
                await Task.Delay(100);
                yIncrement = JUMP_HEIGHT;
            }
            yIncrement = 0;
            patutiPath = IMAGES_PATH + idle[0];
        }

        public async void OnDownClicked(object sender, EventArgs e)
        {
            yIncrement = 50;
            foreach (string move in dockMoves)
            {
                patutiPath = IMAGES_PATH + move;
                await Task.Delay(100);
            }
            yIncrement = 0;
            patutiPath = IMAGES_PATH + idle[0];
        }

        public async void OnLeftClicked(object sender, EventArgs e)
        {
            foreach (string move in leftMoves)
            {
                patutiPath = IMAGES_PATH + move;
                await Task.Delay(100);
                xIncrement -= SPEED;
            }
            patutiPath = IMAGES_PATH + idle[0];
        }

        public async void OnRightClicked(object sender, EventArgs e)
        {
            foreach (string move in rightMoves)
            {
                patutiPath = IMAGES_PATH + move;
                await Task.Delay(100);
                xIncrement += SPEED;
            }
            patutiPath = IMAGES_PATH + idle[0];
        }
    }
}
