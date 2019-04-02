using System;
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
//get type check daloi funci pochva s _ ili char ne cifra

namespace auto_comment
{
    class String_Creator
    {
        public string var_type_public = string.Empty;
        static string[] curr_copy;
        static string[] check_these =
        { "if", "else", "for ", "byte ", "bool ", "decimal ", "using ",
        "double ", "float ", "string ", "string[] ", "char ", "int ",
        "var ", "continue;", "break;" };
        //keywords to check for
        static IDictionary<string, string> user_variables = new Dictionary<string, string>();
        static string keyword_found = string.Empty;
        static string[] split_sentence;
        static string result = string.Empty;
        public static string GetCommentedVersion(string curr)
        {
            curr_copy = TextEdit.Split(curr);
            string[] return_string = new string[curr_copy.Length];
            for (int i = 0; i < curr_copy.Length; i++) //minava prez vseki red v koda
            {
                for (int j = 0; j < check_these.Length; j++) //provera dali reda sadurja nqkoa duma ot check_these
                {
                    //ako {} ili () gi nqma v check_these tva VUTRE v ifa ne se uzpulnqva koeto shte reche ne vliza v comment funkciata i ottam v textedit i zatova izqjda reda
                    if (curr_copy[i].Contains(check_these[j])) //dobavq komentar v string
                    {
                        keyword_found = check_these[j]; //namerenia keyword (s dumi a ne indeksa mu)
                        split_sentence = TextEdit.SplitLine(curr_copy[i]); //razdela tokushtia red na array ot dumi
                        return_string[i] = Comment(keyword_found); //tuk ni e komentara koito shte dobavim v kraq na reda
                        break;
                    }
                    else if (curr_copy[i].Contains("{") || curr_copy[i].Contains("}") || curr_copy[i].Contains("(") || curr_copy[i].Contains(")") || curr_copy[i].Contains("for") || curr_copy[i].Contains("return") || curr_copy[i].Contains(".") || curr_copy[i].Contains("=") || curr_copy[i].Contains("class") || curr_copy[i].Contains("namespace"))
                    {
                        keyword_found = "default";
                        return_string[i] = Comment(keyword_found);
                    }
                }
            }
            for (int i = 0; i < curr_copy.Length; i++)
            {
                if (return_string[i] != null)
                {
                    result += TextEdit.Connect(curr_copy[i], return_string[i]);
                }
                else
                {
                    continue;
                }
            }
            return result;
        }

