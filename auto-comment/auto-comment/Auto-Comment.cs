﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace auto_comment
{
    public partial class Form1 : Form
    {
        static string return_from_linecheker = ""; //string used to store linecheckerretrun return values
        static string return_from_linecheker_using = ""; //reeeee maiche e muda
        static string text = ""; //text received from the user
        static string[] split_curr;
        static string curr_copy;
        static string commeneted_variable_string_global;
        static string commeneted_using_string_global;
        //tuka noiv neshta opitvam
        static bool isUsing = false;
        static bool isInt = false;
        static bool isString = false;
        static bool isFloat = false;
        static bool isDouble = false;
        static bool isBool = false;
        static bool isConsole = false;
        ComboBox Options_DropDown = new ComboBox(); //pravim novo drop down menu
        Button Saved_Option = new Button();

        public Form1()
        {
            InitializeComponent();

            //Everything below is used for the options menu
            // \/\/\/\/\/\/
            //
            //Build a list
            var dataSource = new List<Options_Items>();
            dataSource.Add(new Options_Items() { Commentmethod = "Please select an option." });                                   //0
            dataSource.Add(new Options_Items() { Commentmethod = "Override selected file." });                                    //1
            dataSource.Add(new Options_Items() { Commentmethod = "Create a copy of the selected file at selected location." });    //2
            dataSource.Add(new Options_Items() { Commentmethod = "Copy the commented version to clipboard." });                    //3

            //Setup data binding
            Options_DropDown.DataSource = dataSource;
            Options_DropDown.DisplayMember = "Commentmethod";

            // make it readonly
            Options_DropDown.DropDownStyle = ComboBoxStyle.DropDownList;

            Options_DropDown.SetBounds(401, 290, 171, 23);

            Saved_Option.Click += new System.EventHandler(this.btn_Save_Preferences_Click);

            Saved_Option.Name = "btn_Save_Preferences";
            Saved_Option.Text = "Save";
            Saved_Option.SetBounds(401, 316, 75, 23);
            Controls.Add(Options_DropDown); //dobavqme drop down menuto kum prozoreca
            Controls.Add(Saved_Option);

            Options_DropDown.Visible = false;
            Saved_Option.Visible = false;
            //
            // /\/\/\/\/\/\/\/\
            //Everything above is used for the options menu
        }

        private void btn_twitter_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://twitter.com/FLasersights"); //slagame linka koito iskame da otvorim tuk
            Process.Start(sInfo); //otvara goreposochenia link s default browsera (tozi koito potrebitelq e izbral kato default za negovia comp)
            //ivailo ti shiban idiot, ti si napravil tazi funcia i ako a pipnesh otnovo i ta se schupi shete ubia, big gay faggot
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            //ot tuk nadolu vsichko e copy pasta taka che nz kolko e dobro
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt| C# Files (*.cs)|*.cs";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    if (fileContent != null)
                    {
                        text = fileContent;
                    }
                }
            }
            DialogResult file_selected = MessageBox.Show("File Selected");
        }
        private void btn_comment_Click(object sender, EventArgs e) //boji pederas smotan kak moja da sburkash funkcia ot 5 reda i da breaknesh cqlata programa wtf ebi se
        {
            string[] input = GetText();
            for(int i = 0; i < input.Length; i++)//feeds linetype checker every line
            {
                LineTypeChecker(input[i]);
                //StringCreator_using(null);
                StringCreator_variable(return_from_linecheker);
                CommenetedLineWriter(null);
                //return_from_stringcreator = return_from_linecheker;
                //StringCreator_selector;
            }           
        }
        public static string LineTypeChecker(string curr)
        {
            curr_copy = curr;
            string return_string = "";
            string return_using = "";
            split_curr = curr.Split(' ', '.', ';', '[');
            string[] check_these = {"int", "double", "float", "string","char", "using"};
            foreach (string element in check_these)
            {
                //testing
                if (split_curr[0].Contains(element))
                {
                    return_string = "Variable found is " + element;
                    return_from_linecheker = return_string;//gets the foken value, copy paste this shit
                    return return_string;
                }
            }
            if(split_curr[0] == "using")
            {
                isUsing = true;
            }
            if(split_curr[0] == "int")
            {
                isInt = true;
            }
            if(split_curr[0] == "string")
            {
                isString = true;
            }
            if(split_curr[0] == "float")
            {
                isFloat = true;
            }
            if(split_curr[0] == "double")
            {
                isDouble = true;
            }
            if(split_curr[0] == "bool")
            {
                isBool = true;
            }
            if(split_curr[0] == "Console")
            {
                isConsole = true;
            }
            //if(split_curr[0] == "for")
            //{
            //    return_string = "For";
            //    return_from_linecheker = return_string;//maiche e neshto staro de ne se izpolzva, vikai ot tuka funciata?

            //    return return_string;
            //}
            //else if(split_curr[0] == "Console")
            //{
            //    return_string = "Console";
            //    return_from_linecheker = return_string;
            //    return return_string;
            //}
            //else if(split_curr[0] == "if")
            //{
            //    return_string = "If";
            //    return_from_linecheker = return_string;
            //    return return_string;
            //}
            //if (split_curr[0] == "using")
            //{
            //    //tva maiche trebva taka da go prenapishem
            //    return_string = "using";
            //    return_from_linecheker_using = curr_copy;
            //    return return_using;
            //}
            return null;
        }
        public static string[] GetCurr_split()
        {
            return split_curr;
        }
        public static string BoolCheckerAndFunctionCaller(string BoolCheckerAndFunctionCaller_null)
        {
            if(isUsing == true)
            {
                StringCreator_using(null);
            }
            if(isInt == true)
            {

            }
            return BoolCheckerAndFunctionCaller_null;
        }
        public static string StringCreator_using(string selector_input_using)
        {
            string commeneted_using_string = return_from_linecheker_using + " //Adds library";
            commeneted_using_string_global = commeneted_using_string;
            DialogResult test2 = MessageBox.Show(commeneted_using_string_global);
            return selector_input_using;
        }
        public static string String_creator_int(string String_creator_int_null)
        {
            return String_creator_int_null;
        }
        public static string StringCreator_variable(string StringCreator_variable_input_string)
        {
            string commeneted_variable_string = "";
            if (curr_copy.Contains("Console.ReadLine") == true)
            {
                commeneted_variable_string = " //This is " + split_curr[1] + " a " + split_curr[0] + " type variable and it is entered by the user.";
                commeneted_variable_string_global = commeneted_variable_string;
            }
            else
            {
                //int inx_equals = curr_copy.IndexOf(@"=");
                //int inx_needed = 
                commeneted_variable_string = " //This is a " + split_curr[0] + " type variable and it is currently equal to ";
                commeneted_variable_string_global = commeneted_variable_string;
            }
            return null;
        }
        public static string StringCreator_selector(string selector_input_string)
        {
            return selector_input_string;
        }
        public static string CommenetedLineWriter(string commented_line_input)
        {
            DialogResult test = MessageBox.Show(commeneted_variable_string_global);
            /*
            if(split_curr[4] == "")//this check shows that when the ; is split into the last of split_curr it is empty and making a check if it is empty returns true
            {
                DialogResult test2 = MessageBox.Show("yee");
            }
            */
            return commented_line_input;
        }

        private static string[] GetText() //pulni array s texta vzet ot user
        {
            if (text == "")
            {
                return null;
            }
            else
            {
                string[] return_text = TextEdit.Split(text);
                return return_text;
            }
        }

        private void btn_options_Click(object sender, EventArgs e)
        {
            Options_DropDown.Visible = true;
            Saved_Option.Visible = true;
        }

        private void btn_Save_Preferences_Click(object sender, EventArgs e)
        {
            if (Options_DropDown.SelectedIndex != 0)
            {
                int picked_option = Options_DropDown.SelectedIndex;
                Options_DropDown.Visible = false;
                Saved_Option.Visible = false;
            }
            else
            {
                string Warning_Message = "Please choose an option";
                MessageBox.Show(Warning_Message);
            }
        }
    }
}