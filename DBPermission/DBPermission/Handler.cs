using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Data.SqlClient;

namespace DBPermission
{
    class Handler
    {
        public static string KEY;

        readonly string fileName;
        readonly string ServerElement;
        readonly string rootName;
        readonly string userElement;
        readonly string periodElement;
        readonly string userNameElement;
        readonly string passwordElement;
        readonly string DBNameElement;
        readonly string databaseMainElement;
        readonly string dataBaseElement;
        readonly string UserMainElement;

        readonly string defaultIP;
        readonly string defaultUserName;
        readonly string defaultPassword;
        readonly int defaultPeriod;
        XDocument XDoc;

        //Properties
        public string Server { get { return XDoc.Root.Element(ServerElement).Value; } }
        public string Period { get { return XDoc.Root.Element(periodElement).Value; } }
        public string UserName { get { return XDoc.Root.Element(userNameElement).Value; } }
        public string Password { get { return XDoc.Root.Element(passwordElement).Value; } }
        public string DBName { get { return XDoc.Root.Element(DBNameElement).Value; }}
        public ObservableCollection<User> UserList { get; set; }
        public ObservableCollection<string> ConsoleList { get; set; }

        public Handler()
        {
            ConsoleList = new ObservableCollection<string>();
            XDoc = new XDocument();
            SetEncryptionKey();

            fileName = "ServerConnection.xml";
            ServerElement = "Server";
            rootName = "ServerConnectionInfo";
            periodElement = "Period";
            userNameElement = "UserName";
            passwordElement = "Password";
            DBNameElement = "DatabaseName";
            databaseMainElement = "Databases";
            dataBaseElement = "Database";
            UserMainElement = "Users";
            userElement = "User";
            defaultPeriod = 30;
            defaultIP = "127.0.0.1";
            defaultUserName = "Admin";
            defaultPassword = "Admin";
            ConsoleLog("Default values are initialized!",Codes.INFO);
            
            ReadFile();
            InitializeList();
        }

