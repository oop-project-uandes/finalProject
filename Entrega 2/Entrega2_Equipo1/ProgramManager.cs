using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace Entrega2_Equipo1
{
    public class ProgramManager
    {
        private Library library;
        private Producer producer;
        private readonly string DEFAULT_LIBRARY_PATH = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files\library.bin";
        private bool _continue = true;
        private int startingOption;
        

        public void Run()
        {
            ShowPresentation();
            LoadingLibraryManager();
            this.producer = new Producer();
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
                    // Show library, add elements or erase elements, add or remove labels. Por ahora solo sirve para mostrar la libreria
                    case 3:
                        ShowLibrary();
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


        private void ShowLibrary()
        {
            Console.Clear();
            foreach (Image image in library.Images)
            {
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine($"\n          Name: {image.Name}, Calification: {image.Calification}");
                foreach (Label label in image.Labels)
                {
                    Console.WriteLine("\n" + label.labelType);
                    switch (label.labelType)
                    {
                        case "SimpleLabel":
                            SimpleLabel slabel = (SimpleLabel)label;
                            Console.WriteLine($"Sentence: {slabel.Sentence}");
                            break;
                        case "PersonLabel":
                            PersonLabel plabel = (PersonLabel)label;
                            if (plabel.Name != null) Console.WriteLine($"Name: {plabel.Name}");
                            if (plabel.Surname != null) Console.WriteLine($"Surname: {plabel.Surname}");
                            if (plabel.Nationality != ENationality.None) Console.WriteLine($"Nationality: {plabel.Nationality}");
                            if (plabel.EyesColor != EColor.None) Console.WriteLine($"EyesColor: {plabel.EyesColor}");
                            if (plabel.HairColor != EColor.None) Console.WriteLine($"HairColor: {plabel.HairColor}");
                            if (plabel.Sex!= ESex.None) Console.WriteLine($"Sex: {plabel.Sex}");
                            if (plabel.BirthDate != "") Console.WriteLine($"Birthdate: {plabel.BirthDate}");
                            if (plabel.FaceLocation != null) Console.WriteLine($"FaceLocation: {plabel.FaceLocation[0]}, {plabel.FaceLocation[1]}, {plabel.FaceLocation[2]}, {plabel.FaceLocation[3]}");
                            break;
                        case "SpecialLabel":
                            SpecialLabel splabel = (SpecialLabel)label;
                            if (splabel.GeographicLocation != null) Console.WriteLine($"GeographicLocation: {splabel.GeographicLocation[0]}, {splabel.GeographicLocation[1]}");
                            if (splabel.Address != null) Console.WriteLine($"Address: {splabel.Address}");
                            if (splabel.Photographer != null) Console.WriteLine($"Photographer: {splabel.Photographer}");
                            if (splabel.PhotoMotive != null) Console.WriteLine($"PhotoMotive: {splabel.PhotoMotive}");
                            if (splabel.Selfie == true) Console.WriteLine($"Selfie: It is a Selfie");
                            else Console.WriteLine($"Selfie: It is not a Selfie");
                            break;
                    }
                }
                Console.WriteLine("-----------------------------------------------------------------");
            }
            Console.ReadKey();
        }


        private void ImportImageFromPath()
        {
            string positive = "All chosen files exists...";
            string negative = "[!] ERROR: Some of the chosen files doesn't exist, or the path is incorrect";
            string pressKey = "Press any key to continue...";
            string option1 = "Try importing again";
            string option2 = "Go back to Start Menu";
            List<Image> internalList = new List<Image>();

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
                    
                    foreach (string file in files)
                    {
                        Image newImage = CreatingImageMenu(file, path);
                        internalList.Add(newImage);
                    }
                    break;
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
            foreach (Image image in internalList)
            {
                library.AddImage(image);
            }
            SaveLibrary();
            return;
        }



        // User interaction during the import files, and creates an image to add to the library
        private Image CreatingImageMenu(string name, string path)
        {
            // Se debe poder importar una carpeta completa y decidir si las labels / calificacion se deben agregar
            // a todas las imagenes
            int calification = -1;
            List<Label> imageLabels = new List<Label>();
            string title = "~ " + name + " ~\n";
            string cal = "Calification: ";
            string lab = "Labels: ";
            string separator = "-----------------------------------------------------------------------------";
            int selectedOption = 0;
            List<string> options = new List<string>() {"Set Calification", "Set new Label", "Continue"};

            while (true)
            {
                bool _continue = true;
                while (_continue == true)
                {
                    Console.Clear();
                    Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, Console.CursorTop);
                    Console.WriteLine(title);
                    Console.SetCursorPosition((Console.WindowWidth - cal.Length) / 4, Console.CursorTop);
                    Console.Write(cal);
                    if (calification == -1) Console.WriteLine("Not set\n");
                    else Console.WriteLine(calification);
                    Console.WriteLine("\n");
                    Console.SetCursorPosition((Console.WindowWidth - lab.Length) / 4, Console.CursorTop);
                    Console.Write(lab);

                    if (imageLabels.Count == 0) Console.WriteLine("Not set\n");
                    else
                    {
                        Console.WriteLine("\n");
                        foreach (Label label in imageLabels)
                        {
                            Console.WriteLine(separator);
                            Console.Write($"{label.labelType}\n");

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
                                    string locationlabel = $"\nLocation:{newlabel.FaceLocation[3]},{newlabel.FaceLocation[2]},{newlabel.FaceLocation[0]},{newlabel.FaceLocation[1]}\n";
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
                                if (newlabel.GeographicLocation != null) Console.WriteLine($"\nGeoLocation: {newlabel.GeographicLocation[0]}, {newlabel.GeographicLocation[1]}");
                                if (newlabel.Address != null) Console.WriteLine($"\nAddress: {newlabel.Address}");
                                if (newlabel.Photographer != null) Console.WriteLine($"\nPhotographer: {newlabel.Photographer}");
                                if (newlabel.PhotoMotive != null) Console.WriteLine($"\nPhotoMotive: {newlabel.PhotoMotive}");
                                if (newlabel.Selfie != false) Console.WriteLine($"\nSelfie: It is a Selfie");
                                else Console.WriteLine($"\nSelfie: It is not a Selfie");
                            }
                        }
                    }
                    int i = 1;
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
                        int selectedOption1 = GenerateMenu(new List<string>() { "SimpleLabel", "PersonLabel", "SpecialLabel", "Exit" }, setCalificationTitle, introduceCalification);

                        // If user wants to exit the label selection menu
                        if (selectedOption1 == 3) break;




                        // If user wants to add a SimpleLabel
                        if (selectedOption1 == 0)
                        {
                            Console.Clear();
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
                            int selectedOption2 = GenerateMenu(watsonOptions, SimpleLabelCreation, SelectOption);
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
                                Console.Clear();
                                List<string> personOptions = new List<string>() { "Name", "Surname", "FaceLocation", "Nationality", "EyesColor", "HairColor", "Sex", "Birthdate", "Exit" };
                                userSelection = GenerateMenu(personOptions, PersonLabelCreation, PersonLabelSelection);
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


                                            auxLabel.BirthDate = Convert.ToString(day) +"-"+ Convert.ToString(month) +"-"+ Convert.ToString(year);
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
                                Console.Clear();
                                List<string> personOptions = new List<string>() { "GeographicLocation", "Address", "Photographer", "PhotoMotive", "Selfie", "Exit" };
                                userSelection = GenerateMenu(personOptions, SpecialLabelCreation, SpecialLabelSelection);
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
                                        int usrSelection2 = GenerateMenu(new List<string>() { "Is a selfie", "Is not a selfie" }, AddSelfieTitle, AddSelfieSelection);
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










                if (selectedOption == 2) break;
            }
            Image returningImage = new Image(path + name, imageLabels, calification);
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
                if(title != null && description != null) Console.Clear();
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
 * TODO: 1) PODER IMPORTAR CARPETA COMPLETA
 *       2) PODER AGREGAR UNA MISMA ETIQUETA A TODAS LAS IMAGENES
 *       3) AGREGAR UN MENU EN MANAGE LIBRARY, QUE TENGA LA OPCION DE MOSTRAR LA LIBRERIA, Y SOLO ENTONCES ES QUE LA MUESTRA
 *       4) AGREGAR EL METODO PARA EXPORTAR IMAGENES DESDE EL PROGRAMA
 * 
 */
    }
}
