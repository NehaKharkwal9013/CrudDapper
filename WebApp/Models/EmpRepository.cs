using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class EmpRepository
    {
        private IConfiguration _configuration;
        public EmpRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<EmpModel> GetAllEmployee()
        {
            List<EmpModel> empModel = new List<EmpModel>();
            using (IDbConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
                if(con.State== ConnectionState.Closed)
                {
                    con.Open();
                    empModel = con.Query<EmpModel>("spGetAllEmployees").ToList();
                }
                return empModel;
            }
        }

        public int AddEmployee(EmpModel empModel)
        {
            int rowAffected = 0;
            using (IDbConnection con=new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
                if(con.State== ConnectionState.Closed)
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Name", empModel.Name);
                    parameters.Add("@Gender", empModel.Gender);
                    parameters.Add("@Department", empModel.Department);

                    rowAffected = con.Execute("spAddEmployee",parameters,commandType: CommandType.StoredProcedure);
                }
            }
            return rowAffected;
        }

        public int UpdateEmployee(EmpModel empModel)
        {
            int rowAffected = 0;
            using (IDbConnection con= new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
               if(con.State==ConnectionState.Closed)
               {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@EmpId", empModel.EmployeeId);
                    parameters.Add("@Name", empModel.Name);
                    parameters.Add("@Gender", empModel.Gender);
                    parameters.Add("@Department", empModel.Department);
                    rowAffected = con.Execute("spUpdateEmployee", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            return rowAffected;
        }

        public EmpModel GetEmployeeData(int? id)
        {
            EmpModel empModel = new EmpModel();
            if (id == null)
                return empModel;
            using (IDbConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
                string sqlQuery = "Select * from tblEmployee where EmployeeId=" + id;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@EmpId", empModel.EmployeeId);
                    empModel = con.Query<EmpModel>(sqlQuery, parameters).FirstOrDefault();
                }
                return empModel;
            }

        }

        public int DeleteEmployee(int? id)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@EmpId", id);
                    rowAffected = con.Execute("spDeleteEmployee", parameters, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
        }

    }
}
