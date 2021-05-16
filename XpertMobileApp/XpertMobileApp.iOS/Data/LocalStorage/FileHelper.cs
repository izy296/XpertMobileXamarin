using System;
using System.IO;
using Xamarin.Forms;
using ItMobileSolution.iOS.Data;
using XpertMobileApp.Data;

[assembly: Dependency(typeof(FileHelper))]
namespace ItMobileSolution.iOS.Data
{
	public class FileHelper : IFileHelper
	{
		public string GetLocalFilePath(string filename)
		{
			string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

			if (!Directory.Exists(libFolder))
			{
				Directory.CreateDirectory(libFolder);
			}

			return Path.Combine(libFolder, filename);
		}
	}
}
