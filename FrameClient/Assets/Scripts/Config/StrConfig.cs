using System.Collections.Generic;using UnityEngine;using Newtonsoft.Json;public class StrConfig  {		public int id { get; set; }		public string zh { get; set; }		public string en { get; set; }		public string jp { get; set; }		public string kr { get; set; }		public string sp { get; set; }		public string ge { get; set; }		public int localId { get; set; }		 		public static string configName = "StrConfig";		public static StrConfig config { get; set; }		public string version { get; set; }		public List<StrConfig> datas { get; set; }		public static void Init()		{			ConfigManager.Ins.Readjson(configName, (json) => {			if (!string.IsNullOrEmpty( json))			{				 config = JsonConvert.DeserializeObject<StrConfig>(json);				 PlayerPrefs.SetString("brother_" + configName, config.version);			}			}, false);		 }		public static StrConfig Get(int id)		{			foreach (var item in config.datas)			{				if (item.id == id)				{ 					return item;				}			}			return null;		}}