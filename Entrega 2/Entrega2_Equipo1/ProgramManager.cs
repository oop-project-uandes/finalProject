using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Entrega2_Equipo1
{
    public class ProgramManager
    {
        private Library library;
        private Producer producer;
        private readonly string DEFAULT_LIBRARY_PATH = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\library.bin";
        private readonly string DEFAULT_PRODUCER_PATH = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\producer.bin";
        private bool _continue = true;
        private int startingOption;

        public void Run()
        {
            this.ShowPresentation();
            this.LoadingLibraryManager();
            this.LoadingProducerManager();
            while (this._continue == true)
            {
                this.startingOption = this.StartingMenu();
                switch (this.startingOption)
                {
                    // Import a file to My Library from a path defined by the user
                    // READY
                    case 0:
                        this.ImportImageFromPath();
                        break;

                    // Export a file from My Library to a path defined by the user
                    // READY
                    case 1:
                        this.ExportFileToPath();
                        break;

                    // Editing Area (Apply filters, features, watson, slideshares, etc)
                    case 2:
                        this.EditingArea();
                        break;

                    // Show library, add elements or erase elements, add or remove labels.
                    // READY
                    case 3:
                        this.ManageLibrary();
                        break;

                    // Search in the library
                    //READY
                    case 4:
                        this.ManageSearch();
                        break;

                    // Show smart lists, add smartlists or erase elements
                    //READY
                    case 5:
                        this.ManageSmartList();
                        break;
                    case 6:
                        this._continue = false;
                        break;
                }
            }
            this.SaveLibrary();
            this.SaveProducer();
            this.ShowGoodbye();
            return;
        }



        private void EditingArea()
        {

            while (true)
            {
                // Show the menu of the editing area, with the images currently loaded
                Console.Clear();
                List<string> EditingAreaTitle = this.LoadBannerData("editingarea.txt");
                string curr = "Images currently in the Working Area: \n";
                List<string> imagesIntTheWorkingArea = new List<string>();
                int count = 1;

                // Load the images in the working area
                foreach (Image image in this.producer.imagesInTheWorkingArea())
                {
                    imagesIntTheWorkingArea.Add($"{count}. Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}");
                    count++;
                }


                List<string> options = new List<string>() { "Apply Filters", "Use Features", "Import images to the Editing Area", "Delete images from the Editing Area", "Export images from the Editing Area", "Exit" };
                int selectedOption = 0;

                bool _continue2 = true;

                // Show the menu and gets the number selected
                while (_continue2 == true)
                {
                    Console.Clear();
                    int i = 1;
                    // Show the title
                    foreach (string titlestring in EditingAreaTitle)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                        Console.WriteLine(titlestring);
                    }

                    // Show the images in the working area
                    Console.SetCursorPosition((Console.WindowWidth - curr.Length) / 4, Console.CursorTop);
                    Console.WriteLine(curr);

                    foreach (string imagestring in imagesIntTheWorkingArea)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - imagestring.Length) / 2, Console.CursorTop);
                        Console.WriteLine(imagestring);
                    }



                    foreach (string option in options)
                    {
                        if (selectedOption == i - 1)
                        {
                            Console.WriteLine("\n");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{i}. {option}");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            i++;
                            Console.WriteLine("\n");
                            continue;
                        }
                        Console.WriteLine($"{i}. {option}");
                        i++;
                    }
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedOption -= 1;
                            if (selectedOption < 0)
                            {
                                selectedOption = 0;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            selectedOption += 1;
                            if (selectedOption > options.Count - 1)
                            {
                                selectedOption = options.Count - 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            _continue2 = false;
                            break;
                    }
                }


                int usrDecision1 = selectedOption;



                // If user wants to exit
                if (usrDecision1 == 5) break;

                // In other case, we enter in the switch
                switch (usrDecision1)
                {
                    // User wants to apply filters => READY
                    case 0:
                        this.ApplyFilters();
                        break;

                    // User wants to apply features =>  READY
                    case 1:
                        this.UseFeatures();
                        break;

                    // User wants to import images to the editing area => READY
                    case 2:
                        this.ImportToEditingArea();
                        break;

                    // User wants to delete images from the editing area => READY
                    case 3:
                        this.DeleteFromEditingArea();
                        break;

                    // User wants to export images from the editing area => READY
                    case 4:
                        this.ExportFromEditingArea();
                        break;
                }
            }
        }


        private void UseFeatures()
        {
            string choosefeature = "Please, choose the Feature you want to use: ";
            string presskey = "Press any key to continue...";
            string emptyWorkingArea = "Your Editing Area is empty";


            List<string> UseFeatureTitle = this.LoadBannerData("usefeatures.txt");
            List<string> options = new List<string>() { "Add Censorship", "Watson Face Recognition Analizer",
                                                            "Add text", "Merge images", "Resize image", "Mosaic",
                                                            "Collage", "Exit" };

            while (true)
            {
                // Verify that the working area is not empty
                if (this.producer.imagesInTheWorkingArea().Count == 0)
                {
                    Console.WriteLine("\n");
                    Console.SetCursorPosition((Console.WindowWidth - emptyWorkingArea.Length) / 2, Console.CursorTop);
                    Console.WriteLine(emptyWorkingArea);
                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                    Console.WriteLine(presskey);
                    Console.ReadKey();
                    return;
                }

                // Ask user what he wants to do, and if he wants to exit, we exit
                int usrDec = this.GenerateMenu(options, null, choosefeature, UseFeatureTitle);
                if (options[usrDec] == "Exit") break; ;

                switch (usrDec)
                {
                    // Want to apply censorship => READY
                    case 0:
                        this.ApplyCensorship();
                        break;

                    // Want to use Watson Analizer => READY
                    case 1:
                        //UseWatson(); => NOT IMPLEMENTED YET
                        this.ShowWatsonNotImplemented();
                        break;

                    // Add text feature => READY
                    case 2:
                        this.AddText();
                        break;
					case 3:
						//Merge => READY
						this.Merge();
						break;
					case 4:
                        //Resize => READY
                        this.Resize();
						break;
					case 5:
                        //Mosaic => READY
                        this.Mosaic();
						break;
					case 6:
                        //Collage => READY
                        this.Collage();
						break;

				}
            }
        }


        private void Mosaic()
        {
            Console.Clear();
            List<string> MosaicTitle = this.LoadBannerData("mosaic.txt");
            string completemosaic = "Mosaic Finished!";
            List<string> imageMosaic = this.ChooseWhichImagesWantToApplyFeature();
            List<Image> imagesinworkingarea = this.producer.imagesInTheWorkingArea();
            List<Image> mosaicImages = new List<Image>();
            int width;
            int height;
            Console.Clear();
            foreach (string titlestring in MosaicTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            foreach (string image in imageMosaic)
            {
                foreach (Image image1 in imagesinworkingarea)
                {
                    if (image == $"Name: {image1.Name} - Calification: {image1.Calification} - Resolution: {image1.Resolution[0]}x{image1.Resolution[1]} - AspectRatio: {image1.AspectRatio[0]}x{image1.AspectRatio[1]} - Clear: {image1.DarkClear}\n")
                    {
                        mosaicImages.Add(image1);
                    }
                }
            }

            Console.Write("Ingrese ancho y la altura de las imágenes que crean el mosaico");
            string size = Console.ReadLine();
            string[] sizeArray = size.Split(new string[] { "," }, StringSplitOptions.None);
            width = Convert.ToInt32(sizeArray[0]);
            height = Convert.ToInt32(sizeArray[1]);
            Image imageBase = mosaicImages[0];
            mosaicImages.Remove(imageBase);

            Bitmap mosaic = producer.Mosaic(imageBase, mosaicImages, width, height);
            imageBase.BitmapImage = mosaic;
            imageBase.Name = "Mosaic" + imageBase.Name;
            Console.SetCursorPosition((Console.WindowWidth -  completemosaic.Length) / 2, Console.CursorTop);
            Console.WriteLine(completemosaic);

        }


        private void Collage()
        {
            Console.Clear();
            List<string> CollageTitle = this.LoadBannerData("collage.txt");

            // First, we get the names of the images user wants to add the text
            List<string> filenames = this.ChooseWhichImagesWantToApplyFeature();

            // We get the images currently in the working area
            List<Image> imagesinworkingarea = this.producer.imagesInTheWorkingArea();
            Console.Clear();
            string completestring = "Collage Finished!";

            List<Image> collageImages = new List<Image>();
            foreach (string titlestring in CollageTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            foreach (string filename in filenames)
            {
                foreach (Image image in imagesinworkingarea)
                {
                    if (filename == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n")
                    {
                        collageImages.Add(image);
                    }
                }
            }

            Console.WriteLine("Ingrese el ancho y la altura de la imagen base y de las imágenes a introducir");
            string size = Console.ReadLine();
            string[] sizeArray = size.Split(new string[] { "," }, StringSplitOptions.None);
            int retorno = this.GenerateMenu(new List<string>() { "Yes","No", "Exit" }, null, "Background Image:", CollageTitle);
            string imageFile;
            switch(retorno)
            {
                case 0:
                    Console.WriteLine("Choose an image file");
                    int seleccion = this.GenerateMenu(filenames, null, "Choose an image file", CollageTitle);
                    imageFile = filenames[seleccion];
                    Bitmap backgroundImage = imagesinworkingarea[0].BitmapImage;
                    foreach (Image image in imagesinworkingarea)
                    {
                        if (imageFile == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n")
                        {
                            backgroundImage = image.BitmapImage;
                        }
                    }
                    Bitmap modifiedbitmap = this.producer.Collage(collageImages, Convert.ToInt32(sizeArray[0]), Convert.ToInt32(sizeArray[1]),
                    Convert.ToInt32(sizeArray[2]), Convert.ToInt32(sizeArray[3]), backgroundImage);
                    Image Final = collageImages[0];
                    Final.BitmapImage = modifiedbitmap;
                    Final.Name = "Collage:" + Final.Name;
                    return;

                case 1:
                    Console.WriteLine("Introduzca el color RGB");
                    string rgb = Console.ReadLine();
                    string[] rgbArray = rgb.Split(new string[] { "," }, StringSplitOptions.None);
                    Bitmap modifiedbitmaps = this.producer.Collage(collageImages, Convert.ToInt32(sizeArray[0]), Convert.ToInt32(sizeArray[1]),
                    Convert.ToInt32(sizeArray[2]), Convert.ToInt32(sizeArray[3]), null, Convert.ToInt32(rgbArray[0]), Convert.ToInt32(rgbArray[1]), Convert.ToInt32(rgbArray[2]));
                    Image Finals = collageImages[0];
                    Finals.BitmapImage = modifiedbitmaps;
                    Finals.Name = "Collage:" + Finals.Name;
                    return;
            }
           
            Console.SetCursorPosition((Console.WindowWidth - completestring.Length) / 2, Console.CursorTop);
            Console.WriteLine(completestring);
            System.Threading.Thread.Sleep(500);
        }


        private void Resize()
        {
            Console.Clear();
            Resizer resizer = new Resizer();

            List<string> ResizeTitle = this.LoadBannerData("resize.txt");

            // First, we get the names of the images user wants to add the text
            List<string> filenames = this.ChooseWhichImagesWantToApplyFeature();

            // We get the images currently in the working area
            List<Image> imagesinworkingarea = this.producer.imagesInTheWorkingArea();
            Console.Clear();
            foreach (string filename in filenames)
            {
                string doneon = "Done on ";
                foreach (string titlestring in ResizeTitle)
                {
                    Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                    Console.WriteLine(titlestring);
                }

                foreach (Image image in imagesinworkingarea)
                {
                    if (filename == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n")
                    {
                        Console.WriteLine("Escriba el ancho y la altura separada por coma");
                        string completestring = doneon + image.Name + "!";
                        string size = Console.ReadLine();
                        string[] sizeArray = size.Split(new string[] { "," }, StringSplitOptions.None);
                        Bitmap modifiedbitmap = resizer.ResizeImage(image.BitmapImage, Convert.ToInt32(sizeArray[0]), Convert.ToInt32(sizeArray[1]));
                        image.BitmapImage = modifiedbitmap;
                        Console.SetCursorPosition((Console.WindowWidth - completestring.Length) / 2, Console.CursorTop);
                        Console.WriteLine(completestring);
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
        }


        private void Merge()
		{
            Console.Clear();
            List<string> mergeTitle = this.LoadBannerData("merge.txt");

			// First, we get the names of the images user wants to add the text
			List<string> filenames = this.ChooseWhichImagesWantToApplyFeature();

			// We get the images currently in the working area
			List<Image> imagesinworkingarea = this.producer.imagesInTheWorkingArea();

			List<Image> MergingImages = new List<Image>();
            Console.Clear();
            string done = "Finish merging Images!";
			string file = "";
			foreach (string titlestring in mergeTitle)
			{
				Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
				Console.WriteLine(titlestring);
			}
			foreach (string filename in filenames)
			{
				foreach (Image image in imagesinworkingarea)
				{
					if (filename == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n")
					{
						MergingImages.Add(image);
						file += image.Name+"/";
					}
				}
			}
			Image finalImage = MergingImages[0];
			Bitmap modifiedbitmap = this.producer.Merge(MergingImages);
			finalImage.BitmapImage = modifiedbitmap;
			finalImage.Name = file;
			Console.SetCursorPosition((Console.WindowWidth - done.Length) / 2, Console.CursorTop);
			Console.WriteLine(done);
			System.Threading.Thread.Sleep(500);

		}


		private void AddText()
        {
            List<string> AddTextTitle = this.LoadBannerData("addtext.txt");

            // First, we get the names of the images user wants to add the text
            List<string> filenames = this.ChooseWhichImagesWantToApplyFeature();

            // We get the images currently in the working area
            List<Image> imagesinworkingarea = this.producer.imagesInTheWorkingArea();

            // Now, we need the text, Xpos, Ypos, fontsize, fontstyle, fontname and color1 and color2 for each image
            foreach (string filename in filenames)
            {
                // First, we get the data
                Dictionary<string, string> DataToAddText = GetDataToAddText(filename, imagesinworkingarea);
                if (DataToAddText.Count == 0) return;
                // Now that we have all the data of the text to add to each image, we add the text
                string doneon = "Done on ";
                foreach (string titlestring in AddTextTitle)
                {
                    Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                    Console.WriteLine(titlestring);
                }

                foreach (Image image in imagesinworkingarea)
                {
                    if (filename == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n")
                    {
                        string completestring = doneon + image.Name + "!";
                        Bitmap modifiedbitmap = this.producer.AddText(image.BitmapImage, DataToAddText["text"], Convert.ToInt32(DataToAddText["X"]),
                            Convert.ToInt32(DataToAddText["Y"]), (float)Convert.ToDouble(DataToAddText["fontsize"]), DataToAddText["color1"], DataToAddText["fontstyle"], DataToAddText["fontname"],
                            DataToAddText["color2"]);
                        image.BitmapImage = modifiedbitmap;
                        Console.SetCursorPosition((Console.WindowWidth - completestring.Length) / 2, Console.CursorTop);
                        Console.WriteLine(completestring);
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
        }


        private Dictionary<string, string> GetDataToAddText(string filename, List<Image> listofimages)
        {
            // Load the banner data
            string currentText = "[Not set]", currentY = "[Not set]", currentX = "[Not set]", currentFontSize = "[Not set]", currentFontStyle = "[Not set]", currentFontName = "[Not set]", currentColor1 = "[Not set]", currentColor2 = "[Not set]";
            List<string> AddTextTitle = this.LoadBannerData("addtext.txt");
            string name = "";
            // We find the image name
            foreach (Image image in listofimages)
            {
                if (filename == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n")
                {
                    name = image.Name;
                }
            }

            string chooseOption = $"Please, customize the parameters as you want for {name} and then select continue: ";
            int empty = 0;
            while (true)
            {
                List<string> options = new List<string>() { "Text:\t\t" + currentText, "Y Position:\t\t" + currentY, "X Position:\t\t" +currentX,
                                                            "FontSize:\t\t" + currentFontSize, "FontStyle:\t\t"+currentFontStyle, "FontName:\t\t" + currentFontName,
                                                            "Color1:\t\t" + currentColor1, "Color2:\t\t" + currentColor2, "Done"};

                // Let user edit the parameters
                int usrDec1 = this.GenerateMenu(options, null, chooseOption, AddTextTitle);
                // If press Done while text, Xpos and Ypos are not setted, and doesnt want to return to Add Text
                if (usrDec1 == 8 && (currentText == "[Not set]" || currentY == "[Not set]" || currentX == "[Not set]"))
                {
                    int usrDec2 = this.GenerateMenu(new List<string>() { "Exit", "Return to Add Text" }, null, "[!] ERROR: The minimum parameters to Add Text are [Text], [Y Position] and [X Position]", AddTextTitle);
                    if (usrDec2 == 0)
                    {
                        empty = 1;
                        break;
                    }
                    else continue;
                }
                else if (usrDec1 == 8) break;

                // Else, user wants to edit a parameter
                switch (usrDec1)
                {
                    // Wants to edit text parameter
                    case 0:
                        currentText = this.ChooseText();
                        break;
                    // Wants to enter Y Position
                    case 1:
                        string yvalue = this.ChooseY(filename);
                        if (yvalue != "-1") currentY = yvalue;
                        break;
                    // Wants to enter X Position
                    case 2:
                        string xvalue = this.ChooseX(filename);
                        if (xvalue != "-1") currentX = xvalue;
                        break;
                    // Wants to select a new fontsize
                    case 3:
                        string chosenfontsize = this.ChooseFontSize();
                        if (chosenfontsize != "-1") currentFontSize = chosenfontsize;
                        break;
                    // Wants to select a new font style
                    case 4:
                        string chosenfontstyle = this.ChooseFontStyle();
                        if (chosenfontstyle != "-1") currentFontStyle = chosenfontstyle;
                        break;
                    //Wants to select a new font name
                    case 5:
                        string chosenfontname = this.ChooseFontName();
                        if (chosenfontname != "-1") currentFontName = chosenfontname;
                        break;
                    // Wants to select color1
                    case 6:
                        string chosencolor1 = this.ChooseColor(1);
                        if (chosencolor1 != "-1") currentColor1 = chosencolor1;
                        break;
                    // Wants to select color2
                    case 7:
                        string chosencolor2 = this.ChooseColor(2);
                        if (chosencolor2 != "-1") currentColor2 = chosencolor2;
                        break;
                }
            }


            if (empty == 1) return new Dictionary<string, string>();

            if (currentFontSize == "[Not set]") currentFontSize = "10.0";
            if (currentFontStyle == "[Not set]") currentFontStyle = "bold";
            if (currentFontName == "[Not set]") currentFontName = "Times New Roman";
            if (currentColor1 == "[Not set]") currentColor1 = null;
            if (currentColor2 == "[Not set]") currentColor2 = null;

            Dictionary<string, string> returningDict = new Dictionary<string, string>();
            returningDict.Add("text", currentText);
            returningDict.Add("Y", currentY);
            returningDict.Add("X", currentX);
            returningDict.Add("fontsize", currentFontSize);
            returningDict.Add("fontstyle", currentFontStyle);
            returningDict.Add("fontname", currentFontName);
            returningDict.Add("color1", currentColor1);
            returningDict.Add("color2", currentColor2);
            return returningDict;
        }


        private string ChooseColor(int numberOfTheColor)
        {
            Console.Clear();
            List<string> AddTextTitle = this.LoadBannerData("addtext.txt");
            string choosetext = "Please, introduce the color" + Convert.ToString(numberOfTheColor) + " of the text that you want to add to the image: ";
            string presskey = "Press any key to continue...";
            string notvalid = "[!] ERROR: Color Parameter not valid";
            foreach (string titlestring in AddTextTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            Console.SetCursorPosition((Console.WindowWidth - choosetext.Length) / 2, Console.CursorTop);
            Console.Write(choosetext);
            string chosencolor = Console.ReadLine();
            try
            {
                // Try to parse it to KnownColor. If throws error, the color isnt in the KnownClor enum
                System.Drawing.KnownColor color = (System.Drawing.KnownColor)Enum.Parse(typeof(System.Drawing.KnownColor), chosencolor);
                return chosencolor;
            }
            catch
            {
                Console.SetCursorPosition((Console.WindowWidth - notvalid.Length) / 2, Console.CursorTop);
                Console.WriteLine(notvalid);
                Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskey);
                Console.ReadKey();
                return "-1";
            }
        }


        private string ChooseFontName()
        {
            Console.Clear();
            List<string> AddTextTitle = this.LoadBannerData("addtext.txt");
            string choosetext = "Please, introduce the fontname of the text that you want to add to the image: ";
            string presskey = "Press any key to continue...";
            string notvalid = "[!] ERROR: FontName Parameter not valid, or not installed in your computer";
            foreach (string titlestring in AddTextTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            Console.SetCursorPosition((Console.WindowWidth - choosetext.Length) / 2, Console.CursorTop);
            Console.Write(choosetext);
            string chosenfontname = Console.ReadLine();
            try
            {
                // Try to create a font object. If throws error, the font is not installed 
                Font font = new Font(chosenfontname, 10.0F);
                return chosenfontname;
            }
            catch
            {
                Console.SetCursorPosition((Console.WindowWidth - notvalid.Length) / 2, Console.CursorTop);
                Console.WriteLine(notvalid);
                Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskey);
                Console.ReadKey();
                return "-1";
            }
        }


        private string ChooseFontStyle()
        {
            Console.Clear();
            List<string> AddTextTitle = this.LoadBannerData("addtext.txt");
            string choosetext = "Please, introduce the fontstyle of the text that you want to add to the image <bold-underline-italic-strikeout>: ";
            string presskey = "Press any key to continue...";
            string notvalid = "[!] ERROR: FontStyle Parameter not valid";
            foreach (string titlestring in AddTextTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            Console.SetCursorPosition((Console.WindowWidth - choosetext.Length) / 2, Console.CursorTop);
            Console.Write(choosetext);
            string chosenfontstyle = Console.ReadLine();
            if (chosenfontstyle == "bold" || chosenfontstyle == "underline" || chosenfontstyle == "italic" || chosenfontstyle == "strikeout")
            {
                return chosenfontstyle;
            }
            else
            {
                Console.SetCursorPosition((Console.WindowWidth - notvalid.Length) / 2, Console.CursorTop);
                Console.WriteLine(notvalid);
                Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskey);
                Console.ReadKey();
                return "-1";
            }
        }


        private string ChooseFontSize()
        {
            Console.Clear();
            List<string> AddTextTitle = this.LoadBannerData("addtext.txt");
            string choosetext = "Please, introduce the fontsize of the text that you want to add to the image: ";
            string presskey = "Press any key to continue...";
            string notvalid = "[!] ERROR: FontSize Parameter not valid";
            foreach (string titlestring in AddTextTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            Console.SetCursorPosition((Console.WindowWidth - choosetext.Length) / 2, Console.CursorTop);
            Console.Write(choosetext);
            string chosenfontsize = Console.ReadLine();
            try
            {
                double convert = Convert.ToDouble(chosenfontsize);
                if (convert == 0.0) throw new Exception("Not valid parameter");
                return chosenfontsize;
            }
            catch
            {
                Console.SetCursorPosition((Console.WindowWidth - notvalid.Length) / 2, Console.CursorTop);
                Console.WriteLine(notvalid);
                Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskey);
                Console.ReadKey();
                return "-1";
            }
        }


        private string ChooseX(string filename)
        {
            Console.Clear();
            int width = 0;
            List<string> AddTextTitle = this.LoadBannerData("addtext.txt");
            List<Image> listofimages = this.producer.imagesInTheWorkingArea();
            string presskey = "Press any key to continue...";
            string notvalid = "[!] ERROR: X Parameter not valid";
            string chooseX = "Please, enter the X parameter: ";
            // We need the Height of the image
            foreach (Image image in listofimages)
            {
                if (filename == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n")
                {
                    width = image.BitmapImage.Height;
                }
            }
            // Show the title
            foreach (string titlestring in AddTextTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            Console.SetCursorPosition((Console.WindowWidth - chooseX.Length) / 2, Console.CursorTop);
            Console.Write(chooseX);
            string sx = Console.ReadLine();
            try
            {
                int ix = Convert.ToInt32(sx);
                if (ix > width) throw new Exception("Not valid parameter");
                return Convert.ToString(ix);
            }
            catch
            {
                Console.SetCursorPosition((Console.WindowWidth - notvalid.Length) / 2, Console.CursorTop);
                Console.WriteLine(notvalid);
                Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskey);
                Console.ReadKey();
                return "-1";
            }

        }



        private string ChooseY(string filename)
        {
            Console.Clear();
            int height = 0;
            List<string> AddTextTitle = this.LoadBannerData("addtext.txt");
            List<Image> listofimages = this.producer.imagesInTheWorkingArea();
            string presskey = "Press any key to continue...";
            string notvalid = "[!] ERROR: Y Parameter not valid";
            string chooseY = "Please, enter the Y parameter: ";
            // We need the Height of the image
            foreach (Image image in listofimages)
            {
                if (filename == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n")
                {
                    height = image.BitmapImage.Height;
                }
            }
            // Show the title
            foreach (string titlestring in AddTextTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            Console.SetCursorPosition((Console.WindowWidth - chooseY.Length) / 2, Console.CursorTop);
            Console.Write(chooseY);
            string sy = Console.ReadLine();
            try
            {
                int iy = Convert.ToInt32(sy);
                if (iy > height) throw new Exception("Not valid paramter");
                return Convert.ToString(iy);
            }
            catch
            {
                Console.SetCursorPosition((Console.WindowWidth - notvalid.Length) / 2, Console.CursorTop);
                Console.WriteLine(notvalid);
                Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskey);
                Console.ReadKey();
                return "-1";
            }

        }


        private string ChooseText()
        {
            Console.Clear();
            List<string> AddTextTitle = this.LoadBannerData("addtext.txt");
            string choosetext = "Please, introduce the text you want to add to the image: ";
            foreach (string titlestring in AddTextTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            Console.SetCursorPosition((Console.WindowWidth - choosetext.Length) / 2, Console.CursorTop);
            Console.Write(choosetext);
            return Console.ReadLine();
        }


        // Method to show that Watson find faces is not implemented yet
        private void ShowWatsonNotImplemented()
        {
            string presskey = "Press any key to continue...";
            string notimplemented = "[!] ERROR: Watson Face Recognition not implement yet";
            // Show the title
            List<string> UseFeatureTitle = this.LoadBannerData("usefeatures.txt");
            foreach (string titlestring in UseFeatureTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            Console.SetCursorPosition((Console.WindowWidth - notimplemented.Length) / 2, Console.CursorTop);
            Console.WriteLine(notimplemented);
            Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
            Console.WriteLine(presskey);
            Console.ReadKey();
            return;
        }


        // Watson no lo vamos a implementar aun, pero deberia ser algo como esto, usando Watson Analyzer (Find faces)
        private void UseWatson()
        {
            List<string> UseFeatureTitle = this.LoadBannerData("usefeatures.txt");

            // First, we get the names of the images user wants to apply the censorship
            List<string> filenames = this.ChooseWhichImagesWantToApplyFeature();

            List<Image> inworkingarea = this.producer.imagesInTheWorkingArea();
            List<Dictionary<string, int>> results = new List<Dictionary<string, int>>();

            string loading = "Please, wait while Watson analizes the images...";
            Console.SetCursorPosition((Console.WindowWidth - loading.Length) / 2, Console.CursorTop);
            Console.WriteLine(loading);

            foreach (string filename in filenames)
            {
                foreach (Image image in inworkingarea)
                {
                    if (filename == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n")
                    {
                        Dictionary<string, int> resultanalisis = this.producer.SexAndAgeRecognition(image); // ESTE METODO SE DEBE VOLVER A HACER
                        // por lo que el diccionario resultanalisis debe ser de otro tipo
                        results.Add(resultanalisis);
                    }
                }
            }

            // show the results. esto necesita cambiarse en funcion de lo que s ehaga con watson
            int i = 0;
            foreach (Dictionary<string, int> dict in results)
            {
                // We show the title
                foreach (string titlestring in UseFeatureTitle)
                {
                    Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                    Console.WriteLine(titlestring);
                }

                string resultfor = "Result for: ";
                string name = filenames[i];
                Console.SetCursorPosition((Console.WindowWidth - resultfor.Length) / 2, Console.CursorTop);
                Console.WriteLine(resultfor);
                Console.SetCursorPosition((Console.WindowWidth - name.Length) / 2, Console.CursorTop);
                Console.WriteLine(name);

                foreach (KeyValuePair<string, int> pair in dict)
                {
                    string complete = pair.Key + ": " + Convert.ToString(pair.Value);
                    Console.SetCursorPosition((Console.WindowWidth - complete.Length) / 2, Console.CursorTop);
                    Console.WriteLine(complete);
                }
                i++;
            }
        }



        private void ApplyCensorship()
        {
            string choosetypeofcensorship = "Please, choose which type of censorship you want to use";
            List<string> typesofcensorship = new List<string>() { "Black Censorship", "Pixel Censorship" };
            List<string> UseCensorshipTitle = this.LoadBannerData("usecensorship.txt");
            // First, we get the names of the images user wants to apply the censorship
            List<string> filenames = this.ChooseWhichImagesWantToApplyFeature();
            // Then, we get the type of censorship
            int usrDecCensorship = this.GenerateMenu(typesofcensorship, null, choosetypeofcensorship, UseCensorshipTitle);
            // Next, we get the coordinates of the censorship for each image
            Dictionary<string, int[]> coordinates = this.GetCoordinatesForCensorship(filenames);
            List<Image> imagesInWorkingArea = this.producer.imagesInTheWorkingArea();
            // Next, we do the censorship
            foreach (KeyValuePair<string, int[]> pair in coordinates)
            {
                // We find the bitmap
                foreach (Image image in imagesInWorkingArea)
                {
                    // We found it
                    if (pair.Key == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n")
                    {
                        // And, depending on the censorship, we apply it
                        // If user wants black censorship
                        if (usrDecCensorship == 0)
                        {
                            // Apply the censorship and save it
                            Bitmap censured = this.producer.BlackCensorship(image, pair.Value);
                            image.BitmapImage = censured;
                        }
                        // If user wants pixel censorship
                        else
                        {
                            // Apply the censorship and save it

                            Bitmap censured = this.producer.PixelCensorship(image, pair.Value);
                            image.BitmapImage = censured;

                        }

                        this.ShowDoneApplyingForFeatures(image.Name);
                        break;
                    }
                }
            }
            this.SaveProducer();
            return;
        }



        private Dictionary<string, int[]> GetCoordinatesForCensorship(List<string> files)
        {
            string presskey = "Press any key to continue...";
            string notvalid = "Values not valid";
            string introducecoordinates = "Please, introduce the coordinates of censorship separed by commas (X,Y,TOP,LEFT): ";
            Dictionary<string, int[]> returningDict = new Dictionary<string, int[]>();
            List<string> UseFeatureTitle = this.LoadBannerData("usefeatures.txt");
            int width = 0, height = 0;
            List<Image> imagesInWorkingArea = this.producer.imagesInTheWorkingArea();
            foreach (string file in files)
            {
                string[] separated;
                // We get the width and height of the image
                foreach (Image image in imagesInWorkingArea)
                {
                    if (file == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n") 
                    {
                        width = image.BitmapImage.Width;
                        height = image.BitmapImage.Height;
                        break;
                    }
                }

                while (true)
                {
                    Console.Clear();
                    // We show the title and the image 
                    foreach (string titlestring in UseFeatureTitle)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                        Console.WriteLine(titlestring);
                    }
                    Console.SetCursorPosition((Console.WindowWidth - file.Length) / 2, Console.CursorTop);
                    Console.WriteLine(file);

                    // We ask to user the coordinates
                    Console.WriteLine("\n\n");
                    Console.SetCursorPosition((Console.WindowWidth - introducecoordinates.Length) / 2, Console.CursorTop);
                    Console.Write(introducecoordinates);
                    string values = Console.ReadLine();
                    bool valid = this.VerifyIfAreValidCoordinates(values, width, height);
                    if (valid == true)
                    {
                        separated = values.Split(',');
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\n");
                        Console.SetCursorPosition((Console.WindowWidth - notvalid.Length) / 2, Console.CursorTop);
                        Console.Write(notvalid);
                        Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                        Console.Write(presskey);
                        Console.ReadKey();
                    }
                }

                returningDict.Add(file, new int[] { Convert.ToInt32(separated[0]), Convert.ToInt32(separated[1]), Convert.ToInt32(separated[2]), Convert.ToInt32(separated[3]) });
            }

            return returningDict;
        }



        private bool VerifyIfAreValidCoordinates(string values, int imageWidth, int imageHeight)
        {
            int X, Y, TOP, LEFT;
            string[] separated = values.Split(',');
            try
            {
                X = Convert.ToInt32(separated[0]);
                Y = Convert.ToInt32(separated[1]);
                TOP = Convert.ToInt32(separated[2]);
                LEFT = Convert.ToInt32(separated[3]);
                if ((LEFT + X > imageWidth) || (TOP + Y > imageHeight)) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }



        private List<string> ChooseWhichImagesWantToApplyFeature()
        {
            List<string> applyfiltertitle = this.LoadBannerData("usefeatures.txt");
            string choosefiles = "Please, choose to which images you want to apply the feature: ";
            string chosenfiles = "Chosen images: ";
            List<string> chosenImages = new List<string>();
            List<string> possibleToChoose = new List<string>();
            List<Image> imagesInTheWorkingArea = new List<Image>();

            foreach (Image image in this.producer.imagesInTheWorkingArea())
            {
                imagesInTheWorkingArea.Add(new Image(image.Name, image.Labels, image.Calification, image.BitmapImage, image.Resolution,
                    image.AspectRatio, image.DarkClear, image.Exif));
            }

            // Load the possibilities for the user
            foreach (Image image in imagesInTheWorkingArea)
            {
                possibleToChoose.Add($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n");
            }

            possibleToChoose.Add("Continue");

            while (true)
            {
                int selectedOption = 0;
                bool _continue = true;
                while (_continue == true)
                {
                    // Clear the screen and show the title
                    Console.Clear();
                    int i = 1;
                    foreach (string titlestring in applyfiltertitle)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                        Console.WriteLine(titlestring);
                    }

                    // Show the chosen images till the moment
                    Console.SetCursorPosition((Console.WindowWidth - chosenfiles.Length) / 4, Console.CursorTop);
                    Console.WriteLine(chosenfiles);
                    foreach (string chosenimage in chosenImages)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - chosenimage.Length) / 2, Console.CursorTop);
                        Console.WriteLine(chosenimage);
                    }

                    // Show the options to the user
                    Console.WriteLine("\n\n");
                    Console.SetCursorPosition((Console.WindowWidth - choosefiles.Length) / 10, Console.CursorTop);
                    Console.WriteLine(choosefiles);


                    Console.WriteLine("\n\n");
                    foreach (string option in possibleToChoose)
                    {
                        if (selectedOption == i - 1)
                        {
                            Console.WriteLine("\n");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{i}. {option}");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            i++;
                            Console.WriteLine("\n");
                            continue;
                        }
                        Console.WriteLine($"{i}. {option}");
                        i++;
                    }
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedOption -= 1;
                            if (selectedOption < 0)
                            {
                                selectedOption = 0;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            selectedOption += 1;
                            if (selectedOption > possibleToChoose.Count - 1)
                            {
                                selectedOption = possibleToChoose.Count - 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            _continue = false;
                            break;
                    }

                }


                // If user wants to exit, we return the chosen images
                if (possibleToChoose[selectedOption] == "Continue") return chosenImages;


                // Now, selectedOption has the number of the file that the user wants to apply the filter
                // Whe add to the chosenImages the name of the file
                Image image2 = imagesInTheWorkingArea[selectedOption];
                chosenImages.Add($"Name: {image2.Name} - Calification: {image2.Calification} - Resolution: {image2.Resolution[0]}x{image2.Resolution[1]} - AspectRatio: {image2.AspectRatio[0]}x{image2.AspectRatio[1]} - Clear: {image2.DarkClear}\n");

                // And delete the option from the possibleToChoose
                possibleToChoose.RemoveAt(selectedOption);
                imagesInTheWorkingArea.RemoveAt(selectedOption);
            }

        }



        private void ExportFromEditingArea()
        {
            // Load the title
            List<string> ExportTitle = this.LoadBannerData("exportfromeditingarea.txt");
            string presskey = "Press any key to continue...";
            string emptyWorkingArea = "[!] Your Editing Area is empty";
            string done = "All selected images were exported";

            // Verify that the Editing Area is not empty
            if (this.producer.imagesInTheWorkingArea().Count == 0)
            {
                Console.WriteLine("\n");
                Console.SetCursorPosition((Console.WindowWidth - emptyWorkingArea.Length) / 2, Console.CursorTop);
                Console.WriteLine(emptyWorkingArea);
                Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskey);
                Console.ReadKey();
                return;
            }

            // We get the images that user wants export
            List<string> imagesToExport = this.ChooseWhichImagesWantToExport();


            foreach (string stringimage in imagesToExport)
            {
                int i = 0;
                List<int> removeAt = new List<int>();
                List<Image> possibleImages = this.producer.imagesInTheWorkingArea();
                foreach (Image image in possibleImages)
                {
                    if (stringimage == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n")
                    {
                        // We verify if the library contains the image
                        int librarycontains = this.LibraryContains(stringimage);
                        // If it contains the image, we ask if user wants to replace it
                        if (librarycontains != -1)
                        {
                            string thefollowingimageexists = $"The image {image.Name} exists in your library. Do you want to replace it?";
                            List<string> options = new List<string>() { "Yes", "No" };
                            int usrDec = this.GenerateMenu(options, null, thefollowingimageexists, ExportTitle);
                            // if user wants to replace it
                            if (usrDec == 0)
                            {
                                this.library.Images.RemoveAt(librarycontains);
                                this.library.Images.Add(image);
                                removeAt.Add(i);
                            }
                            // if user doesnt want to replace it
                            else
                            {
                                // We ask if he wants to change the name of the image
                                int usrDec2 = this.GenerateMenu(new List<string>() { "Yes", "No" }, null, "Do you want to change the name of the image?", ExportTitle);
                                if (usrDec2 == 0)
                                {
                                    string introducethenewname = "Please, introduce the new name: ";
                                    foreach (string titlestring in ExportTitle)
                                    {
                                        Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                                        Console.WriteLine(titlestring);
                                    }
                                    Console.WriteLine("\n");
                                    Console.SetCursorPosition((Console.WindowWidth - introducethenewname.Length) / 2, Console.CursorTop);
                                    Console.Write(introducethenewname);
                                    image.Name = Console.ReadLine();
                                }
                                this.library.Images.Add(image);
                                removeAt.Add(i);
                            }
                        }
                        else
                        {
                            this.library.Images.Add(image);
                            removeAt.Add(i);
                        }

                    }
                    i++;
                }
                foreach (int j in removeAt)
                {
                    this.producer.imagesInTheWorkingArea().RemoveAt(j);
                }
                this.SaveProducer();
            }

            Console.Clear();
            foreach (string titlestring in ExportTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }

            Console.WriteLine("\n");
            Console.SetCursorPosition((Console.WindowWidth - done.Length) / 2, Console.CursorTop);
            Console.WriteLine(done);
            Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
            Console.WriteLine(presskey);
            Console.ReadKey();
        }



        // Message of done for features
        private void ShowDoneApplyingForFeatures(string name)
        {
            string applyed = "Done on ";
            string dots = "...";
            List<string> applyfiltertitle = this.LoadBannerData("usefeatures.txt");

            Console.Clear();
            string completeMessage = applyed + name + " !";

            foreach (string titlestring in applyfiltertitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }

            Console.WriteLine("\n");
            Console.SetCursorPosition((Console.WindowWidth - completeMessage.Length) / 2, Console.CursorTop);
            Console.WriteLine(completeMessage);

            System.Threading.Thread.Sleep(500);
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - dots.Length) / 2, Console.CursorTop);
            Console.WriteLine(dots);
        }



        // Method to know if the library contains an image. Returns -1 if image doesnt exists, and
        // the index of the image if it exists
        private int LibraryContains(string stringimage)
        {
            int count = 0;
            foreach (Image image in this.library.Images)
            {
                if (stringimage == $"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n") return count;
                count++;
            }
            return -1;
        }



        // To apply the filters
        private void ApplyFilters()
        {
            while (true)
            {
                // Verify that the working area is not empty
                string presskey = "Press any key to continue...";
                string emptyWorkingArea = "Your Editing Area is empty";
                if (this.producer.imagesInTheWorkingArea().Count == 0)
                {
                    Console.WriteLine("\n");
                    Console.SetCursorPosition((Console.WindowWidth - emptyWorkingArea.Length) / 2, Console.CursorTop);
                    Console.WriteLine(emptyWorkingArea);
                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                    Console.WriteLine(presskey);
                    Console.ReadKey();
                    return;
                }


                // Load the title and the options for the menu
                string choosefilter = "Please, choose the Filter you want to apply: ";
                List<string> ApplyFiltersTitle = this.LoadBannerData("applyfilters.txt");
                List<string> options = new List<string>() { "BlackNWhiteFilter", "BrightnessFilter", "ColorFilter",
                                                        "InvertFilter", "MirrorFilter", "OldFilmFilter", "RotateFlipFilter",
                                                        "SepiaFilter", "WindowsFilter","AutomaticAdjustmentFilter", "Exit"};

                // Ask user what he wants to do, and if he wants to exit, we exit
                int usrDec = this.GenerateMenu(options, null, choosefilter, ApplyFiltersTitle);
                if (options[usrDec] == "Exit") break; ;


                // First, we get the names of the images user wants to apply the filter
                List<string> filenames = this.ChooseWhichImagesWantToApplyFilter();


                // Switch for the usrDecision


                switch (usrDec)
                {
                    // Apply blacknwhite filter
                    case 0:
                        foreach (Image image in this.producer.imagesInTheWorkingArea())
                        {
                            foreach (string filename in filenames)
                            {
                                if ($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n" == filename)
                                {
                                    // First, we apply the filter to the image
                                    Bitmap bitmap = this.producer.ApplyFilter(image, EFilter.BlackNWhiteFilter);
                                    // And then, we change the bitmap of the selectedImage
                                    image.BitmapImage = bitmap;
                                    this.ShowDoneApplyingFor(image.Name);
                                }
                            }
                        }
                        break;


                    // Apply brightness filter
                    case 1:
                        // We get the level of brightness to apply
                        int brightnesslevel = this.ChooseLevelOfBrightness();
                        foreach (Image image in this.producer.imagesInTheWorkingArea())
                        {
                            foreach (string filename in filenames)
                            {
                                if ($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n" == filename)
                                {
                                    // First, we apply the filter to the image
                                    Bitmap bitmap = this.producer.ApplyFilter(image, EFilter.BrightnessFilter, default(Color), brightnesslevel);
                                    // And then, we change the bitmap of the selectedImage
                                    image.BitmapImage = bitmap;
                                    this.ShowDoneApplyingFor(image.Name);
                                }
                            }
                        }
                        break;


                    // Apply color filter
                    case 2:
                        // We get the color that user wants
                        Color usrColor = this.ChooseColor();
                        // We apply the filter
                        foreach (Image image in this.producer.imagesInTheWorkingArea())
                        {
                            foreach (string filename in filenames)
                            {
                                if ($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n" == filename)
                                {
                                    // First, we apply the filter to the image
                                    Bitmap bitmap = this.producer.ApplyFilter(image, EFilter.ColorFilter, usrColor);
                                    // And then, we change the bitmap of the selectedImage
                                    image.BitmapImage = bitmap;
                                    this.ShowDoneApplyingFor(image.Name);
                                }
                            }
                        }
                        break;


                    // Apply invert filter
                    case 3:
                        foreach (Image image in this.producer.imagesInTheWorkingArea())
                        {
                            foreach (string filename in filenames)
                            {
                                if ($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n" == filename)
                                {
                                    // First, we apply the filter to the image
                                    Bitmap bitmap = this.producer.ApplyFilter(image, EFilter.InvertFilter);
                                    // And then, we change the bitmap of the selectedImage
                                    image.BitmapImage = bitmap;
                                    this.ShowDoneApplyingFor(image.Name);
                                }
                            }
                        }
                        break;


                    // Apply mirror filter
                    case 4:
                        foreach (Image image in this.producer.imagesInTheWorkingArea())
                        {
                            foreach (string filename in filenames)
                            {
                                if ($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n" == filename)
                                {
                                    // First, we apply the filter to the image
                                    Bitmap bitmap = this.producer.ApplyFilter(image, EFilter.MirrorFilter);
                                    // And then, we change the bitmap of the selectedImage
                                    image.BitmapImage = bitmap;
                                    this.ShowDoneApplyingFor(image.Name);
                                }
                            }
                        }
                        break;

                    // Apply OldFilm Filter
                    case 5:
                        foreach (Image image in this.producer.imagesInTheWorkingArea())
                        {
                            foreach (string filename in filenames)
                            {
                                if ($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n" == filename)
                                {
                                    // First, we apply the filter to the image
                                    Bitmap bitmap = this.producer.ApplyFilter(image, EFilter.OldFilmFilter);
                                    // And then, we change the bitmap of the selectedImage
                                    image.BitmapImage = bitmap;
                                    this.ShowDoneApplyingFor(image.Name);
                                }
                            }
                        }
                        break;

                    // Apply RotateFlip filter
                    case 6:
                        // We get the rotateFlip type
                        RotateFlipType rotateType = this.ChooseRotateFlipType();
                        foreach (Image image in this.producer.imagesInTheWorkingArea())
                        {
                            foreach (string filename in filenames)
                            {
                                if ($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n" == filename)
                                {
                                    // First, we apply the filter to the image
                                    Bitmap bitmap = this.producer.ApplyFilter(image, EFilter.RotateFlipFilter, default(Color), 0, 0, rotateType);
                                    // And then, we change the bitmap of the selectedImage
                                    image.BitmapImage = bitmap;
                                    this.ShowDoneApplyingFor(image.Name);
                                }
                            }
                        }
                        break;

                    // Apply SepiaFilter
                    case 7:
                        foreach (Image image in this.producer.imagesInTheWorkingArea())
                        {
                            foreach (string filename in filenames)
                            {
                                if ($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n" == filename)
                                {
                                    // First, we apply the filter to the image
                                    Bitmap bitmap = this.producer.ApplyFilter(image, EFilter.SepiaFilter);
                                    // And then, we change the bitmap of the selectedImage
                                    image.BitmapImage = bitmap;
                                    this.ShowDoneApplyingFor(image.Name);
                                }
                            }
                        }
                        break;

                    // Apply WindowsFilter
                    case 8:
                        foreach (Image image in this.producer.imagesInTheWorkingArea())
                        {
                            foreach (string filename in filenames)
                            {
                                if ($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n" == filename)
                                {
                                    // First, we apply the filter to the image
                                    Bitmap bitmap = this.producer.ApplyFilter(image, EFilter.WindowsFilter);
                                    // And then, we change the bitmap of the selectedImage
                                    image.BitmapImage = bitmap;
                                    this.ShowDoneApplyingFor(image.Name);
                                }
                            }
                        }
                        break;

                    // Apply AutomaticAdjustmentFilter
                    case 9:
                        foreach (Image image in this.producer.imagesInTheWorkingArea())
                        {
                            foreach (string filename in filenames)
                            {
                                if ($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n" == filename)
                                {
                                    // First, we apply the filter to the image
                                    Bitmap bitmap = this.producer.ApplyFilter(image, EFilter.AutomaticAdjustmentFilter);
                                    // And then, we change the bitmap of the selectedImage
                                    image.BitmapImage = bitmap;
                                    this.ShowDoneApplyingFor(image.Name);
                                }
                            }
                        }
                        break;
                }
                Console.WriteLine("\n");
                Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskey);
                Console.ReadKey();
            }
            return;
        }



        // To choose the rotateFlipType
        private RotateFlipType ChooseRotateFlipType()
        {
            List<string> applyfiltertitle = this.LoadBannerData("applyfilters.txt");
            List<string> options = new List<string>() { "Rotate180FlipXY", "Rotate90FlipNone", "Rotate270FlipXY",
                                                        "Rotate180FlipNone", "RotateNoneFlipXY", "Rotate270FlipNone",
                                                        "Rotate90FlipXY", "RotateNoneFlipX", "Rotate180FlipY", "Rotate90FlipX",
                                                        "Rotate270FlipY", "Rotate180FlipX", "RotateNoneFlipY", "Rotate270FlipX",
                                                        "Rotate90FlipY" };

            int usrDec = this.GenerateMenu(options, null, "Please, choose the type of Rotation and Flip: ", applyfiltertitle);
            return (RotateFlipType)Enum.Parse(typeof(RotateFlipType), options[usrDec]);
        }



        // To choose the color 
        private Color ChooseColor()
        {
            List<string> applyfiltertitle = this.LoadBannerData("applyfilters.txt");
            Color returningColor = Color.FromArgb(0, 0, 0);
            int usrWants = this.GenerateMenu(new List<string>() { "Select color from list", "Select a custom color" }, null, "Please, select an option: ", applyfiltertitle);
            switch (usrWants)
            {
                case 0:
                    int numberOfTheColor = this.GenerateMenu(new List<string>() { "Red", "Green", "Yellow", "Blue" }, null, "Please, select a color", applyfiltertitle);
                    switch (numberOfTheColor)
                    {
                        case 0:
                            returningColor = Color.FromArgb(255, 0, 0);
                            break;
                        case 1:
                            returningColor = Color.FromArgb(0, 255, 0);
                            break;
                        case 2:
                            returningColor = Color.FromArgb(255, 255, 0);
                            break;
                        case 3:
                            returningColor = Color.FromArgb(0, 0, 255);
                            break;
                    }
                    break;
                case 1:
                    while (true)
                    {
                        Console.Clear();
                        string presskey = "Press any key to continue...";
                        string valueNotValid = "[!] ERROR: Color not valid";
                        string introduceTheValues = "Please, introduce the values in format R,G,B separated by comma <0,255>: ";
                        int R, G, B;
                        foreach (string titlestring in applyfiltertitle)
                        {
                            Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                            Console.WriteLine(titlestring);
                        }
                        Console.WriteLine("\n\n");
                        Console.SetCursorPosition((Console.WindowWidth - introduceTheValues.Length) / 2, Console.CursorTop);
                        Console.Write(introduceTheValues);
                        string[] separatedvalues = Console.ReadLine().Split(',');
                        try
                        {
                            R = Convert.ToInt32(separatedvalues[0]);
                            if (R < 0 || R > 255) throw new Exception("[!] ERROR: Red value not valid");
                            G = Convert.ToInt32(separatedvalues[1]);
                            if (G < 0 || G > 255) throw new Exception("[!] ERROR: Green value not valid");
                            B = Convert.ToInt32(separatedvalues[2]);
                            if (B < 0 || B > 255) throw new Exception("[!] ERROR: Blue value not valid");
                            returningColor = Color.FromArgb(R, G, B);
                            break;
                        }
                        catch
                        {
                            Console.SetCursorPosition((Console.WindowWidth - valueNotValid.Length) / 2, Console.CursorTop);
                            Console.Write(valueNotValid);
                            Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                            Console.Write(presskey);
                            Console.ReadKey();
                        }
                    }
                    break;
            }
            return returningColor;
        }



        // To choose the level of brightness
        private int ChooseLevelOfBrightness()
        {
            List<string> applyfiltertitle = this.LoadBannerData("applyfilters.txt");
            string presskey = "Press any key to continue...";
            string valueNotValid = "[!] ERROR: Brightness value not valid";
            string chooselevel = "Please, introduce the level of brightness <-255, 255>: ";
            int chosen;
            while (true)
            {
                Console.Clear();
                foreach (string titlestring in applyfiltertitle)
                {
                    Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                    Console.WriteLine(titlestring);
                }

                Console.SetCursorPosition((Console.WindowWidth - chooselevel.Length) / 2, Console.CursorTop);
                Console.Write(chooselevel);

                try
                {
                    chosen = Convert.ToInt32(Console.ReadLine());
                    if (chosen < -255 || chosen > 255) throw new Exception("[!] ERROR: Value not valid");
                    break;
                }
                catch
                {
                    Console.SetCursorPosition((Console.WindowWidth - valueNotValid.Length) / 2, Console.CursorTop);
                    Console.Write(valueNotValid);
                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                    Console.Write(presskey);
                }
            }
            return chosen;
        }



        // Show the message of done when applying filters
        private void ShowDoneApplyingFor(string name)
        {
            string applyed = "Done on ";
            string dots = "...";
            List<string> applyfiltertitle = this.LoadBannerData("applyfilters.txt");

            Console.Clear();
            string completeMessage = applyed + name + " !";

            foreach (string titlestring in applyfiltertitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }

            Console.WriteLine("\n");
            Console.SetCursorPosition((Console.WindowWidth - completeMessage.Length) / 2, Console.CursorTop);
            Console.WriteLine(completeMessage);

            System.Threading.Thread.Sleep(500);
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - dots.Length) / 2, Console.CursorTop);
            Console.WriteLine(dots);
        }



        // Returns the list of the names of the image sthat the user wants to apply the filter
        private List<string> ChooseWhichImagesWantToApplyFilter()
        {
            List<string> applyfiltertitle = this.LoadBannerData("applyfilters.txt");
            string choosefiles = "Please, choose to which images you want to apply the filter";
            string chosenfiles = "Chosen images: ";
            List<string> chosenImages = new List<string>();
            List<string> possibleToChoose = new List<string>();
            List<Image> imagesInTheWorkingArea = new List<Image>();

            foreach (Image image in this.producer.imagesInTheWorkingArea())
            {
                imagesInTheWorkingArea.Add(new Image(image.Name, image.Labels, image.Calification, image.BitmapImage, image.Resolution,
                    image.AspectRatio, image.DarkClear, image.Exif));
            }

            // Load the possibilities for the user
            foreach (Image image in imagesInTheWorkingArea)
            {
                possibleToChoose.Add($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n");
            }

            possibleToChoose.Add("Continue");

            while (true)
            {
                int selectedOption = 0;
                bool _continue = true;
                while (_continue == true)
                {
                    // Clear the screen and show the title
                    Console.Clear();
                    int i = 1;
                    foreach (string titlestring in applyfiltertitle)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                        Console.WriteLine(titlestring);
                    }

                    // Show the chosen images till the moment
                    Console.SetCursorPosition((Console.WindowWidth - chosenfiles.Length) / 4, Console.CursorTop);
                    Console.WriteLine(chosenfiles);
                    foreach (string chosenimage in chosenImages)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - chosenimage.Length) / 2, Console.CursorTop);
                        Console.WriteLine(chosenimage);
                    }

                    // Show the options to the user
                    Console.WriteLine("\n\n");
                    Console.SetCursorPosition((Console.WindowWidth - choosefiles.Length) / 10, Console.CursorTop);
                    Console.WriteLine(choosefiles);


                    Console.WriteLine("\n\n");
                    foreach (string option in possibleToChoose)
                    {
                        if (selectedOption == i - 1)
                        {
                            Console.WriteLine("\n");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{i}. {option}");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            i++;
                            Console.WriteLine("\n");
                            continue;
                        }
                        Console.WriteLine($"{i}. {option}");
                        i++;
                    }
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedOption -= 1;
                            if (selectedOption < 0)
                            {
                                selectedOption = 0;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            selectedOption += 1;
                            if (selectedOption > possibleToChoose.Count - 1)
                            {
                                selectedOption = possibleToChoose.Count - 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            _continue = false;
                            break;
                    }

                }


                // If user wants to exit, we return the chosen images
                if (possibleToChoose[selectedOption] == "Continue") return chosenImages;


                // Now, selectedOption has the number of the file that the user wants to apply the filter
                // Whe add to the chosenImages the name of the file
                Image image2 = imagesInTheWorkingArea[selectedOption];
                chosenImages.Add($"Name: {image2.Name} - Calification: {image2.Calification} - Resolution: {image2.Resolution[0]}x{image2.Resolution[1]} - AspectRatio: {image2.AspectRatio[0]}x{image2.AspectRatio[1]} - Clear: {image2.DarkClear}\n");

                // And delete the option from the possibleToChoose
                possibleToChoose.RemoveAt(selectedOption);
                imagesInTheWorkingArea.RemoveAt(selectedOption);
            }
        }



        private List<string> ChooseWhichImagesWantToExport()
        {
            List<string> applyfiltertitle = this.LoadBannerData("exportfromeditingarea.txt");
            string choosefiles = "Please, choose which images you want to export: ";
            string chosenfiles = "Chosen images: ";
            List<string> chosenImages = new List<string>();
            List<string> possibleToChoose = new List<string>();
            List<Image> imagesInTheWorkingArea = new List<Image>();

            foreach (Image image in this.producer.imagesInTheWorkingArea())
            {
                imagesInTheWorkingArea.Add(new Image(image.Name, image.Labels, image.Calification, image.BitmapImage, image.Resolution,
                    image.AspectRatio, image.DarkClear, image.Exif));
            }

            // Load the possibilities for the user
            foreach (Image image in imagesInTheWorkingArea)
            {
                possibleToChoose.Add($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n");
            }

            possibleToChoose.Add("Continue");

            while (true)
            {
                int selectedOption = 0;
                bool _continue = true;
                while (_continue == true)
                {
                    // Clear the screen and show the title
                    Console.Clear();
                    int i = 1;
                    foreach (string titlestring in applyfiltertitle)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                        Console.WriteLine(titlestring);
                    }

                    // Show the chosen images till the moment
                    Console.SetCursorPosition((Console.WindowWidth - chosenfiles.Length) / 4, Console.CursorTop);
                    Console.WriteLine(chosenfiles);
                    foreach (string chosenimage in chosenImages)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - chosenimage.Length) / 2, Console.CursorTop);
                        Console.WriteLine(chosenimage);
                    }

                    // Show the options to the user
                    Console.WriteLine("\n\n");
                    Console.SetCursorPosition((Console.WindowWidth - choosefiles.Length) / 10, Console.CursorTop);
                    Console.WriteLine(choosefiles);


                    Console.WriteLine("\n\n");
                    foreach (string option in possibleToChoose)
                    {
                        if (selectedOption == i - 1)
                        {
                            Console.WriteLine("\n");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{i}. {option}");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            i++;
                            Console.WriteLine("\n");
                            continue;
                        }
                        Console.WriteLine($"{i}. {option}");
                        i++;
                    }
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedOption -= 1;
                            if (selectedOption < 0)
                            {
                                selectedOption = 0;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            selectedOption += 1;
                            if (selectedOption > possibleToChoose.Count - 1)
                            {
                                selectedOption = possibleToChoose.Count - 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            _continue = false;
                            break;
                    }

                }


                // If user wants to exit, we return the chosen images
                if (possibleToChoose[selectedOption] == "Continue") return chosenImages;


                // Now, selectedOption has the number of the file that the user wants to apply the filter
                // Whe add to the chosenImages the name of the file
                Image image2 = imagesInTheWorkingArea[selectedOption];
                chosenImages.Add($"Name: {image2.Name} - Calification: {image2.Calification} - Resolution: {image2.Resolution[0]}x{image2.Resolution[1]} - AspectRatio: {image2.AspectRatio[0]}x{image2.AspectRatio[1]} - Clear: {image2.DarkClear}\n");

                // And delete the option from the possibleToChoose
                possibleToChoose.RemoveAt(selectedOption);
                imagesInTheWorkingArea.RemoveAt(selectedOption);
            }
        }



        private void DeleteFromEditingArea()
        {
            string presskey = "Press any key to continue...";
            string donthaveimages = "You dont have any images to delete in the Editing Area";
            Console.Clear();
            List<string> deletefromeditingareatitle = this.LoadBannerData("deletefromeditingarea.txt");
            while (true)
            {
                List<string> options = new List<string>();
                foreach (Image image in this.producer.imagesInTheWorkingArea())
                {
                    options.Add($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n");
                }
                options.Add("Exit");
                if (options.Count == 1)
                {
                    Console.WriteLine("\n");
                    Console.SetCursorPosition((Console.WindowWidth - donthaveimages.Length) / 2, Console.CursorTop);
                    Console.WriteLine(donthaveimages);
                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                    Console.WriteLine(presskey);
                    Console.ReadKey();
                    return;
                }
                int usrWants = this.GenerateMenu(options, null, "Please, select which image you want to delete: ", deletefromeditingareatitle);
                if (options[usrWants] == "Exit") break;
                else
                {
                    this.producer.DeleteImageInTheWorkingArea(usrWants);
                }
                string dots = "...\n";
                string success = "Image successfully eliminated from Editing Area!";

                System.Threading.Thread.Sleep(1000);
                Console.WriteLine();
                Console.SetCursorPosition((Console.WindowWidth - dots.Length) / 2, Console.CursorTop);
                Console.WriteLine(dots);

                System.Threading.Thread.Sleep(1000);
                Console.WriteLine();
                Console.SetCursorPosition((Console.WindowWidth - dots.Length) / 2, Console.CursorTop);
                Console.WriteLine(dots);

                System.Threading.Thread.Sleep(1000);
                Console.WriteLine();
                Console.SetCursorPosition((Console.WindowWidth - success.Length) / 2, Console.CursorTop);
                Console.WriteLine(success);

                Console.WriteLine();
                Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskey);
                Console.ReadKey();
            }

            return;

        }



        private void ImportToEditingArea()
        {
            Console.Clear();
            List<string> EditingAreaTitle = this.LoadBannerData("importeditingarea.txt");
            List<Image> imagesSelected = new List<Image>();
            List<string> imagesToAdd = new List<string>();
            string curr = "Images selected: \n";

            foreach (Image image in this.library.Images)
            {
                imagesToAdd.Add($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n");
            }

            imagesToAdd.Add("Continue");
            List<string> ImportEditingAreaTitle = this.LoadBannerData("importeditingarea.txt");


            while (true)
            {
                int selectedOption = 1;
                List<string> imagesIntTheWorkingArea = new List<string>();

                foreach (Image image in imagesSelected)
                {
                    imagesIntTheWorkingArea.Add($"Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear}\n");
                }


                bool _continue2 = true;
                // Show the menu and gets the number selected
                while (_continue2 == true)
                {
                    Console.Clear();
                    int i = 1;
                    // Show the title
                    foreach (string titlestring in EditingAreaTitle)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                        Console.WriteLine(titlestring);
                    }

                    // Show the images in the working area
                    Console.SetCursorPosition((Console.WindowWidth - curr.Length) / 4, Console.CursorTop);
                    Console.WriteLine(curr);
                    Console.WriteLine();

                    foreach (string imagestring in imagesIntTheWorkingArea)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - imagestring.Length) / 2, Console.CursorTop);
                        Console.WriteLine(imagestring);
                    }


                    Console.WriteLine("\n\n");
                    foreach (string option in imagesToAdd)
                    {
                        if (selectedOption == i - 1)
                        {
                            Console.WriteLine("\n");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{i}. {option}");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            i++;
                            Console.WriteLine("\n");
                            continue;
                        }
                        Console.WriteLine($"{i}. {option}");
                        i++;
                    }
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedOption -= 1;
                            if (selectedOption < 0)
                            {
                                selectedOption = 0;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            selectedOption += 1;
                            if (selectedOption > imagesToAdd.Count - 1)
                            {
                                selectedOption = imagesToAdd.Count - 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            _continue2 = false;
                            break;
                    }
                }

                int usrDecision = selectedOption;


                if (imagesToAdd[usrDecision] == "Continue") break;
                imagesSelected.Add(this.library.Images[usrDecision]);
            }

            this.producer.LoadImagesToWorkingArea(imagesSelected);
            this.SaveProducer();
        }



        private void ManageLibrary()
        {
            List<string> manageLibraryTitle = this.LoadBannerData("managelibrary.txt");
            List<string> manageLibraryOptions = new List<string>() { "Show My Library", "Add Label", "Edit Label", "Delete Label", "Set Calification", "Delete Image", "Reset Library", "Exit" };
            string manageLibraryDescription = "Please, select an option: ";
            while (true)
            {
                int usrDecision = this.GenerateMenu(manageLibraryOptions, null, manageLibraryDescription, manageLibraryTitle);
                if (usrDecision == 7) break;
                switch (usrDecision)
                {
                    // User wants to see his library => READY
                    case 0:
                        this.ShowLibrary();
                        break;

                    // User wants to add a label => READY
                    case 1:
                        this.AddLabel();
                        break;

                    // User wants to edit a label => READY
                    case 2:
                        this.EditLabel();
                        break;

                    // User wants to delete a label => READY
                    case 3:
                        this.DeleteLabel();
                        break;

                    // User wants to set a new calification => READY
                    case 4:
                        this.SetCalification();
                        break;

                    case 5:
                        this.DeleteImage();
                        break;


                    case 6:
                        this.ResetLibrary();
                        break;
                }
            }
        }



        private void DeleteImage()
        {
            Console.Clear();
            string presskeytocontinue = "Please, press any key to continue...";
            string emptylibraryerror = "[!] ERROR: Sorry, you don't have any images in your library";
            string description1 = "Please, select wich image you want to delete: ";

            List<string> DeleteImageTitle = this.LoadBannerData("deleteimage.txt");
            List<string> DeleteImageOptions1 = new List<string>();
            foreach (Image image in this.library.Images)
            {
                DeleteImageOptions1.Add(image.Name);
            }
            DeleteImageOptions1.Add("Exit");
            if (DeleteImageOptions1.Count == 1)
            {
                Console.SetCursorPosition((Console.WindowWidth - emptylibraryerror.Length) / 2, Console.CursorTop);
                Console.WriteLine(emptylibraryerror);
                Console.SetCursorPosition((Console.WindowWidth - presskeytocontinue.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskeytocontinue);
                Console.ReadKey();
                return;
            }
            int numberOfTheImage = this.GenerateMenu(DeleteImageOptions1, null, description1, DeleteImageTitle);
            // If user selects Exit
            if (DeleteImageOptions1[numberOfTheImage] == "Exit") return;

            this.library.Images.RemoveAt(numberOfTheImage);
            this.SaveLibrary();

            string dots = "...\n";
            string success = "Image successfully eliminated!";
            string presskey = "Please, press any key to continue...";

            System.Threading.Thread.Sleep(1000);
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - dots.Length) / 2, Console.CursorTop);
            Console.WriteLine(dots);

            System.Threading.Thread.Sleep(1000);
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - dots.Length) / 2, Console.CursorTop);
            Console.WriteLine(dots);

            System.Threading.Thread.Sleep(1000);
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - success.Length) / 2, Console.CursorTop);
            Console.WriteLine(success);

            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
            Console.WriteLine(presskey);
            Console.ReadKey();
        }


        private void SetCalification()
        {
            Console.Clear();
            string presskeytocontinue = "Please, press any key to continue...";
            string emptylibraryerror = "[!] ERROR: Sorry, you don't have any images in your library";
            string description1 = "Please, select to which image you want to edit the Calification: ";
            string calificationNotValid = "[!] ERROR: Calification not valid";
            string presskey = "Press any key to continue...";
            List<string> SetCalificationTitle = this.LoadBannerData("setcalification.txt");
            List<string> SetCalificationOptions1 = new List<string>();
            foreach (Image image in this.library.Images)
            {
                SetCalificationOptions1.Add(image.Name);
            }
            SetCalificationOptions1.Add("Exit");
            if (SetCalificationOptions1.Count == 1)
            {
                Console.SetCursorPosition((Console.WindowWidth - emptylibraryerror.Length) / 2, Console.CursorTop);
                Console.WriteLine(emptylibraryerror);
                Console.SetCursorPosition((Console.WindowWidth - presskeytocontinue.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskeytocontinue);
                Console.ReadKey();
                return;
            }
            int numberOfTheImage = this.GenerateMenu(SetCalificationOptions1, null, description1, SetCalificationTitle);
            // If user selects Exit
            if (SetCalificationOptions1[numberOfTheImage] == "Exit") return;

            // We get the image he wants to edit
            Image imageToEditCalification = this.library.Images[numberOfTheImage];

            while (true)
            {
                // Now, we know which image user wants to edit
                Console.Clear();
                foreach (string titlestring in SetCalificationTitle)
                {
                    Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                    Console.WriteLine(titlestring);
                }
                string setNewCalificationDescription = "Please, introduce the new Calification <1-5> (-1 to exit): ";
                Console.SetCursorPosition((Console.WindowWidth - setNewCalificationDescription.Length) / 2, Console.CursorTop);
                Console.Write(setNewCalificationDescription);
                string snewcal = Console.ReadLine();
                try
                {
                    int newcal = Convert.ToInt32(snewcal);
                    if (newcal == -1)
                    {
                        break;
                    }
                    if (newcal < 1 || newcal > 5)
                    {
                        throw new Exception("[!] ERROR: Calification not valid");
                    }
                    imageToEditCalification.Calification = newcal;
                    this.SaveLibrary();
                    break;
                }
                catch
                {
                    Console.WriteLine("\n");
                    Console.SetCursorPosition((Console.WindowWidth - calificationNotValid.Length) / 2, Console.CursorTop);
                    Console.WriteLine(calificationNotValid);
                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                    Console.WriteLine(presskey);
                    Console.ReadKey();
                }
            }

        }


        private void EditLabel()
        {
            Console.Clear();
            List<string> EditLabelTitle = this.LoadBannerData("editlabel.txt");
            List<string> EditLabelOptions1 = new List<string>();
            string presskeytocontinue = "Please, press any key to continue...";
            string emptylibraryerror = "[!] ERROR: Sorry, you don't have any images in your library";
            string description1 = "Please, select to which image you want to add the Label: ";
            string description2 = "Please, select which label you would like to edit ";

            List<string> optionsToEdit = new List<string>();

            // We show all the images to the user, so he can choose which image he wants to edit
            foreach (Image image in this.library.Images)
            {
                EditLabelOptions1.Add(image.Name);
            }

            EditLabelOptions1.Add("Exit");
            if (EditLabelOptions1.Count == 1)
            {
                Console.SetCursorPosition((Console.WindowWidth - emptylibraryerror.Length) / 2, Console.CursorTop);
                Console.WriteLine(emptylibraryerror);
                Console.SetCursorPosition((Console.WindowWidth - presskeytocontinue.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskeytocontinue);
                Console.ReadKey();
                return;
            }
            int numberOfTheImage = this.GenerateMenu(EditLabelOptions1, null, description1, EditLabelTitle);

            // If user selects Exit
            if (EditLabelOptions1[numberOfTheImage] == "Exit") return;

            // We get the image he wants to edit
            Image imageToEditLabel = this.library.Images[numberOfTheImage];

            foreach (Label label in imageToEditLabel.Labels)
            {
                string newstring = "";
                if (label.labelType == "SimpleLabel")
                {
                    SimpleLabel slabel = (SimpleLabel)label;
                    newstring += "\nSimpleLabel";
                    newstring += $"\n        Sentence: {slabel.Sentence}";
                    optionsToEdit.Add(newstring);
                }
                else if (label.labelType == "PersonLabel")
                {
                    PersonLabel slabel = (PersonLabel)label;
                    newstring += "\nPersonLabel";

                    if (slabel.Name != null) newstring += $"\n        Name: {slabel.Name}";
                    else newstring += $"\n        Name: [Not set]";

                    if (slabel.Surname != null) newstring += $"\n        Surname: {slabel.Surname}";
                    else newstring += $"\n        Surname: [Not set]";

                    if (slabel.Nationality != ENationality.None) newstring += $"\n        Nationality: {slabel.Nationality}";
                    else newstring += $"\n        Nationality: [Not set]";

                    if (slabel.EyesColor != EColor.None) newstring += $"\n        EyesColor: {slabel.EyesColor}";
                    else newstring += $"\n        EyesColor: [Not set]";

                    if (slabel.HairColor != EColor.None) newstring += $"\n        HairColor: {slabel.HairColor}";
                    else newstring += $"\n        HairColor: [Not set]";

                    if (slabel.Sex != ESex.None) newstring += $"\n        Sex: {slabel.Sex}";
                    else newstring += $"\n        Sex: [Not set]";

                    if (slabel.BirthDate != "") newstring += $"\n        BirthDate: {slabel.BirthDate}";
                    else newstring += $"\n        BirthDate: [Not set]";

                    if (slabel.FaceLocation != null) newstring += $"\n        Location: {slabel.FaceLocation[3]},{slabel.FaceLocation[2]},{slabel.FaceLocation[0]},{slabel.FaceLocation[1]}";
                    else newstring += $"\n        Location: [Not set]";

                    optionsToEdit.Add(newstring);
                }
                else if (label.labelType == "SpecialLabel")
                {
                    SpecialLabel slabel = (SpecialLabel)label;
                    newstring += "\nSpecialLabel";

                    if (slabel.GeographicLocation != null) newstring += $"\n        GeographicLocation: {slabel.GeographicLocation[0]}, {slabel.GeographicLocation[1]}";
                    else newstring += $"\n        GeographicLocation: [Not set]";

                    if (slabel.Address != null) newstring += $"\n        Address: {slabel.Address}";
                    else newstring += $"\n        Address: [Not set]";

                    if (slabel.Photographer != null) newstring += $"\n        Photographer: {slabel.Photographer}";
                    else newstring += $"\n        Photographer: [Not set]";

                    if (slabel.PhotoMotive != null) newstring += $"\n        PhotoMotive: {slabel.PhotoMotive}";
                    else newstring += $"\n        PhotoMotive: [Not set]";

                    if (slabel.Selfie == true) newstring += $"\n        Selfie: It is a Selfie";
                    else newstring += $"\n        Selfie: It is not a Selfie";

                    optionsToEdit.Add(newstring);
                }
            }
            optionsToEdit.Add("Exit");

            int intUsrWantsToEdit = this.GenerateMenu(optionsToEdit, null, description2, EditLabelTitle);

            if (optionsToEdit[intUsrWantsToEdit] == "Exit") return;

            Label labelUsrWants = imageToEditLabel.Labels[intUsrWantsToEdit];
            string chooseWhatToDo = "Please, select what you want to edit: ";
            switch (labelUsrWants.labelType)
            {
                case "SimpleLabel":

                    string introduceTheTag = "Please, introduce the new tag: ";
                    SimpleLabel silabel = (SimpleLabel)labelUsrWants;
                    while (true)
                    {

                        List<string> optionsToEditSimpleLabel = new List<string>();
                        string sentence = $"Sentence: {silabel.Sentence}";
                        optionsToEditSimpleLabel.Add(sentence);
                        optionsToEditSimpleLabel.Add("Done");
                        int usrWants = this.GenerateMenu(optionsToEditSimpleLabel, null, chooseWhatToDo, EditLabelTitle);
                        if (usrWants == 1) break;
                        else
                        {
                            Console.Clear();
                            foreach (string titlestring in EditLabelTitle)
                            {
                                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                                Console.WriteLine(titlestring);
                            }
                            Console.SetCursorPosition((Console.WindowWidth - introduceTheTag.Length) / 2, Console.CursorTop);
                            Console.Write(introduceTheTag);
                            string newtag = Console.ReadLine();
                            silabel.Sentence = newtag;
                            this.SaveLibrary();
                        }
                    }
                    this.SaveLibrary();
                    break;

                case "PersonLabel":
                    PersonLabel auxLabel = (PersonLabel)labelUsrWants;
                    while (true)
                    {
                        string settedFaceLocation;
                        if (auxLabel.FaceLocation != null) settedFaceLocation = "FaceLocation:\t" + Convert.ToString(auxLabel.FaceLocation[0]) + "," + Convert.ToString(auxLabel.FaceLocation[1]) + "," + Convert.ToString(auxLabel.FaceLocation[2]) + "," + Convert.ToString(auxLabel.FaceLocation[3]);
                        else settedFaceLocation = "FaceLocation:\t[Not set]";

                        string settedNationality;
                        if (auxLabel.Nationality != ENationality.None) settedNationality = "Nationality:\t\t" + Enum.GetName(typeof(ENationality), auxLabel.Nationality);
                        else settedNationality = "Nationality:\t\t[Not set]";

                        string settedEyesColor;
                        if (auxLabel.EyesColor != EColor.None) settedEyesColor = "EyesColor:\t\t" + Enum.GetName(typeof(EColor), auxLabel.EyesColor);
                        else settedEyesColor = "EyesColor:\t\t[Not set]";

                        string settedHairColor;
                        if (auxLabel.HairColor != EColor.None) settedHairColor = "HairColor:\t\t" + Enum.GetName(typeof(EColor), auxLabel.HairColor);
                        else settedHairColor = "HairColor:\t\t[Not set]";

                        string settedSex;
                        if (auxLabel.Sex != ESex.None) settedSex = "Sex:\t\t\t" + Enum.GetName(typeof(ESex), auxLabel.Sex);
                        else settedSex = "Sex:\t\t\t[Not set]";

                        string settedName;
                        if (auxLabel.Name == null) settedName = "Name:\t\t[Not set]";
                        else settedName = "Name:\t\t" + auxLabel.Name;

                        string settedSurname;
                        if (auxLabel.Surname == null) settedSurname = "Surname:\t\t[Not set]";
                        else settedSurname = "Surname:\t\t" + auxLabel.Surname;

                        string settedBirthDate;
                        if (auxLabel.BirthDate == "") settedBirthDate = "BirthDate:\t\t[Not set]";
                        else settedBirthDate = "BirthDate:\t\t" + auxLabel.BirthDate;

                        List<string> optionsToEditPersonLabel = new List<string>() { settedName, settedSurname, settedSex, settedNationality, settedHairColor, settedEyesColor, settedBirthDate, settedFaceLocation, "Done" };
                        int usrWants2 = this.GenerateMenu(optionsToEditPersonLabel, null, chooseWhatToDo, EditLabelTitle);
                        if (optionsToEditPersonLabel[usrWants2] == "Done") break;

                        // Ya sabemos cual opcion quiere editar el usuario, ahora hacemos clear y mostramos el titulo
                        Console.Clear();
                        foreach (string titlestring in EditLabelTitle)
                        {
                            Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                            Console.WriteLine(titlestring);
                        }

                        // Show the user the screen to modify the attribute
                        string enterNew = "Please, enter the new ";
                        string newaux = "";
                        string presskey = "Please, press any key to continue";
                        switch (usrWants2)
                        {
                            case 0:
                                newaux = enterNew + "Name: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux.Length) / 2, Console.CursorTop);
                                Console.Write(newaux);
                                newaux = Console.ReadLine();
                                auxLabel.Name = newaux;
                                break;
                            case 1:
                                newaux = enterNew + "Surname: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux.Length) / 2, Console.CursorTop);
                                Console.Write(newaux);
                                newaux = Console.ReadLine();
                                auxLabel.Surname = newaux;
                                break;

                            case 2:
                                newaux = enterNew + "Sex: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux.Length) / 2, Console.CursorTop);
                                Console.Write(newaux);
                                newaux = Console.ReadLine();
                                try
                                {
                                    auxLabel.Sex = (ESex)Enum.Parse(typeof(ESex), newaux);
                                }
                                catch
                                {
                                    string sexnotvalid = "[!] ERROR: Sex not valid";
                                    Console.SetCursorPosition((Console.WindowWidth - sexnotvalid.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(sexnotvalid);
                                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(presskey);
                                    Console.ReadKey();
                                }
                                break;
                            case 3:
                                newaux = enterNew + "Nationality: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux.Length) / 2, Console.CursorTop);
                                Console.Write(newaux);
                                newaux = Console.ReadLine();
                                try
                                {
                                    auxLabel.Nationality = (ENationality)Enum.Parse(typeof(ENationality), newaux);
                                }
                                catch
                                {
                                    string nationalitynotvalid = "[!] ERROR: Nationality not valid";
                                    Console.SetCursorPosition((Console.WindowWidth - nationalitynotvalid.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(nationalitynotvalid);
                                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(presskey);
                                    Console.ReadKey();
                                }
                                break;
                            case 4:
                                newaux = enterNew + "HairColor: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux.Length) / 2, Console.CursorTop);
                                Console.Write(newaux);
                                newaux = Console.ReadLine();
                                try
                                {
                                    auxLabel.HairColor = (EColor)Enum.Parse(typeof(EColor), newaux);
                                }
                                catch
                                {
                                    string colornotvalid = "[!] ERROR: HairColor not valid";
                                    Console.SetCursorPosition((Console.WindowWidth - colornotvalid.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(colornotvalid);
                                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(presskey);
                                    Console.ReadKey();
                                }
                                break;

                            case 5:
                                newaux = enterNew + "EyesColor: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux.Length) / 2, Console.CursorTop);
                                Console.Write(newaux);
                                newaux = Console.ReadLine();
                                try
                                {
                                    auxLabel.EyesColor = (EColor)Enum.Parse(typeof(EColor), newaux);
                                }
                                catch
                                {
                                    string colornotvalid = "[!] ERROR: EyesColor not valid";
                                    Console.SetCursorPosition((Console.WindowWidth - colornotvalid.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(colornotvalid);
                                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(presskey);
                                    Console.ReadKey();
                                }
                                break;



                            case 6:
                                string newaux1 = enterNew + "Day <1-31>: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux1.Length) / 2, Console.CursorTop);
                                Console.Write(newaux1);
                                newaux1 = Console.ReadLine();
                                int day = Convert.ToInt32(newaux1);
                                if (day < 1 || day > 31)
                                {
                                    string colornotvalid = "[!] ERROR: Day not valid";
                                    Console.SetCursorPosition((Console.WindowWidth - colornotvalid.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(colornotvalid);
                                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(presskey);
                                    Console.ReadKey();
                                    break;
                                }
                                string newaux2 = enterNew + "Month <1-12>: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux2.Length) / 2, Console.CursorTop);
                                Console.Write(newaux2);
                                newaux2 = Console.ReadLine();
                                int month = Convert.ToInt32(newaux2);
                                if (month < 1 || month > 12)
                                {
                                    string colornotvalid = "[!] ERROR: Month not valid";
                                    Console.SetCursorPosition((Console.WindowWidth - colornotvalid.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(colornotvalid);
                                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(presskey);
                                    Console.ReadKey();
                                    break;
                                }
                                string newaux3 = enterNew + "Year <1850-2019>: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux3.Length) / 2, Console.CursorTop);
                                Console.Write(newaux3);
                                newaux3 = Console.ReadLine();
                                int year = Convert.ToInt32(newaux3);
                                if (year < 1850 || year > 2019)
                                {
                                    string colornotvalid = "[!] ERROR: Year not valid";
                                    Console.SetCursorPosition((Console.WindowWidth - colornotvalid.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(colornotvalid);
                                    Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(presskey);
                                    Console.ReadKey();
                                    break;
                                }
                                auxLabel.BirthDate = day.ToString() + "-" + month.ToString() + "-" + year.ToString();
                                break;





                            case 7:
                                double left, top, width, height;
                                string AddLeftSelection = "Please, introduce the LEFT parameter: ";
                                Console.SetCursorPosition((Console.WindowWidth - AddLeftSelection.Length) / 2, Console.CursorTop);
                                Console.Write(AddLeftSelection);
                                try
                                {
                                    left = Convert.ToDouble(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("\n");
                                    string LeftNotValid = "[!] ERROR: LEFT parameter not valid, press any key to continue...";
                                    Console.SetCursorPosition((Console.WindowWidth - LeftNotValid.Length) / 2, Console.CursorTop);
                                    Console.Write(LeftNotValid);
                                    Console.ReadKey();
                                    continue;
                                }

                                string AddTopSelection = "Please, introduce the TOP parameter: ";
                                Console.SetCursorPosition((Console.WindowWidth - AddTopSelection.Length) / 2, Console.CursorTop);
                                Console.Write(AddTopSelection);
                                try
                                {
                                    top = Convert.ToDouble(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("\n");
                                    string TopNotValid = "[!] ERROR: TOP parameter not valid, press any key to continue...";
                                    Console.SetCursorPosition((Console.WindowWidth - TopNotValid.Length) / 2, Console.CursorTop);
                                    Console.Write(TopNotValid);
                                    Console.ReadKey();
                                    continue;
                                }

                                string AddHeightSelection = "Please, introduce the HEIGHT parameter: ";
                                Console.SetCursorPosition((Console.WindowWidth - AddHeightSelection.Length) / 2, Console.CursorTop);
                                Console.Write(AddHeightSelection);
                                try
                                {
                                    height = Convert.ToDouble(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("\n");
                                    string HeightNotValid = "[!] ERROR: HEIGHT parameter not valid, press any key to continue...";
                                    Console.SetCursorPosition((Console.WindowWidth - HeightNotValid.Length) / 2, Console.CursorTop);
                                    Console.Write(HeightNotValid);
                                    Console.ReadKey();
                                    continue;
                                }

                                string AddWidthSelection = "Please, introduce the WIDTH parameter: ";
                                Console.SetCursorPosition((Console.WindowWidth - AddWidthSelection.Length) / 2, Console.CursorTop);
                                Console.Write(AddWidthSelection);
                                try
                                {
                                    width = Convert.ToDouble(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("\n");
                                    string WidthNotValid = "[!] ERROR: WIDTH parameter not valid, press any key to continue...";
                                    Console.SetCursorPosition((Console.WindowWidth - WidthNotValid.Length) / 2, Console.CursorTop);
                                    Console.Write(WidthNotValid);
                                    Console.ReadKey();
                                    continue;
                                }
                                auxLabel.FaceLocation = new double[] { width, height, top, left };

                                break;
                        }
                        this.SaveLibrary();


                    }
                    this.SaveLibrary();
                    break;

                case "SpecialLabel":
                    SpecialLabel auxLabel2 = (SpecialLabel)labelUsrWants;
                    while (true)
                    {
                        string settedGeographicLocation;
                        if (auxLabel2.GeographicLocation != null) settedGeographicLocation = "GeographicLocation:\t" + Convert.ToString(auxLabel2.GeographicLocation[0]) + "," + Convert.ToString(auxLabel2.GeographicLocation[1]);
                        else settedGeographicLocation = "GeographicLocation:\t[Not set]";

                        string settedAddress;
                        if (auxLabel2.Address == null) settedAddress = "Address:\t\t[Not set]";
                        else settedAddress = "Address:\t\t" + auxLabel2.Address;

                        string settedPhotographer;
                        if (auxLabel2.Photographer == null) settedPhotographer = "Photographer:\t[Not set]";
                        else settedPhotographer = "Photographer:\t" + auxLabel2.Photographer;

                        string settedPhotoMotive;
                        if (auxLabel2.PhotoMotive == null) settedPhotoMotive = "PhotoMotive:\t\t[Not set]";
                        else settedPhotoMotive = "PhotoMotive:\t\t" + auxLabel2.PhotoMotive;

                        string settedSelfie;
                        if (auxLabel2.Selfie == false) settedSelfie = "Selfie:\t\tIt is not a Selfie";
                        else settedSelfie = "Selfie:\t\tIt is a Selfie";

                        List<string> optionsToEditSpecialLabel = new List<string>() { settedGeographicLocation, settedAddress, settedPhotographer, settedPhotoMotive, settedSelfie, "Done" };
                        int usrWants2 = this.GenerateMenu(optionsToEditSpecialLabel, null, chooseWhatToDo, EditLabelTitle);
                        if (optionsToEditSpecialLabel[usrWants2] == "Done") break;

                        // Ya sabemos cual opcion quiere editar el usuario, ahora hacemos clear y mostramos el titulo
                        Console.Clear();
                        foreach (string titlestring in EditLabelTitle)
                        {
                            Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                            Console.WriteLine(titlestring);
                        }

                        // Show the user the screen to modify the attribute
                        string enterNew = "Please, enter the new ";
                        string newaux = "";
                        switch (usrWants2)
                        {
                            case 0:
                                int latitude, longitude;
                                while (true)
                                {
                                    Console.Clear();
                                    foreach (string titlestring in EditLabelTitle)
                                    {
                                        Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                                        Console.WriteLine(titlestring);
                                    }

                                    string AddLatitudeSelection = "Please, introduce the LATITUDE parameter <-90, 90>: ";
                                    Console.SetCursorPosition((Console.WindowWidth - AddLatitudeSelection.Length) / 2, Console.CursorTop);
                                    Console.Write(AddLatitudeSelection);
                                    try
                                    {
                                        latitude = Convert.ToInt32(Console.ReadLine());
                                        if (latitude < -90 || latitude > 90)
                                        {
                                            throw new Exception("LATITUDE parameter not valid");
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\n");
                                        string LatitudeNotValid = "[!] ERROR: LATITUDE parameter not valid, press any key to continue...";
                                        Console.SetCursorPosition((Console.WindowWidth - LatitudeNotValid.Length) / 2, Console.CursorTop);
                                        Console.Write(LatitudeNotValid);
                                        Console.ReadKey();
                                        continue;
                                    }


                                    string AddLongitudeSelection = "Please, introduce the LONGITUDE parameter <-180, 180>: ";
                                    Console.SetCursorPosition((Console.WindowWidth - AddLongitudeSelection.Length) / 2, Console.CursorTop);
                                    Console.Write(AddLongitudeSelection);
                                    try
                                    {
                                        longitude = Convert.ToInt32(Console.ReadLine());
                                        if (longitude < -180 || longitude > 180)
                                        {
                                            throw new Exception("LONGITUDE parameter not valid");
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\n");
                                        string LongitudeNotValid = "[!] ERROR: LONGITUDE parameter not valid, press any key to continue...";
                                        Console.SetCursorPosition((Console.WindowWidth - LongitudeNotValid.Length) / 2, Console.CursorTop);
                                        Console.Write(LongitudeNotValid);
                                        Console.ReadKey();
                                        continue;
                                    }
                                    auxLabel2.GeographicLocation = new double[] { latitude, longitude };
                                    break;
                                }
                                break;
                            case 1:
                                newaux = enterNew + "Address: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux.Length) / 2, Console.CursorTop);
                                Console.Write(newaux);
                                newaux = Console.ReadLine();
                                auxLabel2.Address = newaux;
                                break;

                            case 2:
                                newaux = enterNew + "Photographer: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux.Length) / 2, Console.CursorTop);
                                Console.Write(newaux);
                                newaux = Console.ReadLine();
                                auxLabel2.Photographer = newaux;
                                break;
                            case 3:
                                newaux = enterNew + "PhotoMotive: ";
                                Console.SetCursorPosition((Console.WindowWidth - newaux.Length) / 2, Console.CursorTop);
                                Console.Write(newaux);
                                newaux = Console.ReadLine();
                                auxLabel2.PhotoMotive = newaux;
                                break;
                            case 4:
                                newaux = enterNew + "Selfie: ";
                                int usrDec = this.GenerateMenu(new List<string>() { "It is a Selfie", "It is not a Selfie" }, null, newaux, EditLabelTitle);
                                if (usrDec == 0)
                                {
                                    auxLabel2.Selfie = true;
                                }
                                else auxLabel2.Selfie = false;
                                break;
                        }
                        this.SaveLibrary();


                    }
                    this.SaveLibrary();
                    break;
            }
        }


        private void DeleteLabel()
        {
            Console.Clear();
            List<string> DeleteLabelTitle = this.LoadBannerData("deletelabel.txt");
            List<string> DeleteLabelOptions1 = new List<string>();
            string presskeytocontinue = "Please, press any key to continue...";
            string emptylibraryerror = "[!] ERROR: Sorry, you don't have any images in your library";
            string description1 = "Please, select on which image you want to delete a Label: ";
            string description2 = "Please, select which label you want to delete: ";


            foreach (Image image in this.library.Images)
            {
                DeleteLabelOptions1.Add(image.Name);
            }
            DeleteLabelOptions1.Add("Exit");
            if (DeleteLabelOptions1.Count == 1)
            {
                Console.SetCursorPosition((Console.WindowWidth - emptylibraryerror.Length) / 2, Console.CursorTop);
                Console.WriteLine(emptylibraryerror);
                Console.SetCursorPosition((Console.WindowWidth - presskeytocontinue.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskeytocontinue);
                Console.ReadKey();
                return;
            }
            int numberOfTheImage = this.GenerateMenu(DeleteLabelOptions1, null, description1, DeleteLabelTitle);

            // If user selects Exit
            if (DeleteLabelOptions1[numberOfTheImage] == "Exit") return;
            Image imageToDeleteLabel = this.library.Images[numberOfTheImage];


            List<string> optionsToDelete = new List<string>();
            // We load the options of the labels to delete
            foreach (Label label in imageToDeleteLabel.Labels)
            {
                string newstring = "";
                if (label.labelType == "SimpleLabel")
                {
                    SimpleLabel slabel = (SimpleLabel)label;
                    newstring += "\nSimpleLabel";
                    newstring += $"\n        Sentence: {slabel.Sentence}";
                    optionsToDelete.Add(newstring);
                }
                else if (label.labelType == "PersonLabel")
                {
                    PersonLabel slabel = (PersonLabel)label;
                    newstring += "\nPersonLabel";

                    if (slabel.Name != null) newstring += $"\n        Name: {slabel.Name}";
                    else newstring += $"\n        Name: [Not set]";

                    if (slabel.Surname != null) newstring += $"\n        Surname: {slabel.Surname}";
                    else newstring += $"\n        Surname: [Not set]";

                    if (slabel.Nationality != ENationality.None) newstring += $"\n        Nationality: {slabel.Nationality}";
                    else newstring += $"\n        Nationality: [Not set]";

                    if (slabel.EyesColor != EColor.None) newstring += $"\n        EyesColor: {slabel.EyesColor}";
                    else newstring += $"\n        EyesColor: [Not set]";

                    if (slabel.HairColor != EColor.None) newstring += $"\n        HairColor: {slabel.HairColor}";
                    else newstring += $"\n        HairColor: [Not set]";

                    if (slabel.Sex != ESex.None) newstring += $"\n        Sex: {slabel.Sex}";
                    else newstring += $"\n        Sex: [Not set]";

                    if (slabel.BirthDate != "") newstring += $"\n        BirthDate: {slabel.BirthDate}";
                    else newstring += $"\n        BirthDate: [Not set]";

                    if (slabel.FaceLocation != null) newstring += $"\n        Location: {slabel.FaceLocation[3]},{slabel.FaceLocation[2]},{slabel.FaceLocation[0]},{slabel.FaceLocation[1]}";
                    else newstring += $"\n        Location: [Not set]";

                    optionsToDelete.Add(newstring);
                }
                else if (label.labelType == "SpecialLabel")
                {
                    SpecialLabel slabel = (SpecialLabel)label;
                    newstring += "\nSpecialLabel";

                    if (slabel.GeographicLocation != null) newstring += $"\n        GeographicLocation: {slabel.GeographicLocation[0]}, {slabel.GeographicLocation[1]}";
                    else newstring += $"\n        GeographicLocation: [Not set]";

                    if (slabel.Address != null) newstring += $"\n        Address: {slabel.Address}";
                    else newstring += $"\n        Address: [Not set]";

                    if (slabel.Photographer != null) newstring += $"\n        Photographer: {slabel.Photographer}";
                    else newstring += $"\n        Photographer: [Not set]";

                    if (slabel.PhotoMotive != null) newstring += $"\n        PhotoMotive: {slabel.PhotoMotive}";
                    else newstring += $"\n        PhotoMotive: [Not set]";

                    if (slabel.Selfie == true) newstring += $"\n        Selfie: It is a Selfie";
                    else newstring += $"\n        Selfie: It is not a Selfie";

                    optionsToDelete.Add(newstring);
                }
            }


            optionsToDelete.Add("Exit");
            int intUsrWantsToDelete = this.GenerateMenu(optionsToDelete, null, description2, DeleteLabelTitle);

            if (optionsToDelete[intUsrWantsToDelete] == "Exit") return;

            imageToDeleteLabel.Labels.RemoveAt(intUsrWantsToDelete);
            return;

        }


        private void AddLabel()
        {
            Console.Clear();
            List<string> AddLabelTitle = this.LoadBannerData("addlabel.txt");
            List<string> AddLabelOptions1 = new List<string>();
            string presskeytocontinue = "Please, press any key to continue...";
            string emptylibraryerror = "[!] ERROR: Sorry, you don't have any images in your library";
            string description1 = "Please, select to which image you want to add the Label: ";
            string description2 = "Please, select which type of label you would like to add: ";
            string addlabeldescription = "Please, select an option: ";
            List<string> AddLabelOptions2 = new List<string>() { "SimpleLabel", "PersonLabel", "SpecialLabel", "Done" };

            foreach (Image image in this.library.Images)
            {
                AddLabelOptions1.Add(image.Name);
            }
            AddLabelOptions1.Add("Exit");
            if (AddLabelOptions1.Count == 1)
            {
                Console.SetCursorPosition((Console.WindowWidth - emptylibraryerror.Length) / 2, Console.CursorTop);
                Console.WriteLine(emptylibraryerror);
                Console.SetCursorPosition((Console.WindowWidth - presskeytocontinue.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskeytocontinue);
                Console.ReadKey();
                return;
            }
            int numberOfTheImage = this.GenerateMenu(AddLabelOptions1, null, description1, AddLabelTitle);

            // If user selects Exit
            if (AddLabelOptions1[numberOfTheImage] == "Exit") return;


            Image imageToAddLabel = this.library.Images[numberOfTheImage];

            while (true)
            {
                int usrLabel = this.GenerateMenu(AddLabelOptions2, null, description2, AddLabelTitle);
                switch (usrLabel)
                {
                    // If user wants to add a SimpleLabel
                    case 0:
                        Console.Clear();

                        // First, we load watson recommendations
                        Console.WriteLine("\n");
                        string LoadingWatson = "Loading Watson recommendations...";
                        string SimpleLabelSelection = "Please, introduce the tag for this new SimpleLabel: ";
                        Console.SetCursorPosition((Console.WindowWidth - LoadingWatson.Length) / 2, Console.CursorTop);
                        Console.Write(LoadingWatson);

                        string temppath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\Temp\";
                        string[] currentfiles = Directory.GetFiles(temppath);
                        Random randomNumber = new Random();
                        string realpath;
                        while (true)
                        {
                            int newRandom = randomNumber.Next(1, 100000);
                            string number = Convert.ToString(newRandom);
                            realpath = temppath + number + ".jpg";
                            try
                            {
                                this.library.Images[numberOfTheImage].BitmapImage.Save(realpath);
                                break;
                            }
                            catch
                            {
                                continue;
                            }
                        }


                        Dictionary<int, Dictionary<string, double>> watsonResults = this.producer.ClassifyImage(realpath);
                        List<string> watsonOptions = new List<string>();
                        foreach (Dictionary<string, double> dic in watsonResults.Values)
                        {
                            foreach (KeyValuePair<string, double> pair in dic)
                            {
                                watsonOptions.Add(pair.Key);
                            }
                        }
                        // Add the option of a personalized Label
                        watsonOptions.Add("Personalized Label");


                        int selectedOption2 = this.GenerateMenu(watsonOptions, null, addlabeldescription, AddLabelTitle);
                        string selected = watsonOptions[selectedOption2];
                        if (selected != "Personalized Label")
                        {
                            SimpleLabel sauxLabel = new SimpleLabel(selected);
                            imageToAddLabel.AddLabel(sauxLabel);
                            this.SaveLibrary();
                        }
                        else
                        {
                            Console.Clear();
                            foreach (string titlestring in AddLabelTitle)
                            {
                                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                                Console.WriteLine(titlestring);
                            }
                            Console.SetCursorPosition((Console.WindowWidth - SimpleLabelSelection.Length) / 2, Console.CursorTop);
                            Console.Write(SimpleLabelSelection);
                            string userTag = Console.ReadLine();
                            SimpleLabel sauxLabel = new SimpleLabel(userTag);
                            imageToAddLabel.AddLabel(sauxLabel);
                            this.SaveLibrary();
                        }
                        break;


                    // User wants to Add a PersonLabel
                    case 1:

                        int userSelection;
                        PersonLabel auxLabel = new PersonLabel();

                        while (true)
                        {
                            string settedFaceLocation;
                            if (auxLabel.FaceLocation != null) settedFaceLocation = "\t " + Convert.ToString(auxLabel.FaceLocation[0]) + "," + Convert.ToString(auxLabel.FaceLocation[1]) + "," + Convert.ToString(auxLabel.FaceLocation[2]) + "," + Convert.ToString(auxLabel.FaceLocation[3]);
                            else settedFaceLocation = "\t [Not set]";
                            string settedNationality;
                            if (auxLabel.Nationality != ENationality.None) settedNationality = "\t " + Enum.GetName(typeof(ENationality), auxLabel.Nationality);
                            else settedNationality = "\t [Not set]";
                            string settedEyesColor;
                            if (auxLabel.EyesColor != EColor.None) settedEyesColor = "\t\t " + Enum.GetName(typeof(EColor), auxLabel.EyesColor);
                            else settedEyesColor = "\t\t [Not set]";
                            string settedHairColor;
                            if (auxLabel.HairColor != EColor.None) settedHairColor = "\t\t " + Enum.GetName(typeof(EColor), auxLabel.HairColor);
                            else settedHairColor = "\t\t [Not set]";
                            string settedSex;
                            if (auxLabel.Sex != ESex.None) settedSex = "\t\t " + Enum.GetName(typeof(EColor), auxLabel.Sex);
                            else settedSex = "\t\t [Not set]";
                            string settedName;
                            if (auxLabel.Name == null) settedName = "\t\t [Not set]";
                            else settedName = "\t\t " + auxLabel.Name;
                            string settedSurname;
                            if (auxLabel.Surname == null) settedSurname = "\t\t [Not set]";
                            else settedSurname = "\t\t " + auxLabel.Surname;
                            string settedBirthDate;
                            if (auxLabel.BirthDate == "") settedBirthDate = "\t\t [Not set]";
                            else settedBirthDate = "\t\t " + auxLabel.BirthDate;


                            Console.Clear();
                            List<string> personOptions = new List<string>() { "Name: " + settedName, "Surname: " + settedSurname, "FaceLocation: " + settedFaceLocation, "Nationality: " + settedNationality, "EyesColor: " + settedEyesColor, "HairColor: " + settedHairColor, "Sex: " + settedSex, "Birthdate: " + settedBirthDate, "Exit" };
                            userSelection = this.GenerateMenu(personOptions, null, addlabeldescription, AddLabelTitle);
                            if (userSelection == 8) break;
                            switch (userSelection)
                            {
                                case 0:
                                    Console.Clear();
                                    string AddNameTitle = "~ Add Name to PersonLabel ~\n\n";
                                    Console.SetCursorPosition((Console.WindowWidth - AddNameTitle.Length) / 2, Console.CursorTop);
                                    Console.Write(AddNameTitle);
                                    string AddNameSelection = "Please, introduce the name: ";
                                    Console.SetCursorPosition((Console.WindowWidth - AddNameSelection.Length) / 2, Console.CursorTop);
                                    Console.Write(AddNameSelection);
                                    auxLabel.Name = Console.ReadLine();
                                    break;
                                case 1:
                                    Console.Clear();
                                    string AddSurnameTitle = "~ Add Surname to PersonLabel ~\n\n";
                                    Console.SetCursorPosition((Console.WindowWidth - AddSurnameTitle.Length) / 2, Console.CursorTop);
                                    Console.Write(AddSurnameTitle);
                                    string AddSurnameSelection = "Please, introduce the surname: ";
                                    Console.SetCursorPosition((Console.WindowWidth - AddSurnameSelection.Length) / 2, Console.CursorTop);
                                    Console.Write(AddSurnameSelection);
                                    auxLabel.Surname = Console.ReadLine();
                                    break;
                                case 2:
                                    double left, top, width, height;
                                    while (true)
                                    {
                                        Console.Clear();

                                        string AddFaceLocation = "~ Add FaceLocation to PersonLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddFaceLocation.Length) / 2, Console.CursorTop);
                                        Console.Write(AddFaceLocation);

                                        string AddLeftSelection = "Please, introduce the LEFT parameter: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddLeftSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddLeftSelection);
                                        try
                                        {
                                            left = Convert.ToDouble(Console.ReadLine());
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string LeftNotValid = "[!] ERROR: LEFT parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - LeftNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(LeftNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }

                                        string AddTopSelection = "Please, introduce the TOP parameter: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddTopSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddTopSelection);
                                        try
                                        {
                                            top = Convert.ToDouble(Console.ReadLine());
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string TopNotValid = "[!] ERROR: TOP parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - TopNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(TopNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }

                                        string AddHeightSelection = "Please, introduce the HEIGHT parameter: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddHeightSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddHeightSelection);
                                        try
                                        {
                                            height = Convert.ToDouble(Console.ReadLine());
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string HeightNotValid = "[!] ERROR: HEIGHT parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - HeightNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(HeightNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }

                                        string AddWidthSelection = "Please, introduce the WIDTH parameter: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddWidthSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddWidthSelection);
                                        try
                                        {
                                            width = Convert.ToDouble(Console.ReadLine());
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string WidthNotValid = "[!] ERROR: WIDTH parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - WidthNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(WidthNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }
                                        auxLabel.FaceLocation = new double[] { width, height, top, left };
                                        break;
                                    }
                                    break;
                                case 3:
                                    ENationality auxnationality;
                                    while (true)
                                    {
                                        Console.Clear();
                                        string AddNationalityTitle = "~ Add Nationality to PersonLabel ~\n\n";
                                        string AddNationalitySelection = "Please, introduce the nationality: ";

                                        Console.SetCursorPosition((Console.WindowWidth - AddNationalityTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddNationalityTitle);

                                        Console.SetCursorPosition((Console.WindowWidth - AddNationalitySelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddNationalitySelection);

                                        string option = Console.ReadLine();

                                        try
                                        {
                                            auxnationality = (ENationality)Enum.Parse(typeof(ENationality), option);
                                            break;
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string NationalityNotValid = "[!] ERROR: Nationality parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - NationalityNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(NationalityNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }
                                    }
                                    auxLabel.Nationality = auxnationality;
                                    break;
                                case 4:
                                    EColor EyesColor;
                                    while (true)
                                    {
                                        Console.Clear();
                                        string AddEyesColorTitle = "~ Add EyesColor to PersonLabel ~\n\n";
                                        string AddEyesColorSelection = "Please, introduce the EyesColor: ";

                                        Console.SetCursorPosition((Console.WindowWidth - AddEyesColorTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddEyesColorTitle);

                                        Console.SetCursorPosition((Console.WindowWidth - AddEyesColorSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddEyesColorSelection);

                                        string option = Console.ReadLine();

                                        try
                                        {
                                            EyesColor = (EColor)Enum.Parse(typeof(EColor), option);
                                            break;
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string ColorNotValid = "[!] ERROR: Color parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - ColorNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(ColorNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }
                                    }
                                    auxLabel.EyesColor = EyesColor;
                                    break;
                                case 5:
                                    EColor HairColor;
                                    while (true)
                                    {
                                        Console.Clear();
                                        string AddHairColorTitle = "~ Add HairColor to PersonLabel ~\n\n";
                                        string AddHairColorSelection = "Please, introduce the HairColor: ";

                                        Console.SetCursorPosition((Console.WindowWidth - AddHairColorTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddHairColorTitle);

                                        Console.SetCursorPosition((Console.WindowWidth - AddHairColorSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddHairColorSelection);

                                        string option = Console.ReadLine();

                                        try
                                        {
                                            HairColor = (EColor)Enum.Parse(typeof(EColor), option);
                                            break;
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string ColorNotValid = "[!] ERROR: Color parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - ColorNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(ColorNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }
                                    }
                                    auxLabel.HairColor = HairColor;
                                    break;
                                case 6:
                                    ESex Sex;
                                    while (true)
                                    {
                                        Console.Clear();
                                        string AddSexTitle = "~ Add Sex to PersonLabel ~\n\n";
                                        string AddSexSelection = "Please, introduce the Sex <Hombre-Mujer>: ";

                                        Console.SetCursorPosition((Console.WindowWidth - AddSexTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddSexTitle);

                                        Console.SetCursorPosition((Console.WindowWidth - AddSexSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddSexSelection);

                                        string option = Console.ReadLine();

                                        try
                                        {
                                            Sex = (ESex)Enum.Parse(typeof(ESex), option);
                                            break;
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string SexNotValid = "[!] ERROR: Sex parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - SexNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(SexNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }
                                    }
                                    auxLabel.Sex = Sex;
                                    break;
                                case 7:
                                    int day, month, year;
                                    while (true)
                                    {
                                        Console.Clear();

                                        string AddBirthdate = "~ Add Birthdate to PersonLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddBirthdate.Length) / 2, Console.CursorTop);
                                        Console.Write(AddBirthdate);

                                        string AddDaySelection = "Please, introduce the DAY parameter <1-31>: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddDaySelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddDaySelection);
                                        try
                                        {
                                            day = Convert.ToInt32(Console.ReadLine());
                                            if (day < 1 || day > 31)
                                            {
                                                throw new Exception("DAY parameter not valid");
                                            }
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string DayNotValid = "[!] ERROR: DAY parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - DayNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(DayNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }

                                        string AddMonthSelection = "Please, introduce the MONTH parameter <1-12>: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddMonthSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddMonthSelection);
                                        try
                                        {
                                            month = Convert.ToInt32(Console.ReadLine());
                                            if (month < 1 || month > 12)
                                            {
                                                throw new Exception("MONTH parameter not valid");
                                            }
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string MonthNotValid = "[!] ERROR: MONTH parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - MonthNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(MonthNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }

                                        string AddYearSelection = "Please, introduce the YEAR parameter <1850-2019>: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddYearSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddYearSelection);
                                        try
                                        {
                                            year = Convert.ToInt32(Console.ReadLine());
                                            if (year < 1850 || year > 2019)
                                            {
                                                throw new Exception("YEAR parameter not valid");
                                            }
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string YearNotValid = "[!] ERROR: YEAR parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - YearNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(YearNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }


                                        auxLabel.BirthDate = Convert.ToString(day) + "-" + Convert.ToString(month) + "-" + Convert.ToString(year);
                                        break;
                                    }
                                    break;
                            }
                        }
                        imageToAddLabel.AddLabel(auxLabel);
                        this.SaveLibrary();
                        break;



                    // User wants to Add a SpecialLabel
                    case 2:

                        int userSelection2;
                        SpecialLabel auxLabel2 = new SpecialLabel();
                        while (true)
                        {
                            string settedGeographicLocation;
                            if (auxLabel2.GeographicLocation != null) settedGeographicLocation = "\t " + Convert.ToString(auxLabel2.GeographicLocation[0]) + "," + Convert.ToString(auxLabel2.GeographicLocation[1]);
                            else settedGeographicLocation = "\t [Not set]";
                            string settedAddress;
                            if (auxLabel2.Address != null) settedAddress = "\t\t " + auxLabel2.Address;
                            else settedAddress = "\t\t [Not set]";

                            string settedPhotographer;
                            if (auxLabel2.Photographer != null) settedPhotographer = "\t\t " + auxLabel2.Photographer;
                            else settedPhotographer = "\t [Not set]";

                            string settedPhotoMotive;
                            if (auxLabel2.PhotoMotive != null) settedPhotoMotive = "\t\t " + auxLabel2.PhotoMotive;
                            else settedPhotoMotive = "\t [Not set]";

                            string settedSelfie;
                            if (auxLabel2.Selfie != false) settedSelfie = "\t\t It is a Selfie";
                            else settedSelfie = "\t\t It is not a Selfie";

                            Console.Clear();
                            List<string> personOptions = new List<string>() { "GeographicLocation: " + settedGeographicLocation, "Address: " + settedAddress, "Photographer: " + settedPhotographer, "PhotoMotive: " + settedPhotoMotive, "Selfie: " + settedSelfie, "Exit" };
                            userSelection2 = this.GenerateMenu(personOptions, null, addlabeldescription, AddLabelTitle);
                            if (userSelection2 == 5) break;
                            switch (userSelection2)
                            {
                                case 0:
                                    int latitude, longitude;
                                    while (true)
                                    {
                                        Console.Clear();

                                        string AddGeographicLocation = "~ Add GeographicLocation to SpecialLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddGeographicLocation.Length) / 2, Console.CursorTop);
                                        Console.Write(AddGeographicLocation);

                                        string AddLatitudeSelection = "Please, introduce the LATITUDE parameter <-90, 90>: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddLatitudeSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddLatitudeSelection);
                                        try
                                        {
                                            latitude = Convert.ToInt32(Console.ReadLine());
                                            if (latitude < -90 || latitude > 90)
                                            {
                                                throw new Exception("LATITUDE parameter not valid");
                                            }
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string LatitudeNotValid = "[!] ERROR: LATITUDE parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - LatitudeNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(LatitudeNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }


                                        string AddLongitudeSelection = "Please, introduce the LONGITUDE parameter <-180, 180>: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddLongitudeSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddLongitudeSelection);
                                        try
                                        {
                                            longitude = Convert.ToInt32(Console.ReadLine());
                                            if (longitude < -180 || longitude > 180)
                                            {
                                                throw new Exception("LONGITUDE parameter not valid");
                                            }
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\n");
                                            string LongitudeNotValid = "[!] ERROR: LONGITUDE parameter not valid, press any key to continue...";
                                            Console.SetCursorPosition((Console.WindowWidth - LongitudeNotValid.Length) / 2, Console.CursorTop);
                                            Console.Write(LongitudeNotValid);
                                            Console.ReadKey();
                                            continue;
                                        }
                                        auxLabel2.GeographicLocation = new double[] { latitude, longitude };
                                        break;
                                    }
                                    break;
                                case 1:
                                    Console.Clear();
                                    string AddAddressTitle = "~ Add Address to SpecialLabel ~\n\n";
                                    Console.SetCursorPosition((Console.WindowWidth - AddAddressTitle.Length) / 2, Console.CursorTop);
                                    Console.Write(AddAddressTitle);
                                    string AddAddressSelection = "Please, introduce the Address: ";
                                    Console.SetCursorPosition((Console.WindowWidth - AddAddressSelection.Length) / 2, Console.CursorTop);
                                    Console.Write(AddAddressSelection);
                                    auxLabel2.Address = Console.ReadLine();
                                    break;
                                case 2:
                                    Console.Clear();
                                    string AddPhotographerTitle = "~ Add Photographer to SpecialLabel ~\n\n";
                                    Console.SetCursorPosition((Console.WindowWidth - AddPhotographerTitle.Length) / 2, Console.CursorTop);
                                    Console.Write(AddPhotographerTitle);
                                    string AddPhotographerSelection = "Please, introduce the Photographer: ";
                                    Console.SetCursorPosition((Console.WindowWidth - AddPhotographerSelection.Length) / 2, Console.CursorTop);
                                    Console.Write(AddPhotographerSelection);
                                    auxLabel2.Photographer = Console.ReadLine();
                                    break;
                                case 3:
                                    Console.Clear();
                                    string AddPhotoMotiveTitle = "~ Add PhotoMotive to SpecialLabel ~\n\n";
                                    Console.SetCursorPosition((Console.WindowWidth - AddPhotoMotiveTitle.Length) / 2, Console.CursorTop);
                                    Console.Write(AddPhotoMotiveTitle);
                                    string AddPhotoMotiveSelection = "Please, introduce the PhotoMotive: ";
                                    Console.SetCursorPosition((Console.WindowWidth - AddPhotoMotiveSelection.Length) / 2, Console.CursorTop);
                                    Console.Write(AddPhotoMotiveSelection);
                                    auxLabel2.PhotoMotive = Console.ReadLine();
                                    break;
                                case 4:
                                    string AddSelfieTitle = "~ Add Selfie to SpecialLabel ~\n\n";
                                    string AddSelfieSelection = "Please, introduce the Selfie: ";
                                    int usrSelection2 = this.GenerateMenu(new List<string>() { "Is a selfie", "Is not a selfie" }, AddSelfieTitle, AddSelfieSelection);
                                    if (usrSelection2 == 0) auxLabel2.Selfie = true;
                                    else auxLabel2.Selfie = false;
                                    break;
                            }
                        }
                        imageToAddLabel.AddLabel(auxLabel2);
                        this.SaveLibrary();
                        break;


                    // User wants to exit
                    case 3:
                        this.SaveLibrary();
                        break;
                }
                break;
            }
        }


        private void ResetLibrary()
        {
            List<string> ResetLibraryTitle = this.LoadBannerData("resetlibrary.txt");
            string description = "Are you sure that you want to reset your library?: ";
            int usrDecision = this.GenerateMenu(new List<string>() { "Yes, reset My Library", "Exit" }, null, description, ResetLibraryTitle);
            if (usrDecision == 0)
            {
                this.library.ResetImages();
                this.SaveLibrary();
            }
            else return;

            string dots = "...\n";
            string success = "Successful library reset!";
            string presskey = "Please, press any key to continue...";

            System.Threading.Thread.Sleep(1000);
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - dots.Length) / 2, Console.CursorTop);
            Console.WriteLine(dots);

            System.Threading.Thread.Sleep(1000);
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - dots.Length) / 2, Console.CursorTop);
            Console.WriteLine(dots);

            System.Threading.Thread.Sleep(1000);
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - success.Length) / 2, Console.CursorTop);
            Console.WriteLine(success);

            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - presskey.Length) / 2, Console.CursorTop);
            Console.WriteLine(presskey);
            Console.ReadKey();
        }


        private void ExportFileToPath()
        {
            List<string> bannerTitle = this.LoadBannerData("export.txt");
            List<string> options = new List<string>();
            foreach (Image image in this.library.Images)
            {
                options.Add(image.Name);
            }
            options.Add("Exit");
            while (true)
            {
                int usrOption = this.GenerateMenu(options, null, "Please, select the file you want to export: ", bannerTitle);
                if (usrOption == options.Count - 1) return;
                Image usrWants = this.library.Images[usrOption];
                Console.Clear();
                foreach (string linestring in bannerTitle)
                {
                    Console.SetCursorPosition((Console.WindowWidth - linestring.Length) / 2, Console.CursorTop);
                    Console.WriteLine(linestring);
                }

                string introducePath = "Please, introduce the path to save the image: ";
                Console.WriteLine();
                Console.SetCursorPosition((Console.WindowWidth - introducePath.Length) / 2, Console.CursorTop);
                Console.Write(introducePath);
                string path = Console.ReadLine();

                string introduceName = "Please, introduce the name of the new file: ";
                Console.WriteLine();
                Console.SetCursorPosition((Console.WindowWidth - introduceName.Length) / 2, Console.CursorTop);
                Console.Write(introduceName);
                string filename = Console.ReadLine();

                string introduceFormat = "Please, select the exporting format: ";
                List<string> formats = new List<string>() { ".jpg", ".jpeg", ".bmp", ".png" };
                int usrIntFormat = this.GenerateMenu(formats, null, introduceFormat, bannerTitle);
                string selectedFormat = formats[usrIntFormat];

                Bitmap copyUsrWants = (Bitmap)usrWants.BitmapImage.Clone();
                try
                {
                    copyUsrWants.Save(path + filename + selectedFormat);
                    break;
                }
                catch
                {
                    string notValid = "Not valid path or filename";
                    string pressKey = "Press any key to continue...";
                    Console.SetCursorPosition((Console.WindowWidth - notValid.Length) / 2, Console.CursorTop);
                    Console.WriteLine(notValid);
                    Console.SetCursorPosition((Console.WindowWidth - pressKey.Length) / 2, Console.CursorTop);
                    Console.WriteLine(pressKey);
                    Console.ReadKey();
                }
            }
            return;
        }


        private void ManageSearch()
        {
            List<string> manageSearchTitle = this.LoadBannerData("searcher.txt");
            List<string> manageSearchOptions = new List<string>() { "Searcher", "Face Searcher", "Exit" };
            string manageLibraryDescription = "Please, select an option: ";
            while (true)
            {
                int usrDecision = this.GenerateMenu(manageSearchOptions, null, manageLibraryDescription, manageSearchTitle);
                if (usrDecision == 2) break;
                switch (usrDecision)
                {
                    // User wants to see his smart list => READY
                    case 0:
                        this.Search();
                        break;

                    // User wants to add do a facesearch => NOT IMPLEMENTED YET
                    case 1:
                        this.FaceSearch();
                        break;

                }
            }
        }


        private void Search()
        {
            Console.Clear();
            List<string> ShowSearchTitle = this.LoadBannerData("search.txt");
            string separator = "\n<<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>>";
            string presskeytocontinue = "Please, press any key to continue...";
            string emptylibraryerror = "[!] ERROR: Sorry, no image was found with that search pattern";
            Console.WriteLine("Please, write the declaration you want to search");
            string declaration = Console.ReadLine();
            foreach (string titlestring in ShowSearchTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            Searcher searcher= new Searcher();
            List<Image> images = searcher.Search(library.Images, declaration);
            if (images.Count == 0)
            {
                Console.SetCursorPosition((Console.WindowWidth - emptylibraryerror.Length) / 2, Console.CursorTop);
                Console.WriteLine(emptylibraryerror);
            }
            else
            {
                foreach (Image image in images)
                {
                    Console.WriteLine(separator);
                    string patternImage = $"    ~ Pattern: {declaration} - Name: {image.Name} - Calification{image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear} ~";
                    Console.WriteLine(patternImage);
                }
                Console.WriteLine(separator);
            }
            Console.WriteLine("\n");
            Console.SetCursorPosition((Console.WindowWidth - presskeytocontinue.Length) / 4, Console.CursorTop);
            Console.Write(presskeytocontinue);
            Console.ReadKey();

        }


        private void FaceSearch()
        {
            Console.Clear();
            string presskeytocontinue = "Please, press any key to continue...";
            string emptylibraryerror = "[!] ERROR: Sorry, not implemented yet";
            Console.SetCursorPosition((Console.WindowWidth - emptylibraryerror.Length) / 2, Console.CursorTop);
            Console.WriteLine(emptylibraryerror);
            Console.SetCursorPosition((Console.WindowWidth - presskeytocontinue.Length) / 2, Console.CursorTop);
            Console.WriteLine(presskeytocontinue);
            Console.ReadKey();

        }
   
        private void ShowSmartList()
        {
            //Show the title
            Console.Clear();
            library.UpdateSmartList(library.Images);
            string separator = "\n<<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>>";
            List<string> ShowSmarListTitle = this.LoadBannerData("mysmartList.txt");
            string presskeytocontinue = "Please, press any key to continue...";
            foreach (string titlestring in ShowSmarListTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }
            //Show smart list
            foreach (KeyValuePair<string,List<Image>> pattern in this.library.SmartList)
            {
                if (pattern.Value.Count == 0)
                {
                    Console.WriteLine(separator);
                    Console.WriteLine($"   There are  not images associated with the search pattern  ~ Pattern:{pattern.Key} - ");
                }
                else
                {
                    foreach (Image image in pattern.Value)
                    {

                        Console.WriteLine(separator);
                        Console.WriteLine($"    ~ Pattern:{pattern.Key} - Name: {image.Name} - Calification{image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear} ~");
                    }
                    Console.WriteLine(separator);
                }
            }
            Console.WriteLine("\n");
            Console.SetCursorPosition((Console.WindowWidth - presskeytocontinue.Length) / 4, Console.CursorTop);
            Console.Write(presskeytocontinue);
            Console.ReadKey();

        }


        private void AddSmartList()
        {
            Console.Clear();
            List<string> AddSmartListTitle = this.LoadBannerData("addsmartlist.txt");
            string presskeytocontinue = "Please, press any key to continue...";
            string emptylibraryerror = "[!] ERROR: Sorry, you already have that search pattern";
			foreach (string titlestring in AddSmartListTitle)
			{
				Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
				Console.WriteLine(titlestring);
			}
			Console.WriteLine("Please, insert the search pattern you want to add: ");
            string description1 = Console.ReadLine();

            
            if (this.library.SmartList.Count == 0)
            {
					library.AddSmartList(description1, library.Images);
					this.SaveLibrary();
					return;
            }
            else
            {
                foreach (KeyValuePair<string, List<Image>> pattern in  this.library.SmartList)

                {
                    if (pattern.Key == description1)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - emptylibraryerror.Length) / 2, Console.CursorTop);
                        Console.WriteLine(emptylibraryerror);
                        Console.SetCursorPosition((Console.WindowWidth - presskeytocontinue.Length) / 2, Console.CursorTop);
                        Console.WriteLine(presskeytocontinue);
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        library.AddSmartList(description1, library.Images);
                        this.SaveLibrary();
                        break;
                    }
                }
            }



        }


        private void RemoveSmartList()
        {
            Console.Clear();
            List<string> AddSmartListTitle = this.LoadBannerData("removesmartlist.txt");
            List<string> SearchPattern = new List<string>();
            string presskeytocontinue = "Please, press any key to continue...";
            string emptylibraryerror = "[!] ERROR: Sorry, the search pattern you selected was not found";
			foreach (string titlestring in AddSmartListTitle)
			{
				Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
				Console.WriteLine(titlestring);
			}
			Console.WriteLine("Please, insert the search pattern you want to add: ");

            string description1 = Console.ReadLine();
            foreach (KeyValuePair<string, List<Image>> pattern in this.library.SmartList)
            {
                SearchPattern.Add(pattern.Key);
            }
            if(SearchPattern.Contains(description1) == false)
            {
                Console.SetCursorPosition((Console.WindowWidth - emptylibraryerror.Length) / 2, Console.CursorTop);
                Console.WriteLine(emptylibraryerror);
                Console.SetCursorPosition((Console.WindowWidth - presskeytocontinue.Length) / 2, Console.CursorTop);
                Console.WriteLine(presskeytocontinue);
                Console.ReadKey();
                return;
            }
            else
            {
                library.RemoveSmartList(description1);
                this.SaveLibrary();
            }
        }


        private void ManageSmartList()
        {
            List<string> manageSmartListTitle = this.LoadBannerData("managesmartlist.txt");
            List<string> manageSmartListOptions = new List<string>() { "Show My SmartList", "Add SmartList", "Delete SmartList","Exit" };
            string manageLibraryDescription = "Please, select an option: ";
            while (true)
            {
                int usrDecision = this.GenerateMenu(manageSmartListOptions, null, manageLibraryDescription, manageSmartListTitle);
                if (usrDecision == 3) break;
                switch (usrDecision)
                {
                    // User wants to see his smart list => READY
                    case 0:
                        this.ShowSmartList();
                        break;

                    // User wants to add a search pattern => READY
                    case 1:
                        this.AddSmartList();
                        break;

                    // User wants to delete a search pattern => READY
                    case 2:
                        this.RemoveSmartList();
                        break;

                }
            }

        }


        private void ShowLibrary()
        {
            // Show the title
            Console.Clear();
            string separator = "\n<<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>>";
            List<string> ShowLibraryTitle = this.LoadBannerData("mylibrary.txt");
            string presskeytocontinue = "Please, press any key to continue...";
            foreach (string titlestring in ShowLibraryTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);

            }

            // Show all images and their labels
            foreach (Image image in this.library.Images)
            {
                Console.WriteLine(separator);
                string titlewithcal = $"          ~ Name: {image.Name} - Calification: {image.Calification} - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear} ~";
                string titlewithoutcal = $"          ~ Name: {image.Name} - Calification: Not set - Resolution: {image.Resolution[0]}x{image.Resolution[1]} - AspectRatio: {image.AspectRatio[0]}x{image.AspectRatio[1]} - Clear: {image.DarkClear} ~";
                if (image.Calification != -1)
                {
                    Console.SetCursorPosition((Console.WindowWidth - titlewithcal.Length) / 2, Console.CursorTop);
                    Console.WriteLine(titlewithcal);
                }
                else
                {
                    Console.SetCursorPosition((Console.WindowWidth - titlewithoutcal.Length) / 2, Console.CursorTop);
                    Console.WriteLine(titlewithoutcal);
                }
                foreach (Label label in image.Labels)
                {
                    Console.WriteLine("\n");
                    string labeltype = "=> " + label.labelType;
                    Console.SetCursorPosition((Console.WindowWidth - labeltype.Length) / 4, Console.CursorTop);
                    Console.WriteLine(labeltype);
                    switch (label.labelType)
                    {
                        case "SimpleLabel":
                            SimpleLabel slabel = (SimpleLabel)label;
                            string sentence = $"Sentence: {slabel.Sentence}";
                            Console.SetCursorPosition((Console.WindowWidth - sentence.Length) / 2, Console.CursorTop);
                            Console.WriteLine(sentence);
                            break;
                        case "PersonLabel":
                            PersonLabel plabel = (PersonLabel)label;
                            if (plabel.Name != null)
                            {
                                string name = $"Name: {plabel.Name}";
                                Console.SetCursorPosition((Console.WindowWidth - name.Length) / 2, Console.CursorTop);
                                Console.WriteLine(name);
                            }
                            if (plabel.Surname != null)
                            {
                                string surname = $"Surname: {plabel.Surname}";
                                Console.SetCursorPosition((Console.WindowWidth - surname.Length) / 2, Console.CursorTop);
                                Console.WriteLine(surname);
                            }
                            if (plabel.Nationality != ENationality.None)
                            {
                                string nationality = $"Nationality: {plabel.Nationality}";
                                Console.SetCursorPosition((Console.WindowWidth - nationality.Length) / 2, Console.CursorTop);
                                Console.WriteLine(nationality);
                            }
                            if (plabel.EyesColor != EColor.None)
                            {
                                string eyescolor = $"EyesColor: {plabel.EyesColor}";
                                Console.SetCursorPosition((Console.WindowWidth - eyescolor.Length) / 2, Console.CursorTop);
                                Console.WriteLine(eyescolor);
                            }
                            if (plabel.HairColor != EColor.None)
                            {
                                string haircolor = $"HairColor: {plabel.HairColor}";
                                Console.SetCursorPosition((Console.WindowWidth - haircolor.Length) / 2, Console.CursorTop);
                                Console.WriteLine(haircolor);
                            }
                            if (plabel.Sex != ESex.None)
                            {
                                string sex = $"Sex: {plabel.Sex}";
                                Console.SetCursorPosition((Console.WindowWidth - sex.Length) / 2, Console.CursorTop);
                                Console.WriteLine(sex);
                            }
                            if (plabel.BirthDate != "")
                            {
                                string brithdate = $"Birthdate: {plabel.BirthDate}";
                                Console.SetCursorPosition((Console.WindowWidth - brithdate.Length) / 2, Console.CursorTop);
                                Console.WriteLine(brithdate);
                            }
                            if (plabel.FaceLocation != null)
                            {

                                string facelocation = $"FaceLocation: {plabel.FaceLocation[0]}, {plabel.FaceLocation[1]}, {plabel.FaceLocation[2]}, {plabel.FaceLocation[3]}";
                                Console.SetCursorPosition((Console.WindowWidth - facelocation.Length) / 2, Console.CursorTop);
                                Console.WriteLine(facelocation);
                            }
                            break;
                        case "SpecialLabel":
                            SpecialLabel splabel = (SpecialLabel)label;
                            if (splabel.GeographicLocation != null)
                            {
                                string geolocation = $"GeographicLocation: {splabel.GeographicLocation[0]}, {splabel.GeographicLocation[1]}";
                                Console.SetCursorPosition((Console.WindowWidth - geolocation.Length) / 2, Console.CursorTop);
                                Console.WriteLine(geolocation);
                            }
                            if (splabel.Address != null)
                            {
                                string address = $"Address: {splabel.Address}";
                                Console.SetCursorPosition((Console.WindowWidth - address.Length) / 2, Console.CursorTop);
                                Console.WriteLine(address);
                            }
                            if (splabel.Photographer != null)
                            {
                                string photographer = $"Photographer: {splabel.Photographer}";
                                Console.SetCursorPosition((Console.WindowWidth - photographer.Length) / 2, Console.CursorTop);
                                Console.WriteLine(photographer);
                            }
                            if (splabel.PhotoMotive != null)
                            {
                                string photomotive = $"PhotoMotive: {splabel.PhotoMotive}";
                                Console.SetCursorPosition((Console.WindowWidth - photomotive.Length) / 2, Console.CursorTop);
                                Console.WriteLine(photomotive);
                            }
                            if (splabel.Selfie == true)
                            {
                                string isaselfie = "Selfie: It is a Selfie";
                                Console.SetCursorPosition((Console.WindowWidth - isaselfie.Length) / 2, Console.CursorTop);
                                Console.WriteLine(isaselfie);
                            }
                            else
                            {
                                string isnotaselfie = "Selfie: It is not a Selfie";
                                Console.SetCursorPosition((Console.WindowWidth - isnotaselfie.Length) / 2, Console.CursorTop);
                                Console.WriteLine(isnotaselfie);
                            }
                            break;
                    }
                }
                Console.WriteLine(separator);
            }
            Console.WriteLine("\n");
            Console.SetCursorPosition((Console.WindowWidth - presskeytocontinue.Length) / 4, Console.CursorTop);
            Console.Write(presskeytocontinue);
            Console.ReadKey();
        }


        private void ImportImageFromPath()
        {
            string positive = "All chosen files exists...";
            string negative = "[!] ERROR: Some of the chosen files doesn't exist, or the path is incorrect";
            string pressKey = "Press any key to continue...";
            string option1 = "Try importing again";
            string option2 = "Go back to Start Menu";

            while (true)
            {
                string[] userImportingSettings = this.ChoosePathAndImages();
                if (userImportingSettings[1] == "all")
                {
                    string[] jpgfiles = Directory.GetFiles(@userImportingSettings[0], "*.jpg");
                    string[] pngfiles = Directory.GetFiles(@userImportingSettings[0], "*.png");
                    string[] bmpfiles = Directory.GetFiles(@userImportingSettings[0], "*.bmp");
                    string[] jpegfiles = Directory.GetFiles(@userImportingSettings[0], "*.jpeg");
                    Console.WriteLine("\n");
                    int totalLength = jpgfiles.Length + pngfiles.Length + bmpfiles.Length + jpegfiles.Length;
                    List<string[]> allimages = new List<string[]>() { jpgfiles, pngfiles, bmpfiles, jpegfiles };
                    if (totalLength > 0)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - positive.Length) / 2, Console.CursorTop);
                        Console.WriteLine("The number of files is {0}.", totalLength);
                        Console.SetCursorPosition((Console.WindowWidth - positive.Length) / 2, Console.CursorTop);
                        Console.WriteLine(positive);
                        Console.WriteLine("\n");
                        Console.SetCursorPosition((Console.WindowWidth - pressKey.Length) / 2, Console.CursorTop);
                        Console.WriteLine(pressKey);
                        Console.ReadKey();

                        int usrDecision;
                        if (totalLength > 1)
                        {
                            usrDecision = this.GenerateMenu(new List<string>() { "Calificate or add labels individually", "Calificate or add labels to all images" }, "~ Importing options ~", "Please, choose an option: ");
                        }
                        else
                        {
                            usrDecision = 0;
                        }

                        if (usrDecision == 0)
                        {
                            foreach (string[] arrayOfFiles in allimages)
                            {
                                foreach (string file in arrayOfFiles)
                                {
                                    string path = Path.GetDirectoryName(file);
                                    path += @"\";
                                    string filename = Path.GetFileName(file);
                                    Image newImage = this.CreatingImageMenu(filename, path);
                                    this.library.AddImage(newImage);
                                    this.SaveLibrary();
                                }
                            }
                            break;
                        }
                        else
                        {
                            // Asumimos que no se pueden importar 5000 imagenes de un solo tiro
                            string[] allFiles = new string[5000];
                            int count = 0;

                            // First we add all the images to the same array
                            foreach (string[] arrayOfFiles in allimages)
                            {
                                foreach (string file in arrayOfFiles)
                                {
                                    allFiles[count] = Path.GetFileName(file);
                                    count++;
                                    if (count == 5000) throw new Exception("Too much images");
                                }
                            }
                            string path = Path.GetDirectoryName(allimages[0][0]);
                            path += @"\";
                            List<Image> allUserImages = this.CreatingImageMenu(allFiles, path);
                            foreach (Image imagen in allUserImages)
                            {
                                this.library.AddImage(imagen);
                                this.SaveLibrary();
                            }
                            break;
                        }

                    }
                    else
                    {
                        Console.SetCursorPosition((Console.WindowWidth - negative.Length) / 2, Console.CursorTop);
                        Console.WriteLine(negative);
                        Console.WriteLine("\n");
                        Console.SetCursorPosition((Console.WindowWidth - pressKey.Length) / 2, Console.CursorTop);
                        Console.WriteLine(pressKey);
                        Console.ReadKey();
                        List<string> auxList = new List<string>() { option1, option2 };
                        int userChoice = this.GenerateMenu(auxList, "~ [!] ERROR ~", "Please, select an option: ");
                    }
                }
                else
                {
                    string path = userImportingSettings[0];
                    string[] files = userImportingSettings[1].Split(',');
                    bool analisysResult = this.FilesExists(files, path);
                    Console.WriteLine("\n");
                    if (analisysResult == true)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - positive.Length) / 2, Console.CursorTop);
                        Console.WriteLine(positive);
                        Console.WriteLine("\n");
                        Console.SetCursorPosition((Console.WindowWidth - pressKey.Length) / 2, Console.CursorTop);
                        Console.WriteLine(pressKey);
                        Console.ReadKey();

                        int usrDecision;
                        if (files.Length > 1)
                        {
                            usrDecision = this.GenerateMenu(new List<string>() { "Calificate or add labels individually", "Calificate or add labels to all images" }, "~ Importing options ~", "Please, choose an option: ");
                        }
                        else
                        {
                            usrDecision = 0;
                        }

                        if (usrDecision == 0)
                        {
                            foreach (string file in files)
                            {
                                Image newImage = this.CreatingImageMenu(file, path);
                                this.library.AddImage(newImage);
                                this.SaveLibrary();
                            }
                            break;
                        }
                        else
                        {
                            // Asumimos que no se pueden importar 5000 imagenes de un solo tiro
                            List<Image> allUserImages = this.CreatingImageMenu(files, path);
                            foreach (Image imagen in allUserImages)
                            {
                                this.library.AddImage(imagen);
                                this.SaveLibrary();
                            }
                            break;
                        }

                    }
                    else
                    {
                        Console.SetCursorPosition((Console.WindowWidth - negative.Length) / 2, Console.CursorTop);
                        Console.WriteLine(negative);
                        Console.WriteLine("\n");
                        Console.SetCursorPosition((Console.WindowWidth - pressKey.Length) / 2, Console.CursorTop);
                        Console.WriteLine(pressKey);
                        Console.ReadKey();
                        List<string> auxList = new List<string>() { option1, option2 };
                        int userChoice = this.GenerateMenu(auxList, "~ [!] ERROR ~", "Please, select an option: ");
                        if (userChoice == 1) break;
                    }
                }
            }
        }


        // User interaction during the import files, and creates an image to add to the library, if user wants to add calification
        // and labels globally (to every image)
        private List<Image> CreatingImageMenu(string[] names, string path)
        {
            int finalCalification = -1;
            List<Label> imageLabels = new List<Label>();
            List<Image> returningListOfImages = new List<Image>();
            string title = "~ All images ~\n";
            string cal = "Calification: ";
            string lab = "Labels: ";
            string separator = "<<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>>\n";
            int selectedOption = 0;
            List<string> options = new List<string>() { "Set Calification", "Set new Label", "Continue" };

            List<string> newpaths = new List<string>();
            // Copy an object into files
            foreach (string name in names)
            {
                if (name != null)
                {
                    string newpath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\" + name;
                    System.IO.File.Copy(path + name, newpath);
                    newpaths.Add(newpath);
                }
            }


            while (true)
            {
                bool _continue = true;
                while (_continue == true)
                {
                    Console.Clear();
                    Console.WriteLine("\n");
                    Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, Console.CursorTop);
                    Console.WriteLine(title);
                    Console.SetCursorPosition((Console.WindowWidth - cal.Length) / 4, Console.CursorTop);
                    Console.Write(cal);
                    if (finalCalification == -1) Console.WriteLine("\t [Not set]\n");
                    else Console.WriteLine(finalCalification);
                    Console.WriteLine("\n");
                    Console.SetCursorPosition(((Console.WindowWidth - lab.Length) / 4) + 4, Console.CursorTop);
                    Console.Write(lab);

                    if (imageLabels.Count == 0) Console.WriteLine("\t [Not set]\n");
                    else
                    {
                        Console.WriteLine("\n");
                        foreach (Label label in imageLabels)
                        {
                            Console.WriteLine(separator);
                            Console.Write($"=> {label.labelType}\n");

                            if (label.labelType == "SimpleLabel")
                            {
                                SimpleLabel newlabel = (SimpleLabel)label;
                                string tag = $"Tag: {newlabel.Sentence}\n";
                                Console.SetCursorPosition((Console.WindowWidth - tag.Length) / 4, Console.CursorTop);
                                Console.Write(tag);
                                Console.WriteLine(separator);
                            }
                            else if (label.labelType == "PersonLabel")
                            {
                                PersonLabel newlabel = (PersonLabel)label;

                                if (newlabel.Name != null)
                                {
                                    string namelabel = $"Name: {newlabel.Name}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - namelabel.Length) / 4, Console.CursorTop);
                                    Console.Write(namelabel);
                                }
                                if (newlabel.FaceLocation != null)
                                {
                                    string locationlabel = $"Location:{newlabel.FaceLocation[3]},{newlabel.FaceLocation[2]},{newlabel.FaceLocation[0]},{newlabel.FaceLocation[1]}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - locationlabel.Length) / 4, Console.CursorTop);
                                    Console.Write(locationlabel);
                                }
                                if (newlabel.Surname != null)
                                {
                                    string surnamelabel = $"Surname: {newlabel.Surname}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - surnamelabel.Length) / 4, Console.CursorTop);
                                    Console.Write(surnamelabel);
                                }
                                if (newlabel.Nationality != ENationality.None)
                                {
                                    string nationalitylabel = $"Nationality: {newlabel.Nationality}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - nationalitylabel.Length) / 4, Console.CursorTop);
                                    Console.Write(nationalitylabel);
                                }
                                if (newlabel.EyesColor != EColor.None)
                                {
                                    string eyescolorlabel = $"EyesColor: {newlabel.EyesColor}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - eyescolorlabel.Length) / 4, Console.CursorTop);
                                    Console.Write(eyescolorlabel);
                                }
                                if (newlabel.HairColor != EColor.None)
                                {
                                    string haircolorlabel = $"HairColor: {newlabel.HairColor}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - haircolorlabel.Length) / 4, Console.CursorTop);
                                    Console.Write(haircolorlabel);
                                }
                                if (newlabel.Sex != ESex.None)
                                {
                                    string sexlabel = $"Sex: {newlabel.Sex}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - sexlabel.Length) / 4, Console.CursorTop);
                                    Console.Write(sexlabel);
                                }
                                if (newlabel.BirthDate != "")
                                {
                                    string birthdatelabel = $"Birthdate: {newlabel.BirthDate}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - birthdatelabel.Length) / 4, Console.CursorTop);
                                    Console.Write(birthdatelabel);
                                }
                                Console.WriteLine(separator);
                            }
                            else if (label.labelType == "SpecialLabel")
                            {
                                SpecialLabel newlabel = (SpecialLabel)label;
                                if (newlabel.GeographicLocation != null)
                                {
                                    string geolocation = $"GeoLocation: {newlabel.GeographicLocation[0]}, {newlabel.GeographicLocation[1]}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - geolocation.Length) / 4, Console.CursorTop);
                                    Console.Write(geolocation);
                                }
                                if (newlabel.Address != null)
                                {
                                    string address = $"Address: {newlabel.Address}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - address.Length) / 4, Console.CursorTop);
                                    Console.Write(address);
                                }

                                if (newlabel.Photographer != null)
                                {
                                    string photographer = $"Photographer: {newlabel.Photographer}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - photographer.Length) / 4, Console.CursorTop);
                                    Console.Write(photographer);
                                }

                                if (newlabel.PhotoMotive != null)
                                {
                                    string photomotive = $"PhotoMotive: {newlabel.PhotoMotive}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - photomotive.Length) / 4, Console.CursorTop);
                                    Console.Write(photomotive);
                                }

                                if (newlabel.Selfie != false)
                                {
                                    string isselfie = "Selfie: It is a Selfie\n";
                                    Console.SetCursorPosition((Console.WindowWidth - isselfie.Length) / 4, Console.CursorTop);
                                    Console.WriteLine(isselfie);
                                }
                                else
                                {
                                    string isnotselfie = "Selfie: It is not a Selfie\n";
                                    Console.SetCursorPosition((Console.WindowWidth - isnotselfie.Length) / 4, Console.CursorTop);
                                    Console.WriteLine(isnotselfie);
                                }
                                Console.WriteLine(separator);
                            }
                        }
                    }
                    int i = 1;
                    foreach (string option in options)
                    {
                        if (selectedOption == i - 1)
                        {
                            Console.WriteLine("\n");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{i}. {option}");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            i++;
                            Console.WriteLine("\n");
                            continue;
                        }
                        Console.WriteLine($"{i}. {option}");
                        i++;
                    }
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedOption -= 1;
                            if (selectedOption < 0)
                            {
                                selectedOption = 0;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            selectedOption += 1;
                            if (selectedOption > options.Count - 1)
                            {
                                selectedOption = options.Count - 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            _continue = false;
                            break;
                    }
                }

                // If the user wants to set a new calification
                if (selectedOption == 0)
                {
                    while (true)
                    {
                        Console.Clear();
                        string setCalificationTitle = "~ Set Calification ~\n";
                        string introduceCalification = "Introduce the new Calification <1-5> (-1 to exit): ";
                        Console.SetCursorPosition((Console.WindowWidth - setCalificationTitle.Length) / 2, Console.CursorTop);
                        Console.WriteLine(setCalificationTitle);
                        Console.SetCursorPosition((Console.WindowWidth - introduceCalification.Length) / 2, Console.CursorTop);
                        Console.Write(introduceCalification);
                        string snewCalification = Console.ReadLine();
                        try
                        {
                            int newCalification = Convert.ToInt32(snewCalification);
                            if (newCalification != -1 && (newCalification < 1 || newCalification > 5)) throw new Exception("Not valid Calification");
                            finalCalification = newCalification;
                            break;
                        }
                        catch
                        {
                            string notValid = "Not valid Calification";
                            string pressKey = "Press any key to continue...";
                            Console.SetCursorPosition((Console.WindowWidth - notValid.Length) / 2, Console.CursorTop);
                            Console.WriteLine(notValid);
                            Console.SetCursorPosition((Console.WindowWidth - pressKey.Length) / 2, Console.CursorTop);
                            Console.WriteLine(pressKey);
                            Console.ReadKey();
                        }
                    }
                }


                // If the user wants to set a new label
                if (selectedOption == 1)
                {
                    while (true)
                    {
                        Console.Clear();
                        string setCalificationTitle = "~ Set new Label ~\n";
                        string introduceCalification = "Please, select the Label type you want to create: ";
                        string SimpleLabelCreation = "~ SimpleLabel Creation ~\n";
                        string PersonLabelCreation = "~ PersonLabel Creation ~\n";
                        string PersonLabelSelection = "Please, select the attribute you want to add: ";
                        string SpecialLabelCreation = "~ SpecialLabel Creation ~\n";
                        string SpecialLabelSelection = "Please, select the attribute you want to add: ";
                        string SimpleLabelSelection = "Please, introduce the tag for this new SimpleLabel: ";
                        string donthavewatsonrecommendations = "In this case we don't have Watson's recommendations";
                        int selectedOption1 = this.GenerateMenu(new List<string>() { "SimpleLabel", "PersonLabel", "SpecialLabel", "Exit" }, setCalificationTitle, introduceCalification);



                        // If user wants to exit the label selection menu
                        if (selectedOption1 == 3) break;




                        // If user wants to add a SimpleLabel
                        if (selectedOption1 == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("\n");
                            List<string> watsonOptions = new List<string>();

                            watsonOptions.Add("Personalized Label");
                            this.GenerateMenu(watsonOptions, SimpleLabelCreation, donthavewatsonrecommendations);


                            Console.Clear();
                            Console.SetCursorPosition((Console.WindowWidth - SimpleLabelCreation.Length) / 2, Console.CursorTop);
                            Console.WriteLine(SimpleLabelCreation);
                            Console.SetCursorPosition((Console.WindowWidth - SimpleLabelSelection.Length) / 2, Console.CursorTop);
                            Console.Write(SimpleLabelSelection);
                            string userTag = Console.ReadLine();
                            SimpleLabel auxLabel = new SimpleLabel(userTag);
                            imageLabels.Add(auxLabel);
                        }





                        // If user wants to add a PersonLabel
                        if (selectedOption1 == 1)
                        {
                            int userSelection;
                            PersonLabel auxLabel = new PersonLabel();

                            while (true)
                            {
                                string settedFaceLocation;
                                if (auxLabel.FaceLocation != null) settedFaceLocation = "\t " + Convert.ToString(auxLabel.FaceLocation[0]) + "," + Convert.ToString(auxLabel.FaceLocation[1]) + "," + Convert.ToString(auxLabel.FaceLocation[2]) + "," + Convert.ToString(auxLabel.FaceLocation[3]);
                                else settedFaceLocation = "\t [Not set]";
                                string settedNationality;
                                if (auxLabel.Nationality != ENationality.None) settedNationality = "\t " + Enum.GetName(typeof(ENationality), auxLabel.Nationality);
                                else settedNationality = "\t [Not set]";
                                string settedEyesColor;
                                if (auxLabel.EyesColor != EColor.None) settedEyesColor = "\t\t " + Enum.GetName(typeof(EColor), auxLabel.EyesColor);
                                else settedEyesColor = "\t\t [Not set]";
                                string settedHairColor;
                                if (auxLabel.HairColor != EColor.None) settedHairColor = "\t\t " + Enum.GetName(typeof(EColor), auxLabel.HairColor);
                                else settedHairColor = "\t\t [Not set]";
                                string settedSex;
                                if (auxLabel.Sex != ESex.None) settedSex = "\t\t " + Enum.GetName(typeof(ESex), auxLabel.Sex);
                                else settedSex = "\t\t [Not set]";
                                string settedName;
                                if (auxLabel.Name == null) settedName = "\t\t [Not set]";
                                else settedName = "\t\t " + auxLabel.Name;
                                string settedSurname;
                                if (auxLabel.Surname == null) settedSurname = "\t\t [Not set]";
                                else settedSurname = "\t\t " + auxLabel.Surname;
                                string settedBirthDate;
                                if (auxLabel.BirthDate == "") settedBirthDate = "\t\t [Not set]";
                                else settedBirthDate = "\t\t " + auxLabel.BirthDate;


                                Console.Clear();
                                List<string> personOptions = new List<string>() { "Name: " + settedName, "Surname: " + settedSurname, "FaceLocation: " + settedFaceLocation, "Nationality: " + settedNationality, "EyesColor: " + settedEyesColor, "HairColor: " + settedHairColor, "Sex: " + settedSex, "Birthdate: " + settedBirthDate, "Done" };
                                userSelection = this.GenerateMenu(personOptions, PersonLabelCreation, PersonLabelSelection);
                                if (userSelection == 8) break;
                                switch (userSelection)
                                {
                                    case 0:
                                        Console.Clear();
                                        string AddNameTitle = "~ Add Name to PersonLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddNameTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddNameTitle);
                                        string AddNameSelection = "Please, introduce the name: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddNameSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddNameSelection);
                                        auxLabel.Name = Console.ReadLine();
                                        break;
                                    case 1:
                                        Console.Clear();
                                        string AddSurnameTitle = "~ Add Surname to PersonLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddSurnameTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddSurnameTitle);
                                        string AddSurnameSelection = "Please, introduce the surname: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddSurnameSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddSurnameSelection);
                                        auxLabel.Surname = Console.ReadLine();
                                        break;
                                    case 2:
                                        double left, top, width, height;
                                        while (true)
                                        {
                                            Console.Clear();

                                            string AddFaceLocation = "~ Add FaceLocation to PersonLabel ~\n\n";
                                            Console.SetCursorPosition((Console.WindowWidth - AddFaceLocation.Length) / 2, Console.CursorTop);
                                            Console.Write(AddFaceLocation);

                                            string AddLeftSelection = "Please, introduce the LEFT parameter: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddLeftSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddLeftSelection);
                                            try
                                            {
                                                left = Convert.ToDouble(Console.ReadLine());
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string LeftNotValid = "[!] ERROR: LEFT parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - LeftNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(LeftNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }

                                            string AddTopSelection = "Please, introduce the TOP parameter: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddTopSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddTopSelection);
                                            try
                                            {
                                                top = Convert.ToDouble(Console.ReadLine());
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string TopNotValid = "[!] ERROR: TOP parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - TopNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(TopNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }

                                            string AddHeightSelection = "Please, introduce the HEIGHT parameter: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddHeightSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddHeightSelection);
                                            try
                                            {
                                                height = Convert.ToDouble(Console.ReadLine());
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string HeightNotValid = "[!] ERROR: HEIGHT parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - HeightNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(HeightNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }

                                            string AddWidthSelection = "Please, introduce the WIDTH parameter: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddWidthSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddWidthSelection);
                                            try
                                            {
                                                width = Convert.ToDouble(Console.ReadLine());
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string WidthNotValid = "[!] ERROR: WIDTH parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - WidthNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(WidthNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                            auxLabel.FaceLocation = new double[] { width, height, top, left };
                                            break;
                                        }
                                        break;
                                    case 3:
                                        ENationality auxnationality;
                                        while (true)
                                        {
                                            Console.Clear();
                                            string AddNationalityTitle = "~ Add Nationality to PersonLabel ~\n\n";
                                            string AddNationalitySelection = "Please, introduce the nationality: ";

                                            Console.SetCursorPosition((Console.WindowWidth - AddNationalityTitle.Length) / 2, Console.CursorTop);
                                            Console.Write(AddNationalityTitle);

                                            Console.SetCursorPosition((Console.WindowWidth - AddNationalitySelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddNationalitySelection);

                                            string option = Console.ReadLine();

                                            try
                                            {
                                                auxnationality = (ENationality)Enum.Parse(typeof(ENationality), option);
                                                break;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string NationalityNotValid = "[!] ERROR: Nationality parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - NationalityNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(NationalityNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                        }
                                        auxLabel.Nationality = auxnationality;
                                        break;
                                    case 4:
                                        EColor EyesColor;
                                        while (true)
                                        {
                                            Console.Clear();
                                            string AddEyesColorTitle = "~ Add EyesColor to PersonLabel ~\n\n";
                                            string AddEyesColorSelection = "Please, introduce the EyesColor: ";

                                            Console.SetCursorPosition((Console.WindowWidth - AddEyesColorTitle.Length) / 2, Console.CursorTop);
                                            Console.Write(AddEyesColorTitle);

                                            Console.SetCursorPosition((Console.WindowWidth - AddEyesColorSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddEyesColorSelection);

                                            string option = Console.ReadLine();

                                            try
                                            {
                                                EyesColor = (EColor)Enum.Parse(typeof(EColor), option);
                                                break;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string ColorNotValid = "[!] ERROR: Color parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - ColorNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(ColorNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                        }
                                        auxLabel.EyesColor = EyesColor;
                                        break;
                                    case 5:
                                        EColor HairColor;
                                        while (true)
                                        {
                                            Console.Clear();
                                            string AddHairColorTitle = "~ Add HairColor to PersonLabel ~\n\n";
                                            string AddHairColorSelection = "Please, introduce the HairColor: ";

                                            Console.SetCursorPosition((Console.WindowWidth - AddHairColorTitle.Length) / 2, Console.CursorTop);
                                            Console.Write(AddHairColorTitle);

                                            Console.SetCursorPosition((Console.WindowWidth - AddHairColorSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddHairColorSelection);

                                            string option = Console.ReadLine();

                                            try
                                            {
                                                HairColor = (EColor)Enum.Parse(typeof(EColor), option);
                                                break;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string ColorNotValid = "[!] ERROR: Color parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - ColorNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(ColorNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                        }
                                        auxLabel.HairColor = HairColor;
                                        break;
                                    case 6:
                                        ESex Sex;
                                        while (true)
                                        {
                                            Console.Clear();
                                            string AddSexTitle = "~ Add Sex to PersonLabel ~\n\n";
                                            string AddSexSelection = "Please, introduce the Sex <Hombre-Mujer>: ";

                                            Console.SetCursorPosition((Console.WindowWidth - AddSexTitle.Length) / 2, Console.CursorTop);
                                            Console.Write(AddSexTitle);

                                            Console.SetCursorPosition((Console.WindowWidth - AddSexSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddSexSelection);

                                            string option = Console.ReadLine();

                                            try
                                            {
                                                Sex = (ESex)Enum.Parse(typeof(ESex), option);
                                                break;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string SexNotValid = "[!] ERROR: Sex parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - SexNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(SexNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                        }
                                        auxLabel.Sex = Sex;
                                        break;
                                    case 7:
                                        int day, month, year;
                                        while (true)
                                        {
                                            Console.Clear();

                                            string AddBirthdate = "~ Add Birthdate to PersonLabel ~\n\n";
                                            Console.SetCursorPosition((Console.WindowWidth - AddBirthdate.Length) / 2, Console.CursorTop);
                                            Console.Write(AddBirthdate);

                                            string AddDaySelection = "Please, introduce the DAY parameter <1-31>: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddDaySelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddDaySelection);
                                            try
                                            {
                                                day = Convert.ToInt32(Console.ReadLine());
                                                if (day < 1 || day > 31)
                                                {
                                                    throw new Exception("DAY parameter not valid");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string DayNotValid = "[!] ERROR: DAY parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - DayNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(DayNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }

                                            string AddMonthSelection = "Please, introduce the MONTH parameter <1-12>: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddMonthSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddMonthSelection);
                                            try
                                            {
                                                month = Convert.ToInt32(Console.ReadLine());
                                                if (month < 1 || month > 12)
                                                {
                                                    throw new Exception("MONTH parameter not valid");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string MonthNotValid = "[!] ERROR: MONTH parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - MonthNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(MonthNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }

                                            string AddYearSelection = "Please, introduce the YEAR parameter <1850-2019>: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddYearSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddYearSelection);
                                            try
                                            {
                                                year = Convert.ToInt32(Console.ReadLine());
                                                if (year < 1850 || year > 2019)
                                                {
                                                    throw new Exception("YEAR parameter not valid");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string YearNotValid = "[!] ERROR: YEAR parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - YearNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(YearNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }


                                            auxLabel.BirthDate = Convert.ToString(day) + "-" + Convert.ToString(month) + "-" + Convert.ToString(year);
                                            break;
                                        }
                                        break;
                                }
                            }
                            imageLabels.Add(auxLabel);
                            if (userSelection == 8) break;
                        }




                        // If user wants to add a SpecialLabel
                        if (selectedOption1 == 2)
                        {
                            int userSelection;
                            SpecialLabel auxLabel = new SpecialLabel();
                            while (true)
                            {
                                string settedGeographicLocation;
                                if (auxLabel.GeographicLocation != null) settedGeographicLocation = "\t " + Convert.ToString(auxLabel.GeographicLocation[0]) + "," + Convert.ToString(auxLabel.GeographicLocation[1]);
                                else settedGeographicLocation = "\t [Not set]";
                                string settedAddress;
                                if (auxLabel.Address != null) settedAddress = "\t\t " + auxLabel.Address;
                                else settedAddress = "\t\t [Not set]";

                                string settedPhotographer;
                                if (auxLabel.Photographer != null) settedPhotographer = "\t\t " + auxLabel.Photographer;
                                else settedPhotographer = "\t [Not set]";

                                string settedPhotoMotive;
                                if (auxLabel.PhotoMotive != null) settedPhotoMotive = "\t\t " + auxLabel.PhotoMotive;
                                else settedPhotoMotive = "\t [Not set]";

                                string settedSelfie;
                                if (auxLabel.Selfie != false) settedSelfie = "\t\t It is a Selfie";
                                else settedSelfie = "\t\t It is not a Selfie";

                                Console.Clear();
                                List<string> personOptions = new List<string>() { "GeographicLocation: " + settedGeographicLocation, "Address: " + settedAddress, "Photographer: " + settedPhotographer, "PhotoMotive: " + settedPhotoMotive, "Selfie: " + settedSelfie, "Done" };
                                userSelection = this.GenerateMenu(personOptions, SpecialLabelCreation, SpecialLabelSelection);
                                if (userSelection == 5) break;
                                switch (userSelection)
                                {
                                    case 0:
                                        int latitude, longitude;
                                        while (true)
                                        {
                                            Console.Clear();

                                            string AddGeographicLocation = "~ Add GeographicLocation to SpecialLabel ~\n\n";
                                            Console.SetCursorPosition((Console.WindowWidth - AddGeographicLocation.Length) / 2, Console.CursorTop);
                                            Console.Write(AddGeographicLocation);

                                            string AddLatitudeSelection = "Please, introduce the LATITUDE parameter <-90, 90>: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddLatitudeSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddLatitudeSelection);
                                            try
                                            {
                                                latitude = Convert.ToInt32(Console.ReadLine());
                                                if (latitude < -90 || latitude > 90)
                                                {
                                                    throw new Exception("LATITUDE parameter not valid");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string LatitudeNotValid = "[!] ERROR: LATITUDE parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - LatitudeNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(LatitudeNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }


                                            string AddLongitudeSelection = "Please, introduce the LONGITUDE parameter <-180, 180>: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddLongitudeSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddLongitudeSelection);
                                            try
                                            {
                                                longitude = Convert.ToInt32(Console.ReadLine());
                                                if (longitude < -180 || longitude > 180)
                                                {
                                                    throw new Exception("LONGITUDE parameter not valid");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string LongitudeNotValid = "[!] ERROR: LONGITUDE parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - LongitudeNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(LongitudeNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                            auxLabel.GeographicLocation = new double[] { latitude, longitude };
                                            break;
                                        }
                                        break;
                                    case 1:
                                        Console.Clear();
                                        string AddAddressTitle = "~ Add Address to SpecialLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddAddressTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddAddressTitle);
                                        string AddAddressSelection = "Please, introduce the Address: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddAddressSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddAddressSelection);
                                        auxLabel.Address = Console.ReadLine();
                                        break;
                                    case 2:
                                        Console.Clear();
                                        string AddPhotographerTitle = "~ Add Photographer to SpecialLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddPhotographerTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddPhotographerTitle);
                                        string AddPhotographerSelection = "Please, introduce the Photographer: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddPhotographerSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddPhotographerSelection);
                                        auxLabel.Photographer = Console.ReadLine();
                                        break;
                                    case 3:
                                        Console.Clear();
                                        string AddPhotoMotiveTitle = "~ Add PhotoMotive to SpecialLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddPhotoMotiveTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddPhotoMotiveTitle);
                                        string AddPhotoMotiveSelection = "Please, introduce the PhotoMotive: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddPhotoMotiveSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddPhotoMotiveSelection);
                                        auxLabel.PhotoMotive = Console.ReadLine();
                                        break;
                                    case 4:
                                        string AddSelfieTitle = "~ Add Selfie to SpecialLabel ~\n\n";
                                        string AddSelfieSelection = "Please, introduce the Selfie: ";
                                        int usrSelection2 = this.GenerateMenu(new List<string>() { "Is a selfie", "Is not a selfie" }, AddSelfieTitle, AddSelfieSelection);
                                        if (usrSelection2 == 0) auxLabel.Selfie = true;
                                        else auxLabel.Selfie = false;
                                        break;
                                }
                            }
                            imageLabels.Add(auxLabel);
                            if (userSelection == 5) break;
                        }
                    }
                }


                // If the user wants to exit
                if (selectedOption == 2) break;
            }

            int j = 0;
            foreach (string name in names)
            {
                int auxint = finalCalification;
                List<Label> auxLabels = new List<Label>();
                // We do a deep copy of the labels
                foreach (Label label in imageLabels)
                {
                    switch (label.labelType)
                    {
                        case "SimpleLabel":
                            SimpleLabel labelaux1 = (SimpleLabel)label;
                            auxLabels.Add(new SimpleLabel(labelaux1.Sentence, labelaux1.SerialNumber));
                            break;
                        case "PersonLabel":
                            PersonLabel labelaux2 = (PersonLabel)label;
                            auxLabels.Add(new PersonLabel(labelaux2.Name, labelaux2.FaceLocation, labelaux2.Surname, labelaux2.Nationality,
                                labelaux2.EyesColor, labelaux2.HairColor, labelaux2.Sex, labelaux2.BirthDate, labelaux2.SerialNumber));
                            break;
                        case "SpecialLabel":
                            SpecialLabel labelaux3 = (SpecialLabel)label;
                            auxLabels.Add(new SpecialLabel(labelaux3.GeographicLocation, labelaux3.Address, labelaux3.Photographer,
                                labelaux3.PhotoMotive, labelaux3.Selfie, labelaux3.SerialNumber));
                            break;
                    }
                }

                if (name == null) break;
                Image auxImage = new Image(newpaths[j], auxLabels, auxint);
                auxImage.Name = name;
                returningListOfImages.Add(auxImage);
                j++;
            }
            return returningListOfImages;
        }


        // User interaction during the import files, and creates an image to add to the library
        private Image CreatingImageMenu(string name, string path)
        {
            int calification = -1;
            List<Label> imageLabels = new List<Label>();
            string title = "~ " + name + " ~\n";
            string cal = "Calification: ";
            string lab = "Labels: ";
            string separator = "<<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>>\n";
            int selectedOption = 0;
            List<string> options = new List<string>() { "Set Calification", "Set new Label", "Continue" };

            // Copy an object into files
            string newpath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\" + name;
            System.IO.File.Copy(path + name, newpath);

            while (true)
            {
                bool _continue = true;
                while (_continue == true)
                {
                    Console.Clear();
                    Console.WriteLine("\n");
                    Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, Console.CursorTop);
                    Console.WriteLine(title);
                    Console.SetCursorPosition((Console.WindowWidth - cal.Length) / 4, Console.CursorTop);
                    Console.Write(cal);
                    if (calification == -1) Console.WriteLine("\t [Not set]\n");
                    else Console.WriteLine(calification);
                    Console.WriteLine("\n");
                    Console.SetCursorPosition(((Console.WindowWidth - lab.Length) / 4) + 4, Console.CursorTop);
                    Console.Write(lab);

                    if (imageLabels.Count == 0) Console.WriteLine("\t [Not set]\n");
                    else
                    {
                        Console.WriteLine("\n");
                        foreach (Label label in imageLabels)
                        {
                            Console.WriteLine(separator);
                            Console.Write($"=> {label.labelType}\n");

                            if (label.labelType == "SimpleLabel")
                            {
                                SimpleLabel newlabel = (SimpleLabel)label;
                                string tag = $"Tag: {newlabel.Sentence}\n";
                                Console.SetCursorPosition((Console.WindowWidth - tag.Length) / 4, Console.CursorTop);
                                Console.Write(tag);
                                Console.WriteLine(separator);
                            }
                            else if (label.labelType == "PersonLabel")
                            {
                                PersonLabel newlabel = (PersonLabel)label;

                                if (newlabel.Name != null)
                                {
                                    string namelabel = $"Name: {newlabel.Name}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - namelabel.Length) / 4, Console.CursorTop);
                                    Console.Write(namelabel);
                                }
                                if (newlabel.FaceLocation != null)
                                {
                                    string locationlabel = $"Location:{newlabel.FaceLocation[3]},{newlabel.FaceLocation[2]},{newlabel.FaceLocation[0]},{newlabel.FaceLocation[1]}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - locationlabel.Length) / 4, Console.CursorTop);
                                    Console.Write(locationlabel);
                                }
                                if (newlabel.Surname != null)
                                {
                                    string surnamelabel = $"Surname: {newlabel.Surname}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - surnamelabel.Length) / 4, Console.CursorTop);
                                    Console.Write(surnamelabel);
                                }
                                if (newlabel.Nationality != ENationality.None)
                                {
                                    string nationalitylabel = $"Nationality: {newlabel.Nationality}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - nationalitylabel.Length) / 4, Console.CursorTop);
                                    Console.Write(nationalitylabel);
                                }
                                if (newlabel.EyesColor != EColor.None)
                                {
                                    string eyescolorlabel = $"EyesColor: {newlabel.EyesColor}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - eyescolorlabel.Length) / 4, Console.CursorTop);
                                    Console.Write(eyescolorlabel);
                                }
                                if (newlabel.HairColor != EColor.None)
                                {
                                    string haircolorlabel = $"HairColor: {newlabel.HairColor}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - haircolorlabel.Length) / 4, Console.CursorTop);
                                    Console.Write(haircolorlabel);
                                }
                                if (newlabel.Sex != ESex.None)
                                {
                                    string sexlabel = $"Sex: {newlabel.Sex}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - sexlabel.Length) / 4, Console.CursorTop);
                                    Console.Write(sexlabel);
                                }
                                if (newlabel.BirthDate != "")
                                {
                                    string birthdatelabel = $"Birthdate: {newlabel.BirthDate}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - birthdatelabel.Length) / 4, Console.CursorTop);
                                    Console.Write(birthdatelabel);
                                }
                                Console.WriteLine(separator);
                            }
                            else if (label.labelType == "SpecialLabel")
                            {
                                SpecialLabel newlabel = (SpecialLabel)label;
                                if (newlabel.GeographicLocation != null)
                                {
                                    string geolocation = $"GeoLocation: {newlabel.GeographicLocation[0]}, {newlabel.GeographicLocation[1]}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - geolocation.Length) / 4, Console.CursorTop);
                                    Console.Write(geolocation);
                                }
                                if (newlabel.Address != null)
                                {
                                    string address = $"Address: {newlabel.Address}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - address.Length) / 4, Console.CursorTop);
                                    Console.Write(address);
                                }

                                if (newlabel.Photographer != null)
                                {
                                    string photographer = $"Photographer: {newlabel.Photographer}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - photographer.Length) / 4, Console.CursorTop);
                                    Console.Write(photographer);
                                }

                                if (newlabel.PhotoMotive != null)
                                {
                                    string photomotive = $"PhotoMotive: {newlabel.PhotoMotive}\n";
                                    Console.SetCursorPosition((Console.WindowWidth - photomotive.Length) / 4, Console.CursorTop);
                                    Console.Write(photomotive);
                                }

                                if (newlabel.Selfie != false)
                                {
                                    string isselfie = "Selfie: It is a Selfie\n";
                                    Console.SetCursorPosition((Console.WindowWidth - isselfie.Length) / 4, Console.CursorTop);
                                    Console.WriteLine(isselfie);
                                }
                                else
                                {
                                    string isnotselfie = "Selfie: It is not a Selfie\n";
                                    Console.SetCursorPosition((Console.WindowWidth - isnotselfie.Length) / 4, Console.CursorTop);
                                    Console.WriteLine(isnotselfie);
                                }
                                Console.WriteLine(separator);
                            }
                        }
                    }
                    int i = 1;
                    foreach (string option in options)
                    {
                        if (selectedOption == i - 1)
                        {
                            Console.WriteLine("\n");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{i}. {option}");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            i++;
                            Console.WriteLine("\n");
                            continue;
                        }
                        Console.WriteLine($"{i}. {option}");
                        i++;
                    }
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedOption -= 1;
                            if (selectedOption < 0)
                            {
                                selectedOption = 0;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            selectedOption += 1;
                            if (selectedOption > options.Count - 1)
                            {
                                selectedOption = options.Count - 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            _continue = false;
                            break;
                    }
                }


                // If the user wants to set a new calification
                if (selectedOption == 0)
                {
                    while (true)
                    {
                        Console.Clear();
                        string setCalificationTitle = "~ Set Calification ~\n";
                        string introduceCalification = "Introduce the new Calification <1-5> (-1 to exit): ";
                        Console.SetCursorPosition((Console.WindowWidth - setCalificationTitle.Length) / 2, Console.CursorTop);
                        Console.WriteLine(setCalificationTitle);
                        Console.SetCursorPosition((Console.WindowWidth - introduceCalification.Length) / 2, Console.CursorTop);
                        Console.Write(introduceCalification);
                        string snewCalification = Console.ReadLine();
                        try
                        {
                            int newCalification = Convert.ToInt32(snewCalification);
                            if (newCalification != -1 && (newCalification < 1 || newCalification > 5)) throw new Exception("Not valid Calification");
                            calification = newCalification;
                            break;
                        }
                        catch
                        {
                            string notValid = "Not valid Calification";
                            string pressKey = "Press any key to continue...";
                            Console.SetCursorPosition((Console.WindowWidth - notValid.Length) / 2, Console.CursorTop);
                            Console.WriteLine(notValid);
                            Console.SetCursorPosition((Console.WindowWidth - pressKey.Length) / 2, Console.CursorTop);
                            Console.WriteLine(pressKey);
                            Console.ReadKey();
                        }
                    }
                }


                // If the user wants to set a new label
                if (selectedOption == 1)
                {
                    while (true)
                    {
                        Console.Clear();
                        string setCalificationTitle = "~ Set new Label ~\n";
                        string introduceCalification = "Please, select the Label type you want to create: ";
                        string SimpleLabelCreation = "~ SimpleLabel Creation ~\n";
                        string PersonLabelCreation = "~ PersonLabel Creation ~\n";
                        string PersonLabelSelection = "Please, select the attribute you want to add: ";
                        string SpecialLabelCreation = "~ SpecialLabel Creation ~\n";
                        string SpecialLabelSelection = "Please, select the attribute you want to add: ";
                        string SimpleLabelSelection = "Please, introduce the tag for this new SimpleLabel: ";
                        string SelectOption = "Down you can see Watson recommended labels. Choose your option: ";
                        int selectedOption1 = this.GenerateMenu(new List<string>() { "SimpleLabel", "PersonLabel", "SpecialLabel", "Exit" }, setCalificationTitle, introduceCalification);



                        // If user wants to exit the label selection menu
                        if (selectedOption1 == 3) break;




                        // If user wants to add a SimpleLabel
                        if (selectedOption1 == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("\n");
                            string LoadingWatson = "Loading Watson recommendations...";
                            Console.SetCursorPosition((Console.WindowWidth - LoadingWatson.Length) / 2, Console.CursorTop);
                            Console.Write(LoadingWatson);
                            Dictionary<int, Dictionary<string, double>> watsonResults = this.producer.ClassifyImage(path + name);
                            List<string> watsonOptions = new List<string>();
                            foreach (Dictionary<string, double> dic in watsonResults.Values)
                            {
                                foreach (KeyValuePair<string, double> pair in dic)
                                {
                                    watsonOptions.Add(pair.Key);
                                }
                            }
                            watsonOptions.Add("Personalized Label");
                            int selectedOption2 = this.GenerateMenu(watsonOptions, SimpleLabelCreation, SelectOption);
                            string selected = watsonOptions[selectedOption2];
                            if (selected != "Personalized Label")
                            {
                                SimpleLabel auxLabel = new SimpleLabel(selected);
                                imageLabels.Add(auxLabel);
                            }
                            else
                            {
                                Console.Clear();
                                Console.SetCursorPosition((Console.WindowWidth - SimpleLabelCreation.Length) / 2, Console.CursorTop);
                                Console.WriteLine(SimpleLabelCreation);
                                Console.SetCursorPosition((Console.WindowWidth - SimpleLabelSelection.Length) / 2, Console.CursorTop);
                                Console.Write(SimpleLabelSelection);
                                string userTag = Console.ReadLine();
                                SimpleLabel auxLabel = new SimpleLabel(userTag);
                                imageLabels.Add(auxLabel);
                            }
                        }





                        // If user wants to add a PersonLabel
                        if (selectedOption1 == 1)
                        {
                            int userSelection;
                            PersonLabel auxLabel = new PersonLabel();

                            while (true)
                            {
                                string settedFaceLocation;
                                if (auxLabel.FaceLocation != null) settedFaceLocation = "\t " + Convert.ToString(auxLabel.FaceLocation[0]) + "," + Convert.ToString(auxLabel.FaceLocation[1]) + "," + Convert.ToString(auxLabel.FaceLocation[2]) + "," + Convert.ToString(auxLabel.FaceLocation[3]);
                                else settedFaceLocation = "\t [Not set]";
                                string settedNationality;
                                if (auxLabel.Nationality != ENationality.None) settedNationality = "\t " + Enum.GetName(typeof(ENationality), auxLabel.Nationality);
                                else settedNationality = "\t [Not set]";
                                string settedEyesColor;
                                if (auxLabel.EyesColor != EColor.None) settedEyesColor = "\t\t " + Enum.GetName(typeof(EColor), auxLabel.EyesColor);
                                else settedEyesColor = "\t\t [Not set]";
                                string settedHairColor;
                                if (auxLabel.HairColor != EColor.None) settedHairColor = "\t\t " + Enum.GetName(typeof(EColor), auxLabel.HairColor);
                                else settedHairColor = "\t\t [Not set]";
                                string settedSex;
                                if (auxLabel.Sex != ESex.None) settedSex = "\t\t " + Enum.GetName(typeof(EColor), auxLabel.Sex);
                                else settedSex = "\t\t [Not set]";
                                string settedName;
                                if (auxLabel.Name == null) settedName = "\t\t [Not set]";
                                else settedName = "\t\t " + auxLabel.Name;
                                string settedSurname;
                                if (auxLabel.Surname == null) settedSurname = "\t\t [Not set]";
                                else settedSurname = "\t\t " + auxLabel.Surname;
                                string settedBirthDate;
                                if (auxLabel.BirthDate == "") settedBirthDate = "\t\t [Not set]";
                                else settedBirthDate = "\t\t " + auxLabel.BirthDate;


                                Console.Clear();
                                List<string> personOptions = new List<string>() { "Name: " + settedName, "Surname: " + settedSurname, "FaceLocation: " + settedFaceLocation, "Nationality: " + settedNationality, "EyesColor: " + settedEyesColor, "HairColor: " + settedHairColor, "Sex: " + settedSex, "Birthdate: " + settedBirthDate, "Done" };
                                userSelection = this.GenerateMenu(personOptions, PersonLabelCreation, PersonLabelSelection);
                                if (userSelection == 8) break;
                                switch (userSelection)
                                {
                                    case 0:
                                        Console.Clear();
                                        string AddNameTitle = "~ Add Name to PersonLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddNameTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddNameTitle);
                                        string AddNameSelection = "Please, introduce the name: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddNameSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddNameSelection);
                                        auxLabel.Name = Console.ReadLine();
                                        break;
                                    case 1:
                                        Console.Clear();
                                        string AddSurnameTitle = "~ Add Surname to PersonLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddSurnameTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddSurnameTitle);
                                        string AddSurnameSelection = "Please, introduce the surname: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddSurnameSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddSurnameSelection);
                                        auxLabel.Surname = Console.ReadLine();
                                        break;
                                    case 2:
                                        double left, top, width, height;
                                        while (true)
                                        {
                                            Console.Clear();

                                            string AddFaceLocation = "~ Add FaceLocation to PersonLabel ~\n\n";
                                            Console.SetCursorPosition((Console.WindowWidth - AddFaceLocation.Length) / 2, Console.CursorTop);
                                            Console.Write(AddFaceLocation);

                                            string AddLeftSelection = "Please, introduce the LEFT parameter: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddLeftSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddLeftSelection);
                                            try
                                            {
                                                left = Convert.ToDouble(Console.ReadLine());
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string LeftNotValid = "[!] ERROR: LEFT parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - LeftNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(LeftNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }

                                            string AddTopSelection = "Please, introduce the TOP parameter: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddTopSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddTopSelection);
                                            try
                                            {
                                                top = Convert.ToDouble(Console.ReadLine());
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string TopNotValid = "[!] ERROR: TOP parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - TopNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(TopNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }

                                            string AddHeightSelection = "Please, introduce the HEIGHT parameter: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddHeightSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddHeightSelection);
                                            try
                                            {
                                                height = Convert.ToDouble(Console.ReadLine());
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string HeightNotValid = "[!] ERROR: HEIGHT parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - HeightNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(HeightNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }

                                            string AddWidthSelection = "Please, introduce the WIDTH parameter: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddWidthSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddWidthSelection);
                                            try
                                            {
                                                width = Convert.ToDouble(Console.ReadLine());
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string WidthNotValid = "[!] ERROR: WIDTH parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - WidthNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(WidthNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                            auxLabel.FaceLocation = new double[] { width, height, top, left };
                                            break;
                                        }
                                        break;
                                    case 3:
                                        ENationality auxnationality;
                                        while (true)
                                        {
                                            Console.Clear();
                                            string AddNationalityTitle = "~ Add Nationality to PersonLabel ~\n\n";
                                            string AddNationalitySelection = "Please, introduce the nationality: ";

                                            Console.SetCursorPosition((Console.WindowWidth - AddNationalityTitle.Length) / 2, Console.CursorTop);
                                            Console.Write(AddNationalityTitle);

                                            Console.SetCursorPosition((Console.WindowWidth - AddNationalitySelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddNationalitySelection);

                                            string option = Console.ReadLine();

                                            try
                                            {
                                                auxnationality = (ENationality)Enum.Parse(typeof(ENationality), option);
                                                break;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string NationalityNotValid = "[!] ERROR: Nationality parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - NationalityNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(NationalityNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                        }
                                        auxLabel.Nationality = auxnationality;
                                        break;
                                    case 4:
                                        EColor EyesColor;
                                        while (true)
                                        {
                                            Console.Clear();
                                            string AddEyesColorTitle = "~ Add EyesColor to PersonLabel ~\n\n";
                                            string AddEyesColorSelection = "Please, introduce the EyesColor: ";

                                            Console.SetCursorPosition((Console.WindowWidth - AddEyesColorTitle.Length) / 2, Console.CursorTop);
                                            Console.Write(AddEyesColorTitle);

                                            Console.SetCursorPosition((Console.WindowWidth - AddEyesColorSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddEyesColorSelection);

                                            string option = Console.ReadLine();

                                            try
                                            {
                                                EyesColor = (EColor)Enum.Parse(typeof(EColor), option);
                                                break;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string ColorNotValid = "[!] ERROR: Color parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - ColorNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(ColorNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                        }
                                        auxLabel.EyesColor = EyesColor;
                                        break;
                                    case 5:
                                        EColor HairColor;
                                        while (true)
                                        {
                                            Console.Clear();
                                            string AddHairColorTitle = "~ Add HairColor to PersonLabel ~\n\n";
                                            string AddHairColorSelection = "Please, introduce the HairColor: ";

                                            Console.SetCursorPosition((Console.WindowWidth - AddHairColorTitle.Length) / 2, Console.CursorTop);
                                            Console.Write(AddHairColorTitle);

                                            Console.SetCursorPosition((Console.WindowWidth - AddHairColorSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddHairColorSelection);

                                            string option = Console.ReadLine();

                                            try
                                            {
                                                HairColor = (EColor)Enum.Parse(typeof(EColor), option);
                                                break;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string ColorNotValid = "[!] ERROR: Color parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - ColorNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(ColorNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                        }
                                        auxLabel.HairColor = HairColor;
                                        break;
                                    case 6:
                                        ESex Sex;
                                        while (true)
                                        {
                                            Console.Clear();
                                            string AddSexTitle = "~ Add Sex to PersonLabel ~\n\n";
                                            string AddSexSelection = "Please, introduce the Sex <Hombre-Mujer>: ";

                                            Console.SetCursorPosition((Console.WindowWidth - AddSexTitle.Length) / 2, Console.CursorTop);
                                            Console.Write(AddSexTitle);

                                            Console.SetCursorPosition((Console.WindowWidth - AddSexSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddSexSelection);

                                            string option = Console.ReadLine();

                                            try
                                            {
                                                Sex = (ESex)Enum.Parse(typeof(ESex), option);
                                                break;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string SexNotValid = "[!] ERROR: Sex parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - SexNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(SexNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                        }
                                        auxLabel.Sex = Sex;
                                        break;
                                    case 7:
                                        int day, month, year;
                                        while (true)
                                        {
                                            Console.Clear();

                                            string AddBirthdate = "~ Add Birthdate to PersonLabel ~\n\n";
                                            Console.SetCursorPosition((Console.WindowWidth - AddBirthdate.Length) / 2, Console.CursorTop);
                                            Console.Write(AddBirthdate);

                                            string AddDaySelection = "Please, introduce the DAY parameter <1-31>: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddDaySelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddDaySelection);
                                            try
                                            {
                                                day = Convert.ToInt32(Console.ReadLine());
                                                if (day < 1 || day > 31)
                                                {
                                                    throw new Exception("DAY parameter not valid");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string DayNotValid = "[!] ERROR: DAY parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - DayNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(DayNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }

                                            string AddMonthSelection = "Please, introduce the MONTH parameter <1-12>: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddMonthSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddMonthSelection);
                                            try
                                            {
                                                month = Convert.ToInt32(Console.ReadLine());
                                                if (month < 1 || month > 12)
                                                {
                                                    throw new Exception("MONTH parameter not valid");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string MonthNotValid = "[!] ERROR: MONTH parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - MonthNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(MonthNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }

                                            string AddYearSelection = "Please, introduce the YEAR parameter <1850-2019>: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddYearSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddYearSelection);
                                            try
                                            {
                                                year = Convert.ToInt32(Console.ReadLine());
                                                if (year < 1850 || year > 2019)
                                                {
                                                    throw new Exception("YEAR parameter not valid");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string YearNotValid = "[!] ERROR: YEAR parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - YearNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(YearNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }


                                            auxLabel.BirthDate = Convert.ToString(day) + "-" + Convert.ToString(month) + "-" + Convert.ToString(year);
                                            break;
                                        }
                                        break;
                                }
                            }
                            imageLabels.Add(auxLabel);
                            if (userSelection == 8) break;
                        }




                        // If user wants to add a SpecialLabel
                        if (selectedOption1 == 2)
                        {
                            int userSelection;
                            SpecialLabel auxLabel = new SpecialLabel();
                            while (true)
                            {
                                string settedGeographicLocation;
                                if (auxLabel.GeographicLocation != null) settedGeographicLocation = "\t " + Convert.ToString(auxLabel.GeographicLocation[0]) + "," + Convert.ToString(auxLabel.GeographicLocation[1]);
                                else settedGeographicLocation = "\t [Not set]";
                                string settedAddress;
                                if (auxLabel.Address != null) settedAddress = "\t\t " + auxLabel.Address;
                                else settedAddress = "\t\t [Not set]";

                                string settedPhotographer;
                                if (auxLabel.Photographer != null) settedPhotographer = "\t\t " + auxLabel.Photographer;
                                else settedPhotographer = "\t [Not set]";

                                string settedPhotoMotive;
                                if (auxLabel.PhotoMotive != null) settedPhotoMotive = "\t\t " + auxLabel.PhotoMotive;
                                else settedPhotoMotive = "\t [Not set]";

                                string settedSelfie;
                                if (auxLabel.Selfie != false) settedSelfie = "\t\t It is a Selfie";
                                else settedSelfie = "\t\t It is not a Selfie";

                                Console.Clear();
                                List<string> personOptions = new List<string>() { "GeographicLocation: " + settedGeographicLocation, "Address: " + settedAddress, "Photographer: " + settedPhotographer, "PhotoMotive: " + settedPhotoMotive, "Selfie: " + settedSelfie, "Done" };
                                userSelection = this.GenerateMenu(personOptions, SpecialLabelCreation, SpecialLabelSelection);
                                if (userSelection == 5) break;
                                switch (userSelection)
                                {
                                    case 0:
                                        int latitude, longitude;
                                        while (true)
                                        {
                                            Console.Clear();

                                            string AddGeographicLocation = "~ Add GeographicLocation to SpecialLabel ~\n\n";
                                            Console.SetCursorPosition((Console.WindowWidth - AddGeographicLocation.Length) / 2, Console.CursorTop);
                                            Console.Write(AddGeographicLocation);

                                            string AddLatitudeSelection = "Please, introduce the LATITUDE parameter <-90, 90>: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddLatitudeSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddLatitudeSelection);
                                            try
                                            {
                                                latitude = Convert.ToInt32(Console.ReadLine());
                                                if (latitude < -90 || latitude > 90)
                                                {
                                                    throw new Exception("LATITUDE parameter not valid");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string LatitudeNotValid = "[!] ERROR: LATITUDE parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - LatitudeNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(LatitudeNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }


                                            string AddLongitudeSelection = "Please, introduce the LONGITUDE parameter <-180, 180>: ";
                                            Console.SetCursorPosition((Console.WindowWidth - AddLongitudeSelection.Length) / 2, Console.CursorTop);
                                            Console.Write(AddLongitudeSelection);
                                            try
                                            {
                                                longitude = Convert.ToInt32(Console.ReadLine());
                                                if (longitude < -180 || longitude > 180)
                                                {
                                                    throw new Exception("LONGITUDE parameter not valid");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("\n");
                                                string LongitudeNotValid = "[!] ERROR: LONGITUDE parameter not valid, press any key to continue...";
                                                Console.SetCursorPosition((Console.WindowWidth - LongitudeNotValid.Length) / 2, Console.CursorTop);
                                                Console.Write(LongitudeNotValid);
                                                Console.ReadKey();
                                                continue;
                                            }
                                            auxLabel.GeographicLocation = new double[] { latitude, longitude };
                                            break;
                                        }
                                        break;
                                    case 1:
                                        Console.Clear();
                                        string AddAddressTitle = "~ Add Address to SpecialLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddAddressTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddAddressTitle);
                                        string AddAddressSelection = "Please, introduce the Address: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddAddressSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddAddressSelection);
                                        auxLabel.Address = Console.ReadLine();
                                        break;
                                    case 2:
                                        Console.Clear();
                                        string AddPhotographerTitle = "~ Add Photographer to SpecialLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddPhotographerTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddPhotographerTitle);
                                        string AddPhotographerSelection = "Please, introduce the Photographer: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddPhotographerSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddPhotographerSelection);
                                        auxLabel.Photographer = Console.ReadLine();
                                        break;
                                    case 3:
                                        Console.Clear();
                                        string AddPhotoMotiveTitle = "~ Add PhotoMotive to SpecialLabel ~\n\n";
                                        Console.SetCursorPosition((Console.WindowWidth - AddPhotoMotiveTitle.Length) / 2, Console.CursorTop);
                                        Console.Write(AddPhotoMotiveTitle);
                                        string AddPhotoMotiveSelection = "Please, introduce the PhotoMotive: ";
                                        Console.SetCursorPosition((Console.WindowWidth - AddPhotoMotiveSelection.Length) / 2, Console.CursorTop);
                                        Console.Write(AddPhotoMotiveSelection);
                                        auxLabel.PhotoMotive = Console.ReadLine();
                                        break;
                                    case 4:
                                        string AddSelfieTitle = "~ Add Selfie to SpecialLabel ~\n\n";
                                        string AddSelfieSelection = "Please, introduce the Selfie: ";
                                        int usrSelection2 = this.GenerateMenu(new List<string>() { "Is a selfie", "Is not a selfie" }, AddSelfieTitle, AddSelfieSelection);
                                        if (usrSelection2 == 0) auxLabel.Selfie = true;
                                        else auxLabel.Selfie = false;
                                        break;
                                }
                            }
                            imageLabels.Add(auxLabel);
                            if (userSelection == 5) break;
                        }
                    }
                }


                // If the user wants to exit
                if (selectedOption == 2) break;
            }
            Image returningImage = new Image(newpath, imageLabels, calification);
            returningImage.Name = name;
            return returningImage;
        }


        // Verify if the files given by the user exists
        private bool FilesExists(string[] files, string path)
        {
            foreach (string file in files)
            {
                string completePath = @path + file;
                if (!File.Exists(completePath))
                {
                    return false;
                }
            }
            return true;
        }


        // Interaction with the user in the import file from path
        private string[] ChoosePathAndImages()
        {
            Console.Clear();
            List<string> bannerTitle = this.LoadBannerData("import.txt");

            string description = "Please, introduce the path to the files you want to import: ";
            Console.WriteLine("\n");
            foreach (string bannerstring in bannerTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - bannerstring.Length) / 2, Console.CursorTop);
                Console.WriteLine(bannerstring);
            }
            Console.WriteLine("\n");
            Console.SetCursorPosition((Console.WindowWidth - description.Length) / 2, Console.CursorTop);
            Console.WriteLine(description + "\n");
            Console.SetCursorPosition((Console.WindowWidth - description.Length) / 2, Console.CursorTop);
            Console.Write("-> ");
            string userPath = Console.ReadLine();
            Console.WriteLine("\n");
            Console.SetCursorPosition((Console.WindowWidth - description.Length) / 2, Console.CursorTop);
            string description2 = "Please, introduce the name(s) of the file(s) you want to import, separated by comma: ";
            Console.SetCursorPosition((Console.WindowWidth - description.Length) / 2, Console.CursorTop);
            Console.WriteLine(description2);
            Console.WriteLine("\n");
            Console.SetCursorPosition((Console.WindowWidth - description.Length) / 2, Console.CursorTop);
            Console.Write("-> ");
            string filenames = Console.ReadLine();
            string[] returningArray = new string[] { userPath, filenames };
            return returningArray;
        }


        // Manager that verifies if the library exists and loads it or create a new one
        private void LoadingLibraryManager()
        {
            bool exists = this.ExistsLibrary();
            if (exists == true)
            {
                this.LoadLibrary();
            }
            else
            {
                this.ShowLibraryDoesntExistError();
                this.library = new Library();
            }
        }


        // Manager that verifies if the producer exists and loads it or create a new one
        private void LoadingProducerManager()
        {
            bool exists = this.ExistsProducer();
            if (exists == true)
            {
                this.LoadProducer();
            }
            else
            {
                this.ShowProducerDoesntExistError();
                this.producer = new Producer();
                this.producer.LoadWatsonAnalyzer();
            }
        }


        // Saves the library into library.bin
        private void SaveLibrary()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(this.DEFAULT_LIBRARY_PATH, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this.library);
            stream.Close();
        }


        // Saves the producer into producer.bin
        private void SaveProducer()
        {
            this.producer.DeleteWatsonAnalyzer();
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(this.DEFAULT_PRODUCER_PATH, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this.producer);
            stream.Close();
            this.producer.LoadWatsonAnalyzer();
        }


        // Loads the library from library.bin
        private void LoadLibrary()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(this.DEFAULT_LIBRARY_PATH, FileMode.Open, FileAccess.Read, FileShare.None);
            Library library = (Library)formatter.Deserialize(stream);
            stream.Close();
            this.library = library;
        }


        // Loads the producer from producer.bin
        private void LoadProducer()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(this.DEFAULT_PRODUCER_PATH, FileMode.Open, FileAccess.Read, FileShare.None);
            Producer producer = (Producer)formatter.Deserialize(stream);
            stream.Close();
            this.producer = producer;
            this.producer.LoadWatsonAnalyzer();
        }


        // Returns true if the library.bin fil exists, and false in the other case
        private bool ExistsLibrary()
        {
            if (File.Exists(this.DEFAULT_LIBRARY_PATH)) return true;
            else return false;
        }


        // Returns true if the producer.bin file exists, and false in the other case
        private bool ExistsProducer()
        {
            if (File.Exists(this.DEFAULT_PRODUCER_PATH)) return true;
            else return false;
        }


        // Shows an error in case the producer.bin file doesnt exist
        private void ShowProducerDoesntExistError()
        {
            Console.SetWindowSize(213, 50);
            Console.Clear();
            Console.WriteLine("\n[!] CAUTION: The program didn't find the producer.bin file");
            Console.WriteLine("[!]          If you added images to the Editing Area, they are gone");
            Console.WriteLine("\n\nIf this is the first time you run this program, please ignore this advice\n\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        // Shows an error in case the library.bin file doesnt exist
        private void ShowLibraryDoesntExistError()
        {
            Console.SetWindowSize(213, 50);
            Console.Clear();
            Console.WriteLine("\n[!] CAUTION: The program didn't find the library.bin file");
            Console.WriteLine("[!]          If you added images to My Library, they are gone");
            Console.WriteLine("\n\nIf this is the first time you run this program, please ignore this advice\n\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        // Shows presentation message
        private void ShowPresentation()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetWindowSize(100, 30);
            List<string> presentationStrings = this.LoadBannerData("presentation.txt");
            foreach (string presentationString in presentationStrings)
            {
                Console.SetCursorPosition((Console.WindowWidth - presentationString.Length) / 2, Console.CursorTop);
                Console.WriteLine(presentationString);
            }
            Console.ReadKey();
        }


        // Shows goodbye message
        private void ShowGoodbye()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetWindowSize(70, 27);
            List<string> presentationStrings = this.LoadBannerData("goodbye.txt");
            foreach (string presentationString in presentationStrings)
            {
                Console.SetCursorPosition((Console.WindowWidth - presentationString.Length) / 2, Console.CursorTop);
                Console.WriteLine(presentationString);
            }
            System.Threading.Thread.Sleep(2000);
        }


        // Generate a menu, returns the int selected by the user, starting at 0
        private int GenerateMenu(List<string> options, string title = null, string description = null, List<string> bannerTitle = null)
        {
            int totalOptions = options.Count;
            if (totalOptions < 1) return -1;
            int selectedOption = 0;
            bool _continue = true;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            if (bannerTitle == null)
            {
                while (_continue == true)
                {
                    if (title != null && description != null) Console.Clear();
                    int i = 1;
                    if (title != null)
                    {
                        Console.WriteLine("\n");
                        Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, Console.CursorTop);
                        Console.WriteLine(title);
                    }
                    if (description != null)
                    {
                        Console.WriteLine("\n");
                        Console.SetCursorPosition((Console.WindowWidth - description.Length) / 2, Console.CursorTop);
                        Console.WriteLine(description + "\n");
                    }
                    foreach (string option in options)
                    {
                        if (selectedOption == i - 1)
                        {
                            Console.WriteLine("\n");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{i}. {option}");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            i++;
                            Console.WriteLine("\n");
                            continue;
                        }
                        Console.WriteLine($"{i}. {option}");
                        i++;
                    }
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedOption -= 1;
                            if (selectedOption < 0)
                            {
                                selectedOption = 0;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            selectedOption += 1;
                            if (selectedOption > options.Count - 1)
                            {
                                selectedOption = options.Count - 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            _continue = false;
                            break;
                    }
                }
            }
            else
            {
                while (_continue == true)
                {
                    if (bannerTitle != null && description != null) Console.Clear();
                    int i = 1;
                    if (bannerTitle != null)
                    {
                        foreach (string linestring in bannerTitle)
                        {
                            Console.SetCursorPosition((Console.WindowWidth - linestring.Length) / 2, Console.CursorTop);
                            Console.WriteLine(linestring);
                        }
                    }
                    if (description != null)
                    {
                        Console.WriteLine("\n");
                        Console.SetCursorPosition((Console.WindowWidth - description.Length) / 2, Console.CursorTop);
                        Console.WriteLine(description + "\n");
                    }
                    foreach (string option in options)
                    {
                        if (selectedOption == i - 1)
                        {
                            Console.WriteLine("\n");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{i}. {option}");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            i++;
                            Console.WriteLine("\n");
                            continue;
                        }
                        Console.WriteLine($"{i}. {option}");
                        i++;
                    }
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedOption -= 1;
                            if (selectedOption < 0)
                            {
                                selectedOption = 0;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            selectedOption += 1;
                            if (selectedOption > options.Count - 1)
                            {
                                selectedOption = options.Count - 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            _continue = false;
                            break;
                    }
                }
            }
            return selectedOption;
        }


        // Shows to the user all the options, and returns the selected one, starting at 0
        private int StartingMenu()
        {
            Console.SetWindowSize(213, 50);
            Console.Clear();
            List<string> startingStrings = this.LoadBannerData("startmenu.txt");
            int retorno = this.GenerateMenu(new List<string>() { "Import to My Library", "Export from My Library", "Editing Area", "Manage Library", "Search in My Library", "Manage Smart Lists", "Exit" }, null, "", startingStrings);
            return retorno;
        }
        

        // Methods that returns the banner data, given the filename
        private List<string> LoadBannerData(string filename)
        {
            List<string> returningList = new List<string>();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\banners\" + filename;
            StreamReader file = new StreamReader(path);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                returningList.Add(line);
            }
            file.Close();
            return returningList;
        }

    }
}
