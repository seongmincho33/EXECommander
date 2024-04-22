using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXECommander.S3D
{
    public delegate void Command(EXECommander e);

    public class EXES3DCommander
    {
        public EXECommanderArg commanderArg { get; set; }

        private Dictionary<string, Command> Commands { get; set; }

        public EXECommander Commander { get; set; }

        public readonly string PATH_REVIT_TO_CONVERTER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PEDAS_CONVERTER_S3D\\S3D_TO_CONVERTER");

        public EXES3DCommander(string registryValueName, Dictionary<string, Command> commands)
        {
            this.Commands = commands;

            this.commanderArg = new EXECommanderArg()
            {
                DirectoryPath = this.PATH_REVIT_TO_CONVERTER
                ,
                FileName = "PARAM_TABLE1.xml"
                ,
                FullPath = this.PATH_REVIT_TO_CONVERTER + @"\" + "PARAM_TABLE1.xml"
                ,
                RegistryKeyName = "HKEY_CURRENT_USER\\Software\\PEDAS\\"
                ,
                RegistryValueName = registryValueName
                ,
                RegistryValue = "NONE"
            };

            this.Commander = new EXECommander(commanderArg, commands.Keys.ToList());

            this.Commander.CommandRecieved += Commander1_CommandRecieved;
        }


        private Task Commander1_CommandRecieved(object sender, EXECommanderArg e)
        {
            foreach (var command in this.Commands)
            {
                if (e.RegistryValue == command.Key)
                {
                    command.Value(sender as EXECommander);
                }
            }

            return Task.FromResult(true);
        }
    }
}
