using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace ChartsAndDataTable
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sjekk databasen Arthurs på vår felles dbserver.
            //Table Stemme. 1 row only
            //demo for db der det er kun en rad og kolonneoverskrifter er partinavn
            //   CREATE TABLE[dbo].[Stemme] (

            //   [Kid][int] NOT NULL,

            //   [H] [int] NOT NULL,

            //   [Ap] [int] NOT NULL,

            //   [V] [int] NOT NULL,

            //   [Pp] [int] NOT NULL,
            //CONSTRAINT[PK_Stemme] PRIMARY KEY CLUSTERED
            //   (


            //Table Stemme2 - one row per parti
            //     CREATE TABLE[dbo].[Stemme2] (
            //   [Kid][int] NOT NULL,
            //   [Parti] [nvarchar] (max)NOT NULL,
            //[Stemmer][int] NOT NULL
            BindChartTableStemme();
            BindChartTableStemme2();

            //Konklusjon: Stemme2 er mer lett å bruke i forbindelse med charts
        }

        private void BindChartTableStemme()//from table stemme
        {
            DataTable dt = GetVotesFromStemme();
            //get the col name, if needed
            string colName=dt.Columns[0].ColumnName;
            

            //add en new series. It has to be one series per bar/parti can use loop....
            Chart1.Series.Add("Series1");

            //Chart1.Series[0].XValueMember = "Pp";//horisontal (col name)
            //Chart1.Series[0].XValueType = ChartValueType.Int32;//optional
            Chart1.Series[0].YValueMembers = "H";//vertical (col name) the bar for partiet H. H means the value in Column H.
            Chart1.Series[0].ChartType = SeriesChartType.Column;

            Chart1.Series[0].AxisLabel = "Høyre";//label for the bar

            Chart1.DataSource = dt;
            Chart1.DataBind();
        }

        private DataTable GetVotesFromStemme()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnCms"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * from Stemme", conn);
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
                conn.Close();
            }
            return dt;
        }

        private void BindChartTableStemme2()//from table stemme
        {
            DataTable dt = GetVotesFromStemme2();
            //get the col name, if needed
            string colName = dt.Columns[0].ColumnName;


            //add en new series. It has to be one series per bar/parti can use loop....
            Chart2.Series.Add("Series1");

            Chart2.Series[0].XValueMember = "Parti";//horisontal (col name)
            //Chart1.Series[0].XValueType = ChartValueType.Int32;//optional
            Chart2.Series[0].YValueMembers = "Stemmer";//vertical (col name) the bar for partiet H. H means the value in Column H.
            Chart2.Series[0].ChartType = SeriesChartType.Column;

            //Chart2.Series[0].AxisLabel = "Parti";//label for the bar

            Chart2.DataSource = dt;
            Chart2.DataBind();
        }

        private DataTable GetVotesFromStemme2()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnCms"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * from Stemme2", conn);
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
                conn.Close();
            }
            return dt;
        }
    }
}