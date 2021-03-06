﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.OleDb;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Login1
{
    public partial class MainWindow : Window
    {
        public OleDbConnection dbase;
        public MainWindow()
        {
            InitializeComponent();
            DBConnect();
        }
        public void DBConnect()
        {
            try
            {
                string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Users.mdb;";
                dbase = new OleDbConnection(connectionString);
                dbase.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
        private bool IsNullReg()
        {
  
            if (Login.Text.Length > 0 && Password.Password.Length > 0) return true;
            else
            {
                Message.Foreground = Brushes.Red;
                Message.Text = "[Ошибка] Вы ничего не ввели!";
                return false;
            }
        }
        private bool IsEn(string str)
        {
            bool en = true;
            char[] arr_en = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                            '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            for (int i = 0; i < str.Length; i++) // перебираем символы
            {
                if (en)
                {
                    for (int f1 = 0; f1 < arr_en.Length; f1++)
                    {
                        if (str[i] == arr_en[f1])
                        {
                            en = true;
                            break;
                        }
                        else 
                            en = false;
                    }
                }
            }
            return en;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(IsNullReg())
            {
                try
                {

                    string query = "SELECT COUNT(*) FROM users WHERE u_login = '" + Login.Text + "'and u_password = '" + Password.Password + "'";

                    OleDbDataAdapter ada = new OleDbDataAdapter(query, dbase);

                    DataTable dt = new DataTable();
                    ada.Fill(dt);
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        English.MainWindow taskWindow = new English.MainWindow(Login.Text);
                        taskWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        Message.Foreground = Brushes.Red;
                        Message.Text = "[Ошибка] Логин или пароль не верный";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (IsNullReg())
            {
                if (Password.Password.Length >= 6)
                {
                    if (IsEn(Password.Password) && IsEn(Login.Text))
                    {
                        try
                        {
                            string query1 = "SELECT COUNT(*) FROM users WHERE u_login = '" + Login.Text + "'";

                            OleDbDataAdapter ada = new OleDbDataAdapter(query1, dbase);

                            DataTable dt = new DataTable();
                            ada.Fill(dt);
                            if (dt.Rows[0][0].ToString() == "1")
                            {
                                Message.Foreground = Brushes.Red;
                                Message.Text = "[Ошибка] Пользователь с таким логинам уже зарегестрирован!";
                            }
                            else
                            {
                                string query2 = "INSERT INTO users (u_login, u_password)" + "VALUES('" + Login.Text + "', '" + Password.Password + "')";

                                OleDbCommand command = new OleDbCommand(query2, dbase);

                                command.ExecuteNonQuery();
                                Message.Foreground = Brushes.Green;
                                Message.Text = "Успешно зарегистрировались!";
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка: " + ex.Message);
                        }
                    }
                    else
                    {
                        Message.Foreground = Brushes.Red;
                        Message.Text = "[Ошибка] Доступна только английская раскладка";
                    }

                }
                else
                {
                    Message.Foreground = Brushes.Red;
                    Message.Text = "[Ошибка] Вы должны ввести пароль состосоящий из не менее 6 символов";
                }
            }
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
