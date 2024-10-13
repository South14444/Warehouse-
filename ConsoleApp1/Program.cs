using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        SqlConnection conn = null;
        Program()
        {
            string sql = ConfigurationManager.ConnectionStrings["MssqlCoonWarehouse"].ConnectionString;
            conn = new SqlConnection();
            conn.ConnectionString = sql;
        }
        static void Main(string[] args)
        {
            Program pr = new Program();
            pr.SQlOpen();
            Console.WriteLine("1-3 задачи");
            Console.WriteLine("2-4 Задания");
            Console.WriteLine("3-Добавить товар");
            Console.WriteLine("4-Добавить тип товара");
            Console.WriteLine("5-Добавить поставщика");
            Console.WriteLine("6-Обновить товар");
            Console.WriteLine("7-Обновить тип товара");
            Console.WriteLine("8-Обновить поставщика");
            Console.WriteLine("9-Удалить товар");
            Console.WriteLine("10-Удалить тип товара");
            Console.WriteLine("11-Удалить поставщика");
            Console.WriteLine("12-Поставщик с наибольшим количеством товаров");
            Console.WriteLine("13-Поставщик с наименьшим количеством товаров");
            Console.WriteLine("14-Тип товара с наибольшим количеством товаров");
            Console.WriteLine("15-Тип товара с наименьшим количеством товаров");
            Console.WriteLine("16-Показать товары, поставка которых произошла более заданного количества дней");

            string str = Console.ReadLine();
            switch (str)
            {
                case "1":
                    pr.Selectnomber3();
                    break;
                case "2":
                    pr.Selectnomber4();
                    break;
                case "3":
                    pr.InsertProduct();
                    break;
                case "4":
                    pr.InsertType();
                    break;
                case "5":
                    pr.InsertSupplier();
                    break;
                case "6":
                    pr.UpdateProduct();
                    break;
                case "7":
                    pr.UpdateType();
                    break;
                case "8":
                    pr.UpdateSupplier();
                    break;
                case "9":
                    pr.DeleteProduct();
                    break;
                case "10":
                    pr.DeleteType();
                    break;
                case "11":
                    pr.DeleteSupplier();
                    break;
                case "12":
                    pr.ShowSupplierWithMostProducts();
                    break;
                case "13":
                    pr.ShowSupplierWithLeastProducts();
                    break;
                case "14":
                    pr.ShowTypeWithMostProducts();
                    break;
                case "15":
                    pr.ShowTypeWithLeastProducts();
                    break;
                case "16":
                    pr.ShowProductsByDeliveryDays();
                    break;
            }
        }
        public void ShowProductsByDeliveryDays()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Введите количество дней:");
                int days = int.Parse(Console.ReadLine());

                string query = @"SELECT P.Name, T.Name AS TypeName, S.Name AS SupplierName, P.Quantity, P.Cost_price, P.Date
                         FROM Product P
                         JOIN Type T ON P.TypeId = T.ID
                         JOIN Supplier S ON P.SupplierId = S.ID
                         WHERE DATEDIFF(DAY, P.Date, GETDATE()) >= @days";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@days", days);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("Товары, поставка которых произошла более заданного количества дней:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Товар: {reader["Name"]}, Тип: {reader["TypeName"]}, Поставщик: {reader["SupplierName"]}, Количество: {reader["Quantity"]}, Стоимость: {reader["Cost_price"]}, Дата поставки: {reader["Date"]}");
                    }
                }
                else
                {
                    Console.WriteLine("Нет данных для отображения.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void ShowTypeWithLeastProducts()
        {
            try
            {
                conn.Open();
                string query = @"SELECT TOP 1 T.Name, SUM(P.Quantity) AS TotalQuantity
                         FROM Type T
                         JOIN Product P ON T.ID = P.TypeId
                         GROUP BY T.Name
                         ORDER BY TotalQuantity ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Тип товара: {reader["Name"]}, Количество товаров: {reader["TotalQuantity"]}");
                    }
                }
                else
                {
                    Console.WriteLine("Нет данных для отображения.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void ShowTypeWithMostProducts()
        {
            try
            {
                conn.Open();
                string query = @"SELECT TOP 1 T.Name, SUM(P.Quantity) AS TotalQuantity
                         FROM Type T
                         JOIN Product P ON T.ID = P.TypeId
                         GROUP BY T.Name
                         ORDER BY TotalQuantity DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Тип товара: {reader["Name"]}, Количество товаров: {reader["TotalQuantity"]}");
                    }
                }
                else
                {
                    Console.WriteLine("Нет данных для отображения.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void ShowSupplierWithLeastProducts()
        {
            try
            {
                conn.Open();
                string query = @"SELECT TOP 1 S.Name, SUM(P.Quantity) AS TotalQuantity
                         FROM Supplier S
                         JOIN Product P ON S.ID = P.SupplierId
                         GROUP BY S.Name
                         ORDER BY TotalQuantity ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Поставщик: {reader["Name"]}, Количество товаров: {reader["TotalQuantity"]}");
                    }
                }
                else
                {
                    Console.WriteLine("Нет данных для отображения.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void ShowSupplierWithMostProducts()
        {
            try
            {
                conn.Open();
                string query = @"SELECT TOP 1 S.Name, SUM(P.Quantity) AS TotalQuantity
                         FROM Supplier S
                         JOIN Product P ON S.ID = P.SupplierId
                         GROUP BY S.Name
                         ORDER BY TotalQuantity DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Поставщик: {reader["Name"]}, Количество товаров: {reader["TotalQuantity"]}");
                    }
                }
                else
                {
                    Console.WriteLine("Нет данных для отображения.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }


        public void DeleteProduct()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Введите ID товара, который хотите удалить:");
                int productId = int.Parse(Console.ReadLine());

                string query = @"DELETE FROM Product WHERE ID = @productId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@productId", productId);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Товар успешно удален!" : "Товар с таким ID не найден.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        public void DeleteType()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Введите ID типа товара, который хотите удалить:");
                int typeId = int.Parse(Console.ReadLine());

                string query = @"DELETE FROM Type WHERE ID = @typeId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@typeId", typeId);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Тип товара успешно удален!" : "Тип товара с таким ID не найден.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        public void DeleteSupplier()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Введите ID поставщика, которого хотите удалить:");
                int supplierId = int.Parse(Console.ReadLine());

                string query = @"DELETE FROM Supplier WHERE ID = @supplierId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@supplierId", supplierId);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Поставщик успешно удален!" : "Поставщик с таким ID не найден.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void UpdateProduct()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Введите ID товара, который хотите обновить:");
                int productId = int.Parse(Console.ReadLine());

                Console.WriteLine("Введите новое название товара:");
                string newName = Console.ReadLine();

                Console.WriteLine("Введите новый ID типа товара:");
                int newTypeId = int.Parse(Console.ReadLine());

                Console.WriteLine("Введите новый ID поставщика:");
                int newSupplierId = int.Parse(Console.ReadLine());

                Console.WriteLine("Введите новое количество:");
                int newQuantity = int.Parse(Console.ReadLine());

                Console.WriteLine("Введите новую стоимость:");
                decimal newCostPrice = decimal.Parse(Console.ReadLine());

                Console.WriteLine("Введите новую дату (в формате YYYY-MM-DD):");
                string newDate = Console.ReadLine();

                string query = @"UPDATE Product 
                         SET Name = @newName, 
                             TypeId = @newTypeId, 
                             SupplierId = @newSupplierId, 
                             Quantity = @newQuantity, 
                             Cost_price = @newCostPrice, 
                             Date = @newDate 
                         WHERE ID = @productId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@productId", productId);
                cmd.Parameters.AddWithValue("@newName", newName);
                cmd.Parameters.AddWithValue("@newTypeId", newTypeId);
                cmd.Parameters.AddWithValue("@newSupplierId", newSupplierId);
                cmd.Parameters.AddWithValue("@newQuantity", newQuantity);
                cmd.Parameters.AddWithValue("@newCostPrice", newCostPrice);
                cmd.Parameters.AddWithValue("@newDate", newDate);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Товар успешно обновлен!" : "Товар не найден.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        public void UpdateType()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Введите ID типа товара, который хотите обновить:");
                int typeId = int.Parse(Console.ReadLine());

                Console.WriteLine("Введите новое название типа товара:");
                string newTypeName = Console.ReadLine();

                string query = @"UPDATE Type 
                         SET Name = @newName 
                         WHERE ID = @typeId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@typeId", typeId);
                cmd.Parameters.AddWithValue("@newName", newTypeName);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Тип товара успешно обновлен!" : "Тип товара не найден.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        public void UpdateSupplier()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Введите ID поставщика, которого хотите обновить:");
                int supplierId = int.Parse(Console.ReadLine());

                Console.WriteLine("Введите новое название поставщика:");
                string newSupplierName = Console.ReadLine();

                string query = @"UPDATE Supplier 
                         SET Name = @newName 
                         WHERE ID = @supplierId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@supplierId", supplierId);
                cmd.Parameters.AddWithValue("@newName", newSupplierName);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Поставщик успешно обновлен!" : "Поставщик не найден.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void InsertProduct()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Введите название товара:");
                string productName = Console.ReadLine();
                Console.WriteLine("Введите ID типа товара:");
                int typeId = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите ID поставщика:");
                int supplierId = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите количество:");
                int quantity = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите стоимость:");
                decimal costPrice = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Введите дату (в формате YYYY-MM-DD):");
                string date = Console.ReadLine();

                string query = @"INSERT INTO Product (Name, TypeId, SupplierId, Quantity, Cost_price, Date) 
                         VALUES (@name, @typeId, @supplierId, @quantity, @costPrice, @date)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", productName);
                cmd.Parameters.AddWithValue("@typeId", typeId);
                cmd.Parameters.AddWithValue("@supplierId", supplierId);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@costPrice", costPrice);
                cmd.Parameters.AddWithValue("@date", date);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Товар успешно добавлен!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void InsertType()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Введите название типа товара:");
                string typeName = Console.ReadLine();

                string query = @"INSERT INTO Type (Name) VALUES (@name)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", typeName);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Тип товара успешно добавлен!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void InsertSupplier()
        {
            try
            {
                conn.Open();
                Console.WriteLine("Введите название поставщика:");
                string supplierName = Console.ReadLine();

                string query = @"INSERT INTO Supplier (Name) VALUES (@name)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", supplierName);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Поставщик успешно добавлен!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void Selectnomber4()
        {
            // Выполняем подключение к бд
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                string str = Console.ReadLine();
                string query = string.Empty;
                string stre2 = string.Empty;
                switch (str)
                {
                    case "1":
                         stre2 = Console.ReadLine();
                        query = @"select P.Name,T.name,S.name,Quantity,Cost_price,Date date
                            from Product as P
                            join Type as T On P.TypeId=t.ID
                            join Supplier as S On P.SupplierId=S.ID
                            where T.Name like @p1;";
                        break;
                    case "2":
                        stre2 = Console.ReadLine();
                        query = @"select P.Name,T.name,S.name,Quantity,Cost_price,Date date
                            from Product as P
                            join Type as T On P.TypeId=t.ID
                            join Supplier as S On P.SupplierId=S.ID
                            where S.Name like @p1;";
                        break;
                    case "3":
                        query = @"select P.Name,T.name,S.name,Quantity,Cost_price,Min(Date)
                            from Product as P
                            join Type as T On P.TypeId=t.ID
                            join Supplier as S On P.SupplierId=S.ID;";
                        break;
                    case "4":
                        query = @"select T.name, sum(Quantity)/COUNT(P.ID) as 'Среднее значение'
                            from Product as P
                            join Type as T On P.TypeId=t.ID
                            join Supplier as S On P.SupplierId=S.ID
                            group by T.name;";
                        break;
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = stre2;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    //Названия Столбцов
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}\t");
                    }
                    Console.WriteLine();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetValue(i)}\t");
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public void Selectnomber3()
        {
            // Выполняем подключение к бд
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                string str = Console.ReadLine();
                string query = string.Empty;
                bool flag = true;
                switch (str)
                {
                    case "1":
                        query = @"select *
                            from Product as P
                            join Type as T On P.TypeId=t.ID
                            join Supplier as S ON P.SupplierId=S.ID";
                        break;
                    case "2":
                        query = @"select * from Type;";
                        break;
                    case "3":
                        query = @"select * from Supplier;";
                        break;
                    case "4":
                        query = @"select Max(Quantity) as 'Max(Quantity)' from Product;";
                        flag = false;
                        break;
                    case "5":
                        flag = false;
                        query = @"select Min(Quantity) as 'Min(Quantity)'from Product;";
                        break;
                    case "6":
                        flag = false;
                        query = @"select Max(Cost_price) as 'Max(Cost_price)' from Product;";
                        break;
                    case "7":
                        flag = false;
                        query = @"select Min(Cost_price) as 'Min(Cost_price)' from Product;";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                if (flag)
                {
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //Названия Столбцов
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetName(i)}\t");
                        }
                        Console.WriteLine();
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write($"{reader.GetValue(i)}\t");
                            }
                            Console.WriteLine();
                        }
                    }
                }
                else
                {
                    Console.WriteLine(cmd.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public void SQlOpen()
        {            //подключение к бд
            try
            {
                conn.Open();
                Console.WriteLine("Успешно открыто");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}
