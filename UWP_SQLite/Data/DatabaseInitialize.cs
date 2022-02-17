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
        private static readonly SQLiteConnection cnn = new SQLiteConnection("sqlitedemo.db");

        public static bool CreateTables()
        {
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
            string sqlDropCategory = @"DROP TABLE IF EXISTS Category;";
            using (var statement = cnn.Prepare(sqlDropCategory))
            {
                statement.Step();
            }
            string sqlCategory = @"CREATE TABLE IF NOT EXISTS
                                Category (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                Name VARCHAR( 140 )
                                );";
            using (var statement = cnn.Prepare(sqlCategory))
            {
                statement.Step();
            }
            using (var category = cnn.Prepare("INSERT INTO Category(Name) VALUES(?),(?),(?)"))
            {
                category.Bind(1, "Tien An");
                category.Bind(2, "Tien Su");
                category.Bind(3, "Tien Phong Bank");
                category.Step();
            }
            return true;
        }

        public static List<Category> ShowListCategory()
        {
            var listCategory = new List<Category>();
            
                using (var stt = cnn.Prepare("select * from Category"))
                {
                    while (stt.Step() == SQLiteResult.ROW)
                    {
                        var category = new Category()
                        {
                            Id = Convert.ToInt32(stt["Id"]),
                            Name = (string)stt["Name"],
                        };
                        listCategory.Add(category);
                    }
                }
                //Debug.WriteLine(list[0]);
                return listCategory;

        }
        public static bool InsertData(PersonalTransaction transaction)
        {
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
            using (var personalTransaction = cnn.Prepare("select * from PersonalTransaction"))
            {

                while (personalTransaction.Step() == SQLiteResult.ROW)
                {

                    PersonalTransaction personal = new PersonalTransaction()
                    {
                        Id = Convert.ToInt32(personalTransaction["Id"]),
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
        public static List<PersonalTransaction> SearchByDay(string fromDate, string endDAte)
        {
            List<PersonalTransaction> list = new List<PersonalTransaction>();
            string sql = @"SELECT * FROM PersonalTransaction WHERE CreatedDate BETWEEN '" + fromDate + "' AND '" + endDAte + "';";
            using (ISQLiteStatement statement = cnn.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    list.Add(new PersonalTransaction()
                    {
                        Id = Convert.ToInt32(statement[0]),
                        Name = (string)statement[1],
                        Description = (string)statement[2],
                        Detail = (string)statement[3],
                        Amount = (double)statement[4],
                        CreatedAt = (DateTime)statement[5],
                        Category = Convert.ToInt32(statement[6])
                    });
                }
            }
            return list;
        }

    }

}
