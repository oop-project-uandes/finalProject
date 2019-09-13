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


        private void SaveLibrary()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(DEFAULT_LIBRARY_PATH, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this.library);
            stream.Close();
        }


        private void LoadLibrary()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(DEFAULT_LIBRARY_PATH, FileMode.Open, FileAccess.Read, FileShare.None);
            Library library = (Library)formatter.Deserialize(stream);
            stream.Close();
            this.library = library;
        }


        private bool ExistsLibrary()
        {
            if (File.Exists(DEFAULT_LIBRARY_PATH)) return true;
            else return false;
        }


        private void ShowLibraryDoesntExistError()
        {
            Console.Clear();
            Console.WriteLine("\n[!] CAUTION: The program didn't find the library.bin file");
            Console.WriteLine("[!]          If you added images to My Library, they are gone");
            Console.WriteLine("\n\nIf this is the first time you run this program, please ignore this advice\n\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        private void ShowPresentation()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue ;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetWindowSize(74, 28);
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
        private int GenerateMenu(List<string> options, string title, string description)
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
                Console.WriteLine("\n");
                Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, Console.CursorTop);
                Console.WriteLine(title + "\n");
                Console.SetCursorPosition((Console.WindowWidth - description.Length) / 2, Console.CursorTop);
                Console.WriteLine(description + "\n");
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
