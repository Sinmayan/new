using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Windows.Forms;

namespace aps_finance
{
    public partial class report : Form
    {
        int month,year;
        String mte;

        private SqlConnection con = dbc.sqlc();
        public report(String mt,int m,int y)
        {

            InitializeComponent();
            month = m;
            year = y;
            mte = mt;

        }

        

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            
        }

        private void report_Load(object sender, EventArgs e)
        {
            try
            {
                
                con = dbc.sqlc();
                con.Open();
                CrystalReport1 cr = new CrystalReport1();
                empsalary es = new empsalary();
                String d1 = year + "-" + month + "-01";
                DateTime x = Convert.ToDateTime(d1);
                String d2 = x.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                TextObject text = (TextObject)cr.ReportDefinition.Sections["Section1"].ReportObjects["Text3"];
                text.Text = mte+" "+year;
             
                String query4 = "select SUM(com) from salaryh where salary_date BETWEEN '"+d1+"' AND '"+d2+"' ";
                SqlCommand sc3 = new SqlCommand(query4, con);
                double com = (double)sc3.ExecuteScalar();


                //FieldObject text3 = (FieldObject)cr.ReportDefinition.Sections["DetailSection2"].ReportObjects["Text2"];
                //text3 = com;

                cr.DataDefinition.FormulaFields["salco"].Text=com.ToString();

                



                //string query5 = "select m.Pno,Name,SUM(i.Quantity) as Buying_Quantity,i.price as Buying_Price,(i.price*SUM(i.Quantity)) as Buy_Total,SUM(o.Quantity) as Selling_Quantity,o.tPrice as Sellig_Price,(o.tprice*SUM(o.Quantity)) as Sell_Total,((o.tprice*SUM(o.Quantity))-(i.price*SUM(i.Quantity))) as Net_Total from Medicine m,inMedicine i, outMedicine o where i.Pno = m.Pno AND o.Pno = m.Pno group by m.Pno,Name,i.Quantity,i.price,o.Quantity,o.tPrice,i.datei,o.dateo"; 
                String query1 =
                    "select e.emp_id,emp_name,emp_designation,salary_month as salary_date,salary from employee e,salary s WHERE e.emp_id=s.emp_id AND salary_month BETWEEN '" +
                    d1 + "' AND '" + d2 + "'";
                String query2 =
                    "select m.Pno,Name,SUM(i.Quantity) as Buying_Quantity,i.price as Buying_Price,(i.price*SUM(i.Quantity)) as Buy_Total,SUM(o.Quantity) as Selling_Quantity,o.tPrice as Sellig_Price,(o.tprice*SUM(o.Quantity)) as Sell_Total,((o.tprice*SUM(o.Quantity))-(i.price*SUM(i.Quantity))) as Net_Total from Medicine m,inMedicine i,outMedicine o where i.Pno=m.Pno AND o.Pno=m.Pno AND i.datei BETWEEN '" +
                    d1 + "' AND '" + d2 + "' AND o.dateo BETWEEN '" + d1 + "' AND '"+d2+"' group by m.Pno,Name,i.Quantity,i.price,o.Quantity,o.tPrice,i.datei,o.dateo";
                String query3 ="select BillNo,i.Item_code,t.Item_nmae,date1,i.Quantity AS Quantity,((i.Quantity)*i.price) AS Total_Price from inItems i,Items t where i.Item_code=t.Item_code AND date1 BETWEEN '" +d1 + "' AND '" + d2 + "'";

                SqlDataAdapter sda = new SqlDataAdapter(query1, con);
                DataSet ds = new DataSet();
                sda.Fill(ds,"new");
                SqlDataAdapter sda2 = new SqlDataAdapter(query2, con);
                DataSet ds2 = new DataSet();
                sda2.Fill(ds2, "new2");
                SqlDataAdapter sda3 = new SqlDataAdapter(query3, con);
                DataSet ds3 = new DataSet();
                sda3.Fill(ds3, "new3");



                //es.SetDataSource(ds.Tables["new"]);
                //crystalReportViewer1.ReportSource = es;
                cr.Subreports[0].SetDataSource(ds.Tables["new"]);
                cr.Subreports["mediRip.rpt"].SetDataSource(ds2.Tables["new2"]);
                cr.Subreports["itemR.rpt"].SetDataSource(ds3.Tables[0]);
                crystalReportViewer1.ReportSource = cr;

                crystalReportViewer1.Refresh();
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            
            
        }
    }
}
