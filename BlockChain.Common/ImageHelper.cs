using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BlockChain.Common
{
	public class ImageHelper
	{

		private static Image UrlToImage(string url)
		{
            //严重性	代码	说明	项目	文件	行	禁止显示状态
            //警告 SYSLIB0014	“WebClient.WebClient()”已过时:“WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.”	BlockChain.Common F:\MyCode\2.0\BlockChain\BlockChain.Common\ImageHelper.cs   17


            WebClient mywebclient = new WebClient();
			byte[] Bytes = mywebclient.DownloadData(url);
			using (MemoryStream ms = new MemoryStream(Bytes))
			{
				Image outputImg = Image.FromStream(ms);
				return outputImg;
			}
		}

		/// <summary>
		/// 把网络图片保存为本地图片，可以提升效率
		/// </summary>
		/// <param name="url"></param>
		/// <param name="BasePath"></param>
		/// <returns></returns>
		public static string SaveUrlImageToLocalFile(string url, string BasePath)
		{
			var img = UrlToImage(url);
			string jpeg = Path.GetExtension(url);
			if (jpeg == null || string.IsNullOrWhiteSpace(jpeg))
			{
				jpeg = ".png";
			}

			var filename = Guid.NewGuid().ToString("N") + jpeg;
			filename = System.IO.Path.Combine(BasePath, filename);
			img.Save(filename);
			return filename;
		}


	}
}
