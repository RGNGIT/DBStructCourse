using System;
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
            MainTabUpdate(tabControlMain.SelectedIndex);
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
                        "Db_Locale.Код, Название_НасПункта as Название, Кр_Название_НасПункта as КраткоеНазвание, Db_LocaleType.ТипНасПункт as Тип", 
                        "Db_Locale, Db_LocaleType", 
                        "WHERE Db_Locale.КодТипа = Db_LocaleType.Код").Tables[0].DefaultView;
                    break;
                case 1: // Обновить таблицу областных организаций
                    dataGridViewRegion.DataSource = database.ReturnTable(
                        "Db_Region.Код, Название_ОблОрг as Название, Кр_Назв_ОблОрг as КраткоеНазвание, ЭлАдрес_ОблОрг as ЭлАдрес, Db_Locale.Название_НасПункта as НаселенныйПункт",
                        "Db_Region, Db_Locale",
                        "WHERE Db_Region.КодНасПункта = Db_Locale.Код").Tables[0].DefaultView;
                    break;
                case 2:
                    dataGridViewConstruct.DataSource = database.ReturnTable(
                        "Db_Construct.Код, Название_Сооруж as Название, Кр_Название_Сооруж as КраткоеНазвание, ДатаПринятия_Сооруж as ДатаПринятия, Вместимость_Сооруж as Вместимость, Площадь_Сооруж as Площадь, Db_ConstructType.Тип_Сооруж as Тип, Db_Region.Название_ОблОрг as ОбластнаяОрганизация, Db_Address.АдресЗнач as Адрес",
                        "Db_Construct, Db_ConstructType, Db_Region, Db_Address",
                        "WHERE Db_Construct.КодТипа = Db_ConstructType.Код AND Db_Construct.КодОблорг = Db_Region.Код AND Db_Construct.Код = Db_Address.Код").Tables[0].DefaultView;
                    break;
            }
            database.Dispose();
        }

        // Заполнение комбо боксов

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
                case 1: // ТелефонТип
                    dataGridViewListReturner.DataSource = database.ReturnTable("Тип_Телефона", "Db_Phones", null).Tables[0].DefaultView;
                    for (int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
                    {
                        Temp.Add(dataGridViewListReturner.Rows[i].Cells[0].Value.ToString());
                    }
                    break;
                case 2: // ТелефонНомер
                    dataGridViewListReturner.DataSource = database.ReturnTable("Номер", "Db_Phones", null).Tables[0].DefaultView;
                    for (int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
                    {
                        Temp.Add(dataGridViewListReturner.Rows[i].Cells[0].Value.ToString());
                    }
                    break;
                case 3: // ТипСооруж
                    dataGridViewListReturner.DataSource = database.ReturnTable("Тип_Сооруж", "Db_ConstructType", null).Tables[0].DefaultView;
                    for (int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
                    {
                        Temp.Add(dataGridViewListReturner.Rows[i].Cells[0].Value.ToString());
                    }
                    break;
                case 4: // ТипМероприятия
                    dataGridViewListReturner.DataSource = database.ReturnTable("Тип_Мероприятия", "Db_EventType", null).Tables[0].DefaultView;
                    for (int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
                    {
                        Temp.Add(dataGridViewListReturner.Rows[i].Cells[0].Value.ToString());
                    }
                    break;
                case 5: // Список населенных пунктов
                    dataGridViewListReturner.DataSource = database.ReturnTable("Название_НасПункта", "Db_Locale", null).Tables[0].DefaultView;
                    for (int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
                    {
                        Temp.Add(dataGridViewListReturner.Rows[i].Cells[0].Value.ToString());
                    }
                    break;
                case 6:
                    dataGridViewListReturner.DataSource = database.ReturnTable("Название_ОблОрг", "Db_Region", null).Tables[0].DefaultView;
                    for (int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
                    {
                        Temp.Add(dataGridViewListReturner.Rows[i].Cells[0].Value.ToString());
                    }
                    break;
            }
            database.Dispose();
            return Temp;
        }

        void ComboUpdates()
        {
            comboBoxLocaleType.Items.Clear(); // При добавлении нас.пункта
            comboBoxRegionLocale.Items.Clear(); // При добавлении Облорга
            comboBoxPhoneRegion.Items.Clear(); // При связи телефонов
            comboBoxPhoneRegionPhone.Items.Clear(); // При связи телефонов
            comboBoxConstructType.Items.Clear(); // При добавлении сооружения
            comboBoxConstructRegion.Items.Clear(); // При добавлении сооружения
            foreach(string i in BufferListUpdate(0))
            {
                comboBoxLocaleType.Items.Add(i);
            }
            foreach (string i in BufferListUpdate(5))
            {
                comboBoxRegionLocale.Items.Add(i);
            }
            foreach(string i in BufferListUpdate(6))
            {
                comboBoxPhoneRegion.Items.Add(i);
            }
            foreach(string i in BufferListUpdate(2))
            {
                comboBoxPhoneRegionPhone.Items.Add(i);
            }
            foreach(string i in BufferListUpdate(3))
            {
                comboBoxConstructType.Items.Add(i);
            }
            foreach (string i in BufferListUpdate(6))
            {
                comboBoxConstructRegion.Items.Add(i);
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

        int GetDirCode(string Table, string ToFind, int TableIndex) // Вернуть код (итератор) из справочника
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewListReturner.DataSource = database.ReturnTable("*", Table, null).Tables[0].DefaultView;
            for(int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
            {
                if(dataGridViewListReturner.Rows[i].Cells[TableIndex].Value.ToString() == ToFind)
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
            listBoxMainLog.Items.Add(database.AddLocale(
                textBoxLocaleName.Text, 
                textBoxLocaleShortName.Text, 
                GetDirCode("Db_LocaleType", comboBoxLocaleType.SelectedItem.ToString(), 1)));
            MainTabUpdate(0);
            database.Dispose();
        }

        private void buttonAddRegion_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.AddRegion(
                textBoxRegionName.Text, 
                textBoxRegionShortName.Text, 
                textBoxRegionEmail.Text, 
                GetDirCode("Db_Locale", comboBoxRegionLocale.SelectedItem.ToString(), 1)));
            MainTabUpdate(1);
            database.Dispose();
        }

        // Работа с телефонами

        private void buttonConnectPhone_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxPhoneRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewRegionPhones.DataSource = database.ReturnTable(
                "Db_Region.Название_ОблОрг, Db_Phones.Тип_Телефона, Db_Phones.Номер",
                "Db_Region, Db_Phones, Col_RegionsAndPhones",
                $"WHERE Col_RegionsAndPhones.КодРегиона = {GetDirCode("Db_Region", comboBoxPhoneRegion.SelectedItem.ToString(), 1)} " +
                $"AND Col_RegionsAndPhones.КодТелефона = Db_Phones.Код");
            database.Dispose();
            */
        }

        private void comboBoxPhoneRegionPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewListReturner.DataSource = database.ReturnTable("*", "Db_Phones", null).Tables[0].DefaultView;
            for(int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
            {
                if(comboBoxPhoneRegionPhone.SelectedItem.ToString() == dataGridViewListReturner.Rows[i].Cells[2].Value.ToString())
                {
                    labelPhone.Text = $"Телефон ({dataGridViewListReturner.Rows[i].Cells[1].Value})";
                }
            }
            database.Dispose();
        }

        // Обновление и удаление данных справочников

        private void buttonDirUpdate_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            switch(tabControl2.SelectedIndex)
            {
                case 0:
                    listBoxMainLog.Items.Add(database.UpdateDirectory(
                        "Db_Phones",
                        $"Тип_Телефона = '{textBoxDirPhoneType.Text}', Номер = '{textBoxDirPhoneNum.Text}'",
                        $"Db_Phones.Код = {dataGridViewDir.SelectedRows[0].Cells[0].Value}"));
                    break;
                case 1:
                    listBoxMainLog.Items.Add(database.UpdateDirectory(
                        "Db_LocaleType",
                        $"ТипНасПункт = '{textBoxDirLocaleType.Text}'",
                        $"Db_LocaleType.Код = {dataGridViewDir.SelectedRows[0].Cells[0].Value}"));
                    break;
                case 2:
                    listBoxMainLog.Items.Add(database.UpdateDirectory(
                        "Db_ConstructType",
                        $"Тип_Сооруж = '{textBoxDirConstructType.Text}'",
                        $"Db_ConstructType.Код = {dataGridViewDir.SelectedRows[0].Cells[0].Value}"));
                    break;
                case 3:
                    listBoxMainLog.Items.Add(database.UpdateDirectory(
                        "Db_EventType",
                        $"Тип_Мероприятия = '{textBoxDirEventType.Text}'",
                        $"Db_EventType.Код = {dataGridViewDir.SelectedRows[0].Cells[0].Value}"));
                    break;
            }
            DirTabUpdate();
            database.Dispose();
        }

        private void buttonDirDelete_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);

            database.Dispose();
        }

        private void buttonAddConstruct_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(
                database.AddConstruct(
                    textBoxConstructName.Text,
                    textBoxConstructShortName.Text,
                    dateTimePickerConstructBalance.Value,
                    Convert.ToInt32(textBoxConstructCapacity.Text),
                    Convert.ToInt32(textBoxConstructSquare.Text),
                    GetDirCode("Db_ConstructType", comboBoxConstructType.SelectedItem.ToString(), 1),
                    GetDirCode("Db_Region", comboBoxConstructRegion.SelectedItem.ToString(), 1),
                    textBoxConstructAddress.Text));
            MainTabUpdate(2);
            database.Dispose();
        }
    }
}
