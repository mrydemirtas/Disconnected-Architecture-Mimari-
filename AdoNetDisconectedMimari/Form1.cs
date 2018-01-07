using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetDisconectedMimari
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataSet ds = new DataSet();

        private void Form1_Load(object sender, EventArgs e)
        {
            //database kendisi

            DataTable table1 = new DataTable("Ogrenci");

            DataColumn column1 = new DataColumn("Id");
            column1.AutoIncrement = true; // identity
            column1.AutoIncrementSeed = 1; // 1 den başlasın, // tablonun numarası
            column1.Caption = "OgrenciId";
            column1.DataType = typeof(int);
            //tablonun primarykey alanı colmun1

            DataColumn column2 = new DataColumn();
            column2.ColumnName = "StudentName";
            column2.Caption = "ÖğrenciAdi";
            column2.DataType = typeof(string);
            column2.MaxLength = 50;
            column2.AllowDBNull = false;

            DataColumn column3 = new DataColumn();
            column3.ColumnName = "StudentSurName";
            column3.Caption = "ÖğrenciSoyad";
            column3.DataType = typeof(string);
            column3.MaxLength = 50;
            column3.AllowDBNull = false;

            table1.Columns.Add(column1);
            table1.Columns.Add(column2);
            table1.Columns.Add(column3);
            table1.PrimaryKey = new DataColumn[] { column1 };

            //boş tablo oluştu

            ds.Tables.Add(table1);

        }

        public void InsertToTable(DataTable source, string studentName, string studentSurName)
        {
            DataRow row = source.NewRow(); //newRow yeni bir satır aç
            row["StudentName"] = studentName;
            row["StudentSurName"] = studentSurName;
            source.Rows.Add(row); //tabloya oıluşturulan satır bilgilerini ekle
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            InsertToTable(ds.Tables["Ogrenci"], txtStudentName.Text, txtStudentSurName.Text);

            //öğrenci tablosunu veri kaynağı olarak göster.
            studentTable.DataSource = ds.Tables["Ogrenci"];

        }

        public void SendDatabase()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Database=MyTestDB;uid=sa;pwd=1234;";

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            object result = null;
            SqlCommand cmd = new SqlCommand();

            try
            {

                cmd.CommandText = "select count(*) from Ogrenci";
                cmd.Connection = conn;
                result = cmd.ExecuteScalar();

            }
            catch (Exception)
            {
                result = null;
            }

            if (result == null)
            {
                cmd = new SqlCommand();
                cmd.CommandText = "create table " + ds.Tables["Ogrenci"].TableName + "(" + ds.Tables["Ogrenci"].Columns[0].ColumnName + " int primary key identity(1,1)," + ds.Tables["Ogrenci"].Columns[1].ColumnName + " nvarchar(50) " + "," + ds.Tables["Ogrenci"].Columns[2].ColumnName + " nvarchar(50) " + ")";
                cmd.Connection = conn;

                cmd.ExecuteNonQuery();

                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                {
                    //hangi tabloya yazdıracağımıza karar verir.
                    bulkcopy.DestinationTableName = ds.Tables["Ogrenci"].TableName;
                    //sunucuya yazdır.
                    bulkcopy.WriteToServer(ds.Tables["Ogrenci"]);
                }

            }
            else
            {
                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                {
                    //hangi tabloya yazdıracağımıza karar verir.
                    bulkcopy.DestinationTableName = ds.Tables["Ogrenci"].TableName;
                    //sunucuya yazdır.
                    bulkcopy.WriteToServer(ds.Tables["Ogrenci"]);
                }
            }

        }

        public void Test()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Database=MyTestDB;uid=sa;pwd=1234;";

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            object result = null;
            SqlCommand cmd = new SqlCommand();

            try
            {

                cmd.CommandText = "select count(*) from Ogrenci";
                cmd.Connection = conn;
                result = cmd.ExecuteScalar();

            }
            catch (Exception)
            {
                result = null;
            }

            if (result == null)
            {
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandText = "create table " + ds.Tables["Ogrenci"].TableName + "(" + ds.Tables["Ogrenci"].Columns[0].ColumnName + " int primary key identity(1,1)," + ds.Tables["Ogrenci"].Columns[1].ColumnName + " nvarchar(50) " + "," + ds.Tables["Ogrenci"].Columns[2].ColumnName + " nvarchar(50) " + ")";
                cmd1.Connection = conn;

                cmd1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("insert into Ogrenci(StudentName,StudentSurName) values(@StudentName,@StudentSurName)");
                cmd2.Connection = conn;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = cmd2;

                cmd2.Parameters.Add("@StudentName", SqlDbType.NVarChar, 50, "StudentName");
                cmd2.Parameters.Add("@StudentSurName", SqlDbType.NVarChar, 50,"StudentSurName");

                adapter.Update(ds.Tables["Ogrenci"]);
            }
            else
            {
                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                {
                    //hangi tabloya yazdıracağımıza karar verir.
                    bulkcopy.DestinationTableName = ds.Tables["Ogrenci"].TableName;
                    //sunucuya yazdır.
                    bulkcopy.WriteToServer(ds.Tables["Ogrenci"]);
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //SendDatabase();
            Test();
        }
    }
}
