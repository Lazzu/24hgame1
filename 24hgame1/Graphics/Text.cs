using System;
using System.Drawing;
using System.Drawing.Text;
using OpenTK.Graphics.OpenGL;
using hgame1.Graphics.Textures;
using System.Drawing.Imaging;
using hgame1.Graphics.GUI;

namespace hgame1.Graphics
{
	/// <summary>
	///  Text GUI element. Renders text in gui. Updates automatically whenever needed.
	/// </summary>
	public class Text
	{
		/// <summary>
		/// The texture where text will be rendered on.
		/// </summary>
		public Texture Texture;

		Font textFont = new Font(FontFamily.GenericMonospace, 24);

		/// <summary> 
		/// Property for setting the font of the text.
		/// </summary> 
		public Font Font {
			get {
				return textFont;
			}
			set {
				textFont = value;
			}
		}

		float maxWidth = 1280;
		/// <summary>
		/// Gets or sets the maximum width of one line of text.
		/// </summary>
		/// <value>The maximum width of the line.</value>
		public float MaxWidth {
			get {
				return maxWidth;
			}
			set {
				maxWidth = value;
			}
		}

		Size size;
		/// <summary>
		/// Gets the size of the texture.
		/// </summary>
		/// <value>The size.</value>
		public Size Size {
			get {
				return size;
			}
		}

		Brush brush = new SolidBrush(Color.White);
		public Brush Brush {
			get {
				return brush;
			}
			set {
				brush = value;
			}
		}

		Bitmap textBitmap;

		/// <summary>
		/// Initializes a new instance of the <see cref="MGLALLib.Graphics.Text"/> class.
		/// </summary>
		/// <param name="texture">Texture where the text will be drawn on.</param>
		public Text (Texture texture)
		{
			Texture = texture;
		}

		BitmapData textureData;

