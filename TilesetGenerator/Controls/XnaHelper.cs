
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TilesetGenerator.Common;

namespace TilesetGenerator.Controls
{
    public class XnaHelper : UserControl
    {
        private static XnaHelper instance = new XnaHelper();

        private GraphicsDeviceService service;

        public static XnaHelper Instance
        {
            get
            {
                return instance;
            }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return service.GraphicsDevice; }
        }

        private XnaHelper()
        {
            service = GraphicsDeviceService.AddRef(Handle, ClientSize.Width, ClientSize.Height);
        }

        public Texture2D LoadTexture(string path)
        {
            Bitmap bitmap = new Bitmap(path); // Must be .png type
            // Need a universal graphics device
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);

                return Texture2D.FromStream(GraphicsDevice, stream);
            }
        }

        public Task<Texture2D> LoadTextureAsync(string path)
        {
            return Task.Run(() =>
                {                   
                    Bitmap bitmap = new Bitmap(path); // Must be .png type
                    // Need a universal graphics device
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        stream.Seek(0, SeekOrigin.Begin);

                        return Texture2D.FromStream(GraphicsDevice, stream);
                    }                  
                });
        }

        public Texture2D LoadTexture(Bitmap image)
        {
            Bitmap bitmap = image; // Must be .png type
            // Need a universal graphics device
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);

                return Texture2D.FromStream(GraphicsDevice, stream);
            }
        }

        public Task<Texture2D> LoadTextureAsync(Bitmap image)
        {
            return Task.Run(() =>
                {
                    Bitmap bitmap = image; // Must be .png type
                    // Need a universal graphics device
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        stream.Seek(0, SeekOrigin.Begin);

                        return Texture2D.FromStream(GraphicsDevice, stream);
                    }
                });
        }

        public Bitmap TextureToBitmap(Texture2D texture)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                texture.SaveAsJpeg(memoryStream, texture.Width, texture.Height); //Or SaveAsPng( memoryStream, texture.Width, texture.Height )

                return new System.Drawing.Bitmap(memoryStream);
            }
        }

    }
}
