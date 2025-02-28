using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskManager.classes;

namespace TaskManager
{
    public class ConsoleUI
    {
        public void Start()
        {
            TaskManagerCommands.Help();
            TaskManagerCommands.Load();
            while (true)
            {
                Console.Write("> : ");
                string? command = Console.ReadLine();
                if (string.IsNullOrEmpty(command) == false)
                    Parse(command);
                else
                    Console.WriteLine("unknown command");

            }
        }

        public void Parse(string command)
        {
            command = command.Trim();
            if (command == "ADD")
                TaskManagerCommands.Add();
            else if (command == "EDIT")
                TaskManagerCommands.Edit();

            else if (command == "DELETE")
                TaskManagerCommands.Delete();
            else if (command == "HELP")
                TaskManagerCommands.Help();
            else if (command == "EXIT")
                TaskManagerCommands.Exit();
            else if (command == "LIST")
                TaskManagerCommands.List();
            else if (command == "SAVE")
                TaskManagerCommands.Save();            
            else
                Console.WriteLine("unknown command");
        }
    }
}