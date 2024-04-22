using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXECommander;

namespace EXECommander.PDMS
{
    public class EXEPDMSCommander
    {
        public EXECommanderArg commanderArg { get; set; }

        /// <summary>
        /// XML 파일을 저장할 경로다.
        /// </summary>
        string PATH_PEDAS_CONVERTER_PDMS = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PEDAS_CONVERTER_PDMS");

        string PATH_CONVERTER_TO_PDMS = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PEDAS_CONVERTER_PDMS\\CONVERTER_TO_PDMS");

        string PATH_CONVERTER_TO_PDMS_FOUNDATION = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PEDAS_CONVERTER_PDMS\\CONVERTER_TO_PDMS\\FOUNDATION");

        string PATH_CONVERTER_TO_PDMS_ETC = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PEDAS_CONVERTER_PDMS\\CONVERTER_TO_PDMS\\ETC");

        string PATH_PDMS_TO_CONVERTER = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PEDAS_CONVERTER_PDMS\\PDMS_TO_CONVERTER");


        /// <summary>
        /// DesignExplorerTable을 Xml형식으로 변환한 파일 이름이다.
        /// </summary>
        readonly string DesignExplorerXmlFileName = @"\DesignExplorer.xml";

        /// <summary>
        /// PedasConverter객체들을 담고있는 site, zone들을 xml로 저장할 파일이름이다.
        /// </summary>
        readonly string PedasItemDesignExplorerXmlFileName = @"\DesignExplorer_PedasItems.xml";

        /// <summary>
        /// NOTE와 PDMS객체의 Refno의 매핑된 xml파일이름이다.
        /// </summary>
        readonly string NoteXmlFileName = @"\PEDAS_NOTE.xml";

        /// <summary>
        /// 리비전용 파일 이름이다.
        /// </summary>
        readonly string RevisionXmlDataFileName = @"\REVISION.xml";

        public delegate void Command(EXECommander e);

        private Dictionary<string, Command> Commands { get; set; }

        public EXEPDMSCommander(Dictionary<string, Command> commands)
        {
            this.Commands = commands;

            this.commanderArg = new EXECommanderArg()
            {
                DirectoryPath = this.PATH_CONVERTER_TO_PDMS
                , FileName = this.DesignExplorerXmlFileName
                , FullPath = this.PATH_CONVERTER_TO_PDMS + @"\" + this.DesignExplorerXmlFileName
                , RegistryKeyName = "HKEY_CURRENT_USER\\Software\\PEDAS\\"
                , RegistryValueName = "PDMSCommand"
                , RegistryValue = "NONE"
            };

            EXECommander commander1 = new EXECommander(commanderArg, commands.Keys.ToList());

            commander1.CommandRecieved += Commander1_CommandRecieved;
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
