using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KeepNote.DAL.Entities;

namespace KeepNote.DAL
{
    public class UserRepository
    {

        /*
          Declare variables of type SqlConnection and SqlCommand
        */
        private SqlConnection _con;
        private SqlDataAdapter _adp;
        private DataSet _ds;

        public UserRepository(string connectionString)
        {
            /*
              1. create SqlConnection instance with connectionString passed
              2. create SqlDataAdapter instance for users table
              3. create DataSet instance
              4. populate DataSet with records fetched
             */
            _con = new SqlConnection(connectionString);
            _adp = new SqlDataAdapter("Select * from Users", _con);
            _ds = new DataSet();
            _adp.Fill(_ds, "UsersData");
        }

        public List<User> GetAllUsers()
        {
            /*
              1. Traverse through the rows in table Users of DataSet
              2. For each row, populate the user object
              3. Populate list with user object
              4. return the list
             */
            List<User> users = new List<User>();

            try
            {
                foreach (DataRow item in _ds.Tables[0].Rows)
                {
                    users.Add(new User()
                    {
                        UserId = Convert.ToInt16(item["UserID"]),
                        UserName = item["UserName"].ToString(),
                        Password = item["Password"].ToString(),
                        Email = item["EMail"].ToString()
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }

            return users;
        }

        public int AddUser(User user)
        {
            /*
              1. create new DataRow
              2. populate the new DataRow with user values
              3. add this DataRow to the Rows of DataTable for Users 
              4. return the count of records
            */
            try
            {
                DataRow dr = _ds.Tables["UsersData"].NewRow();
                dr["UserID"] = 3;
                dr["UserName"] = "Steve";
                dr["Password"] = "$tevesP@55word";
                dr["EMail"] = "Steve@gmail.com";
                _ds.Tables[0].Rows.Add(dr);

                return _ds.Tables["UsersData"].Rows.Count;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int SaveChanges()
        {
            /*
              using SqlCommandBuilder update the Users table with User Records from DataSet
            */
            SqlCommandBuilder builder = new SqlCommandBuilder(_adp);
            builder.GetInsertCommand();
            return _adp.Update(_ds, "UsersData");
        }
    }
}
