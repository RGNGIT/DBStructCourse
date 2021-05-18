using System;
using System.Data;
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
            connection.Close();
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

        public string AddPhonesData(string PhoneType, string Number) // Tab::(Db_Phones), Fields::(Тип_Телефона, Номер)
        {
            try
            {
                SqlCommand command = new SqlCommand($"INSERT INTO Db_Phones (Тип_Телефона, Номер) VALUES ('{PhoneType}', '{Number}')", connection);
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
                SqlCommand command = new SqlCommand($"INSERT INTO Db_EventType (Тип_Мероприятия) VALUES ('{EventType}')", connection);
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
                SqlCommand command = new SqlCommand($"INSERT INTO Db_ConstructType (Тип_Сооруж) VALUES ('{ConstructType}')", connection);
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
                SqlCommand command = new SqlCommand($"INSERT INTO Db_LocaleType (ТипНасПункт) VALUES ('{LocaleType}')", connection);
                return $"Команда выполнена. Задействовано строк таблицы: {command.ExecuteNonQuery()}";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        // База



    }
}
