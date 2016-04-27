using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace Zeje.Staticize_.AppCode
{
    public class HtmlPageHelper
    {
        private ArrayList htmlCreatedList = new ArrayList();

        /// <summary>
        /// 递归实现页面静态化功能
        /// </summary>
        /// <param name="urlString">要访问的页面链接地址</param>
        public void SaveHtmlCode(string urlString)
        {
            if (htmlCreatedList.Contains(urlString))
            {
                return;
            }
            string htmlCode = GetHtmlCodeFromUrl(urlString);
            string htmlPath = GetHtmlPathFromUrl(urlString);
            string direcHtmlPath = Path.GetDirectoryName(htmlPath);
            if (!Directory.Exists(direcHtmlPath))
            {
                Directory.CreateDirectory(direcHtmlPath);
            }
            File.WriteAllText(htmlPath, htmlCode);
            htmlCreatedList.Add(urlString);

            ArrayList urlList = GetUrlLinkFromHtmlCode(htmlCode);
            string urlTemp = string.Empty;
            foreach (string url in urlList)
            {
                Uri uri = new Uri(urlString);
                urlTemp = url;
                urlTemp = Regex.Replace(urlTemp, "href\\s*=\\s*", "");
                urlTemp = urlTemp.Replace("\"", "");
                urlTemp = urlTemp.Replace("\\", "/");
                urlTemp = HtmlPageHelper.GetUrlRoot(HttpContext.Current.Request.Url) + urlTemp;
                SaveHtmlCode(urlTemp);
            }
        }

        /// <summary>
        /// 通过HttpWebRequest页面链接的html代码
        /// </summary>
        /// <param name="urlString">页面链接地址</param>
        /// <returns>页面链接对应的html代码</returns>
        private string GetHtmlCodeFromUrl(string urlString)
        {
            HttpWebRequest hwRequest = (HttpWebRequest)WebRequest.Create(urlString);
            hwRequest.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            hwRequest.Accept = "*/*";
            hwRequest.KeepAlive = true;
            hwRequest.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse hwResponse = (HttpWebResponse)hwRequest.GetResponse();
            Stream streamResponse = hwResponse.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(streamResponse, System.Text.Encoding.GetEncoding("utf-8"));
            string strHtml = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            streamResponse.Close();
            hwResponse.Close();
            return strHtml;
        }

        /// <summary>
        /// 分析页面链接地址，生成静态页面保存的物理路径
        /// </summary>
        /// <param name="urlString">页面链接地址</param>
        /// <returns>静态页面保存的物理路径</returns>
        public static string GetHtmlPathFromUrl(string urlString)
        {
            Uri uri = new Uri(urlString);
            bool hasQus = uri.AbsoluteUri.IndexOf('?') > -1;
            bool hasDot = uri.AbsoluteUri.IndexOf('.') > -1;
            string filePath = HttpContext.Current.Request.PhysicalApplicationPath + "zeje.github.io";
            if (hasQus && hasDot)
            {
                string[] querys = uri.Query.Split(new char[] { '?', '&', '=' }, StringSplitOptions.RemoveEmptyEntries);
                filePath += uri.AbsolutePath.Insert(uri.AbsolutePath.IndexOf('.'), string.Join("", querys));
            }
            else if (hasQus)
            {
                string[] querys = uri.Query.Split(new char[] { '?', '&', '=' }, StringSplitOptions.RemoveEmptyEntries);
                filePath += uri.AbsolutePath + string.Join("", querys);
            }
            else if (hasDot)
            {
                //全部路径
                filePath += uri.AbsolutePath;
            }
            else
            {
                filePath += "\\Index.html";
            }
            filePath = filePath.Replace("/", "\\");
            filePath = filePath.Replace("\\\\", "\\");
            return filePath;
        }

        /// <summary>
        /// 正则表达式匹配出html代码中的超链接
        /// </summary>
        /// <param name="htmlCode">要找出超链接的html代码</param>
        /// <returns></returns>
        private ArrayList GetUrlLinkFromHtmlCode(string htmlCode)
        {
            ArrayList aList = new ArrayList();
            string strRegex = "href\\s*=\\s*(?:[\"'](?<1>[^\"'.#:]*)[\"'])";
            Regex r = new Regex(strRegex, RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(htmlCode);
            for (int i = 0; i <= m.Count - 1; i++)
            {
                string strNew = m[i].ToString().Replace("amp;", "");
                if (!aList.Contains(strNew))
                {
                    aList.Add(strNew);
                }
            }
            return aList;
        }


        public static string GetUrlRoot(Uri uri)
        {
            int lastIndex = uri.AbsoluteUri.LastIndexOf(uri.AbsolutePath);
            if (lastIndex > 0)
            {
                return uri.AbsoluteUri.Remove(lastIndex);
            }
            else
            {
                return uri.AbsoluteUri;
            }
        }
    }
}