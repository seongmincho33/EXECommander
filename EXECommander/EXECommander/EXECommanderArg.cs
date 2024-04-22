using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXECommander
{
    public class EXECommanderArg
    {
        public string DirectoryPath { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }

        /// <summary>
        /// 레지스트리 경로
        /// </summary>
        public string RegistryKeyName { get; set; }

        /// <summary>
        /// 레지스트리 값 이름
        /// </summary>
        public string RegistryValueName { get; set; }

        /// <summary>
        /// 레지스트리 값
        /// </summary>
        public string RegistryValue { get; set; }

        public EXECommanderArg()
        {
            this.DirectoryPath = "";
            this.FileName = "";
            this.FullPath = "";
            this.RegistryKeyName = "";
            this.RegistryValueName = "";
            this.RegistryValue = "";
        }
    }
}
