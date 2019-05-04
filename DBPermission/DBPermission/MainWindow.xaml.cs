using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace DBPermission
{
    enum Codes { ERRO, INFO, WARN, SUCC }
    public partial class MainWindow : Window
    {
        NotifyIcon nIcon = new NotifyIcon();
        System.Timers.Timer timer;
        Handler handler;

        //Poroperties
        public ObservableCollection<string> ConsoleList { get { return handler.ConsoleList; } }
        public ObservableCollection<User> ObservableUserList { get { return handler.UserList; } }
        public string SelectedListBoxValue { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            KillProcess();
            handler = new Handler();
            SetUIFields();
            SetTimer(false);
        }

        //Events
        private void button_SaveAdress_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_ServerName.Text == String.Empty || 
                textBox_Period.Text == String.Empty ||
                textBox_UserName.Text == String.Empty)
            {
                handler.ConsoleLog("Missing value(s) from the textboxes!",Codes.WARN);
            }
            else
            {
                handler.CheckAndWrite(textBox_ServerName.Text,
                                      textBox_Period.Text,
                                      textBox_DBName.Text,
                                      textBox_UserName.Text,
                                      passwordBox_Password.Password);
                passwordBox_Password.Clear();
                handler.ConsoleLog("Changes have been saved successfully!", Codes.SUCC);
                timer.Stop();
                SetTimer(false);
            }
        }
        private void button_Restart_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            SetTimer(true);
            handler.ConsoleLog("Timer has restarted.",Codes.INFO);
        }
        private void button_Restore_Click(object sender, RoutedEventArgs e)
        {
            handler.ConsoleLog("Stored values are restored from XML.", Codes.INFO);
            SetUIFields();
        }
        private void button_AddNew_Click(object sender, RoutedEventArgs e)
        {
            string userName = textBox_AddNew.Text;
            if(userName == String.Empty)
            {
                handler.ConsoleLog("Missing username!", Codes.WARN);
            }
            else
            {
                if(handler.UserAlreadyExists(userName))
                {
                    handler.ConsoleLog("Existing username!", Codes.WARN);
                }
                else
                {
                    textBox_AddNew.Clear();
                    User u = new User(userName);
                    handler.UserList.Add(u);
                    handler.AddUser(u);
                }
            }
        }
        private void button_Remove_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedListBoxValue != null && SelectedListBoxValue != String.Empty)
            {
                User user = new User(SelectedListBoxValue.ToString());
                handler.UserList.Remove(user);
                handler.RemoveUser(user);
                handler.ConsoleLog("User removed from list: " + user.Name, Codes.INFO);
            }
        }
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.Width = 1070;
        }
        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.Width = 720;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConsoleList.Clear();
        }
        protected override void OnStateChanged(EventArgs e)
        {
            //if (WindowState == WindowState.Minimized) this.Hide();
            //this.WindowState = WindowState.Minimized;
            //base.OnStateChanged(e);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);

            nIcon.Icon = new Icon("dbp.ico");
            nIcon.ShowBalloonTip(5000, "DBP", "Database permission for SQL Server", ToolTipIcon.Info);
            nIcon.MouseDoubleClick += NIcon_MouseDoubleClick;
            nIcon.Visible = true;
        }
        private void NIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            nIcon.Visible = false;
        }

        //Methods
        private void KillProcess()
        {
            string PN = "DBPermission";
            Process[] processlist = Process.GetProcesses();
            foreach (Process p in processlist)
            {
                if (p.ProcessName.Contains(PN) && p.Id != Process.GetCurrentProcess().Id)
                {
                    p.Kill();
                }
            }
        }
        private void SetUIFields()
        {
            try
            {
                textBox_ServerName.Text = handler.Server;
                textBox_Period.Text = handler.Period;
                textBox_DBName.Text = handler.DBName;
                textBox_UserName.Text = handler.UserName;
            }
            catch (Exception)
            {
                handler.ConsoleLog("Error occured while processing the XML file!", Codes.ERRO);
                this.Close();
            }
        }
        private void SetTimer(bool startFirst)
        {
            if(startFirst)
                Tick(this, null);

            timer = new System.Timers.Timer(handler.GetPeriodInMs());
            timer.Elapsed += Tick;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }
        private void Tick(object sender, ElapsedEventArgs e)
        {
            var result = GetDBChanges();
            if(result != null)
                SetPermissions(result);
        }
        private void BeginInvoke(string message, Codes code = Codes.INFO)
        {
            DispatcherOperation op = Dispatcher.BeginInvoke((Action)(() => {
                handler.ConsoleLog(message, code);
            }));
        }
        private List<string> GetDBChanges()
        {
            BeginInvoke("Prepare for SQL connection");
            Dispatcher.Invoke(() => handler.ReadFile());
            string connetionString;
            string[] ipAndPort = handler.CheckAndGetIPAddress();

            string password = Security.Decrypt(handler.Password);
            if (password == String.Empty)
            {
                BeginInvoke("Decryption is impossible!!", Codes.ERRO);
                return null;
            }

            if (ipAndPort != null)
                connetionString = "Server=" + ipAndPort[0] + "," + ipAndPort[1] + ";User ID=" + handler.UserName + ";Password=" + password + ";";
            else
                connetionString = "Server=" + handler.Server + ";User ID=" + handler.UserName + ";Password=" + password + ";";

            List<string> newDBs = new List<string>();
            using (SqlConnection connection = new SqlConnection(connetionString))
            {
                try
                {
                    connection.Open();
                    BeginInvoke("SQL Connection is opened successfully!", Codes.SUCC);
                    List<string> cDBNames = new List<string>();
                    string dbNameLocal = handler.DBName;
                    string query = String.Empty;

                    if (dbNameLocal != "")
                    {
                        if (dbNameLocal[0] == '*')
                        {
                            dbNameLocal = dbNameLocal.Replace("*", string.Empty);
                            query = "SELECT name FROM master.dbo.sysdatabases WHERE name like '%" + dbNameLocal + "'";
                        }
                        else if (handler.DBName[handler.DBName.Length - 1] == '*')
                        {
                            dbNameLocal = dbNameLocal.Replace("*", string.Empty);
                            query = "SELECT name FROM master.dbo.sysdatabases WHERE name like '" + dbNameLocal + "%'";
                        }
                        else
                            query = "SELECT name FROM master.dbo.sysdatabases WHERE name like '%" + dbNameLocal + "%'";
                    }
                    else
                        query = "SELECT name FROM master.dbo.sysdatabases";

                    BeginInvoke("Query is set: " + query, Codes.INFO);

                    SqlCommand cmd = new SqlCommand(query, connection);
                    var result = cmd.ExecuteReader();
                    BeginInvoke("Query is exetuted!", Codes.INFO);
                    while (result.Read())
                        cDBNames.Add(result.GetString(0));

                    if (cDBNames.Count == 0)
                        return null;

                    return Dispatcher.Invoke(() => handler.CompareLists(cDBNames, handler.GetDatabaseNames()));
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                    BeginInvoke("An error occured during the SQL connection or execution!", Codes.ERRO);
                }
                finally
                {
                    connection.Close();
                    BeginInvoke("SQL connection is closed!", Codes.INFO);
                }
                return null;
            }
        }
        public void SetPermissions(List<string> newDBs)
        {
            string connetionString;
            string[] ipAndPort = handler.CheckAndGetIPAddress();
            string password = Security.Decrypt(handler.Password);
            if (password == String.Empty)
                return;

            foreach (string databaseName in newDBs)
            {
                if (ipAndPort != null)
                    connetionString = "Server=" + ipAndPort[0] + "," + ipAndPort[1] + ";Initial Catalog=" + databaseName + ";User ID=" + handler.UserName + ";Password=" + password + ";";
                else
                    connetionString = "Server=" + handler.Server + ";Initial Catalog=" + databaseName + ";User ID=" + handler.UserName + ";Password=" + password + ";";

                using (SqlConnection c = new SqlConnection(connetionString))
                {
                    try
                    {
                        c.Open();
                        foreach (User user in handler.UserList)
                        {
                            string query = "grant select to " + user.Name + ";" +
                                            "deny insert to " + user.Name + ";" +
                                            "deny update to " + user.Name + ";" +
                                            "deny execute to " + user.Name + ";";
                            SqlCommand command = new SqlCommand(query, c);
                            var s = command.ExecuteNonQuery();
                            BeginInvoke("Permissions are set to " + user.Name + " on " + databaseName + "!", Codes.SUCC);
                        }
                    }
                    catch (Exception)
                    {
                        BeginInvoke("Error occured while setting permissions!", Codes.ERRO);
                    }
                    finally
                    {
                        c.Close();
                        BeginInvoke("Connection is closed!");
                    }
                }
            }
        }
    }
}