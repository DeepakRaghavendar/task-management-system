using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Tasks_Management_System.DBConnection;
using Tasks_Management_System.Models;
using LicenseContext = OfficeOpenXml.LicenseContext;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tasks_Management_System.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        // GET: api/tasks
        [HttpGet]
        public IActionResult Get()
        {
            try //Exception Handling
            {
                //Database Connection Object
                DBContext db = new DBContext();
                //Gets the list of tasks in JSON format
                var response = db.getTasks();
                return new JsonResult(new { message = response });
            }

            catch (Exception ex)
            {
                //throwing the exception message in JSON Format
                return new JsonResult(new { message = ex.Message });
            }

        }


        // GET api/tasks/1
        [HttpGet("{id}")]
        public IActionResult Get([FromBody] Tasks taskObject)
        {
            try
            {
                DBContext db = new DBContext();
                //Gets the particular task in JSON format
                var result = db.getTasksWithID(taskObject.TASK_ID);
                return new JsonResult(new { message = result });
            }

            catch (Exception ex)
            {
                //throwing the exception message in JSON Format
                return new JsonResult(new { message = ex.Message });
            }
        }

        // GET: api/tasks/exportv2
        [HttpGet("exportv2")]
        public async Task<IActionResult> ExportV2([FromBody]CancellationToken cancellationToken, Tasks taskObject)
        {
            // LicenseContext of the ExcelPackage class:
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:

            await Task.Yield();
            DBContext db = new DBContext();

            // query data from database 
            var taskList = db.retrieveTasks(taskObject.TASK_STATE);
            var stream = new MemoryStream();
             
            //creating the excel worksheet using excel package manager(EPP)
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(taskList, true);
                package.Save();
            }
            stream.Position = 0;

            //filename for excel worksheet
            string excelName = $"TaskReport{(" for " + taskObject.TASK_STATE)}.xlsx";

            //Returns the excel file
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        // POST api/tasks
        [HttpPost]
        public IActionResult Post([FromBody] Tasks taskObject )
        {
            
            try      //Exception Handling
            {
                DBContext db = new DBContext();

                //query from database
                var result = db.saveTasks(taskObject.CUSTOMER_NAME, taskObject.TASK_DESCRIPTION,
                    taskObject.START_DATE, taskObject.FINISH_DATE, taskObject.TASK_STATE);

                //displays the message based on the response code

                if (Response.StatusCode == 200)
                {
                    var successMessage = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Task Created Successfully" };
                    return new JsonResult(new { message = successMessage });
                }

                else
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Something Went Wrong" };
                    return new JsonResult(new { message = responseMessage });
                }


            }

            catch (Exception ex)
            {
                //throws the message if we have the exception
                return new JsonResult(new { message = ex.Message });
            }


        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Tasks taskObject)
        {
            try
            {
                DBContext db = new DBContext();

                //query from database
                var result = db.updateTask(taskObject.TASK_ID, taskObject.CUSTOMER_NAME, taskObject.TASK_DESCRIPTION,
                    taskObject.START_DATE, taskObject.FINISH_DATE, taskObject.TASK_STATE);

                //displays the message based on the response code

                if (Response.StatusCode == 200)
                {
                    var successMessage = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Task Updated Successfully" };
                    return new JsonResult(new { message = successMessage });
                }

                else
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Something Went Wrong" };
                    return new JsonResult(new { message = responseMessage });
                }

            }

            catch(Exception ex)
            {
                //throws the message if we have the exception

                return new JsonResult(new { message = ex.Message });

            }

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromBody] Tasks taskObject)
        {
            try
            {
                DBContext db = new DBContext();

                //query from database
                var result = db.deleteTask(taskObject.TASK_ID);


                //displays the message based on the response code

                if (Response.StatusCode == 200)
                {
                    var successMessage = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Task Deleted Successfully" };
                    return new JsonResult(new { message = successMessage });
                }

                else
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Something Went Wrong" };
                    return new JsonResult(new { message = responseMessage });
                }

            }

            catch (Exception ex)
            {
                //throws the message if we have the exception
                return new JsonResult(new { message = ex.Message });

            }
        }

        }

    }
