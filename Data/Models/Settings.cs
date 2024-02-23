using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Plan_current_repairs.Data.Models
{
    public class Settings
    {
        public int CurrentYear { get; set; }
        public string ChiefEngineer_Post { get; set; }
        public string ChiefEngineer_Name { get; set; }
        public string HeadOfPTO_Post { get; set; }
        public string HeadOfPTO_Name { get; set; }
        public string EngineerOfPTO_Post { get; set; }
        public string EngineerOfPTO_Name { get; set; }

        public Settings() { }


        public void LoadSetting(Settings settings)
        {
            string TempSettings = "{ \"SettingsApp\": " + JsonConvert.SerializeObject(settings) + " }";
            StreamWriter sw = File.CreateText("Settings.json");
            sw.WriteLine(TempSettings);
            sw.Close();
        }
    }

}
