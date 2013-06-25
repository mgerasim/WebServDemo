using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using WebServDemo.Models;
using WebServDemo.Repositories;
using System.Net;

namespace WebServDemo
{
    public abstract class HttpServerBase
    {

        protected int port;
        TcpListener listener;
        bool is_active = true;

        public HttpServerBase(int port)
        {
            this.port = port;
        }

        public void listen()
        {
            listener = new TcpListener(port);
            listener.Start();
            while (is_active)
            {
                TcpClient s = listener.AcceptTcpClient();
                HttpProcessor processor = new HttpProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }
        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }

    public class HttpServer : HttpServerBase
    {
        public HttpServer(int port)
            : base(port)
        {
        }
        public override void handleGETRequest(HttpProcessor p)
        {

            Console.WriteLine("request: {0}", p.http_url);
            p.writeSuccess();
            p.outputStream.WriteLine("<html><body><h1>test server</h1>");
            p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
            p.outputStream.WriteLine("url : {0}", p.http_url);

            p.outputStream.WriteLine("<form method=post action=/form>");
            p.outputStream.WriteLine("<input type=text name=foo value=foovalue>");
            p.outputStream.WriteLine("<input type=submit name=bar value=barvalue>");
            p.outputStream.WriteLine("</form>");
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            Console.WriteLine("POST request: {0}", p.http_url);
            string data = inputData.ReadToEnd();

            p.writeSuccess();
            p.outputStream.WriteLine("<html><body><h1>test server</h1>");
            p.outputStream.WriteLine("<a href=/test>return</a><p>");
            p.outputStream.WriteLine("postbody: <pre>{0}</pre>", data);


        }

        public void handleGETtest(HttpProcessor p)
        {
            Console.WriteLine("request: {0}", p.http_url);
            p.writeSuccess();
            p.outputStream.WriteLine("<html><body><h1>Test</h1>");
            
        }

        public void handleDefault(HttpProcessor p)
        {
            Console.WriteLine("request: {0}", p.http_url);
            p.writeSuccess();
            p.outputStream.WriteLine("<html><body><h1>Hello World</h1>");
        }

        public void handleRoot(HttpProcessor p)
        {
            Console.WriteLine("request: {0}", p.http_url);
            p.writeSuccess();
            p.outputStream.WriteLine("<html><body><h1>Root</h1>");
        }

        public void handleGETguestbook(HttpProcessor p)
        {
            Console.WriteLine("request: {0}", p.http_url);
            p.writeSuccess();
            p.outputStream.WriteLine("<head>");
		    p.outputStream.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />");
            p.outputStream.WriteLine("</head>");
            p.outputStream.WriteLine("<html>");
            p.outputStream.WriteLine("  <body>");
            p.outputStream.WriteLine("      <p>");
            
            Guestnote theGuestnote = new Guestnote();
            foreach(Guestnote _guestnote in theGuestnote.GetAll())
            {
                p.outputStream.WriteLine("<p><h4>{0} пишет:</h4> </p>", _guestnote.User);
                p.outputStream.WriteLine("<p>{0}</p>", _guestnote.Message);
                p.outputStream.WriteLine("<hr>");
            }
            p.outputStream.WriteLine("      </p>");
            p.outputStream.WriteLine("  </body>");
            p.outputStream.WriteLine("</html>");
            
        }

        public void handlePOSTguestbook(HttpProcessor p, StreamReader inputData)
        {
            Console.WriteLine("request: {0}", p.http_url);
            string data = inputData.ReadToEnd();
            string User="";
            string Message="";
            foreach(string item in data.Split('&'))
            {
                string[] parts = item.Split('=');
                if(parts[0] == "user")
                {
	                User = parts[1];	                
                }
                if(parts[0] == "message")
                {
	                Message = parts[1];	                
                }
            }
            p.writeSuccess();

            p.outputStream.WriteLine("<head>");
            p.outputStream.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />");
            p.outputStream.WriteLine("</head>");

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            p.outputStream.WriteLine("<html>");
            p.outputStream.WriteLine("  <body>");
            p.outputStream.WriteLine("      <h1>Сообщение успешно отправлено!</h1>");
            p.outputStream.WriteLine("      <p><a href=\"/guestbook\">Перейти в Гостевую книгу</a></p>");
            p.outputStream.WriteLine("      <p><a href=\"/new\">Добавить Гостевую запись</a></p>");
            p.outputStream.WriteLine("  </body>");
            p.outputStream.WriteLine("</html>");
            
            Guestnote theGuestnote = new Guestnote();
            theGuestnote.User = Uri.UnescapeDataString(User);
            theGuestnote.Message = Uri.UnescapeDataString(Message);
            theGuestnote.Save(theGuestnote);
        }

        public void handleGETnew(HttpProcessor p)
        {
            Console.WriteLine("request: {0}", p.http_url);
            p.writeSuccess();
            p.outputStream.WriteLine("<head>");
            p.outputStream.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />");
            p.outputStream.WriteLine("</head>");
            p.outputStream.WriteLine("<form method=post action=/guestbook>");
            p.outputStream.WriteLine("<input type=text name=user value=Имя>");
            p.outputStream.WriteLine("<input type=text name=message value=Сообщение>");
            p.outputStream.WriteLine("<input type=submit name=bar value=Сохранить>");
            p.outputStream.WriteLine("</form>");
        }

        public void handleGETproxy(HttpProcessor p)
        {
            Console.WriteLine("request: {0}", p.http_url);
            p.writeSuccess();
            string[] parts = p.http_url.Split('?');
            
            string param_name = parts[1].Split('=')[0];
            string param_value = parts[1].Split('=')[1];

            if (!param_name.Equals("url")) 
            {
                return;
            }
            Console.WriteLine("url: {0}", param_value);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(param_value);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader res = new StreamReader(response.GetResponseStream());
            string answer = res.ReadToEnd();
            Console.WriteLine(answer);
            p.outputStream.WriteLine(answer);
        }
    }

}
