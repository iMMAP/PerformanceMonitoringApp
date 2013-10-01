using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;

	/// <summary>
	/// Implements database routines
	/// Author: Asad Aziz
    /// Date: 06/19/2008
	/// </summary>
	public class DbManager : IDisposable
    {        
        public static DbManager GetDbManager()
        {
            return new DbManager(System.Configuration.ConfigurationManager.ConnectionStrings["OCHASRConnectionString"].ConnectionString);
        }

        


		#region Member variables

		private SqlConnection cn;
		private string parameters;
        private string connectionString;
        private string procName;
        private bool _disposed = false;

		#endregion

        #region Properties
        public virtual bool Disposed
        {
            get
            {
                return _disposed;
            }
            set
            {
                _disposed = value;
            }
        }

        public string ProcName
        {
            get { return procName; }
            set { procName = value;    }
        }

        #endregion

        #region Constructor of Data Handler

        public DbManager(string dbConnectionString)
        {
            this.connectionString = dbConnectionString;
        }

		public DbManager(string dbConnectionString, string storedProcedureName)
		{
            procName = storedProcedureName;
            this.connectionString = dbConnectionString;
		}

		#endregion

		#region GetDataSet(string Name)
		
		/// <summary>
		/// Make input param.
		/// Create a data set object
		/// <param name="name">Name of table, with which we want to create data set </param>
		/// <returns>Data set object </returns>
		
		public DataSet GetDataSet(string name)
		{
            try
            {
                Open();
                SqlDataAdapter adapter = new SqlDataAdapter(name + " " + parameters, cn);
                DataSet dataset = new DataSet(name);
                adapter.Fill(dataset);
                
                return dataset;
            }
            finally
            {
                Close();
            }
			
		}
		#endregion

		#region GetDataSet()
		/// <summary>
		/// Create a data set object
		/// <param name="name">Name of table, with which we want to create data set </param>
		/// <returns>Data set object </returns>
		public DataSet GetDataSet()
		{
            try
            {
                if (procName == "")
                    throw new Exception("Procedure name must be set.");
                Open();
                SqlDataAdapter adapter = new SqlDataAdapter(procName + " " + parameters, cn);
                DataSet dataset = new DataSet();
                adapter.Fill(dataset);
                
                return dataset;
            }
            finally
            {
                Close();
            }
			
		}
		#endregion

		#region FillDataSet(DataSet)
		public DataSet FillDataSet( DataSet dataset)
		{
            try
            {
                Open();
                SqlDataAdapter adapter = new SqlDataAdapter(procName + " " + parameters, cn);
                adapter.Fill(dataset);
                
                return dataset;
            }
            finally
            {
                Close();
            }
		}
		#endregion

		#region GetDataSet(string Name,int startRow,int maxRow)
		/// <summary>
		/// Create a data set object containing specified no. of records
		/// <param name="name">Name of table, with which we want to create data set </param>
		/// <param name="startRow">Start row, of data set</param>
		/// <param name="maxRecords">Maximum no of records to be returned in data set</param>
		/// <returns>Data set object </returns>

		public DataSet GetDataSet(string name,int startRow,int maxRecords)
		{
            try
            {
                if (procName == "")
                    throw new Exception("Procedure name must be set.");

                Open();
                SqlDataAdapter adapter = new SqlDataAdapter(procName + " " + parameters, cn);
                DataSet dataset = new DataSet(name);
                adapter.Fill(dataset, startRow, maxRecords, name);
              
                return dataset;
            }
            finally
            {
                Close();
            }

		}
        #endregion

		#region GetDataView(string Name,int startRow,int maxRow)
		/// <summary>
		/// Create a data view object containing specified no of records
		/// <param name="name">Name of table, with which we want to create data set </param>
		/// <param name="startRow">Start row, of data set</param>
		/// <param name="maxRecords">Maximum no of records to be returned in data set</param>
		/// <returns>Data set object </returns>


		public DataView GetDataView(string name,int startRow,int maxRecords)
		{
            try
            {
                if (procName == "")
                    throw new Exception("Procedure name must be set.");
                Open();
                SqlDataAdapter adapter = new SqlDataAdapter(procName + " " + parameters, cn);
                DataSet dataset = new DataSet(name);
                adapter.Fill(dataset, startRow, maxRecords, name);
                return dataset.Tables[0].DefaultView;
            }
            finally
            {
                Close();
            }

		}
		#endregion

		#region GetDataView
		/// <summary>
		/// Create a data set object
		/// <param name="name">Name of table, with which we want to create data set </param>
		/// <returns>Data set object </returns>

		public DataView GetDataView(string name)
		{
            try{
			if (procName=="")
				throw new Exception("Procedure name must be set.");
			Open();
			SqlDataAdapter adapter=new SqlDataAdapter(procName+" "+parameters ,cn);
			DataSet dataset=new DataSet(name);
			adapter.Fill(dataset,name);		
			return dataset.Tables[0].DefaultView;
        }
        finally
        {
            Close();
        }

		}
		#endregion

		#region GetDataReader
		/// <summary>
		/// Create a data reader object
		/// <returns>Data set object </returns>
		
		public SqlDataReader GetDataReader()
		{            
		    if (procName=="")
			    throw new Exception("Procedure name must be set.");
		    Open();
		    SqlCommand cmd=new SqlCommand(procName+" "+parameters ,cn);
		    SqlDataReader reader=cmd.ExecuteReader(CommandBehavior.CloseConnection);
		    return reader;            
		}	
		#endregion

		#region ExecuteScalar
		/// <summary>
		/// Execute procedure and returns single value returned by it.
		/// <returns> int containing value returned by user. </returns>
		///  <summary>
		public int ExecuteScalar()
		{
            try
            {
                if (procName == "")
                    throw new Exception("Procedure name must be set.");
                Open();
                int result;
                SqlCommand cmd = new SqlCommand(procName + " " + parameters, cn);
                result = (int)cmd.ExecuteScalar();
                return result;
            }            
            finally
            {
                Close();
            }
		}
		#endregion

		#region AddParameter(string Name,Object Data)
		/// Add Parameter
		/// <param name="name">Name of parameter </param>
		/// <returns>Value of parameter</returns>


		public void AddParameter(string name,Object data )
		{
			parameters=parameters + name + "=" + data;
		}
		#endregion

		#region RunProcedure(string name)
		/// <summary>
		/// Run stored procedure.
		/// </summary>
		/// <param name="procName">Name of stored procedure.</param>
		/// <param name="prams">Stored procedure params.</param>
		/// <returns>Stored procedure return value.</returns>
		
		public int RunProc(string procName) 
		{
            try{
			SqlCommand cmd = CreateCommand(procName, null);
			cmd.ExecuteNonQuery();		
			return (int)cmd.Parameters["ReturnValue"].Value;
        }
        finally
        {
            Close();
        }
			
		}
		#endregion

		#region GetDataSet(string procName, SqlParams[])
		public DataSet GetDataSet(string procName, SqlParameter[] prams)
		{
			try
            {
			    SqlCommand cmd = CreateCommand(procName, prams);
			    SqlDataAdapter adapter=new SqlDataAdapter(cmd);
			    DataSet dataset=new DataSet();
			    adapter.Fill(dataset);
    		
			    return dataset;
            }
            finally
            {
                Close();
            }			
		}
		#endregion

		#region RunProcedure(string name, SqlParam[])
		/// <summary>
		/// Run stored procedure.
		/// </summary>
		/// <param name="procName">Name of stored procedure.</param>
		/// <param name="prams">Stored procedure params.</param>
		/// <returns>Stored procedure return value.</returns>
		
		public int RunProc(string procName, SqlParameter[] prams)
		{
            try
            {
			    SqlCommand cmd = CreateCommand(procName, prams);
			    cmd.ExecuteNonQuery();
                SqlParameter param = cmd.Parameters["ReturnValue"];
                if (param != null && param.Value != null)
                    return (int)param.Value;
                else
                    return 0;
            }
            finally
            {
                Close();
            }
		}

        

        #endregion

        #region RunProcedure(string name, TimeSpan timeout, SqlParam[])
        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <returns>Stored procedure return value.</returns>

        public int RunProc(string procName, int timeout, SqlParameter[] prams)
        {
            try
            {
                SqlCommand cmd = CreateCommand(procName, prams);
                cmd.CommandTimeout = timeout;
                cmd.ExecuteNonQuery();
                SqlParameter param = cmd.Parameters["ReturnValue"];
                if (param != null && param.Value != null)
                    return (int)param.Value;
                else
                    return 0;
            }
            finally
            {
                Close();
            }
        }

        #endregion

		#region RunProcedure(string name, SqlParams[], bool close)
		public int RunProc(string procName, SqlParameter[] prams,bool mustClose)
		{
            try
            {
			    SqlCommand cmd = CreateCommand(procName, prams);
			    cmd.ExecuteNonQuery();
    						
			    return (int)cmd.Parameters["ReturnValue"].Value;
            }
            finally
            {
                if (mustClose == true)
                    Close();
            }
		}
		#endregion

		#region RunProcedure(string name, SqlParams[], out int records)
		/// <summary>
		/// Run stored procedure.
		/// </summary>
		/// <param name="procName">Name of stored procedure.</param>
		/// <param name="prams">Stored procedure params.</param>
		/// <returns>Stored procedure return value.</returns>
		
		public int RunProc(string procName, SqlParameter[] prams, out int recordsAffected)
		{
            try
            {
                SqlCommand cmd = CreateCommand(procName, prams);
                recordsAffected = cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["ReturnValue"].Value;
            }
            finally
            {
                Close();
            }
		}
		#endregion

		#region GetDataReader(string procName, SqlParameter[] prams )
		/// <summary>
		/// Run stored procedure.
		/// </summary>
		/// <param name="procName">Name of stored procedure.</param>
		/// <param name="dataReader">Return result of procedure.</param>
		public SqlDataReader GetDataReader(string procName, params SqlParameter[] prams )
		{
			
			SqlCommand cmd = CreateCommand(procName, prams);
			return  cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
		}
        #endregion



        #region GetXmlReader(string procName, SqlParameter[] prams )
        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="parameters">parameters.</param>
        public XmlReader GetXmlReader(string procName, params SqlParameter[] prams)
        {

            SqlCommand cmd = CreateCommand(procName, prams);
            try
            {
                return cmd.ExecuteXmlReader(); //known bug in .NET
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        #endregion

        #region GetDataReader(string procName, CommandType type, SqlParameter[] prams )
        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public SqlDataReader GetDataReader(string procName, CommandType type, params SqlParameter[] prams)
        {

            SqlCommand cmd = CreateCommand(procName, prams);
            cmd.CommandType = type;
            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }
        #endregion

        #region GetDataReader(string procName, CommandType type, CommandBehavior cmdBehavior, SqlParameter[] prams )
        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public SqlDataReader GetDataReader(string procName, CommandType type, CommandBehavior cmdBehavior, params SqlParameter[] prams)
        {

            SqlCommand cmd = CreateCommand(procName, prams);
            cmd.CommandType = type;
            return cmd.ExecuteReader(cmdBehavior);
        }
        #endregion
		
		#region GetDataReader(string procName)
		public SqlDataReader GetDataReader(string procName)
		{
			
			SqlCommand cmd = CreateCommand(procName, null);
			return  cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
		}
		#endregion




		#region RunProc(string procName, out SqlDataReader dataReader)
		/// <summary>
		/// Run stored procedure.
		/// </summary>
		/// <param name="procName">Name of stored procedure.</param>
		/// <param name="dataReader">Return result of procedure.</param>
		public void RunProc(string procName, out SqlDataReader dataReader) 
		{
			
			SqlCommand cmd = CreateCommand(procName, null);
			dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
		}
		#endregion

		#region RunProc(string procName, SqlParameter[] prams, out SqlDataReader dataReader) 
		/// <summary>
		/// Run stored procedure.
		/// </summary>
		/// <param name="procName">Name of stored procedure.</param>
		/// <param name="prams">Stored procedure params.</param>
		/// <param name="dataReader">Return result of procedure.</param>
		public void RunProc(string procName, SqlParameter[] prams, out SqlDataReader dataReader) 
		{
			
			SqlCommand cmd = CreateCommand(procName, prams);
			dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
		}
		#endregion
		
		#region CreateCommand(string procName, SqlParameter[] prams) 
		/// <summary>
		/// Create command object used to call stored procedure.
		/// </summary>
		/// <param name="procName">Name of stored procedure.</param>
		/// <param name="prams">Params to stored procedure.</param>
		/// <returns>Command object.</returns>
		private SqlCommand CreateCommand(string procName, SqlParameter[] prams) 
		{
			Open();		
			SqlCommand cmd = new SqlCommand(procName, cn);
            if (cmd.Parameters.Count > 0)
                cmd.Parameters.Clear();
			cmd.CommandType = CommandType.StoredProcedure;

			if (prams != null) 
			{
				foreach (SqlParameter parameter in prams)
					cmd.Parameters.Add(parameter);
			}
			
			cmd.Parameters.Add(
				new SqlParameter("ReturnValue", SqlDbType.Int, 4,
				ParameterDirection.ReturnValue, false, 0, 0,
				string.Empty, DataRowVersion.Default, null));

			return cmd;
		}
		#endregion

		#region Open the connection.
		/// <summary>
		/// Open the connection.
		/// </summary>
		private void Open() 
		{
			// open connection
			
			if (cn == null) 
			{
				cn = new SqlConnection(this.connectionString);
				cn.Open();
						
			}
		}
		#endregion

		#region Close the connection.
		/// <summary>
		/// Close the connection.
		/// </summary>
		public void Close() 
		{
			if (cn != null)
				cn.Close();
		}
		#endregion

		#region Make Input param (paramName, DbType, size, Value)
		/// <summary>
		/// Make input param.
		/// </summary>
		/// <param name="ParamName">Name of param.</param>
		/// <param name="DbType">Param type.</param>
		/// <param name="Size">Param size.</param>
		/// <param name="Value">Param value.</param>
		/// <returns>New parameter.</returns>
		public SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value) 
		{
			return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
		}		
		#endregion

		#region Make Output Param (param name, DbType, Size)
		/// <summary>
		/// Make input param.
		/// </summary>
		/// <param name="ParamName">Name of param.</param>
		/// <param name="DbType">Param type.</param>
		/// <param name="Size">Param size.</param>
		/// <returns>New parameter.</returns>
		public SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size) 
		{
			return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
		}
        #endregion

        #region Make Return Param (DbType, Size)
        /// <summary>
        /// Creates a sql return parameter
        /// </summary>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeReturnParam(SqlDbType DbType, int Size)
        {
            return MakeParam("@return_value", DbType, Size, ParameterDirection.ReturnValue, null);
        }
        #endregion

		#region Make stored procedure param.
		/// <summary>
		/// Make stored procedure param.
		/// </summary>
		/// <param name="ParamName">Name of param.</param>
		/// <param name="DbType">Param type.</param>
		/// <param name="Size">Param size.</param>
		/// <param name="Direction">Parm direction.</param>
		/// <param name="Value">Param value.</param>
		/// <returns>New parameter.</returns>
		public SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value) 
		{
			SqlParameter param;

			if(Size > 0)
				param = new SqlParameter(ParamName, DbType, Size);
			else
				param = new SqlParameter(ParamName, DbType);

			param.Direction = Direction;
			if (!(Direction == ParameterDirection.Output && Value == null))
				param.Value = Value;

			return param;
		}
		#endregion
		
        #region Finalization

		public virtual void Dispose()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (Disposed == true)
			{
				return;
			}
			

			try
			{
				GC.SuppressFinalize(this);

				if (cn != null)
				{
					cn.Dispose();
				}				
				cn = null;

			}
			catch
			{
			}
			finally
			{
				Disposed = true;
			}
		}

		~DbManager()
		{
			Dispose(true);
		}

#endregion




        
    }//EndClass DbManager