        public void ReadFile()
        {
            CheckFile();
            XDoc = XDocument.Load(fileName);
            ConsoleLog("XML is read!", Codes.INFO);
        }
        private void InitializeList()
        {
            IEnumerable<User> query = from x in XDoc.Root.Element(UserMainElement).Elements(userElement) select new User(x.Element("Name").Value);
            if (!query.Any())
                UserList = new ObservableCollection<User>();
            else
                UserList = new ObservableCollection<User>(query);
            ConsoleLog("User list is initialized!", Codes.INFO);
        }
        private void WriteFile(string server, string period, string dbName, string userName, string password)
        {
            ReadFile();

            XElement se = XDoc.Root.Element(ServerElement);
            se.Value = server;

            XElement periodEle = XDoc.Root.Element(periodElement);
            periodEle.Value = period;

            XElement dbEl = XDoc.Root.Element(DBNameElement);
            dbEl.Value = dbName;

            XElement userEl = XDoc.Root.Element(userNameElement);
            userEl.Value = userName;

            if(password != String.Empty)
            {
                XElement pwEl = XDoc.Root.Element(passwordElement);
                pwEl.Value = Security.Encrypt(password);
            }

            XDoc.Save(fileName);
            ConsoleLog("XML is overwritten!", Codes.INFO);
        }
        private void CheckFile()
        {
            ConsoleLog("XML file is checked!", Codes.INFO);
            if (!File.Exists(fileName))
                CreateFile();
        }
        public void CheckAndWrite(string server, string period, string dbName, string userName, string password)
        {
            CheckFile();
            try
            {
                WriteFile(server, period, dbName, userName, password);
            }
            catch (NullReferenceException)
            {
                ConsoleLog("Unexpected error occured while reading XML file!", Codes.ERRO);
            }
        }
        public bool UserAlreadyExists(string username)
        {
            foreach (User i in UserList)
                if (i.Name == username)
                    return true;

            return false;
        }
        public void AddUser(User user)
        {
            ReadFile();
            XElement UsersElement = XDoc.Root.Element(UserMainElement);
            UsersElement.Add(new XElement(userElement, new XElement("Name",user.Name)));
            XDoc.Save(fileName);
            ConsoleLog("New user is added to list!", Codes.INFO);
        }
        public void RemoveUser(User user)
        {
            ReadFile();
            XElement cUser = (from x in XDoc.Root.Element(UserMainElement).Elements(userElement) where x.Element("Name").Value == user.Name select x).FirstOrDefault();
            if (cUser != null)
                cUser.Remove();
            XDoc.Save(fileName);
        }
        public void CreateFile()
        {
            XElement xelement = new XElement(rootName);
            xelement.Add(new XElement(ServerElement, defaultIP), 
                         new XElement(periodElement, defaultPeriod),
                         new XElement(userNameElement, defaultUserName),
                         new XElement(passwordElement, defaultPassword),
                         new XElement(DBNameElement),
                         new XElement(databaseMainElement),
                         new XElement(UserMainElement));
            XDoc.Add(xelement);
            XDoc.Save(fileName);
            ConsoleLog("New XML file is created!", Codes.INFO);
        }
        public int GetPeriodInMs()
        {
            int p = 0;
            var xel = XDoc.Root.Element(periodElement);
            if (xel != null && int.TryParse(xel.Value, out p))
                return p * 60 * 1000;
            return defaultPeriod * 60 * 1000;
        }
        public string[] CheckAndGetIPAddress()
        {
            if (Server.Contains(':'))
            {
                string[] ipAndPort = new string[2];
                ipAndPort[0] = Server.Substring(0, Server.IndexOf(':'));
                ipAndPort[1] = Server.Substring(Server.IndexOf(':') + 1);
                return ipAndPort;
            }
            return null;
        }
        public List<string> CompareLists(List<string> SQLDBNames, List<string> NamesFromXML)
        {
            List<string> newDBs = SQLDBNames.Except(NamesFromXML).ToList();
            List<string> oldDBs = NamesFromXML.Except(SQLDBNames).ToList();
            ConsoleLog("Number of new databases since last check: " + newDBs.Count, Codes.INFO);
            ConsoleLog("Number of outdated databases since last check: " + oldDBs.Count, Codes.INFO);

            //Remove old name
            XDoc.Root.Element(databaseMainElement).Elements(dataBaseElement).Where(x => oldDBs.Contains(x.Value)).Remove();
            ConsoleLog("Unnecessary databases are deleted!", Codes.INFO);

            //Add new names
            XElement databaseMain = XDoc.Root.Element(databaseMainElement);
            foreach (string i in newDBs)
                databaseMain.Add(new XElement(dataBaseElement, i));

            XDoc.Save(fileName);
            ConsoleLog("New databases saved to XML!", Codes.INFO);

            return newDBs;
        }
        public List<string> GetDatabaseNames()
        {
            return (XDoc.Root.Element(databaseMainElement).Elements(dataBaseElement).Select(x=> x.Value)).ToList();
        }
        public void ConsoleLog(string message, Codes codes = Codes.INFO)
        {
            ConsoleList.Add(codes.ToString() + "\t" + message);
        }
        private string ReadKey()
        {
            string path = @"C:\DatabasePermission\Security.txt";

            if (!File.Exists(path))
            {
                ConsoleLog("Security file cannot be found!", Codes.ERRO);
                return String.Empty;
            }

            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string key = sr.ReadLine();
                if (key == null)
                {
                    ConsoleLog("Key is missing from file!", Codes.ERRO);
                    return String.Empty;
                }
                ConsoleLog("Key is found!", Codes.INFO);
                return key;
            }
            catch (Exception ex)
            {
                ConsoleLog("Error while reading key!", Codes.ERRO);
                return String.Empty;
            }
        }
        private void SetEncryptionKey()
        {
            KEY = ReadKey();
        }
    }
}
