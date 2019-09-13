using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega2_Equipo1
{
    public class ProgramManager
    {
        private bool _continue;
        private int startingOption;





        public ProgramManager()
        {
            this._continue = true;
        }





        public void Run()
        {
            ShowPresentation();
            while (this._continue == true)
            {
                this.startingOption = StartingMenu();
                
            }
        }

        








        private void ShowPresentation()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue ;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetWindowSize(70, 27);
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
                Console.WriteLine(presentationString);
            }
            Console.ReadKey();
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
            int retorno = GenerateMenu(new List<string>() { "Import to My Library", "Export from My Library", "Editing Area", "Show My Library and add labels to pictures", "Search in My Library", "Manage smart lists", "Exit" }, "~ START MENU ~", "Please, select an option:");
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
