using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Zeje.Staticize_.AppCode
{
    public class HttpResponseFilterWrapper : Stream
    {
        private Stream inner;
        private HttpContext context;
        public HttpResponseFilterWrapper(System.IO.Stream s, HttpContext context)
        {
            this.inner = s;
            this.context = context;
        }

        public override bool CanRead
        {
            get { return inner.CanRead; }
        }

        public override bool CanSeek
        {
            get { return inner.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return inner.CanWrite; }
        }

        public override void Flush()
        {
            inner.Flush();
        }

        public override long Length
        {
            get { return inner.Length; }
        }

        public override long Position
        {
            get { return inner.Position; }
            set { inner.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return inner.Read(buffer, offset, count);
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            return inner.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            inner.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!context.Response.StatusCode.Equals(200))
            {
                return;
            }
            inner.Write(buffer, offset, count);
            //if (!context.HttpContext.Request.FilePath.ToLower().StartsWith("/admin"))
            //当前请求不是后台管理；并且返回客户端是html才生成静态页面
            if (!context.Request.FilePath.ToLower().StartsWith("/admin") && context.Response.ContentType.Equals("text/html"))
            {
                //静态页面保存路径信息
                string htmlPath = HtmlPageHelper.GetHtmlPathFromUrl(context.Request.Url.AbsoluteUri);
                string direcHtmlPath = Path.GetDirectoryName(htmlPath);
                if (!Directory.Exists(direcHtmlPath))
                {
                    Directory.CreateDirectory(direcHtmlPath);
                }
                //获取返回客户端的html代码，并进行压缩处理
                string htmlCode = System.Text.Encoding.UTF8.GetString(buffer);
                string isZipHtml = System.Configuration.ConfigurationManager.AppSettings["IsCompressed"];
                //如果“IsCompressed”的值为空或0，则表示不压缩
                if (!string.IsNullOrEmpty(isZipHtml) && !isZipHtml.Equals(0))
                {
                    htmlCode = Regex.Replace(htmlCode, "^\\s*", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
                    htmlCode = Regex.Replace(htmlCode, "\\r\\n", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
                    htmlCode = Regex.Replace(htmlCode, "<!--*.*?-->", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
                }
                //保存文件，这里不能用File.WriteAllText
                File.AppendAllText(htmlPath, htmlCode);
            }
        }
    }
}