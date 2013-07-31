using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Strawhat.Games._2DModelEditor.Models
{
    public class StringResources
    {
#if DEBUG
        public static readonly string _2D_MODEL_FILE_FILTER_ = "2D Model File (*.SGR_2DM)|*.SGR_2DM|All Files|*.*";
        public static readonly string _TEXTURE_FILE_FILTER_ = "Alpha Enabled Texture (Recommended) (*.png, *.gif, *.tga)|*.PNG;*.GIF;*.TGA|No Alpha Enabled Texture (*.bmp, *.jpg)|*.bmp;*.jpg|All Files (*.*)|*.*";
#else
        public const string _2D_MODEL_FILE_FILTER_ = "2D Model File (*.SGR_2DM)|*.SGR_2DM|All Files|*.*";
        public const string _TEXTURE_FILE_FILTER_ = "Alpha Enabled Texture (*.png, *.gif, *.tga)|*.PNG;*.GIF;*.TGA|No Alpha Enabled Texture (*.bmp, *.jpg)|*.bmp;*.jpg|All Files (*.*)|*.*";
#endif
    }
}
