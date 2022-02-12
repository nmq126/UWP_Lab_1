using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_SQLite.Entity;

namespace UWP_SQLite.Data
{
    class DatabaseInitialize
    {
        public static bool CreateTables()
        {
            var cnn = new SQLiteConnection("sqlitedemo.db");
            string sql = @"CREATE TABLE IF NOT EXISTS
                          PersonalTransaction (Id      INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    Name    VARCHAR( 140 ),
                                    Description    TEXT,
                                    Detail    TEXT,
                                    Amount DOUBLE,
                                    CreatedAt DATETIME,
                                    Category INT
                                    );";
            using (var statement = cnn.Prepare(sql))
            {
                statement.Step();
            }
            return true;
        }

        public static bool InsertData(PersonalTransaction transaction)
        {
            var cnn = new SQLiteConnection("sqlitedemo.db");
            using (var trnsctn = cnn.Prepare("INSERT INTO PersonalTransaction(Name, Description, Detail, Amount, CreatedAt, Category) VALUES (?, ?, ?, ?, ?, ?)"))
            {
                trnsctn.Bind(1, transaction.Name);
                trnsctn.Bind(2, transaction.Description);
                trnsctn.Bind(3, transaction.Detail);
                trnsctn.Bind(4, transaction.Amount);
                trnsctn.Bind(5, transaction.CreatedAt.ToString("yyyy-MM-dd"));
                trnsctn.Bind(6, transaction.Category);
                trnsctn.Step();
            }
            return true;
        }
        public static List<PersonalTransaction> ShowList()
        {
            List<PersonalTransaction> list = new List<PersonalTransaction>();
            var cnn = new SQLiteConnection("sqlitedemo.db");
            using (var personalTransaction = cnn.Prepare("select * from PersonalTransaction"))
            {

                while (personalTransaction.Step() == SQLiteResult.ROW)
                {

                    PersonalTransaction personal = new PersonalTransaction()
                    {
                        Name = (string)personalTransaction["Name"],
                        Description = (string)personalTransaction["Description"],
                        Detail = (string)personalTransaction["Detail"],
                        Amount = (double)personalTransaction["Amount"]
                    };
                    var date = (string)personalTransaction["CreatedAt"];
                    var category = (Int64)personalTransaction["Category"];
                    personal.CreatedAt = DateTime.Parse(date);
                    personal.Category = (int)category;
                    list.Add(personal);
                }
                return list;
            }
        }

    }

}
