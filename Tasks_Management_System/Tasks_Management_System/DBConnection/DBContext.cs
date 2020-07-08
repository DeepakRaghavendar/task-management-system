using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Tasks_Management_System.Models;
using System.Collections;
using System.Linq;

namespace Tasks_Management_System.DBConnection
{
    public class DBContext
    {
        String connectionString = "Datasource=localhost;Database=Task_Management_System;uid=root;pwd=Desept@3;persistsecurityinfo=True";

        //Authentication Purpose

        //public bool isAuthorizedUser(string userName)
        //{
        //    MySqlConnection connection = new MySqlConnection(connectionString);
        //    connection.Open();
        //    MySqlCommand cmd = new MySqlCommand("Select * From user_groups where username = '" + userName + "'", connection);
        //    var isAuthenticated = cmd.ExecuteScalar();

        //    if (isAuthenticated != null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //getTasks method shows the list of tasks

        public DataSet getTasks()
        {
            DataSet ds = new DataSet();

            try    //exception handling
            {
                string SP_NAME = "GET_ALL_TASKS"; // procedure name

                //establishing the connection

                MySqlConnection connection = new MySqlConnection(connectionString);

                connection.Open();

                //command execution

                MySqlCommand cmd = new MySqlCommand(SP_NAME, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                connection.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ds;   //returns the dataset
        }


        //getTasksWithID method shows the tasks based on the single id
        public DataSet getTasksWithID(int TASK_ID)
        {
            DataSet ds = new DataSet();

            try
            {
                string SP_NAME = "GET_INDIVIDUAL_TASK";  //Procedure name

                //establishing the connection
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                //command execution
                MySqlCommand cmd = new MySqlCommand(SP_NAME, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@INDIVIDUAL_TASK_ID", TASK_ID));
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                connection.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //returns the dataset
            return ds;
        }


        //saveTasks method inserts the tasks to the database
        public string saveTasks(string CUSTOMER_NAME, string TASK_DESCRIPTION, string START_DATE, string FINISH_DATE, string TASK_STATE)
        {
            string message = "";
            try   //exception handling
            {
                string SP_NAME = "INSERT_TASK"; //procedure name
                int rowCount = 0;         
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                //establishing the connection
                MySqlCommand cmd = new MySqlCommand(SP_NAME, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@CUSTOMER_NAME", CUSTOMER_NAME));
                cmd.Parameters.Add(new MySqlParameter("@TASK_DESCRIPTION", TASK_DESCRIPTION));
                cmd.Parameters.Add(new MySqlParameter("@START_DATE", START_DATE));
                cmd.Parameters.Add(new MySqlParameter("@FINISH_DATE", FINISH_DATE));
                cmd.Parameters.Add(new MySqlParameter("@TASK_STATE", TASK_STATE));

                //query execution
                rowCount = cmd.ExecuteNonQuery();
                if(rowCount > 0)
                {

                    //returns success message, if execution is successfull
                    return message = "Task Inserted Successfully";
                }
                
            }
            catch(Exception ex)
            {
                //returns message, if there is any exception

                return message = ex.Message;
            }

            //returns message, if execution is unsuccessfull

            return message = "Task Insertion Unsuccessful"; ;
        }


        //updateTask method updates the tasks with unique parameter task id
        public string updateTask(int TASK_ID, string CUSTOMER_NAME, string TASK_DESCRIPTION, string START_DATE, string FINISH_DATE, string TASK_STATE)
        {
            string message = "";
            try  //exception handling
            {
                string SP_NAME = "UPDATE_TASK";  //procedure name
                int rowCount = 0;
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                //establishes the connection
                MySqlCommand cmd = new MySqlCommand(SP_NAME, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@INDIVIDUAL_TASK_ID", TASK_ID));
                cmd.Parameters.Add(new MySqlParameter("@CUSTOMER_NAME", CUSTOMER_NAME));
                cmd.Parameters.Add(new MySqlParameter("@TASK_DESCRIPTION", TASK_DESCRIPTION));
                cmd.Parameters.Add(new MySqlParameter("@START_DATE", START_DATE));
                cmd.Parameters.Add(new MySqlParameter("@FINISH_DATE", FINISH_DATE));
                cmd.Parameters.Add(new MySqlParameter("@TASK_STATE", TASK_STATE));

                //query execution
                rowCount = cmd.ExecuteNonQuery();
                if (rowCount > 0)
                {
                    //returns message when update is successful
                    return message = "Task Updated Successfully";
                }

            }
            catch (Exception ex)
            {
                //returns the exception message, when there is any exception
                return message = ex.Message;
            }

            //returns message, if execution is unsuccessfull

            return message = "Task Updation Unsuccessful"; ;
        }

        //deleteTask method deletes the task with unique parameter task id
        public string deleteTask(int TASK_ID)
        {
            string message = "";
            try   //exception handling
            {
                string SP_NAME = "DELETE_TASK";
                int rowCount = 0;

                //establishing the connection
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(SP_NAME, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@INDIVIDUAL_TASK_ID", TASK_ID));
                //query execution
                rowCount = cmd.ExecuteNonQuery();
                if (rowCount > 0)
                {
                    //returns message when delete is successful

                    return message = "Task Deleted Successfully";
                }

            }
            catch (Exception ex)
            {
                //returns exception message when there is any exception

                return message = ex.Message;
            }

            //returns message when delete is unsuccessful

            return message = "Task Deletion Unsuccessful"; ;
        }

        //retrieveTasks retrives the list of tasks based on the input parameter task state
        public List<Tasks> retrieveTasks(string task_State)
        {

            Tasks objEmp = new Tasks();

            //creating the list
            List<Tasks> taskList = new List<Tasks>();
            try    //exception handling
            {
                DataTable dt = new DataTable();  //datatable declaration
                string SP_NAME = "RETRIEVE_TASKS";

                //establishes the connection
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(SP_NAME, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@INDIVIDUAL_TASK_STATE", task_State));
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                connection.Close();
                if(dt.Rows.Count > 0)
                {
                    taskList = (from DataRow dr in dt.Rows
                                   select new Tasks()
                                   {
                                       TASK_ID = Convert.ToInt32(dr["TASK_ID"]),
                                       CUSTOMER_NAME = dr["CUSTOMER_NAME"].ToString(),
                                       TASK_DESCRIPTION = dr["TASK_DESCRIPTION"].ToString(),
                                       START_DATE = dr["START_DATE"].ToString(),
                                       FINISH_DATE = dr["FINISH_DATE"].ToString(),
                                       TASK_STATE = dr["TASK_STATE"].ToString(),
                                       
                                   }).ToList();   // converts the data ro list format

                }

            }

            catch (Exception ex)
            {
                //throws the exception message
                Console.WriteLine(ex.Message);
            }

            return taskList;

        }

    }
}
