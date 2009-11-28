namespace MinuteMan
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Data;
    // using System.Transactions;
    using Mono.Data.Sqlite;		


    public class Storage
    {
 
    /*
        var connection = GetConnection ();  
        using (var cmd = connection.CreateCommand ()) {  
            connection.Open ();  
            cmd.CommandText = "SELECT * FROM People";  
            using (var reader = cmd.ExecuteReader ()) {  
                while (reader.Read ()) {  
                    Console.Error.Write ("(Row ");  
                    Write (reader, 0);  
                    for (int i = 1; i < reader.FieldCount; ++i) {  
                        Console.Error.Write(" ");  
                        Write (reader, i);  
                    }  
                    Console.Error.WriteLine(")");  
                }  
            }  
            connection.Close ();  
        }  
    */
  
	public SqliteConnection GetConnection()  
	{  
	    var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);  
	    string db = Path.Combine(documents, "database.db3");  
	    bool exists = File.Exists(db);  
	    if (!exists) {
		SqliteConnection.CreateFile (db);  
	    }

	    var conn = new SqliteConnection("Data Source=" + db);  
	    if (!exists) {  
		var commands = new[] {  
		    "CREATE TABLE meeting (id INTEGER PRIMARY KEY, starte,d stopped, people INTEGER, rate REAL )"
		};  
		foreach (var cmd in commands) {
		    using (var c = conn.CreateCommand()) {  
			c.CommandText = cmd;  
			c.CommandType = CommandType.Text;  
			conn.Open ();  
			c.ExecuteNonQuery ();  
			conn.Close ();  
		    }
		}
	    }
	    return conn;  
	}     
    }
}
