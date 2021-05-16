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
        }

        ~DatabaseWorks()
        {
            Dispose(false);
        }

        // Реализация работы с базой
        // Вывод таблицы

        public DataSet ReturnTable(string TableName)
        {
            SqlDataAdapter sqlData = new SqlDataAdapter($"SELECT * FROM {TableName}", connection);
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

        public void AddEventType(string EventType) // Tab::(Db_EventType), Fields::(Тип_Мероприятия)
        {

        }

        public void AddConstructType(string ConstructType) // Tab::(Db_ConstructType), Fields::(Тип_Сооруж)
        {

        }
        
        public void AddLocaleType(string LocaleType) // Tab::(Db_LocaleType), Fields::(ТипНасПункт)
        {

        }

    }
}
