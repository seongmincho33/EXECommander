using EXECommander;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EXECommander
{
    public class EXECommander
    {
        private readonly string NONE = "NONE";

        public delegate Task CommandRecieverHandler(object sender, EXECommanderArg e);

        public event CommandRecieverHandler CommandRecieved;

        public EXECommanderArg commanderArg { get; set; }

        private Timer timer { get; set; }
        
        private Serializer Serializer { get; set; }

        /// <summary>
        /// 생성자 셋팅
        /// </summary>
        /// <param name="exeCommanderArg">XML, DataTable, Registry 값</param>
        /// <param name="registryValues">Registry Value 에 넣을 커맨드들</param>
        public EXECommander(EXECommanderArg exeCommanderArg, List<string> registryValues)
        {
            registryValues.Add(this.NONE);

            this.commanderArg = exeCommanderArg;

            this.Serializer = new Serializer(this.commanderArg.DirectoryPath);

            this.SetTimer();
        }


        private void SetTimer()
        {
            this.timer = new Timer();
            this.timer.Interval = 500;
            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Start();
        }


        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string registryValue = this.GetRegistryValue();

            switch (registryValue.ToUpper())
            {
                case nameof(this.NONE):
                    break;

                default:

                    commanderArg.RegistryValue = registryValue;

                    await CommandRecieved?.Invoke(this, commanderArg);

                    break;
            }
        }


        public void CreateDirectory()
        {
            if (Directory.Exists(this.commanderArg.DirectoryPath) == false)
            {
                Directory.CreateDirectory(this.commanderArg.DirectoryPath);
            }
        }


        public void SetXMLByDataSet(DataSet dataSet)
        {
            this.CreateDirectory();

            dataSet.WriteXml(this.commanderArg.DirectoryPath + @"\" + this.commanderArg.FileName);
        }


        public DataSet GetDataSetFromXML()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(this.commanderArg.FullPath);
            return ds;
        }
        

        public void SetXMLByCLassInstance<T>(T toSerialize)
        {
            this.Serializer.SetDataToXMLFile(toSerialize);
        }


        public T GetXMLByClassInstance<T>()
        {
            return this.Serializer.GetDataFromXMLFile<T>();
        }


        public async void SetRegistryValue(string value)
        {
            this.timer.Stop();
            Registry.SetValue(this.commanderArg.RegistryKeyName, this.commanderArg.RegistryValueName, value);
            await Task.Delay(1000);
            this.timer.Start();
        }


        public string GetRegistryValue()
        {
            object obj;
            obj = Registry.GetValue(this.commanderArg.RegistryKeyName, this.commanderArg.RegistryValueName, "");

            if (obj == null)
                return "";
            else
                return obj.ToString();
        }

    }
}