        private static string Comment(string var_type)
        {
            var_type = var_type.Trim();
            var_type_public = var_type;
            string var_name = string.Empty;
            string var_value = string.Empty;
            string comment = string.Empty;
            string for_checked_part = string.Empty;
            string gore_dolu = string.Empty;
            string for_trueorfalse = string.Empty;
            string for_whatcheck = string.Empty;
            string if_whatcheck = string.Empty;
            string if_trueorfalse = string.Empty;

            if (string.Join(" ", split_sentence).Contains("//") || var_type == "default")
            {
                if (var_type == "default")
                    return Environment.NewLine;
                else
                    return " +" + Environment.NewLine;
            }
            else
            {
                if (var_type.Equals("int"))
                {
                    for (int i = 0; i < split_sentence.Length; i++)
                    {
                        if (split_sentence[i] == var_type)
                        {
                            var_name = split_sentence[i + 1];
                        }
                        if (split_sentence[i] == "=")
                        {
                            var_value = split_sentence[i + 1];
                        }
                    }
                    user_variables.Add(new KeyValuePair<string, string>(var_name, var_value));
                    comment = " //The integer (a whole number) " + var_name + " is declared and it's value is " + var_value + Environment.NewLine;
                    return comment;
                }
                //down from here is probably not working and requires testing
                else if (var_type == "for")
                {
                    for (int i = 0; i < split_sentence.Length; i++)
                    {
                        if (split_sentence[i] == "(int")
                        {
                            var_name = split_sentence[i + 1];
                        }
                        else if (split_sentence[i] == "int")
                        {
                            var_name = split_sentence[i + 1];
                        }
                        if (split_sentence[i] == "=")
                        {
                            var_value = split_sentence[i + 1].Trim(';');
                        }
                        if (split_sentence[i] == "=" && split_sentence[i - 2] != "int")
                        {
                            for_whatcheck = " equals to ";
                            for_checked_part = var_name + " is equeal to " + split_sentence[i + 1].Trim(';');
                            foreach (KeyValuePair<string, string> item in user_variables)
                            {
                                if (item.Key == split_sentence[i + 1])
                                {
                                    if (Convert.ToInt32(var_value) == Convert.ToInt32(item.Value))
                                    {
                                        for_trueorfalse = "(true)";
                                    }
                                    else if (Convert.ToInt32(var_value) == Convert.ToInt32(split_sentence[i + 1].Trim(';')))
                                    {
                                        for_trueorfalse = "(true)";
                                    }
                                    else
                                    {
                                        for_trueorfalse = "(false)";
                                    }
                                }
                                else
                                {
                                    for_trueorfalse = "(false)";
                                }
                            }
                        }
                        if (split_sentence[i] == "<") //checkva kav check she ima
                        {
                            for_whatcheck = " smaller then ";
                            for_checked_part = var_name + " is smaller then " + split_sentence[i + 1].Trim(';');
                            foreach (KeyValuePair<string, string> item in user_variables)
                            {
                                if (item.Key == split_sentence[i + 1])
                                {
                                    if (Convert.ToInt32(var_value) < Convert.ToInt32(item.Value))
                                    {
                                        for_trueorfalse = "(true)";
                                    }
                                    else if (Convert.ToInt32(var_value) < Convert.ToInt32(split_sentence[i + 1].Trim(';')))
                                    {
                                        for_trueorfalse = "(true)";
                                    }
                                    else
                                    {
                                        for_trueorfalse = "(false)";
                                    }
                                }
                                else
                                {
                                    for_trueorfalse = "(false)";
                                }
                            }
                        }
                        if (split_sentence[i] == ">")
                        {
                            for_whatcheck = " bigger then ";
                            for_checked_part = var_name + " is bigger then " + split_sentence[i + 1].Trim(';');
                            foreach (KeyValuePair<string, string> item in user_variables)
                            {
                                if (item.Key == split_sentence[i + 1])
                                {
                                    if (Convert.ToInt32(var_value) > Convert.ToInt32(item.Value))
                                    {
                                        for_trueorfalse = "(true)";
                                    }
                                    else if (Convert.ToInt32(var_value) > Convert.ToInt32(split_sentence[i + 1].Trim(';')))
                                    {
                                        for_trueorfalse = "(true)";
                                    }
                                    else
                                    {
                                        for_trueorfalse = "(false)";
                                    }
                                }
                                else
                                {
                                    for_trueorfalse = "(false)";
                                }
                            }
                        }
                        if (split_sentence[i] == var_name + "++)") //checkva dali she adne ili she mahne edno
                        {
                            gore_dolu = " ,if so plus 1 (one) to " + var_name;
                        }
                        if (split_sentence[i] == var_name + "--)")
                        {
                            gore_dolu = " ,if so minus 1 (one) to " + var_name;
                        }
                    }
                    comment = " //A for loop with inner variable named " + var_name + " is" + for_whatcheck + var_value + for_trueorfalse + ", then asked if " + for_checked_part + gore_dolu + Environment.NewLine;
                    return comment;
                }
                else if (var_type == "double")
                {
                    for (int i = 0; i < split_sentence.Length; i++)
                    {
                        if (split_sentence[i] == var_type)
                        {
                            var_name = split_sentence[i + 1];
                        }
                        if (split_sentence[i] == "=")
                        {
                            var_value = split_sentence[i + 1];
                        }
                    }
                    user_variables.Add(new KeyValuePair<string, string>(var_name, var_value));
                    comment = " //The double (precision: 15-17) " + var_name + " is declared and it's value is " + var_value + Environment.NewLine;
                    return comment;
                }
                else if (var_type == "float")
                {
                    for (int i = 0; i < split_sentence.Length; i++)
                    {
                        if (split_sentence[i] == var_type)
                        {
                            var_name = split_sentence[i + 1];
                        }
                        if (split_sentence[i] == "=")
                        {
                            var_value = split_sentence[i + 1];
                        }
                    }
                    comment = " //The float (precision: 6-9) " + var_name + " is declared and it's value is " + var_value + Environment.NewLine;
                    return comment;
                }
                else if (var_type == "decimal")
                {
                    for (int i = 0; i < split_sentence.Length; i++)
                    {
                        if (split_sentence[i] == var_type)
                        {
                            var_name = split_sentence[i + 1];
                        }
                        if (split_sentence[i] == "=")
                        {
                            var_value = split_sentence[i + 1];
                        }
                    }
                    user_variables.Add(new KeyValuePair<string, string>(var_name, var_value));
                    comment = " //The decimal (preicion: 28-29) " + var_name + " is declared and it's value is " + var_value + Environment.NewLine;
                    return comment;
                }
                else if (var_type.Equals("else"))
                {
                    return " //If none of the statements above are true the following code will execute" + Environment.NewLine;
                }
                else if (var_type == "if")
                {
                    int innner = 0;
                    for (int i = 0; i < split_sentence.Length; i++)
                    {
                        if (split_sentence[i] == var_type)
                        {
                            var_name = split_sentence[i + 1];
                            foreach (KeyValuePair<string, string> item in user_variables)
                            {
                                if (item.Key == var_name)
                                {
                                    innner = Convert.ToInt32(item.Value);
                                }
                                else
                                {
                                    innner = Convert.ToInt32(item.Value.TrimEnd(';'));
                                }
                            }
                        }
                        if (split_sentence[i] == "=")
                        {
                            if_whatcheck = " is equal to ";
                            var_value = split_sentence[i + 1];
                            foreach (KeyValuePair<string, string> item in user_variables)
                            {
                                if (item.Key == split_sentence[i + 1])
                                {
                                    if (innner == Convert.ToInt32(item.Value))
                                    {
                                        if_trueorfalse = "(true)";
                                    }
                                    else if (innner == Convert.ToInt32(split_sentence[i + 1].Trim(';')))
                                    {
                                        if_trueorfalse = "(true)";
                                    }
                                    else
                                    {
                                        if_trueorfalse = "(false)";
                                    }
                                }
                                else
                                {
                                    if_trueorfalse = "(false)";
                                }
                            }
                        }
                        if (split_sentence[i] == ">")
                        {
                            if_whatcheck = " is bigger then ";
                            var_value = split_sentence[i + 1];
                            foreach (KeyValuePair<string, string> item in user_variables)
                            {
                                if (item.Key == split_sentence[i + 1])
                                {
                                    if (innner > Convert.ToInt32(item.Value))
                                    {
                                        if_trueorfalse = "(true)";
                                    }
                                    else if (innner > Convert.ToInt32(split_sentence[i + 1].Trim(';')))
                                    {
                                        if_trueorfalse = "(true)";
                                    }
                                    else
                                    {
                                        if_trueorfalse = "(false)";
                                    }
                                }
                                else
                                {
                                    if_trueorfalse = "(false)";
                                }
                            }
                        }
                        if (split_sentence[i] == "<")
                        {
                            if_whatcheck = " is smaller then ";
                            var_value = split_sentence[i + 1];
                            foreach (KeyValuePair<string, string> item in user_variables)
                            {
                                if (item.Key == split_sentence[i + 1])
                                {
                                    if (innner < Convert.ToInt32(item.Value))
                                    {
                                        if_trueorfalse = "(true)";
                                    }
                                    else if (innner < Convert.ToInt32(split_sentence[i + 1].Trim(';')))
                                    {
                                        if_trueorfalse = "(true)";
                                    }
                                    else
                                    {
                                        if_trueorfalse = "(false)";
                                    }
                                }
                                else
                                {
                                    if_trueorfalse = "(false)";
                                }
                            }
                        }
                    }
                    comment = " //The if question checks whether " + var_name.TrimStart('(') + if_whatcheck + var_value.TrimEnd(')') + if_trueorfalse + Environment.NewLine;
                    return comment;
                }
                else if (var_type == "break;")
                {
                    comment = " //The break statement terminates the closest enclosing loop or switch statement" + Environment.NewLine;
                    return comment;
                }
                else if (var_type == "continue;")
                {
                    comment = " //The continue statement passes control to the next iteration of the enclosing loop or switch statement" + Environment.NewLine;
                    return comment;
                }
                else if (var_type == "var")
                {
                    for (int i = 0; i < split_sentence.Length; i++)
                    {
                        if (split_sentence[i] == var_type)
                        {
                            var_name = split_sentence[i + 1];
                        }
                        if (split_sentence[i] == "=")
                        {
                            if(split_sentence[i + 1] == "new")
                            {
                                var_value = split_sentence[i + 2];
                            }
                            else
                            {
                                var_value = split_sentence[i + 1];
                            }
                        }
                    }
                    if(var_name != "" && var_value != "")
                    {
                        user_variables.Add(new KeyValuePair<string, string>(var_name, var_value));
                    }
                    comment = " //The var (the compiler determines the type) " + var_name + " is declared and it's value is " + var_value + Environment.NewLine;
                    return comment;
                }
                else if (var_type == "string")
                {
                    for (int i = 0; i < split_sentence.Length; i++)
                    {
                        if (split_sentence[i] == var_type)
                        {
                            var_name = split_sentence[i + 1];
                        }
                        if (split_sentence[i] == "=")
                        {
                            var_value = split_sentence[i + 1];
                        }
                    }
                    user_variables.Add(new KeyValuePair<string, string>(var_name, var_value));
                    comment = " //The string (a collection of characters) " + var_name + " is declared and it's value is " + var_value + Environment.NewLine;
                    return comment;
                }
                else if (var_type == "char")
                {
                    for (int i = 0; i < split_sentence.Length; i++)
                    {
                        if (split_sentence[i] == var_type)
                        {
                            var_name = split_sentence[i + 1];
                        }
                        if (split_sentence[i] == "=")
                        {
                            var_value = split_sentence[i + 1];
                        }
                    }
                    user_variables.Add(new KeyValuePair<string, string>(var_name, var_value));
                    comment = " //The char (a unicode character) " + var_name + " is declared and it's value is " + var_value + Environment.NewLine;
                    return comment;
                }
                else if (var_type == "bool")
                {
                    for (int i = 0; i < split_sentence.Length; i++)
                    {
                        if (split_sentence[i] == var_type)
                        {
                            var_name = split_sentence[i + 1];
                        }
                        if (split_sentence[i] == "=")
                        {
                            var_value = split_sentence[i + 1];
                        }
                    }
                    user_variables.Add(new KeyValuePair<string, string>(var_name, var_value));
                    comment = " //The boolean (true or false) " + var_name + " is declared and it's value is " + var_value + Environment.NewLine;
                    return comment;
                }
                else if (var_type == "byte")
                {
                    for (int i = 0; i < split_sentence.Length; i++)
                    {
                        if (split_sentence[i] == var_type)
                        {
                            var_name = split_sentence[i + 1];
                        }
                        if (split_sentence[i] == "=")
                        {
                            var_value = split_sentence[i + 1];
                        }
                    }
                    user_variables.Add(new KeyValuePair<string, string>(var_name, var_value));
                    comment = " //The byte (whole number from 0 to 255) " + var_name + " is declared and it's value is " + var_value + Environment.NewLine;
                    return comment;
                }
                else if (var_type == "using")
                {
                    comment = " //Here we are declaring a namespace" + Environment.NewLine;
                    return comment;
                }
            }
            return Environment.NewLine;
        }
    }
}