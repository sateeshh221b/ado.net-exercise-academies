using KeepNote.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace KeepNote.DAL
{
    public class NoteRepository
    {
        /*
          Declare variables of type SqlConnection and SqlCommand
        */
        SqlConnection con;
        SqlCommand cmd;

        public NoteRepository(string connectionString)
        {
            /*
              Instantiate SqlConnection object with the connectionString passed to the constructor
              Instantiate SqlCommand object
             */
            con = new SqlConnection(connectionString);
            cmd = new SqlCommand();
        }

        //Read all notes 
        public List<Note> GetAllNotes()
        {
            /*
              1. open connection
              2. set the command text of SqlCommand object with appropriate query to read all notes
              3. using ExecuteReader() method of SqlCommand object fetch data
              4. Recursively read the records fetced one by one and populate the note object
              5. Populate the list object with note object on each iteration
              6. close the connection
              7. Return the populated list
            */


            try
            {
                cmd.CommandText = "Select * from Notes";
                cmd.Connection = con;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                List<Note> allNotes = new List<Note>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        allNotes.Add(new Note()
                        {
                            NoteId = Convert.ToInt16(dr["NoteID"]),
                            Title = dr["Title"].ToString(),
                            Description = dr["Description"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["CreatedBy"])
                        });
                    }
                }
                return allNotes;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public int AddNote(Note note)
        {

            /*
              1. open connection
              2. set the command text of SqlCommand object with appropriate query to insert note record
              3. execute ExecuteNonQuery() method 
              4. close the connection
              5. return the count of records
            */

            try
            {
                cmd.Connection = con;
                cmd.CommandText = "INSERT INTO[dbo].[Notes] ([NoteID],[Title],[Description],[CreatedBy]) VALUES (@noteId, @title, @description, @createdBy)";
                cmd.Parameters.AddWithValue("@noteId", Convert.ToInt16(note.NoteId));
                cmd.Parameters.AddWithValue("@title", note.Title);
                cmd.Parameters.AddWithValue("@description", note.Description);
                cmd.Parameters.AddWithValue("@createdBy", Convert.ToInt16(note.CreatedBy));
                con.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
