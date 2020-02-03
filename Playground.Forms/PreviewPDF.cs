
using ImageMagick;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Playground.Forms
{
    public partial class PreviewPDF : Form
    {
        public PreviewPDF()
        {
            InitializeComponent();
            MagickNET.SetGhostscriptDirectory(@"gs\");
        }

        private void BtnRead_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (dialog.CheckFileExists)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    previewBox.Image = Image.FromFile("giphy.gif");
                    Task.Run(async () =>
                    {
                        MagickReadSettings settings = new MagickReadSettings
                        {
                            // Settings the density to 300 dpi will create an image with a better quality
                            //Density = new Density(96)
                        };

                        using (MagickImageCollection images = new MagickImageCollection())
                        {
                            // Add all the pages of the pdf file to the collection
                            images.Read(dialog.OpenFile(), settings);
                            for (int i = 1; i <= images.Count; i++)
                            {
                                //foreach (MagickImage image in images)
                                //{
                                //    using (var stream = new MemoryStream())
                                //    {
                                //        previewBox.Image = image.ToBitmap();
                                //    }
                                //}
                                var image = images.AppendHorizontally();
                                using (var stream = new MemoryStream())
                                {
                                    previewBox.Image = image.ToBitmap();
                                }
                            }
                        }
                    }).ContinueWith(new Action<Task>((t) =>
                    {
                        Console.WriteLine(sw.Elapsed);
                    }));
                }
            }
        }
    }
}
