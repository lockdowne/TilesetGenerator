using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using TilesetGenerator.Controls;
using TilesetGenerator.Common;
using Microsoft.Xna.Framework;
using Telerik.WinControls.Drawing;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace TilesetGenerator.Views
{
    public partial class MainView : RadForm
    {
        private RootNode root;

        public RootNode Root
        {
            get { return root; }
            set { root = value; }
        }

        public RadTreeNode SelectedNode
        {
            get { return radTreeView1.SelectedNode; }
        }

        public MainView()
        {
            InitializeComponent();

            this.root = new RootNode();
            this.root.ContextMenu = radContextMenu1;
            this.radDock1.DockWindowClosing += (sender, e) =>
            {
                e.Cancel = true;
            };
            this.commandBarStripElement1.OverflowButton.Visibility = ElementVisibility.Hidden;
          
            this.btnAddImage.Click += async (sender, e) =>
            {
                try
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Filter = "Image Files (.png)|*.png;";
                        dialog.Multiselect = true;
                        dialog.RestoreDirectory = true;

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            // Get filepath
                            string[] filePaths = dialog.FileNames;

                            foreach (string str in filePaths)
                            {
                                string name = System.IO.Path.GetFileNameWithoutExtension(str);

                                foreach(RadTreeNode node in Root.Nodes)
                                {
                                    if (name == node.Text)
                                        return;
                                }

                                Bitmap original = new Bitmap(str);
                                Bitmap bitmap = new Bitmap((Image)original, 16, 16);

                                
                                Texture2D texture = await XnaHelper.Instance.LoadTextureAsync(str);                                
                                
                                DragTexture dragTexture = new DragTexture() { Texture = XnaHelper.Instance.LoadTexture(str), Position = Vector2.Zero, Name = name };

                                Root.Nodes.Add(new RadTreeNode() { Image = bitmap, Text = name, Value = dragTexture });
                            }
                        }
                    }
                }
                catch(Exception exception)
                {
                    RadMessageBox.Show(exception.ToString(), "Exception");
                }
            };
            this.radTreeView1.MouseDoubleClick += (sender, e) =>
            {
                try
                {
                    if (SelectedNode == null)
                        return;

                    if (SelectedNode == Root)
                        return;

                    DragTexture texture = (DragTexture)SelectedNode.Value;

                    if (!renderControl1.Textures.Any(t => t.Name == texture.Name))
                        renderControl1.Textures.Add(texture);
                }
                catch(Exception exception)
                {
                    RadMessageBox.Show(exception.ToString(), "Exception");
                }
                
            };

           

            try
            {
                this.radTreeView1.Nodes.Add(root);
                
            }
            catch(Exception exception)
            {
                RadMessageBox.Show(exception.ToString(), "Exception");
            }

            
        }

        private void btnNewTileset_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = RadMessageBox.Show("Are you sure you want to clear all images?", "Hey Listen!", MessageBoxButtons.YesNo);

                if(dialog == System.Windows.Forms.DialogResult.Yes)
                {
                    renderControl1.Textures.Clear();
                }
            }
            catch(Exception exception)
            {
                RadMessageBox.Show(exception.ToString(), "Exception");
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            // Hard part
            try
            {
                using(SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "Image Files (.png)|*.png;";
                    dialog.RestoreDirectory = true;

                    if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        //string filePath = dialog.FileName;

                        //await SaveImage(filePath);
                        await Task.Run(() =>
                            {
                                renderControl1.TakeScreenShot(dialog.FileName);
                            });
                    }
                }
            }
            catch(Exception exception)
            {
                RadMessageBox.Show(exception.ToString(), "Exception");
            }
        }
    }
}
