using System.IO;
using Xamarin.Forms;
using Windows.Storage;
using XpertMobileApp.Data;
using XpertMobileApp.UWP.Data;

[assembly: Dependency(typeof(FileHelper))]
namespace XpertMobileApp.UWP.Data
{
	public class FileHelper : IFileHelper
	{
		public string GetLocalFilePath(string filename)
		{
			return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
		}
	}
}
