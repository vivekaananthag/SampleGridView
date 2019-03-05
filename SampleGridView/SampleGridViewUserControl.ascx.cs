﻿using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Web.UI.WebControls;

public partial class SampleGridViewUserControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindGrid();
        }
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        this.BindGrid();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int customerId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
        string deleteQuery = @"DELETE FROM Customers WHERE CustomerId=@CustomerId";
        string constr = ConfigurationManager.ConnectionStrings["ServiceUserMappingEntities"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(deleteQuery))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        this.BindGrid();
    }


    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = GridView1.Rows[e.RowIndex];
        int customerId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
        string name = (row.FindControl("txtName") as TextBox).Text;
        string country = (row.FindControl("txtCountry") as TextBox).Text;
        string query = "UPDATE Customers SET Name=@Name, Country=@Country WHERE CustomerId=@CustomerId";
        string constr = ConfigurationManager.ConnectionStrings["ServiceUserMappingEntities"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Country", country);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        GridView1.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        GridView1.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        this.BindGrid();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
        {
            (e.Row.Cells[2].Controls[2] as LinkButton).Attributes["onclick"] = @"return confirm('Do you want to delete this row?')";
        }
    }

    private void BindGrid()
    {
        string constr = ConfigurationManager.ConnectionStrings["ServiceUserMappingEntities"].ConnectionString;
        string query = "SELECT * FROM Customers";
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
            {
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }
    }

    protected void Insert(object sender, EventArgs e)
    {
        var name = txtName.Text;
        var country = txtCountry.Text;

        txtName.Text = string.Empty;
        txtCountry.Text = string.Empty;

        string queryInsert = @"INSERT INTO dbo.Customers VALUES (@Name, @Country)";
        string constr = ConfigurationManager.ConnectionStrings["ServiceUserMappingEntities"].ConnectionString;

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(queryInsert))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Country", country);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        this.BindGrid();
    }
}