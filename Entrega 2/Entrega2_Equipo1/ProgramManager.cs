﻿using System;
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
                    // READY
                    case 0:
                        ImportImageFromPath();
                        break;

                    // Export a file from My Library to a path defined by the user
                    // READY
                    case 1:
                        ExportFileToPath();
                        break;

                    // Editing Area (Apply filters, features, watson, slideshares, etc)
                    case 2:

                        break;

                    // Show library, add elements or erase elements, add or remove labels. Por ahora solo sirve para mostrar la libreria
                    case 3:
                        ManageLibrary();
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


        // TRABAJANDO EN ESTE METODO

        private void ManageLibrary()
        {
            List<string> manageLibraryTitle = LoadBannerData("managelibrary.txt");
            List<string> manageLibraryOptions = new List<string>() { "Show My Library", "Add Label", "Edit Label", "Delete Label", "Set Calification", "Delete Image", "Reset Library" , "Exit"};
            string manageLibraryDescription = "Please, select an option: ";
            while (true)
            {
                int usrDecision = GenerateMenu(manageLibraryOptions, null, manageLibraryDescription, manageLibraryTitle);
                if (usrDecision == 7) break;
                switch (usrDecision)
                {
                    // User wants to see his library
                    case 0:
                        ShowLibrary();
                        break;
                    // User wants to add a label
                    case 1:
                        AddLabel();
                        break;
                    // User wants to edit a label
                    case 2:
                        break;
                    // User wants to delete a label => WORKING HERE
                    case 3:
                        DeleteLabel();
                        break;
                    


                    // FALTAN LOS DEMAS CASES
                    case 6:
                        ResetLibrary();
                        break;
                }
            }
        }


        // WORKING ON THIS FUNCTION
        private void DeleteLabel()
        {
            Console.Clear();
            List<string> DeleteLabelTitle = LoadBannerData("deletelabel.txt");
            List<string> DeleteLabelOptions1 = new List<string>();
            string presskeytocontinue = "Please, press any key to continue...";
            string emptylibraryerror = "[!] ERROR: Sorry, you don't have any images in your library";
            string description1 = "Please, select on which image you want to delete a Label: ";
            string description2 = "Please, select which label you want to delete: ";
            

            foreach (Image image in library.Images)
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
            int numberOfTheImage = GenerateMenu(DeleteLabelOptions1, null, description1, DeleteLabelTitle);

            // If user selects Exit
            if (DeleteLabelOptions1[numberOfTheImage] == "Exit") return;
            Image imageToDeleteLabel = library.Images[numberOfTheImage];


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

                    if(slabel.Name != null) newstring += $"\n        Name: {slabel.Name}";
                    else newstring += $"\n        Name: [Not set]";

                    if (slabel.Surname != null) newstring += $"\n        Surname: {slabel.Surname}";
                    else newstring += $"\n        Surname: [Not set]";

                    if (slabel.Nationality != ENationality.None) newstring += $"\n        Nationality: {slabel.Nationality}";
                    else newstring += $"\n        Nationality: [Not set]";

                    if (slabel.EyesColor != EColor.None) newstring += $"\n        EyesColor: {slabel.EyesColor}";
                    else newstring += $"\n        EyesColor: [Not set]";

                    if (slabel.HairColor != EColor.None) newstring += $"\n        HairColor: {slabel.HairColor}";
                    else newstring += $"\n        HairColor: [Not set]";

                    if (slabel.Sex!= ESex.None) newstring += $"\n        Sex: {slabel.Sex}";
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
            int intUsrWantsToDelete = GenerateMenu(optionsToDelete, null, description2, DeleteLabelTitle);

            if (optionsToDelete[intUsrWantsToDelete] == "Exit") return;

            imageToDeleteLabel.Labels.RemoveAt(intUsrWantsToDelete);
            return;

        }
        


        private void AddLabel()
        {
            Console.Clear();
            List<string> AddLabelTitle = LoadBannerData("addlabel.txt");
            List<string> AddLabelOptions1 = new List<string>();
            string presskeytocontinue = "Please, press any key to continue...";
            string emptylibraryerror = "[!] ERROR: Sorry, you don't have any images in your library";
            string description1 = "Please, select to which image you want to add the Label: ";
            string description2 = "Please, select which type of label you would like to add: ";
            string addlabeldescription = "Please, select an option: ";
            List<string> AddLabelOptions2 = new List<string>() { "SimpleLabel", "PersonLabel", "SpecialLabel", "Done" };

            foreach (Image image in library.Images)
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
            int numberOfTheImage = GenerateMenu(AddLabelOptions1, null, description1, AddLabelOptions1);

            // If user selects Exit
            if (AddLabelOptions1[numberOfTheImage] == "Exit") return;
            
            
            Image imageToAddLabel = library.Images[numberOfTheImage];

            while (true)
            {
                int usrLabel = GenerateMenu(AddLabelOptions2, null, description2, AddLabelTitle);
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
                                library.Images[numberOfTheImage].BitmapImage.Save(realpath);
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
                        

                        int selectedOption2 = GenerateMenu(watsonOptions, null, addlabeldescription, AddLabelTitle);
                        string selected = watsonOptions[selectedOption2];
                        if (selected != "Personalized Label")
                        {
                            SimpleLabel sauxLabel = new SimpleLabel(selected);
                            imageToAddLabel.AddLabel(sauxLabel);
                            SaveLibrary();
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
                            SaveLibrary();
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
                            userSelection = GenerateMenu(personOptions, null, addlabeldescription, AddLabelTitle);
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
                        SaveLibrary();
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
                            userSelection2 = GenerateMenu(personOptions, null, addlabeldescription, AddLabelTitle);
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
                                    int usrSelection2 = GenerateMenu(new List<string>() { "Is a selfie", "Is not a selfie" }, AddSelfieTitle, AddSelfieSelection);
                                    if (usrSelection2 == 0) auxLabel2.Selfie = true;
                                    else auxLabel2.Selfie = false;
                                    break;
                            }
                        }
                        imageToAddLabel.AddLabel(auxLabel2);
                        SaveLibrary();
                        break;
                    

                    // User wants to exit
                    case 3:
                        SaveLibrary();
                        break;
                }
                break;
            }
        }


        private void ResetLibrary()
        {
            List<string> ResetLibraryTitle = LoadBannerData("resetlibrary.txt");
            string description = "Are you sure that you want to reset your library?: ";
            int usrDecision = GenerateMenu(new List<string>() { "Yes, reset My Library", "Exit" }, null, description, ResetLibraryTitle);
            if (usrDecision == 0)
            {
                library.ResetImages();
                SaveLibrary();
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
            List<string> bannerTitle = LoadBannerData("export.txt");
            List<string> options = new List<string>();
            foreach (Image image in library.Images)
            {
                options.Add(image.Name);
            }
            options.Add("Exit");
            while (true)
            {
                int usrOption = GenerateMenu(options, null, "Please, select the file you want to export: ", bannerTitle);
                if (usrOption == options.Count - 1) return;
                Image usrWants = library.Images[usrOption];
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
                int usrIntFormat = GenerateMenu(formats, null, introduceFormat, bannerTitle);
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


        private void ShowLibrary()
        {
            // Show the title
            Console.Clear();
            string separator = "\n<<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>><<>>";
            List<string> ShowLibraryTitle = LoadBannerData("mylibrary.txt");
            string presskeytocontinue = "Please, press any key to continue...";
            foreach (string titlestring in ShowLibraryTitle)
            {
                Console.SetCursorPosition((Console.WindowWidth - titlestring.Length) / 2, Console.CursorTop);
                Console.WriteLine(titlestring);
            }

            // Show all images and their labels
            foreach (Image image in library.Images)
            {
                Console.WriteLine(separator);
                string titlewithcal = $"          ~ Name: {image.Name} Calification: {image.Calification} ~";
                string titlewithoutcal = $"          ~ Name: {image.Name} Calification: Not set ~";
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
                string[] userImportingSettings = ChoosePathAndImages();
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
                            usrDecision = GenerateMenu(new List<string>() { "Calificate or add labels individually", "Calificate or add labels to all images" }, "~ Importing options ~", "Please, choose an option: ");
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
                                    Image newImage = CreatingImageMenu(filename, path);
                                    library.AddImage(newImage);
                                    SaveLibrary();
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
                                    count ++;
                                    if (count == 5000) throw new Exception("Too much images");
                                }
                            }
                            string path = Path.GetDirectoryName(allimages[0][0]);
                            path += @"\";
                            List<Image> allUserImages = CreatingImageMenu(allFiles, path);
                            foreach (Image imagen in allUserImages)
                            {
                                library.AddImage(imagen);
                                SaveLibrary();
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
                        int userChoice = GenerateMenu(auxList, "~ [!] ERROR ~", "Please, select an option: ");
                    }
                }
                else
                {
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

                        int usrDecision;
                        if (files.Length > 1)
                        {
                            usrDecision = GenerateMenu(new List<string>() { "Calificate or add labels individually", "Calificate or add labels to all images" }, "~ Importing options ~", "Please, choose an option: ");
                        }
                        else
                        {
                            usrDecision = 0;
                        }

                        if (usrDecision == 0)
                        {
                            foreach (string file in files)
                            {
                                Image newImage = CreatingImageMenu(file, path);
                                library.AddImage(newImage);
                                SaveLibrary();
                            }
                            break;
                        }
                        else
                        {
                            // Asumimos que no se pueden importar 5000 imagenes de un solo tiro
                            List<Image> allUserImages = CreatingImageMenu(files, path);
                            foreach (Image imagen in allUserImages)
                            {
                                library.AddImage(imagen);
                                SaveLibrary();
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
                        int userChoice = GenerateMenu(auxList, "~ [!] ERROR ~", "Please, select an option: ");
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
                                if (newlabel.GeographicLocation != null) Console.Write($"GeoLocation: {newlabel.GeographicLocation[0]}, {newlabel.GeographicLocation[1]}");
                                if (newlabel.Address != null) Console.Write($"Address: {newlabel.Address}");
                                if (newlabel.Photographer != null) Console.Write($"Photographer: {newlabel.Photographer}");
                                if (newlabel.PhotoMotive != null) Console.Write($"PhotoMotive: {newlabel.PhotoMotive}");
                                if (newlabel.Selfie != false) Console.Write($"Selfie: It is a Selfie");
                                else Console.Write($"Selfie: It is not a Selfie");
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
                        int selectedOption1 = GenerateMenu(new List<string>() { "SimpleLabel", "PersonLabel", "SpecialLabel", "Exit" }, setCalificationTitle, introduceCalification);



                        // If user wants to exit the label selection menu
                        if (selectedOption1 == 3) break;




                        // If user wants to add a SimpleLabel
                        if (selectedOption1 == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("\n");
                            List<string> watsonOptions = new List<string>();
                            
                            watsonOptions.Add("Personalized Label");
                            GenerateMenu(watsonOptions, SimpleLabelCreation, donthavewatsonrecommendations);


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
                                List<string> personOptions = new List<string>() { "GeographicLocation: " + settedGeographicLocation, "Address: " + settedAddress, "Photographer: " + settedPhotographer, "PhotoMotive: " + settedPhotoMotive, "Selfie: " + settedSelfie, "Exit" };
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


                // If the user wants to exit
                if (selectedOption == 2) break;
            }

            foreach (string name in names)
            {
                if (name == null) break;
                Image auxImage = new Image(path + name, imageLabels, finalCalification);
                auxImage.Name = name;
                returningListOfImages.Add(auxImage);
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
            List<string> options = new List<string>() {"Set Calification", "Set new Label", "Continue"};

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
                                if (newlabel.GeographicLocation != null) Console.Write($"GeoLocation: {newlabel.GeographicLocation[0]}, {newlabel.GeographicLocation[1]}");
                                if (newlabel.Address != null) Console.Write($"Address: {newlabel.Address}");
                                if (newlabel.Photographer != null) Console.Write($"Photographer: {newlabel.Photographer}");
                                if (newlabel.PhotoMotive != null) Console.Write($"PhotoMotive: {newlabel.PhotoMotive}");
                                if (newlabel.Selfie != false) Console.Write($"Selfie: It is a Selfie");
                                else Console.Write($"Selfie: It is not a Selfie");
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
                        int selectedOption1 = GenerateMenu(new List<string>() { "SimpleLabel", "PersonLabel", "SpecialLabel", "Exit" }, setCalificationTitle, introduceCalification);



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
                                List<string> personOptions = new List<string>() { "GeographicLocation: " + settedGeographicLocation, "Address: " + settedAddress, "Photographer: " + settedPhotographer, "PhotoMotive: " + settedPhotoMotive, "Selfie: " + settedSelfie, "Exit" };
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


                // If the user wants to exit
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
            List<string> bannerTitle = LoadBannerData("import.txt");

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
            Console.SetWindowSize(60, 28);
            List<string> presentationStrings = LoadBannerData("presentation.txt");
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
            List<string> presentationStrings = LoadBannerData("goodbye.txt");
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
            Console.SetWindowSize(200, 50);
            Console.Clear();
            List<string> startingStrings = LoadBannerData("startmenu.txt");
            int retorno = GenerateMenu(new List<string>() { "Import to My Library", "Export from My Library", "Editing Area", "Manage Library", "Search in My Library", "Manage Smart Lists", "Exit" }, null, "Please, select an option:", startingStrings);
            return retorno;
        }


        // Methods that returns the banner data, given the filename
        private List<string> LoadBannerData(string filename)
        {
            List<string> returningList = new List<string>();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\banners\"+filename;
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
