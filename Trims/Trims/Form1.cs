using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColourTrimmer
{
	public partial class Form1 : Form
	{

		ImageList imageList = new ImageList();


		List<Bitmap> bitmaps;
		string ReadColour = "";
		Color colorTochange;
		Color changedColour = Color.Transparent;
		string fileName;

		public Form1()
		{
			
			InitializeComponent();
		}

	
		//Code that adds two zeros to the front of a color code which was specified.
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			string ReadColour = PlaceColourHere.Text;
			
		}

		//Event handler to change the cursor when it hovers over the box
		private void OnFileDragged(object sender, DragEventArgs e){
			e.Effect = DragDropEffects.All;	
		}


		//EventHandler for Images being dropped in to the program
		private void OnFileDropped(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			Bitmap[] images = GetBitmapsFromImages(files);

			try
			{
				colorTochange = ColorTranslator.FromHtml("#00" + PlaceColourHere.Text.Substring(1));
				GetPathToSave();
				RemoveColourFromBitmaps(images);
			}
			catch (ArgumentException){
				MessageBox.Show("Please enter a valid 6-digit hex colour code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
				
				
			
			


		}


		// I took out the Image.Save so this method now returns a DialogResult that we can  get a directory from

		private void GetPathToSave()
		{

			var saveFileDialog1 = new SaveFileDialog();

			saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			saveFileDialog1.Filter = string.Format("Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...");
			saveFileDialog1.RestoreDirectory = true;
			saveFileDialog1.ShowHelp = true;
			saveFileDialog1.CheckFileExists = false;
		
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				fileName = saveFileDialog1.FileName;
			}
			
		}



		//Method to change the image files into editable bitmap objects
		Bitmap[] GetBitmapsFromImages(string[] fls)
		{
			Bitmap[] images = new Bitmap[fls.Length];

			for (int i = 0; i < fls.Length; i++){
				try{
					images[i] = (Bitmap)Image.FromFile(fls[i]);
				}
				catch (OutOfMemoryException){
					MessageBox.Show("Please enter an image file(s) which is compitable with GDI+\n(.bmp,.png,.gif,.tiff)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			return images;
		}

		//This method can maybe be used to save a bitmap to files too.
		void  RemoveColourFromBitmaps(Bitmap[] bitmaps)
		{
			try
			{
				for (int i = 0; i < bitmaps.Length; i++)
				{
					Cursor.Current = Cursors.WaitCursor;
					bitmaps[i].MakeTransparent(colorTochange);
					bitmaps[i].Save(String.Format(fileName+"_{0}.png",i), System.Drawing.Imaging.ImageFormat.Png);
				}
			}
			catch (Exception) {
				MessageBox.Show("It is possible your colour is not present in this image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			
		}

					
		


		private void Form1_Load(object sender, EventArgs e)
		{

		}
	}

	



}
