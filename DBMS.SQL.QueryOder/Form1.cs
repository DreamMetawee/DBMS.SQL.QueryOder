using Microsoft.Data.SqlClient;
using System.Data;

namespace DBMS.SQL.QueryOder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;

        private void connectDB()
        {
            string server = @".\SQLEXPRESS";
            string db = "northwind";
            string strCon = string.Format(@"Data Source={0};Initial Catalog={1};"
                            + "Integrated Security=True;Encrypt=False", server, db);

            conn = new SqlConnection(strCon);
            conn.Open();
        }

        private void disconnectDB()
        {
            conn.Close();
        }

        private void showdata(string sql, DataGridView dgv)
        {
            da = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dgv.DataSource = ds.Tables[0];
            dgvOrder.DataSource = ds.Tables[0];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connectDB();
            string sqlQuery = "select o.OrderID,Format(o.OrderDate,'dd-MM-yy') as Order_date,"
                + " FORMAT(o.ShippedDate,'dd-MM-yy') as ShippedDate,o.ShipVia,"
                + " TitleOfCourtesy+FirstName+ ' ' + LastName EmployeeName,c.CompanyName,c.Phone ,round(SUM(OD.Quantity * OD.UnitPrice * (1 - OD.Discount)), 2) as ยอดเงินรวม   from Orders o "
                + " join [Order Details] od on o.OrderID = od.OrderID"
                + " join Shippers s on o.ShipVia = s.ShipperID"
                + " join Employees e on o.EmployeeID = e.EmployeeID"
                + " join Customers c on o.CustomerID = c.CustomerID"
                + " group by  o.OrderID, o.OrderDate, o.ShippedDate,o.ShipVia,"
                + " TitleOfCourtesy+FirstName+ ' ' + LastName ,c.CompanyName,c.Phone";
            showdata(sqlQuery, dgvOrder);

        }

        private void dgvOrder_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0) 
            { 
                int id = Convert.ToInt32(dgvOrder.CurrentRow.Cells[0].Value);
                string sqlQuery = "select p.ProductID,p.ProductName,od.Quantity,od.UnitPrice,od.Discount*100 ," 
                    + " od.Quantity + od.UnitPrice ยอดเต็ม,od.Quantity * od.UnitPrice * od.Discount ส่วนลด ," 
                    + " OD.Quantity * OD.UnitPrice * (1 - OD.Discount)" 
                    + " from Orders o " 
                    + " join [Order Details] od on o.OrderID = od.OrderID" 
                    + " join Products p on od.ProductID = p.ProductID" 
                    + " where o.OrderID = @id";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dgvDetails.DataSource = ds.Tables[0];
            }
        }
    }
}
