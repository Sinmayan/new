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

namespace aps_finance
{
    public partial class sal_edit : Form
    {
        SqlConnection con = dbc.sqlc();
        int empID;
        String s1, s2;
        DateTime t;
        public sal_edit(int x,String x1)
        {
            InitializeComponent();
            empID = x;
            s2 = x1;
            try
            {
                con.Open();
                t = Convert.ToDateTime(x1);
                s2 = t.ToString("yyyy-MM-dd");
                //MessageBox.Show(x.ToString());
                SqlDataReader sd;
                String query = "select e.emp_id,emp_name,emp_designation,salary_month,salary from salary s,employee e where e.emp_id=s.emp_id AND e.emp_id=" + empID +" AND salary_month='"+s2+"'";
                SqlCommand sc = new SqlCommand(query, con);
                sd = sc.ExecuteReader();
                if (sd.Read()) { 
                    s1= (sd["emp_id"].ToString());
                    e1.Text = s1;
                    e2.Text = (sd["emp_name"].ToString());
                    e3.Text = (sd["emp_designation"].ToString());
                    e4.Text = s2;
                    e5.Text = (sd["salary"].ToString());
                    textBox1.Text = e5.Text;
                }
                con.Close();
            }catch(SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            float f;
            bool a = float.TryParse(textBox1.Text, out f);
            if (!a)
            {
                MessageBox.Show("Enter correct amount in number");
                return;
            }
            if (MessageBox.Show("DO you want to change recode of emp_id=" + s1 + " on " + s2 + " salary " + e5.Text + " to" + textBox1.Text, "Confrim Change salary?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    String query = "UPDATE salary set salary=" + f + " where emp_id=" + s1 + " AND salary_month='" + s2 + "'";
                    SqlCommand sc = new SqlCommand(query, con);
                    sc.ExecuteNonQuery();
                    MessageBox.Show("Successfully Updated");
                    con.Close();
                    this.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("DO you want to delete recode of emp_id="+s1+" on "+s2,"Confrim Delete?",MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    String query = "DELETE from salary where emp_id = " + s1 + " AND salary_month = '" + s2 + "'";
                    SqlCommand sc = new SqlCommand(query, con);
                    sc.ExecuteNonQuery();
                    MessageBox.Show("Successfully Deleted");
                    con.Close();
                    this.Close();
                }catch(SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
