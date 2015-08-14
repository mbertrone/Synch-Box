﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
//using System.Windows;

namespace SynchBox_Client
{   
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SyncSocketClient sender_SyncSocketClient;
        NetworkStream sender_stream;
        proto_client protoClient;
        
        Logging log = new Logging();

        string username = "";
        string uid = "";
        string ip = "";
        string port = "";
        bool connected;

        private void initializeSessionParam()
        {
            username = "";
            uid = "";
            ip = "";
            port = "";
            connected = false;
        }

        public MainWindow()
        {
            InitializeComponent();
            log.WriteToLog("-----CLIENT STARTED------");
            initializeSessionParam();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void b_login_login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                log.WriteToLog("login click");
                //check non null textbox
                log.WriteToLog("validating textboxes");
                validateTextBoxes();
                //throw new Exception("Complete Login/Registration information");

                log.WriteToLog("if ip!=ip || port!=port || !connected -> new SynsocketCliuent");
                if ( (!ip.Equals(ip_tb.Text))  || (!port.Equals(port_tb.Text)) || (!connected) ) {
                    log.WriteToLog("tying to get new SyncSocketClient");
                    sender_SyncSocketClient = new SyncSocketClient(ip_tb.Text, int.Parse(port_tb.Text),log);
                    log.WriteToLog("trying to connecting");
                    sender_SyncSocketClient.Connect();
                    log.WriteToLog("connection successfull");
                    ip = ip_tb.Text;
                    port = port_tb.Text;
                    connected = true;
                    log.WriteToLog("connection successfull -> " + ip + port);

                    log.WriteToLog("getting sender stream and protoclient");

                    sender_stream = sender_SyncSocketClient.getStream();
                    protoClient = new proto_client(sender_stream,log);
                    log.WriteToLog("sender stream and protoclient succedeed! OK");
                }

               // proto_client.login_c log = new proto_client.login_c();
                log.WriteToLog("trying protoclient.do_login");
                var login_result = protoClient.do_login(username_tb.Text, password_tb.Password);

                log.WriteToLog("user logged? "+login_result.is_logged);
                if (!login_result.is_logged)
                    throw new Exception("Login Failed!");

                
                //set them to the calass params for login
                username = login_result.username;
                uid = login_result.password;

                log.WriteToLog("setting session username & uid" +username + uid);

                login_ui();

                log.WriteToLog("set login name, clear textbox, disable text box");
                //istantiate myprotocol

                //myprotocol login
                //throws exception

                //set session, user etc parameters

                //UI login-> logout & not possible to login again

                //set them (textbox values) to the class params

                //spostati su home
            }
            catch (Exception exc)
            {
                MessageBox.Show("not possible to login or connect to server! Error : " + exc.ToString());
                log.WriteToLog("not possible to login or connect to server! Error : " + exc.ToString());
            }
        }

        private void b_register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                log.WriteToLog("register button");
                //check non null textbox
                log.WriteToLog("validating textboxes");
                validateTextBoxes();
                //throw new Exception("Complete Login/Registration information");

                log.WriteToLog("if ip!=ip || port!=port || !connected -> new SynsocketCliuent");
                if ((!ip.Equals(ip_tb.Text)) || (!port.Equals(port_tb.Text)) || (!connected))
                {
                    log.WriteToLog("tying to get new SyncSocketClient");
                    sender_SyncSocketClient = new SyncSocketClient(ip_tb.Text, int.Parse(port_tb.Text), log);
                    log.WriteToLog("trying to connecting");
                    sender_SyncSocketClient.Connect();
                    log.WriteToLog("connection successfull");
                    ip = ip_tb.Text;
                    port = port_tb.Text;
                    connected = true;
                    log.WriteToLog("connection successfull -> " + ip + port);

                    log.WriteToLog("getting sender stream and protoclient");

                    sender_stream = sender_SyncSocketClient.getStream();
                    protoClient = new proto_client(sender_stream, log);
                    log.WriteToLog("sender stream and protoclient succedeed! OK");
                }

                // proto_client.login_c log = new proto_client.login_c();
                log.WriteToLog("trying protoclient.do_register");

                var login_result = protoClient.do_register(username_tb.Text, password_tb.Password);

                log.WriteToLog("user registered/logged? " + login_result.is_logged);
                if (!login_result.is_logged)
                    throw new Exception("Registration Failed!");

                //set them to the calass params for login
                username = login_result.username;
                uid = login_result.password;

                log.WriteToLog("setting session username & uid" + username + uid);

                login_ui();

                log.WriteToLog("set login name, clear textbox, disable text box");
                //istantiate myprotocol

                //myprotocol login
                //throws exception

                //set session, user etc parameters

                //UI login-> logout & not possible to login again

                //set them (textbox values) to the class params

                //spostati su home

                
            }
            catch (Exception exc)
            {
                MessageBox.Show("not possible to login or connect to server! Error : " + exc.ToString());
                log.WriteToLog("not possible to login or connect to server! Error : " + exc.ToString());
            }
        }

        private void setNameLogin() {
            welcome_l.Content = "welcome, " + username + " @ " + ip + ":" + port;
        }

        private void unsetNameLogin() {
            welcome_l.Content = "Welcome, no user logged in !";
        }


        private void validateTextBoxes() { 
            if(username_tb.Text.Equals(""))
                throw new Exception ("No username!");
            if (password_tb.Password.Equals("")) 
                throw new Exception("No password!");
            if (ip_tb.Text.Equals(""))
                throw new Exception("No IP!");
            if (port_tb.Text.Equals(""))
                throw new Exception("No Port!");
            try{
             int t;
             t = int.Parse(port_tb.Text);
             if (t <= 0 || t > 65535)
                 throw new Exception("port exc");
            }
            catch (Exception exc)
            {
                throw new Exception("Port Format not correct! Must Be a number (0..65535)");
            }
        }

        private void login_ui() {
            setNameLogin();
            //clearTextBox();
            disableTextBox();

        }

        private void logout_ui()
        {
            clearTextBox();
            enableTextBox();
            unsetNameLogin();
        }

        private void clearTextBox() {
            username_tb.Clear();
            password_tb.Clear();
            //ip_tb.Clear();
            //port_tb.Clear();
        }

        private void enableTextBox()
        {   username_tb.IsEnabled = true;
            password_tb.IsEnabled = true;
            ip_tb.IsEnabled = true;
            port_tb.IsEnabled = true;
            b_login_login.Visibility = Visibility.Visible;
            b_register.Visibility = Visibility.Visible;
            b_logout_login.Visibility = Visibility.Hidden;
        }
        private void disableTextBox()
        {   username_tb.IsEnabled = false;
            password_tb.IsEnabled = false;
            ip_tb.IsEnabled = false;
            port_tb.IsEnabled = false;
            b_login_login.Visibility = Visibility.Hidden;
            b_register.Visibility = Visibility.Hidden;
            b_logout_login.Visibility = Visibility.Visible;
        }

        private void b_logout_login_Click(object sender, RoutedEventArgs e)
        {
            //do logout
            username = "";
            uid = "";

            //ui logout
            logout_ui();
        }
    }
}