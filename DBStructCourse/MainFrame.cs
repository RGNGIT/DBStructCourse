﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBStructCourse
{
    public partial class MainFrame : Form
    {
        public MainFrame()
        {
            InitializeComponent();
        }

        string Credentials = 
            "Server = localhost;" +
            "Integrated security = SSPI;" +
            "database = course";

        private void buttonAddDir_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    listBoxMainLog.Items.Add(database.AddPhonesData(textBoxDirPhoneType.Text, textBoxDirPhoneNum.Text));
                    break;
                case 1:
                    listBoxMainLog.Items.Add(database.AddLocaleType(textBoxDirLocaleType.Text));
                    break;
                case 2:
                    listBoxMainLog.Items.Add(database.AddConstructType(textBoxDirConstructType.Text));
                    break;
                case 3:
                    listBoxMainLog.Items.Add(database.AddEventType(textBoxDirEventType.Text));
                    break;
            }
            database.Dispose();
            DirTabUpdate();
            ComboUpdates();
        }

        string[] DirTables = new string[4] 
        { 
            "Db_Phones", 
            "Db_LocaleType", 
            "Db_ConstructType", 
            "Db_EventType"
        };

        // SELECT - FROM - -

        void DirTabUpdate()
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewDir.DataSource = database.ReturnTable("*", DirTables[tabControl2.SelectedIndex], null).Tables[0].DefaultView;
            database.Dispose();
        }

        void MainTabUpdate(int Index)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            switch (Index)
            {
                case 0: // Обновить таблицу нас.пунктов
                    dataGridViewLocale.DataSource = database.ReturnTable(
                        "Db_Locale.Код, Название_НасПункта, Кр_Название_НасПункта, Db_LocaleType.ТипНасПункт as Тип", 
                        "Db_Locale, Db_LocaleType", 
                        "WHERE Db_Locale.КодТипа = Db_LocaleType.Код").Tables[0].DefaultView;
                    break;
            }
            database.Dispose();
        }

        List<string> BufferListUpdate(int Index)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            List<string> Temp = new List<string>();
            switch (Index)
            {
                case 0: // Заполнение типов нас.пунктов
                    dataGridViewListReturner.DataSource = database.ReturnTable("ТипНасПункт", "Db_LocaleType", null).Tables[0].DefaultView;
                    for(int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
                    {
                        Temp.Add(dataGridViewListReturner.Rows[i].Cells[0].Value.ToString());
                    }
                    break;
                case 1: // ОблОргов

                    break;
            }
            database.Dispose();
            return Temp;
        }

        void ComboUpdates()
        {
            comboBoxLocaleType.Items.Clear(); // При добавлении нас.пункта
            foreach(string i in BufferListUpdate(0))
            {
                comboBoxLocaleType.Items.Add(i);
            }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirTabUpdate();
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainTabUpdate(tabControlMain.SelectedIndex);
            ComboUpdates();
        }

        // Добавления в главные таблички

        int GetDirCode(string Table, string ToFind) // Вернуть код (итератор) из справочника
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewListReturner.DataSource = database.ReturnTable("*", Table, null).Tables[0].DefaultView;
            for(int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
            {
                if(dataGridViewListReturner.Rows[i].Cells[1].Value.ToString() == ToFind)
                {
                    return Convert.ToInt32(dataGridViewListReturner.Rows[i].Cells[0].Value);
                }
            }
            database.Dispose();
            return -1;
        }

        private void buttonAddLocale_Click(object sender, EventArgs e) // Добавить нас.пункт
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.AddLocale(textBoxLocaleName.Text, textBoxLocaleShortName.Text, GetDirCode("Db_LocaleType", comboBoxLocaleType.SelectedItem.ToString())));
            MainTabUpdate(0);
            database.Dispose();
        }
    }
}