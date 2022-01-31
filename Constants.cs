using System;

public class Constants
{
	public const string DatabaseFilename = "TodoSQLite.db3";

	public const SQLite.SQLiteOpenFlags Flags =
		//open the database in read/write mode
		SQLite.SQLiteOpenFlags.ReadWrite |
		//create database if it doesnt exist
		SQLite.SQLiteOpenFlags.Create |
		//enable multi-threaded database access
		SQLite.SQLiteOpenFlags.SharedCache;

	public static string DatabasePath
    {
        get
        {
			var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			return basePath.Combine(basePath, DatabaseFilename);
        }
    }


}