		/// <summary>
		/// Writes the given text on the texture.
		/// </summary>
		/// <param name="text">Text.</param>
		public void WriteText(string text)
        {
			// Check that we actually have something in the string. Ignore if there is nothing.
            if (text.Length > 0)
            {
				// Variable for getting the text bitmap size
				SizeF size;

				// Split the text to the max width and get the size of the bitmap
				text = SplitToWidth (text, out size);

				// Set the text size
				this.size = new Size ((int)size.Width + 1, (int)size.Height + 1);

				// Create new text bitmap with the size
				textBitmap = new Bitmap(this.size.Width, this.size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				// Get Graphics from the text bitmap
				using(System.Drawing.Graphics gfx = System.Drawing.Graphics.FromImage(textBitmap))
				{
					// Set clear type hinting
					gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
					gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
					gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

					// Transparent background
					//gfx.Clear (Color.White);
					Color c = Color.FromArgb(0, ((SolidBrush)brush).Color);
					gfx.Clear (c);

					// Draw the string on the bitmap
					gfx.DrawString(text, textFont, brush, new PointF(0,0));
				}

				GrayscaleToAlpha (textBitmap);

				// Get the texture data
                textureData = textBitmap.LockBits(new Rectangle(0, 0, textBitmap.Width, textBitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				// Upload the texture data
				Upload ();
            }
        }

		void GrayscaleToAlpha(Bitmap image) {
			var lockData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			unsafe {
				// Pointer to the current pixel
				uint* pPixel = (uint*)lockData.Scan0;
				// Pointer value at which we terminate the loop (end of pixel data)
				var pLastPixel = pPixel + image.Width * image.Height;

				while (pPixel < pLastPixel) {
					// Get pixel data
					uint pixelValue = *pPixel;
					// Average RGBA
					uint brightness = ((pixelValue & 0xFF) + ((pixelValue >> 8) & 0xFF) + ((pixelValue >> 16) & 0xFF) + ((pixelValue >> 24) & 0xFF)) / 4;

					// Use brightness for alpha value, leave R, G, and B zero (black)
					pixelValue = 0xFFFFFFFF - ((0xFF - brightness) << 24);
					//pixelValue = 0xFFFFFFFF - pixelValue;

					// Copy back to image
					*pPixel = pixelValue;
					// Next pixel
					pPixel++;
				}

			}
			image.UnlockBits(lockData);
		}

		/// <summary>
		/// Splits the width of the text by splitting up the text.
		/// </summary>
		/// <returns>The to width.</returns>
		/// <param name="text">Text.</param>
		/// <param name="size">Size.</param>
		string SplitToWidth(string text, out SizeF size)
		{
			size = Text.GetTextSize(text, textFont);

			// Split the text from line breaks and spaces
			string[] words = text.Split (
				new string[]{
					"\n",
					"\r",
					" "
				}, 
				StringSplitOptions.RemoveEmptyEntries
			);

			// temp string for reconstructing the string
			string temp = "";

			// More temp variables :D
			string line = "";
			string lastLine = "";
			bool first = true;

			// Tuo foreach looppi toimii jostain syystä, en tiedä enää miksi. En mä sitä tuollain alunperin suunnitellu mutta tommonen siitä tuli.
			// Siinä on bugi. Jos ikkunan leveys on pienempi kuin ensimmäisen sanan leveys, ensimmäinen sana tulee jostakin syystä kahdesti.
			// TODO: Fiksaa bugi.

			// Loop through all words
			foreach (var word in words) {

				// Add the word on the line some way depending on if it is the first word
				if(first)
					line = word;
				else
					line += " " + word;

				// Get the size (width) of the line
				SizeF linesize = Text.GetTextSize(line, textFont);

				// If line width is less than the max width
				if(linesize.Width < maxWidth)
				{
					// Continue with the line. Set the "last line" as the current line for next round
					lastLine = line;
				}
				else
				{
					// The line would be longer than the maximum allowed

					// Add the word or last round's line line to the temp string
					if (first)
						temp += word + "\n";
					else
						temp += lastLine + "\n";

					// Set the last line and current line as the current word.
					lastLine = word;
					line = word;
				}

				// We are no longer on the first word
				first = false;
			}

			// Add the last line
			temp += line;

			// Overwrite the old string with the new
			text = temp;

			text = text.Replace ("<br>", "\n");

			// Get the current size of the text
			size = Text.GetTextSize(text, textFont);

			// Hurr durr a comment telling you the next line of code returns the text string to the caller. Talking about overcommenting?
			return text;
		}

		/// <summary>
		/// Upload the texture to the GPU.
		/// </summary>
		void Upload()
		{
			// Check if we have actually drawn something
			if (textureData != null || Texture.Disposed) {

				// Bind the texture
				Texture.Bind ();

				// Set the filters of the texture
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

				// Upload the texture
				GL.TexImage2D (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
				    			textureData.Width, textureData.Height, 0,
	        					OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);

				Texture.Size = new OpenTK.Vector2 (textureData.Width, textureData.Height);

				// Generate mipmaps
				GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

				// Unbind the texture
				Texture.UnBind ();

				// Unlock the bits that had been locked by the caller of this method.
				textBitmap.UnlockBits (textureData);

				// Set the data ready for GC
				textureData = null;


			}
		}

		static System.Drawing.Graphics gfx;
		static SizeF GetTextSize (string text, Font font)
		{
			// If there is no Graphics object for getting the text size, create one
			if(gfx == null)
			{
				gfx = System.Drawing.Graphics.FromImage (new Bitmap (1,1));
				gfx.Clear(Color.Transparent);
				gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			}

			SizeF mSize = gfx.MeasureString(text, font);

			return mSize;
		}

		#region IDisposable implementation
		/// <summary> 
		/// Implement the IDisposable pattern
		/// </summary> 
		public void Dispose()
		{
			textFont.Dispose ();
			textureData = null;
			GC.SuppressFinalize(this);
		}
		/// <summary> 
		/// Destructor detects leaked resources
		/// </summary> 
		~Text()
		{
			Dispose ();
		}
		#endregion
	}
}

