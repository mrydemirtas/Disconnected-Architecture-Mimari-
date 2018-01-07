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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        DataSet ds = new DataSet();

        private void Form2_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=.;Initial Catalog=NORTHWND;uid=sa;pwd=1234;";

            conn.Open();

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand("select * from Products;select * from Categories;select * from suppliers");
            //adapter.SelectCommand = new SqlCommand("select p.ProductName,c.CategoryName from Products p join Categories c  on p.CategoryId=c.CategoryId");
            //inner join ile de sorgu atabiliriz.
            adapter.SelectCommand.Connection = conn;

            //DataTable table = new DataTable(); // tek bir tablo çekmek için dataTable yeterli fakat birden fazla tablo seçimi yapacak isek dataset kullanmalıyız.

            /*adapter.Fill(table);*/ // dataTable içerisini databaseden doldurma
            //productTable.DataSource = table;

            
            adapter.Fill(ds);
            //dataset li hali
            productTable.DataSource = ds.Tables[0];

            productTable.Columns[0].Visible = false;
            productTable.Columns[1].HeaderText = "Ürün Adi";


            cmbCategories.DataSource = ds.Tables[1];
            cmbCategories.ValueMember = "CategoryID";
            cmbCategories.DisplayMember = "CategoryName";

            cmbSuppliers.DataSource = ds.Tables[2];
            cmbSuppliers.ValueMember = "SupplierID";
            cmbSuppliers.DisplayMember = "ContactName";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataRow r = ds.Tables[0].NewRow();
            r["ProductName"] = txtProductName.Text;
            r["UnitPrice"] = decimal.Parse(txtPrice.Text);
            r["UnitsInStock"] = short.Parse(txtStock.Text);
            r["CategoryID"] = (int)cmbCategories.SelectedValue;
            r["SupplierID"] = (int)cmbSuppliers.SelectedValue;

            ds.Tables[0].Rows.Add(r);

            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = "Data Source=.;Initial Catalog=NORTHWND;uid=sa;pwd=1234;";
            cnn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "insert into Products(ProductName,UnitPrice,UnitsInStock,CategoryID,SupplierID) values(@ProductName,@UnitPrice,@UnitsInStock,@CategoryID,@SupplierID)";
            cmd.Connection = cnn;

            cmd.Parameters.AddWithValue("@ProductName", r["ProductName"]);
            cmd.Parameters.AddWithValue("@CategoryID", r["CategoryID"]);
            cmd.Parameters.AddWithValue("@SupplierID", r["SupplierID"]);
            cmd.Parameters.AddWithValue("@UnitsInStock", r["UnitsInStock"]);
            cmd.Parameters.AddWithValue("@UnitPrice", r["UnitPrice"]);

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = cmd;

            adapter.Update(ds.Tables[0]);

        }
    }
}
