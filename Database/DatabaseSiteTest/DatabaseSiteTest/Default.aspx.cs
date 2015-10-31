using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DatabaseSiteTest {
    
    public partial class _Default : Page {
        private bool success = true;
        private string message = "";

        protected void Page_Load(object sender, EventArgs e) {
            //Obtain the link from database and get the data
            string connString = ConfigurationManager.ConnectionStrings["SQLServerTest1"]
                .ConnectionString;
            string query = "SELECT * " +
                "FROM dbo.databasetest";
            DataTable temp = getSQLDatabase(connString, query);
            temp.TableName = "Temp table";

            GridView1.DataSource = temp;
            GridView1.DataBind();

            if(!success) {
                failmessage.Text = message;
            }
        }

        public DataTable getSQLDatabase(string connString, string query) {
            DataTable dataTable = new DataTable();
            dataTable.TableName = "SQL Database";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);

            try {
                conn.Open();
            } catch(SqlException ex) {
                success = false;
                message = ex.Message + "<br><br>Failed to connect to the database";
                //message = "<br><br>Failed to connect to the database";
                Console.WriteLine(ex.Message + "<br><br>Failed to connect to the database");
            } finally {
                conn.Close();
            }

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            try {
                da.Fill(dataTable);
            } catch(SqlException ex) {
                Console.WriteLine(ex.Message + "<br><br>Failed to connect to the database");
            } finally {

                conn.Close();
                da.Dispose();
            }



            return dataTable;
        }


    }
}