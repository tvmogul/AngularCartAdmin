using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Web;
//using QuickDL.Properties;
using System.Globalization;


namespace QuickDL
{
	/// <summary>
	/// This class is used to get data from a SQL Server database
	/// </summary>
	public class SqlUtil
	{
        string _connStr = "";  // ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        public string ZConnStr
        {
            get { return _connStr; }
            set { _connStr = value; }
        }

		public class DBContext
		{

			public SqlConnection Connection { get; set; }
			public SqlTransaction Transaction { get; set; }

		}

		#region Private Constants

		private readonly string DB_SCHEMA_NAME = (ConfigurationManager.AppSettings["DB_SCHEMA_NAME"] != null ? ConfigurationManager.AppSettings["DB_SCHEMA_NAME"].ToString() : "{0}");
		private readonly int COMMAND_TIMEOUT = (ConfigurationManager.AppSettings["SQL_TIMEOUT"] != null ? int.Parse(ConfigurationManager.AppSettings["SQL_TIMEOUT"].ToString()) : 30);

		#endregion

		#region Private Variables

		SqlConnection _conn = null;

		#endregion

		#region Public Methods

		/// <summary>
		/// Executes a non query result
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		public void ExecSPNonQuery(string spName, Hashtable spParams)
		{
			try
			{
				ExecSPNonQuery(spName, spParams, _connStr, DB_SCHEMA_NAME);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a dataset from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <returns>dataset returned from SP</returns>
		public DataSet ExecSP(string spName, Hashtable spParams)
		{
			try
			{
				return (ExecSP(spName, spParams, _connStr, DB_SCHEMA_NAME));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a string from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <returns>string returned from SP or defaultValue if failed</returns>
		public string ExecSP(string spName, Hashtable spParams, string defaultValue)
		{
			try
			{
				return (ExecSP(spName, spParams, _connStr, DB_SCHEMA_NAME, defaultValue));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a string from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <param name="fieldToReturn">The name of the field to return</param>
		/// <returns>byte[] returned from SP or defaultValue if failed</returns>
		public byte[] ExecSP(string spName, Hashtable spParams, byte[] defaultValue, string fieldToReturn)
		{
			try
			{
				return (ExecSP(spName, spParams, _connStr, DB_SCHEMA_NAME, defaultValue, fieldToReturn));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a string from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <returns>int returned from SP or default value if failed</returns>
		public int ExecSP(string spName, Hashtable spParams, int defaultValue)
		{
			try
			{
				return (ExecSP(spName, spParams, _connStr, DB_SCHEMA_NAME, defaultValue));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a string from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <returns>int returned from SP or default value if failed</returns>
		public bool ExecSP(string spName, Hashtable spParams, bool defaultValue)
		{
			try
			{
				return (ExecSP(spName, spParams, _connStr, DB_SCHEMA_NAME, defaultValue));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a string from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <returns>int returned from SP or default value if failed</returns>
		public double ExecSP(string spName, Hashtable spParams, double defaultValue)
		{
			try
			{
                //string connStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                return (ExecSP(spName, spParams, _connStr, DB_SCHEMA_NAME, defaultValue));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a XmlDocument from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="transformFile">The transform file to use with the XML</param>
		/// <returns>byte representing transformed html returned from SP</returns>
		public string ExecXMLSP(string spName, Hashtable spParams, string transformFile)
		{
			try
			{
				return (ExecXMLSP(spName, spParams, _connStr, DB_SCHEMA_NAME, transformFile));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a XmlDocument from a SP
		/// </summary>
		/// <param name="sql">The sql to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="transformFile">The transform file to use with the XML</param>
		/// <returns>byte representing transformed html returned from SP</returns>
		public string ExecXMLSQL(string sql, Hashtable spParams, string transformFile)
		{
			try
			{
				return (ExecXMLSQL(sql, spParams, _connStr, transformFile));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Executes a non query result
		/// </summary>
		/// <param name="spName">The sql to run</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		public void ExecSQLNonQuery(string sql, Hashtable spParams)
		{
			try
			{
				ExecSQLNonQuery(sql, spParams, _connStr);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a dataset from SQL
		/// </summary>
		/// <param name="sql">The sql to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <returns>dataset returned from SP</returns>
		public DataSet ExecSQL(string sql, Hashtable spParams)
		{
			try
			{
				return (ExecSQLConn(sql, spParams, _connStr));
			}
			catch
			{
				throw;
			}
		}


		/// <summary>
		/// Returns a string from a SQL
		/// </summary>
		/// <param name="sql">The SQL to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <returns>string returned from SQL or defaultValue if failed</returns>
		public string ExecSQL(string sql, Hashtable spParams, string defaultValue)
		{
			try
			{
				return (ExecSQL(sql, spParams, _connStr, defaultValue));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a string from sql
		/// </summary>
		/// <param name="sql">The SQL to run</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <param name="fieldToReturn">The name of the field to return</param>
		/// <returns>byte[] returned from SQL or defaultValue if failed</returns>
		public byte[] ExecSQL(string sql, Hashtable spParams, byte[] defaultValue, string fieldToReturn)
		{
			try
			{
				return (ExecSQL(sql, spParams, _connStr, defaultValue, fieldToReturn));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a string from a SQL
		/// </summary>
		/// <param name="sql">The SQL to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <returns>int returned from SQL or default value if failed</returns>
		public int ExecSQL(string sql, Hashtable spParams, int defaultValue)
		{
			try
			{
				return (ExecSQL(sql, spParams, _connStr, defaultValue));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a string from a SQL
		/// </summary>
		/// <param name="sql">The SQL to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <returns>int returned from SQL or default value if failed</returns>
		public double ExecSQL(string sql, Hashtable spParams, double defaultValue)
		{
			try
			{
				return (ExecSQL(sql, spParams, _connStr, defaultValue));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Executes a non query result
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="connStr">The connection string</param>
		/// <param name="schema">The database schema to use in a formatted string... ex [DatabaseName].[SchemaName].{0}</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		public void ExecSPNonQuery(string spName, Hashtable spParams, string connStr, string schema)
		{
			try
			{
				ExecSPTrace(spName, spParams, connStr, schema, null, null);
				
				_conn = new SqlConnection(connStr);

				SqlCommand cmd = new SqlCommand(String.Format(schema, spName));
				cmd.CommandType = CommandType.StoredProcedure;
				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					_conn.Open();
					cmd.ExecuteNonQuery();
				}
				catch
				{
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		public int ExecSPReturnIntValue(string spName, Hashtable spParams)
		{
			try
			{
				ExecSPTrace(spName, spParams, null, null, null, null);

				_conn = new SqlConnection(_connStr);

				SqlCommand cmd = new SqlCommand(String.Format(DB_SCHEMA_NAME, spName));
				cmd.CommandType = CommandType.StoredProcedure;
				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				const string RETURN_VALUE_NAME = "@ReturnValue";

				SqlParameter returnValueParam = new SqlParameter(RETURN_VALUE_NAME, SqlDbType.Int);
				returnValueParam.Direction = ParameterDirection.ReturnValue;
				cmd.Parameters.Add(returnValueParam);

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					_conn.Open();
					cmd.ExecuteNonQuery();
					object returnValue = cmd.Parameters[RETURN_VALUE_NAME].Value;
					return (int)returnValue;
				}
				catch
				{
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		void ExecSPTrace(string spName, Hashtable spParams, string connStr, string schema, string defaultValue, string fieldToReturn)
		{
            //if (!Settings.Default.SqlTraceEnabled) return;
            bool SqlTraceEnabled = false;
            if (!SqlTraceEnabled) return;

			if (spName == null) spName = "***NULL***";
			if (connStr == null) connStr = "***NULL***";
			if (schema == null) schema = "***NULL***";
			if (defaultValue == null) defaultValue = "***NULL***";
			if (fieldToReturn == null) fieldToReturn = "***NULL***";
			if (spParams == null) spParams = new Hashtable();

			StringBuilder sql = new StringBuilder();
			sql.AppendFormat("{0}", spName);
			foreach (DictionaryEntry param in spParams)
			{
				string value;
				if (param.Value == null)
				{
					value = "<NULL>";
				}
				else
				{
					string s = param.Value.ToString();
					value = s.Length == 0 ? "<BLANK>" : s;
				}
				sql.AppendFormat(" {0}=[{1}]", param.Key, value);
			}

			HttpContext context = HttpContext.Current;
			if (context != null)
			{
				context.Trace.Write("DB", sql.ToString());
				return;
			}

			// Quick-and-dirty output to a text file for a non-Web app.

			string logFile = Path.Combine(Environment.CurrentDirectory, "core-sql.log");
			File.AppendAllText(logFile, sql.ToString() + Environment.NewLine);
		}

		/// <summary>
		/// Returns a dataset from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="connStr">The connection string</param>
		/// <param name="schema">The database schema to use in a formatted string... ex [DatabaseName].[SchemaName].{0}</param>
		/// <returns>dataset returned from SP</returns>
		public DataSet ExecSP(string spName, Hashtable spParams, string connStr, string schema)
		{
			try
			{
				ExecSPTrace(spName, spParams, connStr, schema, null, null);

				_conn = new SqlConnection(connStr);

				DataSet ds = new DataSet();

				SqlCommand cmd = new SqlCommand(String.Format(schema, spName));
				cmd.CommandType = CommandType.StoredProcedure;

				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				
				da.Fill(ds);

				return ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a string from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <param name="connStr">The connection string</param>
		/// <param name="schema">The database schema to use in a formatted string... ex [DatabaseName].[SchemaName].{0}</param>
		/// <returns>string returned from SP or defaultValue if failed</returns>
		public string ExecSP(string spName, Hashtable spParams, string connStr, string schema, string defaultValue)
		{
			try
			{
				ExecSPTrace(spName, spParams, connStr, schema, defaultValue, null);

				_conn = new SqlConnection(connStr);

				DataSet ds = new DataSet();

				SqlCommand cmd = new SqlCommand(String.Format(schema, spName));
				cmd.CommandType = CommandType.StoredProcedure;
				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					_conn.Open();
					object fieldValue = cmd.ExecuteScalar();
					if (fieldValue == null) return null;
					string strReturn = fieldValue.ToString();
					return strReturn;
				}
				catch
				{
					//return (defaultValue);
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a string from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <param name="fieldToReturn">The name of the field to return</param>
		/// <param name="connStr">The connection string</param>
		/// <param name="schema">The database schema to use in a formatted string... ex [DatabaseName].[SchemaName].{0}</param>
		/// <returns>byte[] returned from SP or defaultValue if failed</returns>
		public byte[] ExecSP(string spName, Hashtable spParams, string connStr, string schema, byte[] defaultValue, string fieldToReturn)
		{
			try
			{
				ExecSPTrace(spName, spParams, connStr, schema, "byte[]", fieldToReturn);

				_conn = new SqlConnection(connStr);

				DataSet ds = new DataSet();

				SqlCommand cmd = new SqlCommand(String.Format(schema, spName));
				cmd.CommandType = CommandType.StoredProcedure;
				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					//open the connection
					_conn.Open();
					//create a reader
					SqlDataReader sr = cmd.ExecuteReader();

					//read the data
					sr.Read();

					byte[] objReturn = (byte[])sr[fieldToReturn];

					sr.Close();
					sr.Dispose();

					return objReturn;
				}
				catch
				{
					//return (defaultValue);
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a string from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <param name="connStr">The connection string</param>
		/// <param name="schema">The database schema to use in a formatted string... ex [DatabaseName].[SchemaName].{0}</param>
		/// <returns>int returned from SP or default value if failed</returns>
		public int ExecSP(string spName, Hashtable spParams, string connStr, string schema, int defaultValue)
		{
			try
			{
				ExecSPTrace(spName, spParams, connStr, schema,
					"int=" + defaultValue == null ? "NULL" : defaultValue.ToString(), null);

				_conn = new SqlConnection(connStr);

				DataSet ds = new DataSet();

				SqlCommand cmd = new SqlCommand(String.Format(schema, spName));
				cmd.CommandType = CommandType.StoredProcedure;
				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					_conn.Open();
					object obj = cmd.ExecuteScalar();
					if (obj == null) return defaultValue;
					int strReturn = int.Parse(obj.ToString());
					return strReturn;
				}
				catch
				{
					//return (defaultValue);
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a string from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <param name="connStr">The connection string</param>
		/// <param name="schema">The database schema to use in a formatted string... ex [DatabaseName].[SchemaName].{0}</param>
		/// <returns>int returned from SP or default value if failed</returns>
		public bool ExecSP(string spName, Hashtable spParams, string connStr, string schema, bool defaultValue)
		{
			try
			{
				ExecSPTrace(spName, spParams, connStr, schema,
					"bool=" + defaultValue == null ? "NULL" : defaultValue.ToString(), null);

				_conn = new SqlConnection(connStr);

				DataSet ds = new DataSet();

				SqlCommand cmd = new SqlCommand(String.Format(schema, spName));
				cmd.CommandType = CommandType.StoredProcedure;
				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					_conn.Open();
					object obj = cmd.ExecuteScalar();
					if (obj == null) return defaultValue;
					bool value = (bool)obj;
					return value;
				}
				catch
				{
					//return (defaultValue);
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a string from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <param name="connStr">The connection string</param>
		/// <param name="schema">The database schema to use in a formatted string... ex [DatabaseName].[SchemaName].{0}</param>
		/// <returns>int returned from SP or default value if failed</returns>
		public double ExecSP(string spName, Hashtable spParams, string connStr, string schema, double defaultValue)
		{
			try
			{
				ExecSPTrace(spName, spParams, connStr, schema,
					"double=" + defaultValue == null ? "NULL" : defaultValue.ToString(), null);

				_conn = new SqlConnection(connStr);

				DataSet ds = new DataSet();

				SqlCommand cmd = new SqlCommand(String.Format(schema, spName));
				cmd.CommandType = CommandType.StoredProcedure;
				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					_conn.Open();
					object obj = cmd.ExecuteScalar();
					double strReturn = double.Parse(obj.ToString());
					return strReturn;
				}
				catch
				{
					//return (defaultValue);
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a XmlDocument from a SP
		/// </summary>
		/// <param name="spName">The SP Name to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="transformFile">The transform file to use with the XML</param>
		/// <param name="connStr">The connection string</param>
		/// <param name="schema">The database schema to use in a formatted string... ex [DatabaseName].[SchemaName].{0}</param>
		/// <returns>byte representing transformed html returned from SP</returns>
		public string ExecXMLSP(string spName, Hashtable spParams, string connStr, string schema, string transformFile)
		{
			try
			{
				ExecSPTrace(spName, spParams, connStr, schema, transformFile, null);

				_conn = new SqlConnection(connStr);

				SqlCommand cmd = new SqlCommand(String.Format(schema, spName));
				cmd.CommandType = CommandType.StoredProcedure;
				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				//declare transform variable
				XslCompiledTransform xctrans = new XslCompiledTransform();

				//create a memory stream
				MemoryStream ms = new MemoryStream();
				//create a xmlWrite and attach it to the memorystream
				XmlWriter xWriter = new XmlTextWriter(ms, Encoding.Default);

				//open the connection
				_conn.Open();
				//read in the XML to the xReader
				XmlReader xReader = cmd.ExecuteXmlReader();

				try
				{
					//check to make sure the transform exists
					if (File.Exists(transformFile))
					{
						//load the transform file
						xctrans.Load(transformFile);
						//transform the XML from the xReader and output it to the memorystream attached
						//to the xWriter
						xctrans.Transform(xReader, null, xWriter);
						//set the memory streams position back to the beginning of the stream
						ms.Position = 0;

						//create a streamreader on the memory stream
						StreamReader sr = new StreamReader(ms);
						try
						{
							//read the memory stream to the end and output it as a string
							return (sr.ReadToEnd());
						}
						catch
						{
							throw;
						}
						finally
						{
							//close the streamreader
							sr.Close();
						}
					}
					else
					{
						throw (new Exception(String.Concat("XML Loading: File does not exist! - ", transformFile)));
					}
				}
				catch
				{
					throw;
				}
				finally
				{
					//close the rest of the objects (xmlreader,xmlwriter,memorystream)
					xReader.Close();
					xWriter.Close();
					ms.Close();
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a XmlDocument from a SP
		/// </summary>
		/// <param name="sql">The sql to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="transformFile">The transform file to use with the XML</param>
		/// <param name="connStr">The connection string</param>
		/// <returns>byte representing transformed html returned from SP</returns>
		public string ExecXMLSQL(string sql, Hashtable spParams, string connStr, string transformFile)
		{
			try
			{
				ExecSPTrace(sql, spParams, connStr, null, transformFile, null);

				_conn = new SqlConnection(connStr);

				SqlCommand cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				//declare transform variable
				XslCompiledTransform xctrans = new XslCompiledTransform();

				//create a memory stream
				MemoryStream ms = new MemoryStream();
				//create a xmlWrite and attach it to the memorystream
				XmlWriter xWriter = new XmlTextWriter(ms, Encoding.Default);

				//open the connection
				_conn.Open();
				//read in the XML to the xReader
				XmlReader xReader = cmd.ExecuteXmlReader();

				try
				{
					//check to make sure the transform exists
					if (File.Exists(transformFile))
					{
						//load the transform file
						xctrans.Load(transformFile);
						//transform the XML from the xReader and output it to the memorystream attached
						//to the xWriter
						xctrans.Transform(xReader, null, xWriter);
						//set the memory streams position back to the beginning of the stream
						ms.Position = 0;

						//create a streamreader on the memory stream
						StreamReader sr = new StreamReader(ms);
						try
						{
							//read the memory stream to the end and output it as a string
							return (sr.ReadToEnd());
						}
						catch
						{
							throw;
						}
						finally
						{
							//close the streamreader
							sr.Close();
						}
					}
					else
					{
						throw (new Exception(String.Concat("XML Loading: File does not exist! - ", transformFile)));
					}
				}
				catch
				{
					throw;
				}
				finally
				{
					//close the rest of the objects (xmlreader,xmlwriter,memorystream)
					xReader.Close();
					xWriter.Close();
					ms.Close();
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Executes a non query result
		/// </summary>
		/// <param name="sql">The sql to run</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="connStr">The connection string</param>
		/// <param name="schema">The database schema to use in a formatted string... ex [DatabaseName].[SchemaName].{0}</param>
		public void ExecSQLNonQuery(string sql, Hashtable spParams, string connStr)
		{
			try
			{
				ExecSPTrace(sql, spParams, connStr, null, null, null);

				_conn = new SqlConnection(connStr);

				SqlCommand cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					_conn.Open();
					cmd.ExecuteNonQuery();
				}
				catch
				{
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a dataset from SQL
		/// </summary>
		/// <param name="sql">The sql to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="connStr">The connection string</param>
		/// <returns>dataset returned from SP</returns>
		public DataSet ExecSQLConn(string sql, Hashtable spParams, string connStr)
		{
			try
			{
				ExecSPTrace(sql, spParams, connStr, null, null, null);

				_conn = new SqlConnection(connStr);

				DataSet ds = new DataSet();

				SqlCommand cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				SqlDataAdapter da = new SqlDataAdapter(cmd);

				da.Fill(ds);

				return ds;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a string from a SQL
		/// </summary>
		/// <param name="sql">The SQL to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <param name="connStr">The connection string</param>
		/// <returns>string returned from SQL or defaultValue if failed</returns>
		public string ExecSQL(string sql, Hashtable spParams, string connStr, string defaultValue)
		{
			try
			{
				ExecSPTrace(sql, spParams, connStr, null,
					"string=" + defaultValue == null ? "NULL" : defaultValue.ToString(), null);

				_conn = new SqlConnection(connStr);

				DataSet ds = new DataSet();

				SqlCommand cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					_conn.Open();
					string strReturn = cmd.ExecuteScalar().ToString();
					return strReturn;
				}
				catch
				{
					//return (defaultValue);
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a string from sql
		/// </summary>
		/// <param name="sql">The SQL to run</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <param name="fieldToReturn">The name of the field to return</param>
		/// <param name="connStr">The connection string</param>
		/// <returns>byte[] returned from SQL or defaultValue if failed</returns>
		public byte[] ExecSQL(string sql, Hashtable spParams, string connStr, byte[] defaultValue, string fieldToReturn)
		{
			try
			{
				ExecSPTrace(sql, spParams, connStr, null,
					defaultValue == null ? "NO" : "YES", fieldToReturn);

				_conn = new SqlConnection(connStr);

				DataSet ds = new DataSet();

				SqlCommand cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					//open the connection
					_conn.Open();
					//create a reader
					SqlDataReader sr = cmd.ExecuteReader();

					//read the data
					sr.Read();

					byte[] objReturn = (byte[])sr[fieldToReturn];

					sr.Close();
					sr.Dispose();

					return objReturn;
				}
				catch
				{
					//return (defaultValue);
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a string from a SQL
		/// </summary>
		/// <param name="sql">The SQL to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <param name="connStr">The connection string</param>
		/// <returns>int returned from SQL or default value if failed</returns>
		public int ExecSQL(string sql, Hashtable spParams, string connStr, int defaultValue)
		{
			try
			{
				ExecSPTrace(sql, spParams, connStr, null,
					"int=" + defaultValue == null ? "NULL" : defaultValue.ToString(), null);

				_conn = new SqlConnection(connStr);

				DataSet ds = new DataSet();

				SqlCommand cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					_conn.Open();
					object o = cmd.ExecuteScalar();
					if (o == null) return defaultValue;
					int i = int.Parse(o.ToString(), CultureInfo.InvariantCulture);
					return i;
				}
				catch
				{
					//return (defaultValue);
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns a string from a SQL
		/// </summary>
		/// <param name="sql">The SQL to execute</param>
		/// <param name="spParams">The parameters for the sp in the form of a hashtable (key = param name , value=value)</param>
		/// <param name="defaultValue">The default value to return if something fails</param>
		/// <param name="connStr">The connection string</param>
		/// <returns>int returned from SQL or default value if failed</returns>
		public double ExecSQL(string sql, Hashtable spParams, string connStr, double defaultValue)
		{
			try
			{
				ExecSPTrace(sql, spParams, connStr, null,
					"double=" + defaultValue == null ? "NULL" : defaultValue.ToString(), null);

				_conn = new SqlConnection(connStr);

				DataSet ds = new DataSet();

				SqlCommand cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;

				cmd.Connection = _conn;
				cmd.CommandTimeout = COMMAND_TIMEOUT;

				foreach (DictionaryEntry deParam in spParams)
				{
					cmd.Parameters.AddWithValue(deParam.Key.ToString(), deParam.Value);
				}

				//SqlDataAdapter da = new SqlDataAdapter(cmd);
				try
				{
					_conn.Open();
					double strReturn = double.Parse(cmd.ExecuteScalar().ToString());
					return strReturn;
				}
				catch
				{
					//return (defaultValue);
					throw;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_conn != null)
				{
					_conn.Close();
					_conn.Dispose();
				}
			}
		}

		#endregion

		#region Connection-Oriented Methods (With Transaction)

		public object Connect()
		{
			return Connect(IsolationLevel.Unspecified);
		}

		public object Connect(IsolationLevel iso)
		{
			string connStr = _connStr;
			
			DBContext context = new DBContext();
			context.Connection = new SqlConnection(connStr);
			
			context.Connection.Open();
	
			context.Transaction = iso == IsolationLevel.Unspecified ?
				context.Connection.BeginTransaction() :
				context.Connection.BeginTransaction(iso);

			return context;
		}

		public void Commit(object context)
		{
			DBContext db = (DBContext)context;
			db.Transaction.Commit();
			db.Connection.Close();
		}

		public void Rollback(object context)
		{
			DBContext db = (DBContext)context;
			db.Transaction.Rollback();
			db.Connection.Close();
		}

		public int ExecuteScalar(string sql, object context)
		{
			ExecSPTrace(sql, null, "***CONTEXT***", null, null, null);

			DBContext db = (DBContext)context;

			DataSet ds = new DataSet();

			SqlCommand cmd = new SqlCommand(sql);
			cmd.CommandType = CommandType.Text;
			cmd.Transaction = db.Transaction;
			cmd.Connection = db.Connection;
			cmd.CommandTimeout = COMMAND_TIMEOUT;

			//SqlDataAdapter da = new SqlDataAdapter(cmd);

			object obj = cmd.ExecuteScalar();
			if (obj == null) return -1;
			int value = int.Parse(obj.ToString());
			return value;
		}

		public void ExecuteNonQuery(string sql, object context)
		{
			ExecSPTrace(sql, null, "***CONTEXT***", null, null, null);

			DBContext db = (DBContext)context;

			SqlCommand cmd = new SqlCommand(sql);
			cmd.CommandType = CommandType.Text;
			cmd.Transaction = db.Transaction;
			cmd.Connection = db.Connection;
			cmd.CommandTimeout = COMMAND_TIMEOUT;

			//SqlDataAdapter da = new SqlDataAdapter(cmd);
			cmd.ExecuteNonQuery();
		}

		public SqlDataReader ExecuteQuery(string sql, object context)
		{
			ExecSPTrace(sql, null, "***CONTEXT***", null, null, null);

			DBContext db = (DBContext)context;

			SqlCommand cmd = new SqlCommand(sql);
			cmd.CommandType = CommandType.Text;
			cmd.Transaction = db.Transaction;
			cmd.Connection = db.Connection;
			cmd.CommandTimeout = COMMAND_TIMEOUT;

			//SqlDataAdapter da = new SqlDataAdapter(cmd);
			return cmd.ExecuteReader();
		}

		#endregion
	}
}
