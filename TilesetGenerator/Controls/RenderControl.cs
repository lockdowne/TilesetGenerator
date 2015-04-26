using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using TilesetGenerator.Common;

namespace TilesetGenerator.Controls
{
    public class RenderControl : GraphicsDeviceControl
    {
        private SpriteBatch spriteBatch;

        private Vector2 cameraPosition;
        private Vector2 currentMousePosition;
        private Vector2 previousMousePosition;

        private float cameraZoom;

        private Camera camera;

        private DragTexture selectedTexture;

        private bool isLeftMouseDown;
        private bool isRightMouseDown;

        private Texture2D pixel;

        public List<DragTexture> Textures = new List<DragTexture>();        

        protected override void Initialize()
        {
            try
            {
                spriteBatch = new SpriteBatch(GraphicsDevice);
                camera = new Camera() { LerpAmount = 0.25f, Zoom = 1.0f };
                cameraZoom = 1.0f;
                pixel = new Texture2D(GraphicsDevice, 2, 2, false, SurfaceFormat.Color);
                pixel.SetData<Color>(new Color[] { Color.White, Color.White, Color.White, Color.White });
                KeyDown += (sender, e) =>
                {
                    if(e.KeyCode == Keys.Delete)
                    {
                        if (selectedTexture != null)
                            Textures.Remove(selectedTexture);
                    }
                };

                MouseDown += (sender, e) =>
                {
                    switch (e.Button)
                    {
                        case System.Windows.Forms.MouseButtons.Left:
                            isLeftMouseDown = true;

                            foreach (DragTexture t in Textures)
                            {
                                if (t.Intersects(MathExtensions.InvertMatrixAtVector(new Vector2(e.Location.X, e.Location.Y), camera.CameraTransformation)))
                                {
                                    selectedTexture = t;
                                }
                            }
                            break;
                        case System.Windows.Forms.MouseButtons.Right:
                            previousMousePosition = MathExtensions.InvertMatrixAtVector(new Vector2(e.Location.X, e.Location.Y), camera.CameraTransformation);
                            isRightMouseDown = true;
                            break;
                        case System.Windows.Forms.MouseButtons.Middle:
                            selectedTexture = null;
                            break;
                    }
                };

                MouseMove += (sender, e) =>
                {
                    if(isLeftMouseDown)
                    {
                        if(selectedTexture != null)
                        {
                            selectedTexture.Position = MathExtensions.OrthogonalSnap(MathExtensions.InvertMatrixAtVector(new Vector2(e.Location.X, e.Location.Y), camera.CameraTransformation), Consts.TileWidth, Consts.TileHeight);
                        }
                    }
                    else if(isRightMouseDown)
                    {
                        currentMousePosition = MathExtensions.InvertMatrixAtVector(new Vector2(e.Location.X, e.Location.Y), camera.CameraTransformation);

                        Vector2 difference = currentMousePosition - previousMousePosition;

                        cameraPosition += -difference;

                        camera.UpdatePosition(cameraPosition, Vector2.Zero, new Vector2(Consts.MaxTextureWidth - GraphicsDevice.Viewport.Width + 2, Consts.MaxTextureHeight - GraphicsDevice.Viewport.Height + 2));

                        cameraPosition = camera.Position;
                    }
                };

                MouseUp += (sender, e) =>
                {
                    if (isLeftMouseDown)
                        isLeftMouseDown = false;

                    if (isRightMouseDown)
                        isRightMouseDown = false;
                };
            }
            catch(Exception exception)
            {
                RadMessageBox.Show(exception.ToString(), "Exception");
            }
            Application.Idle += (sender, e) => { Invalidate(); };
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(new Color(40, 40, 40));           
            
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.CameraTransformation);
            DrawRectangle(spriteBatch, new Rectangle(0, 0, Consts.MaxTextureWidth, Consts.MaxTextureHeight), Color.White, 2);
            new List<DragTexture>(Textures).ForEach(t =>
            {
                if (t == selectedTexture)
                {
                    DrawRectangle(spriteBatch, selectedTexture.Bounds, Color.Red, 2);                    
                }
                
                spriteBatch.Draw(t.Texture, t.Position, Color.White);
            });
            spriteBatch.End();
        }

        public void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(pixel, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        } 

        public void TakeScreenShot(string path)
        {
            try
            {
                int width = (int)MathHelper.Clamp(Textures.Max(r => r.Texture.Width + r.Position.X), 1, Consts.MaxTextureWidth);
                int height = (int)MathHelper.Clamp(Textures.Max(r => r.Texture.Height + r.Position.Y), 1, Consts.MaxTextureHeight);
                selectedTexture = null;
                RenderTarget2D screenshot = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);
                GraphicsDevice.SetRenderTarget(screenshot);
                GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin();
                new List<DragTexture>(Textures).ForEach(t =>
                {
                    if (t == selectedTexture)
                    {
                        DrawRectangle(spriteBatch, selectedTexture.Bounds, Color.Red, 2);
                    }

                    spriteBatch.Draw(t.Texture, t.Position, Color.White);
                });
                spriteBatch.End();

                GraphicsDevice.SetRenderTarget(null);

                Save(screenshot, path, width, height);
            }
            catch (Exception)
            {

            }
        }

        private void Save(RenderTarget2D texture, string path, int width, int height)
        {
            using(FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                texture.SaveAsPng(stream, width, height);
            }
        }
     
    }
}
