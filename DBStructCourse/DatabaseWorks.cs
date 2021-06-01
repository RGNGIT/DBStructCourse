using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBStructCourse
{
    public class DatabaseWorks : IDisposable
    {

        // Реализация интерфейса IDisposable

        private System.ComponentModel.Component components = new System.ComponentModel.Component();
        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    components.Dispose();
                }
                disposed = true;
            }
        }

        // Реализация коструктора/деструктора модуля

        SqlConnection connection;

        public DatabaseWorks(string Credentials)
        {
            connection = new SqlConnection(Credentials);
            connection.Open();
        }

        ~DatabaseWorks()
        {
            Dispose(false);
        }

        // Реализация работы с базой
        // Вывод таблицы

        public DataSet ReturnTable(string Columns, string TablesName, string Arguments)
        {
            SqlDataAdapter sqlData = new SqlDataAdapter($"SELECT {Columns} FROM {TablesName} {Arguments}", connection);
            DataSet dataSet = new DataSet();
            sqlData.Fill(dataSet);
            return dataSet;
        }

        // Справочники

        public string UpdateDirectory(string Table, string Value, string Key)
        {
            try
            {
                SqlCommand command = new SqlCommand($"UPDATE {Table} SET {Value} WHERE {Key}", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string Delete(string Table, string Argument)
        {
            try
            {
                SqlCommand command = new SqlCommand($"DELETE FROM {Table} WHERE {Argument}", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string AddPhonesData(string PhoneType, string Number) // Tab::(Db_Phones), Fields::(Тип_Телефона, Номер)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO Db_Phones (Тип_Телефона, Номер) " +
                    $"VALUES ('{PhoneType}', '{Number}')", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string AddEventType(string EventType) // Tab::(Db_EventType), Fields::(Тип_Мероприятия)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO Db_EventType (Тип_Мероприятия) " +
                    $"VALUES ('{EventType}')", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string AddConstructType(string ConstructType) // Tab::(Db_ConstructType), Fields::(Тип_Сооруж)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO Db_ConstructType (Тип_Сооруж) " +
                    $"VALUES ('{ConstructType}')", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        
        public string AddLocaleType(string LocaleType) // Tab::(Db_LocaleType), Fields::(ТипНасПункт)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO Db_LocaleType (ТипНасПункт) " +
                    $"VALUES ('{LocaleType}')", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        // База

        public string AddLocale(string Name, string ShName, int TypeCode)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO Db_Locale (Название_НасПункта, Кр_Название_НасПункта, КодТипа) " +
                    $"VALUES ('{Name}', '{ShName}', {TypeCode})", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string PhoneRegionConnect(int RegCode, int PhoneCode)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO Col_RegionsAndPhones (КодРегиона, КодТелефона) " +
                    $"VALUES ({RegCode}, {PhoneCode})", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string AddRegion(string Name, string ShName, string email, int LocaleCode)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO Db_Region (Название_ОблОрг, Кр_Назв_ОблОрг, ЭлАдрес_ОблОрг, КодНасПункта) " +
                    $"VALUES ('{Name}', '{ShName}', '{email}', {LocaleCode})", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string AddConstruct(string Name, string ShName, DateTime Date, int Capacity, float Square, int TypeCode, string Address)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO Db_Construct (Название_Сооруж, Кр_Название_Сооруж, ДатаПринятия_Сооруж, Вместимость_Сооруж, Площадь_Сооруж, КодТипа) " +
                    $"VALUES ('{Name}', '{ShName}', '{Date}', {Capacity}, {Square}, {TypeCode})", connection);
                int Amount = command.ExecuteNonQuery();
                command = new SqlCommand(
                    $"INSERT INTO Db_Address (АдресЗнач) " +
                    $"VALUES ('{Address}')", connection);
                return $"Команда выполнена. Задействовано строк таблиц: {command.ExecuteNonQuery() + Amount}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string ConstructRegionConnect(int RegionCode, int ConstructCode)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO Col_RegionsAndConstructs (КодРегиона, КодСооруж) " +
                    $"VALUES ({RegionCode}, {ConstructCode})", connection);
                return $"Команда выполнена. Задействовано строк таблиц: {command.ExecuteNonQuery()}";
            }
            catch(Exception e)
            {
                return e.ToString();
            }
        }

        public string AddEvent(string Name, string ShName, int EventTypeCode, int LocaleCode)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO Db_Event (Название_Мероприятия, Кр_Название_Мероприятия, КодТипа, КодНасПункта) " +
                    $"VALUES ('{Name}', '{ShName}', {EventTypeCode}, {LocaleCode})", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string AddEventDate(int EventCode, int ConstructCode, DateTime Date, int PeopleAmount)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO Db_EventDate (Код_Мероприятия, Код_Сооруж, ДатаПроведения, КолВо_Человек)" +
                    $"VALUES ({EventCode}, {ConstructCode}, '{Date}', {PeopleAmount})", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        // Редактирование, удаление

        public string UpdateLocale(string Name, string ShName, int TypeCode, int Key)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    "UPDATE Db_Locale SET " +
                    $"Db_Locale.Название_НасПункта = '{Name}', " +
                    $"Db_Locale.Кр_Название_НасПункта = '{ShName}', " +
                    $"Db_Locale.КодТипа = {TypeCode} " +
                    $"WHERE Db_Locale.Код = {Key}", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string UpdateRegion(string Name, string ShName, string email, int LocaleCode, int Key)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    "UPDATE Db_Region SET " +
                    $"Db_Region.Название_ОблОрг = '{Name}', " +
                    $"Db_Region.Кр_Назв_ОблОрг = '{ShName}', " +
                    $"Db_Region.ЭлАдрес_ОблОрг = '{email}', " +
                    $"Db_Region.КодНасПункта = {LocaleCode} " +
                    $"WHERE Db_Region.Код = {Key}", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch(Exception e)
            {
                return e.ToString();
            }
        }

        public string UpdateConstruct(string Name, string ShName, DateTime Date, int Capacity, float Square, int TypeCode, string Address, int Key)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    "UPDATE Db_Construct SET " +
                    $"Db_Construct.Название_Сооруж = '{Name}', " +
                    $"Db_Construct.Кр_Название_Сооруж = '{ShName}', " +
                    $"Db_Construct.ДатаПринятия_Сооруж = '{Date}', " +
                    $"Db_Construct.Вместимость_Сооруж = {Capacity}, " +
                    $"Db_Construct.Площадь_Сооруж = {Square}, " +
                    $"Db_Construct.КодТипа = {TypeCode} " +
                    $"WHERE Db_Construct.Код = {Key}", connection);
                int Amount = command.ExecuteNonQuery();
                command = new SqlCommand(
                    "UPDATE Db_Address SET " +
                    $"Db_Address.АдресЗнач = '{Address}' " +
                    $"WHERE Db_Address.Код = {Key}", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery() + Amount}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string UpdateConstructRegionConnection(int RegionCode, int Key)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    "UPDATE Col_RegionsAndConstructs SET " +
                    $"Col_RegionsAndConstructs.КодРегиона = {RegionCode} " +
                    $"WHERE Col_RegionsAndConstructs.КодСооруж = {Key}", connection);
                return $"Команда выполнена. Задействовано строк таблиц: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

    }
}
