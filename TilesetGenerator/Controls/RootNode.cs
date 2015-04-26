using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace TilesetGenerator.Controls
{
    public class RootNode : RadTreeNode
    {

        public RootNode()
            : base()
        {
            Image = global::TilesetGenerator.Properties.Resources.Folder_6222;

            Text = "Images";

            Expand();
        }

        protected override void NotifyExpandedChanged(RadTreeNode node)
        {
            base.NotifyExpandedChanged(node);

            if (Expanded)
                Image = global::TilesetGenerator.Properties.Resources.Folder_6221;
            else
                Image = global::TilesetGenerator.Properties.Resources.Folder_6222;
        }
    }
}
