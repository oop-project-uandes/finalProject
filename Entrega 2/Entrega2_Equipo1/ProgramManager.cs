using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace Entrega2_Equipo1
{
    public class ProgramManager
    {
        private Library library;
        private readonly string DEFAULT_LIBRARY_PATH = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\library.bin";
        private bool _continue = true;
        private int startingOption;
        






        public void Run()
        {
            ShowPresentation();
            LoadingLibraryManager();
            while (this._continue == true)
            {
                this.startingOption = StartingMenu();
                switch (this.startingOption)
                {
                    // Import a file to My Library from a path defined by the user
                    case 0:
                        ImportImageFromPath();
                        break;
                    // Export a file from My Library to a path defined by the user
                    case 1:
                        break;
                    // Editing Area (Apply filters, features, watson, slideshares, etc)
                    case 2:
                        break;
                    // Show library, add elements or erase elements, add or remove labels
                    case 3:
                        break;
                    // Search in the library
                    case 4:
                        break;
                    // Show smart lists, add smartlists or erase elements
                    case 5:
                        break;
                    case 6:
                        this._continue = false;
                        break;
                }
            }
            SaveLibrary();
            ShowGoodbye();
            return;
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
                string[] userImportingSettings = ChoosePathAndImages();
                string path = userImportingSettings[0];
                string[] files = userImportingSettings[1].Split(',');
                bool analisysResult = FilesExists(files, path);

                Console.WriteLine("\n");
                if (analisysResult == true)
                {
                    Console.SetCursorPosition((Console.WindowWidth - positive.Length) / 2, Console.CursorTop);
                    Console.WriteLine(positive);
                    Console.WriteLine("\n");
                    Console.SetCursorPosition((Console.WindowWidth - pressKey.Length) / 2, Console.CursorTop);
                    Console.WriteLine(pressKey);
                    Console.ReadKey();
                    List <Image> internalList = new List<Image>();
                    foreach (string file in files)
                    {
                        // Aqui quede. La idea es hacer algo tipo internalList.Add(CreateImage(file, path));
                        // y luego todas esas images agregarlas a la biblioteca, y estaria listo este metodo
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
                    int userChoice = GenerateMenu(auxList, "~ [!] ERROR ~", "Please, select an option: ");
                    if (userChoice == 1) break;
                }
            }
            return;
        }



        
        // User interaction during the import files, and creates an image to add to the library
        private Image CreatingImageMenu(string name, string path)
        {
            int calification = -1;
            List<Label> imageLabels = new List<Label>();

            Console.Clear();
            Console.WriteLine("\n~ " + name + " ~\n");

            Console.Write("Calification: ");
            if (calification == -1) Console.WriteLine("Not set");
            else Console.WriteLine(calification);

            Console.Write("Labels: ");
            if (imageLabels.Count == 0) Console.WriteLine("Not set");
            else
            {
                foreach (Label label in imageLabels)
                {
                    Console.WriteLine($"\n{label.labelType}");
                    if (label.labelType == "SimpleLabel")
                    {
                        SimpleLabel newlabel = (SimpleLabel)label;
                        Console.WriteLine($"\nTag: {newlabel.Sentence}");
                    }
                    else if (label.labelType == "PersonLabel")
                    {
                        PersonLabel newlabel = (PersonLabel)label;
                        Console.WriteLine($"\nName: {newlabel.Name}");
                        Console.WriteLine($"\nFaceLocation: LEFT = {newlabel.FaceLocation[0]}, TOP = {newlabel.FaceLocation[1]}, WIDTH = {newlabel.FaceLocation[2]}, HEIGHT = {newlabel.FaceLocation[3]}");
                        if (newlabel.Surname != null) Console.WriteLine($"\nSurname: {newlabel.Surname}");
                        if (newlabel.Nationality != ENationality.None) Console.WriteLine($"\nNationality: {newlabel.Nationality}");
                        if (newlabel.EyesColor != EColor.None) Console.WriteLine($"\nEyesColor: {newlabel.EyesColor}");
                        if (newlabel.HairColor != EColor.None) Console.WriteLine($"\nHairColor: {newlabel.HairColor}");
                        if (newlabel.Sex != ESex.None) Console.WriteLine($"\nSex: {newlabel.Sex}");
                        if (newlabel.BirthDate != "") Console.WriteLine($"\nSex: {newlabel.BirthDate}");
                    }
                    else if (label.labelType == "SpecialLabel")
                    {
                        SpecialLabel newlabel = (SpecialLabel)label;
                        if (newlabel.GeographicLocation != null) Console.WriteLine($"\nGeoLocation: {newlabel.GeographicLocation[0]}, {newlabel.GeographicLocation[1]}");
                        if (newlabel.Address != null) Console.WriteLine($"\nAddress: {newlabel.Address}");
                        if (newlabel.Photographer != null) Console.WriteLine($"\nPhotographer: {newlabel.Photographer}");
                        if (newlabel.PhotoMotive != null) Console.WriteLine($"\nPhotoMotive: {newlabel.PhotoMotive}");
                        if (newlabel.Selfie != false) Console.WriteLine($"\nSelfie: No");
                        else Console.WriteLine($"\nSelfie: Yes");
                    }
                }
            }

            int selectedOption = CreateMenu(...)
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
            string title = "~ IMPORT FROM PATH ~";
            string description = "Please, introduce the path to the files you want to import: ";
            Console.WriteLine("\n");
            Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, Console.CursorTop);
            Console.WriteLine(title + "\n");
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
            bool exists = ExistsLibrary();
            if (exists == true)
            {
                LoadLibrary();
            }
            else
            {
                ShowLibraryDoesntExistError();
                this.library = new Library();
            }
        }


        // Saves the library into library.bin
        private void SaveLibrary()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(DEFAULT_LIBRARY_PATH, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this.library);
            stream.Close();
        }


        // Loads the library from library.bin
        private void LoadLibrary()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(DEFAULT_LIBRARY_PATH, FileMode.Open, FileAccess.Read, FileShare.None);
            Library library = (Library)formatter.Deserialize(stream);
            stream.Close();
            this.library = library;
        }


        // Returns true if the library.bin fil exists, and false in the other case
        private bool ExistsLibrary()
        {
            if (File.Exists(DEFAULT_LIBRARY_PATH)) return true;
            else return false;
        }


        // Shows an error in case the library.bin file doesnt exist
        private void ShowLibraryDoesntExistError()
        {
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
            Console.BackgroundColor = ConsoleColor.DarkBlue ;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetWindowSize(115, 28);
            List<string> presentationStrings = new List<string>();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\presentation.txt";
            StreamReader file = new StreamReader(path);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                presentationStrings.Add(line);
            }
            foreach (string presentationString in presentationStrings)
            {
                Console.SetCursorPosition((Console.WindowWidth - presentationString.Length) / 2, Console.CursorTop);
                Console.WriteLine(presentationString);
            }
            file.Close();
            Console.ReadKey();
        }


        // Shows goodbye message
        private void ShowGoodbye()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetWindowSize(70, 27);
            List<string> presentationStrings = new List<string>();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\goodbye.txt";
            StreamReader file = new StreamReader(path);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                presentationStrings.Add(line);
            }
            foreach (string presentationString in presentationStrings)
            {
                Console.SetCursorPosition((Console.WindowWidth - presentationString.Length) / 2, Console.CursorTop);
                Console.WriteLine(presentationString);
            }
            file.Close();
            System.Threading.Thread.Sleep(2000);
        }


        // Generate a menu, returns the int selected by the user, starting at 0
        private int GenerateMenu(List<string> options, string title = null, string description = null)
        {
            int totalOptions = options.Count;
            if (totalOptions < 1) return -1;
            int selectedOption = 0;
            bool _continue = true;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            while (_continue == true)
            {
                Console.Clear();
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
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($"{i}. {option}");
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                        i++;
                        continue;
                    }
                    Console.WriteLine($"{i}. {option}");
                    i++;
                }
                ConsoleKey key = Console.ReadKey(true).Key;
                switch(key)
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
                        if (selectedOption > options.Count-1)
                        {
                            selectedOption = options.Count-1;
                        }
                        break;
                    case ConsoleKey.Enter:
                        _continue = false;
                        break;
                }
            }
            return selectedOption;
        }


        // Shows to the user all the options, and returns the selected one, starting at 0
        private int StartingMenu()
        {
            int retorno = GenerateMenu(new List<string>() { "Import to My Library", "Export from My Library", "Editing Area", "Manage Library", "Search in My Library", "Manage Smart Lists", "Exit" }, "~ START MENU ~", "Please, select an option:");
            return retorno;
        }



 /*
 * 
 * 1) Agregar metodo para importar Bitmaps a la libreria
 * 2) Cuando se importa en flujo programa, se agrega una copia de la imagen en Files, y luego
 * se invoca el constructor 
 */
    }
}
