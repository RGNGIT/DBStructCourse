using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace DBStructCourse
{
    public partial class MainFrame : Form
    {
        public MainFrame()
        {
            InitializeComponent();
            DirTabUpdate();
            MainTabUpdate(tabControlMain.SelectedIndex);
            ComboUpdates();
            chartReport2.Series.Clear();
            dataGridViewReport3Amount.Columns.Add("_regName", "НазваниеОрганизации");
            dataGridViewReport3Amount.Columns.Add("_consAmount", "КоличествоСооружений");
        }

        string Credentials =
            $"Server = {Program.Server};" +
            $"Integrated security = {Program.Security};" +
            $"database = {Program.Database};";

        private void buttonShowLog_Click(object sender, EventArgs e)
        {
            List<string> Temp = new List<string>();
            foreach (string i in listBoxMainLog.Items)
            {
                Temp.Add(i);
            }
            File.WriteAllLines("log.txt", Temp);
            Process.Start("log.txt");
        }

        private void buttonAddDir_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            switch (tabControlDir.SelectedIndex)
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
            dataGridViewDir.DataSource = database.ReturnTable("*", DirTables[tabControlDir.SelectedIndex], null).Tables[0].DefaultView;
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
                case 2: // Обновить таблицу сооружений
                    dataGridViewConstruct.DataSource = database.ReturnTable(
                        "Db_Construct.Код, Название_Сооруж as Название, Кр_Название_Сооруж as КраткоеНазвание, ДатаПринятия_Сооруж as ДатаПринятия, Вместимость_Сооруж as Вместимость, Площадь_Сооруж as Площадь, Db_ConstructType.Тип_Сооруж as Тип, Db_Region.Название_ОблОрг as ОбластнаяОрганизация, Db_Address.АдресЗнач as Адрес",
                        "Db_Construct, Db_ConstructType, Db_Region, Db_Address, Col_RegionsAndConstructs",
                        "WHERE Db_Construct.КодТипа = Db_ConstructType.Код AND Col_RegionsAndConstructs.КодРегиона = Db_Region.Код AND Col_RegionsAndConstructs.КодСооруж = Db_Construct.Код AND Db_Construct.Код = Db_Address.Код").Tables[0].DefaultView;
                    break;
                case 3: // Обновить таблицу мероприятий
                    dataGridViewEvent.DataSource = database.ReturnTable(
                        "Db_Event.Код, Название_Мероприятия as Название, Кр_Название_Мероприятия as КраткоеНазвание, Db_EventType.Тип_Мероприятия as Тип, Db_Locale.Название_НасПункта as НаселенныйПункт, Db_EventDate.ДатаПроведения as ДатаПроведения, Db_EventDate.КолВо_Человек as КоличествоЧеловек, Db_Construct.Название_Сооруж as Сооружение",
                        "Db_Event, Db_EventType, Db_EventDate, Db_Locale, Db_Construct",
                        "WHERE Db_Event.КодТипа = Db_EventType.Код AND Db_Event.КодНасПункта = Db_Locale.Код AND Db_EventDate.Код_Мероприятия = Db_Event.Код AND Db_EventDate.Код_Сооруж = Db_Construct.Код").Tables[0].DefaultView;
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
                    for (int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
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
                case 7:
                    dataGridViewListReturner.DataSource = database.ReturnTable("Название_Сооруж", "Db_Construct", null).Tables[0].DefaultView;
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
            comboBoxEventConstruct.Items.Clear();
            comboBoxEventType.Items.Clear();
            comboBoxLocaleEvent.Items.Clear();
            comboBoxReportType.Items.Clear();
            comboBoxReport2Const.Items.Clear();
            comboBoxReport2Event.Items.Clear();
            foreach (string i in BufferListUpdate(0))
            {
                comboBoxLocaleType.Items.Add(i);
            }
            foreach (string i in BufferListUpdate(5))
            {
                comboBoxRegionLocale.Items.Add(i);
                comboBoxLocaleEvent.Items.Add(i);
            }
            foreach (string i in BufferListUpdate(6))
            {
                comboBoxPhoneRegion.Items.Add(i);
                comboBoxConstructRegion.Items.Add(i);
            }
            foreach (string i in BufferListUpdate(2))
            {
                comboBoxPhoneRegionPhone.Items.Add(i);
            }
            foreach (string i in BufferListUpdate(3))
            {
                comboBoxConstructType.Items.Add(i);
                comboBoxReportType.Items.Add(i);
            }
            foreach (string i in BufferListUpdate(7))
            {
                comboBoxEventConstruct.Items.Add(i);
                comboBoxReport2Const.Items.Add(i);
            }
            foreach (string i in BufferListUpdate(4))
            {
                comboBoxEventType.Items.Add(i);
                comboBoxReport2Event.Items.Add(i);
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

        // Работа по выборке из гридов

        private void dataGridViewDir_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (tabControlDir.SelectedIndex)
            {
                case 0:
                    textBoxDirPhoneType.Text = dataGridViewDir.SelectedRows[0].Cells[1].Value.ToString();
                    textBoxDirPhoneNum.Text = dataGridViewDir.SelectedRows[0].Cells[2].Value.ToString();
                    break;
                case 1:
                    textBoxDirLocaleType.Text = dataGridViewDir.SelectedRows[0].Cells[1].Value.ToString();
                    break;
                case 2:
                    textBoxDirConstructType.Text = dataGridViewDir.SelectedRows[0].Cells[1].Value.ToString();
                    break;
                case 3:
                    textBoxDirEventType.Text = dataGridViewDir.SelectedRows[0].Cells[1].Value.ToString();
                    break;
            }
        }

        // Добавления в главные таблички

        int GetDirCode(string Table, string ToFind, int TableIndex) // Вернуть код (итератор) из справочника
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewListReturner.DataSource = database.ReturnTable("*", Table, null).Tables[0].DefaultView;
            for (int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
            {
                if (dataGridViewListReturner.Rows[i].Cells[TableIndex].Value.ToString() == ToFind)
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
            ComboUpdates();
            MainTabUpdate(1);
            database.Dispose();
        }

        // Работа с телефонами

        private void buttonConnectPhone_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.PhoneRegionConnect(GetDirCode("Db_Region", comboBoxPhoneRegion.Text, 1), GetDirCode("Db_Phones", comboBoxPhoneRegionPhone.Text, 2)));
            UpdatePhones();
            database.Dispose();
        }

        void UpdatePhones()
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewRegionPhones.DataSource = database.ReturnTable(
                "Тип_Телефона as Тип, Номер",
                "Db_Phones, Col_RegionsAndPhones",
                $"WHERE Db_Phones.Код = Col_RegionsAndPhones.КодТелефона AND Col_RegionsAndPhones.КодРегиона = {GetDirCode("Db_Region", comboBoxPhoneRegion.Text, 1)}").Tables[0].DefaultView;
            database.Dispose();
        }

        private void comboBoxPhoneRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePhones();
        }

        private void comboBoxPhoneRegionPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewListReturner.DataSource = database.ReturnTable("*", "Db_Phones", null).Tables[0].DefaultView;
            for (int i = 0; i < dataGridViewListReturner.Rows.Count - 1; i++)
            {
                if (comboBoxPhoneRegionPhone.SelectedItem.ToString() == dataGridViewListReturner.Rows[i].Cells[2].Value.ToString())
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
            switch (tabControlDir.SelectedIndex)
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
            listBoxMainLog.Items.Add(database.Delete(DirTables[tabControlDir.SelectedIndex], $"Код = {dataGridViewDir.SelectedRows[0].Cells[0].Value}"));
            DirTabUpdate();
            database.Dispose();
        }

        int GetAddingConstructCode()
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewConsSelect.DataSource = database.ReturnTable(
            "Код",
            "Db_Construct",
            "WHERE Код = (SELECT MAX(Код) FROM Db_Construct)").Tables[0].DefaultView;
            database.Dispose();
            return Convert.ToInt32(dataGridViewConsSelect.Rows[0].Cells[0].Value);
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
                    Convert.ToSingle(textBoxConstructSquare.Text),
                    GetDirCode("Db_ConstructType", comboBoxConstructType.SelectedItem.ToString(), 1),
                    textBoxConstructAddress.Text));
            listBoxMainLog.Items.Add(database.ConstructRegionConnect(
                GetDirCode("Db_Region", comboBoxConstructRegion.SelectedItem.ToString(), 1),
                GetAddingConstructCode()));
            MainTabUpdate(2);
            database.Dispose();
        }

        int GetAddingEventCode()
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewListReturner.DataSource = database.ReturnTable(
                "Код",
                "Db_Event",
                "WHERE Код = (SELECT MAX(Код) FROM Db_Event)").Tables[0].DefaultView;
            database.Dispose();
            return Convert.ToInt32(dataGridViewListReturner.Rows[0].Cells[0].Value);
        }

        private void buttonEventAdd_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.AddEvent(
                textBoxEventName.Text,
                textBoxEventShortName.Text,
                GetDirCode("Db_EventType", comboBoxEventType.SelectedItem.ToString(), 1),
                GetDirCode("Db_Locale", comboBoxLocaleEvent.SelectedItem.ToString(), 1)));
            listBoxMainLog.Items.Add(database.AddEventDate(
                GetAddingEventCode(),
                GetDirCode("Db_Construct", comboBoxEventConstruct.SelectedItem.ToString(), 1),
                dateTimePickerEventDate.Value,
                Convert.ToInt32(textBoxEventPplAmount.Text)));
            MainTabUpdate(3);
            database.Dispose();
        }

        // Запросы на отчет

        string GetSQLFormatDate(DateTime Date)
        {
            string Temp = string.Empty;
            foreach (char i in Date.ToString("yyyy/MM/dd"))
            {
                if (i != '.')
                {
                    Temp += i;
                }
            }
            return Temp;
        }

        private void buttonReport1_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewReport1.DataSource = database.ReturnTable(
                "Db_Construct.Код as Код, Название_Сооруж as Название, Кр_Название_Сооруж as КраткоеНазвание, " +
                "ДатаПринятия_Сооруж as ДатаПринятия, Вместимость_Сооруж as Вместимость," +
                " Площадь_Сооруж as Площадь, Db_Region.Название_ОблОрг as Организация",
                "Db_Construct, Db_Region, Col_RegionsAndConstructs",
                $"WHERE ДатаПринятия_Сооруж > '{GetSQLFormatDate(dateTimePickerReport1From.Value)}' " +
                $"AND ДатаПринятия_Сооруж < '{GetSQLFormatDate(dateTimePickerReport1To.Value)}' " +
                $"AND КодТипа = {GetDirCode("Db_ConstructType", comboBoxReportType.SelectedItem.ToString(), 1)} " +
                $"AND Db_Construct.Код = Col_RegionsAndConstructs.КодСооруж " +
                $"AND Db_Region.Код = Col_RegionsAndConstructs.КодРегиона").Tables[0].DefaultView;
            database.Dispose();
        }

        string[] Months = new string[12]
        {
                "Январь",
                "Февраль",
                "Март",
                "Апрель",
                "Май",
                "Июнь",
                "Июль",
                "Август",
                "Сентябрь",
                "Октябрь",
                "Ноябрь",
                "Декабрь"
        };
        
        private void buttonReport2_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            chartReport2.Series.Clear();
            chartReport2.Series.Add(new Series($"{comboBoxReport2Const.SelectedItem}/{comboBoxReport2Event.SelectedItem}")
            {
                ChartType = SeriesChartType.Column
            });
            dataGridViewReport2.DataSource = database.ReturnTable(
                "Db_Event.Название_Мероприятия, Db_EventDate.ДатаПроведения",
                "Db_Event, Db_EventDate, Db_EventType, Db_Construct",
                "WHERE Db_EventDate.Код_Мероприятия = Db_Event.Код " +
                "AND Db_Event.КодТипа = Db_EventType.Код " +
                "AND Db_EventDate.Код_Сооруж = Db_Construct.Код " +
                $"AND Db_Construct.Код = {GetDirCode("Db_Construct", comboBoxReport2Const.SelectedItem.ToString(), 1)}" +
                $"AND Db_EventType.Код = {GetDirCode("Db_EventType", comboBoxReport2Event.SelectedItem.ToString(), 1)} " +
                $"AND Db_EventDate.ДатаПроведения > '{GetSQLFormatDate(dateTimePickerReport2From.Value)}' " +
                $"AND Db_EventDate.ДатаПроведения < '{GetSQLFormatDate(dateTimePickerReport2To.Value)}'").Tables[0].DefaultView;
            int TempCount = 0;
            double CalcYear;
            for (int i = 0; i < Math.Abs(dateTimePickerReport2From.Value.Year - dateTimePickerReport2To.Value.Year) * 12; i++)
            {
                CalcYear = dateTimePickerReport2From.Value.Year + Math.Round((double)(i / 12));
                for (int j = 0; j < dataGridViewReport2.Rows.Count - 1; j++)
                {
                    if(CalcYear == Convert.ToDateTime(dataGridViewReport2.Rows[j].Cells[1].Value).Year && Convert.ToDateTime(dataGridViewReport2.Rows[j].Cells[1].Value).Month == ((i % 12) + 1))
                    {
                        TempCount++;
                    }
                }
                chartReport2.Series[$"{comboBoxReport2Const.SelectedItem}/{comboBoxReport2Event.SelectedItem}"].Points.AddXY($"{Months[i % 12]} {CalcYear}", TempCount);
                TempCount = 0;
            }
            database.Dispose();
        }

        void CountAmount()
        {
            List<string> Name = new List<string>();
            List<int> Amount = new List<int>();
            int AmountCounter = 0;
            string Check = dataGridViewReport3.Rows[0].Cells[0].Value.ToString();
            dataGridViewReport3Amount.Rows.Clear();
            for(int i = 0; i < dataGridViewReport3.Rows.Count - 1; i++)
            {
                if(dataGridViewReport3.Rows[i].Cells[0].Value.ToString() != Check)
                {
                    Amount.Add(AmountCounter);
                    AmountCounter = 0;
                }
                if(!Name.Contains(dataGridViewReport3.Rows[i].Cells[0].Value.ToString()))
                {
                    Name.Add(dataGridViewReport3.Rows[i].Cells[0].Value.ToString());
                }
                Check = dataGridViewReport3.Rows[i].Cells[0].Value.ToString();
                AmountCounter++;
                if(i == dataGridViewReport3.Rows.Count - 2)
                {
                    Amount.Add(AmountCounter);
                }
            }
            for(int i = 0; i < Name.Count; i++)
            {
                dataGridViewReport3Amount.Rows.Add(Name[i], Amount[i]);
            }
        }

        private void buttonReport3_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            dataGridViewReport3.DataSource = database.ReturnTable(
                "Db_Region.Название_ОблОрг as НазваниеОрганизации, Db_Construct.Название_Сооруж as НазваниеСооружения, " +
                "Db_ConstructType.Тип_Сооруж as ТипСооружения", 
                "Db_Region, Db_Construct, Db_ConstructType, Col_RegionsAndConstructs ", 
                $"WHERE Db_Construct.ДатаПринятия_Сооруж > '{GetSQLFormatDate(dateTimePickerReport3From.Value)}' " +
                $"AND Db_Construct.ДатаПринятия_Сооруж < '{GetSQLFormatDate(dateTimePickerReport3To.Value)}' " +
                $"AND Col_RegionsAndConstructs.КодРегиона = Db_Region.Код " +
                $"AND Col_RegionsAndConstructs.КодСооруж = Db_Construct.Код " +
                $"AND Db_ConstructType.Код = Db_Construct.КодТипа " +
                $"ORDER BY Db_Region.Название_ОблОрг").Tables[0].DefaultView;
            CountAmount();
            database.Dispose();
        }

        // Редактировать, удалить населенные пункты

        private void buttonRedactLocale_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.UpdateLocale(
                textBoxLocaleName.Text,
                textBoxLocaleShortName.Text,
                GetDirCode("Db_LocaleType", comboBoxLocaleType.SelectedItem.ToString(), 1),
                Convert.ToInt32(dataGridViewLocale.SelectedRows[0].Cells[0].Value)));
            MainTabUpdate(0);
            database.Dispose();
        }

        private void buttonDeleteLocale_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.Delete(
                "Db_Locale",
                $"Db_Locale.Код = {dataGridViewLocale.SelectedRows[0].Cells[0].Value}"));
            MainTabUpdate(0);
            database.Dispose();
        }

        // Редактировать, удалить областную организацию

        private void buttonRedactRegion_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.UpdateRegion(
                textBoxRegionName.Text,
                textBoxRegionShortName.Text,
                textBoxRegionEmail.Text,
                GetDirCode("Db_Locale", comboBoxRegionLocale.SelectedItem.ToString(), 1),
                Convert.ToInt32(dataGridViewRegion.SelectedRows[0].Cells[0].Value)));
            MainTabUpdate(1);
            database.Dispose();
        }

        private void buttonRegDelete_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.Delete(
                "Db_Region",
                $"Db_Region.Код = {dataGridViewRegion.SelectedRows[0].Cells[0].Value}"));
            MainTabUpdate(1);
            database.Dispose();
        }

        // Редактировать, удалить сооружение

        private void buttonConstructionRedact_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.UpdateConstruct(
                textBoxConstructName.Text,
                textBoxConstructShortName.Text,
                dateTimePickerConstructBalance.Value,
                Convert.ToInt32(textBoxConstructCapacity.Text),
                Convert.ToSingle(textBoxConstructSquare.Text),
                GetDirCode("Db_ConstructType", comboBoxConstructType.SelectedItem.ToString(), 1),
                textBoxConstructAddress.Text,
                Convert.ToInt32(dataGridViewConstruct.SelectedRows[0].Cells[0].Value)));
            listBoxMainLog.Items.Add(database.UpdateConstructRegionConnection(
                GetDirCode("Db_Region", comboBoxConstructRegion.SelectedItem.ToString(), 1),
                Convert.ToInt32(dataGridViewConstruct.SelectedRows[0].Cells[0].Value)));
            MainTabUpdate(2);
            database.Dispose();
        }

        private void buttonConstructDelete_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.Delete(
                "Db_Construct",
                $"Db_Construct.Код = {dataGridViewConstruct.SelectedRows[0].Cells[0].Value}"));
            MainTabUpdate(2);
            database.Dispose();
        }

        // Редактировать, удалить событие

        private void buttonEventRedact_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.UpdateEvent(
                textBoxEventName.Text,
                textBoxEventShortName.Text,
                GetDirCode("Db_EventType", comboBoxEventType.SelectedItem.ToString(), 1),
                GetDirCode("Db_Locale", comboBoxLocaleEvent.SelectedItem.ToString(), 1),
                Convert.ToInt32(dataGridViewEvent.SelectedRows[0].Cells[0].Value)));
            listBoxMainLog.Items.Add(database.UpdateEventDate(
                Convert.ToInt32(dataGridViewEvent.SelectedRows[0].Cells[0].Value),
                GetDirCode("Db_Construct", comboBoxEventConstruct.SelectedItem.ToString(), 1),
                dateTimePickerEventDate.Value,
                Convert.ToInt32(textBoxEventPplAmount.Text)));
            MainTabUpdate(3);
            database.Dispose();
        }

        private void buttonEventDelete_Click(object sender, EventArgs e)
        {
            DatabaseWorks database = new DatabaseWorks(Credentials);
            listBoxMainLog.Items.Add(database.Delete(
                "Db_Event",
                $"Db_Event.Код = {dataGridViewEvent.SelectedRows[0].Cells[0].Value}"));
            MainTabUpdate(3);
            database.Dispose();
        }

        // Клики на гриды

        private void dataGridViewLocale_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBoxLocaleName.Text = dataGridViewLocale.SelectedRows[0].Cells[1].Value.ToString();
            textBoxLocaleShortName.Text = dataGridViewLocale.SelectedRows[0].Cells[2].Value.ToString();
            comboBoxLocaleType.Text = dataGridViewLocale.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void dataGridViewRegion_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBoxRegionName.Text = dataGridViewRegion.SelectedRows[0].Cells[1].Value.ToString();
            textBoxRegionShortName.Text = dataGridViewRegion.SelectedRows[0].Cells[2].Value.ToString();
            textBoxRegionEmail.Text = dataGridViewRegion.SelectedRows[0].Cells[3].Value.ToString();
            comboBoxRegionLocale.Text = dataGridViewRegion.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void dataGridViewConstruct_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBoxConstructName.Text = dataGridViewConstruct.SelectedRows[0].Cells[1].Value.ToString();
            textBoxConstructShortName.Text = dataGridViewConstruct.SelectedRows[0].Cells[2].Value.ToString();
            dateTimePickerConstructBalance.Value = Convert.ToDateTime(dataGridViewConstruct.SelectedRows[0].Cells[3].Value);
            textBoxConstructCapacity.Text = dataGridViewConstruct.SelectedRows[0].Cells[4].Value.ToString();
            textBoxConstructSquare.Text = dataGridViewConstruct.SelectedRows[0].Cells[5].Value.ToString();
            comboBoxConstructType.Text = dataGridViewConstruct.SelectedRows[0].Cells[6].Value.ToString();
            comboBoxConstructRegion.Text = dataGridViewConstruct.SelectedRows[0].Cells[7].Value.ToString();
            textBoxConstructAddress.Text = dataGridViewConstruct.SelectedRows[0].Cells[8].Value.ToString();
        }

        private void dataGridViewEvent_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBoxEventName.Text = dataGridViewEvent.SelectedRows[0].Cells[1].Value.ToString();
            textBoxEventShortName.Text = dataGridViewEvent.SelectedRows[0].Cells[2].Value.ToString();
            comboBoxEventType.Text = dataGridViewEvent.SelectedRows[0].Cells[3].Value.ToString();
            comboBoxLocaleEvent.Text = dataGridViewEvent.SelectedRows[0].Cells[4].Value.ToString();
            dateTimePickerEventDate.Value = Convert.ToDateTime(dataGridViewEvent.SelectedRows[0].Cells[5].Value);
            textBoxEventPplAmount.Text = dataGridViewEvent.SelectedRows[0].Cells[6].Value.ToString();
            comboBoxEventConstruct.Text = dataGridViewEvent.SelectedRows[0].Cells[7].Value.ToString();
        }

    }
}
