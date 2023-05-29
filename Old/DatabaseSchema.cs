using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MVS.Database.Old;

//i feel nothing but pain...
//10 columns ought to be enough, otherwise just use the crooked method with dynamics

//I mean we have cool features now like dynamic types, implicit types, "better" generics, primary constructors, ...
//BUT NO PROPPER GENERICS YET, why do i still have to these massive classes to make something work and make sense... PLEASE i just want optional generic parameters and interface support for generic types,
//like returning a selected generic type in an indexer bc now when u have multiple types or ur classes are fractured like this mess it's impossible to use generics for whats they're even supposed to be used for.
//Try making an interface for these classes (which is what i would want to do but microsoft said NO)
//idk maybe i'm missing something, i guess it's almost 2am and my monster energy has worn off -_-

//i don't even know why i'm still writing these comments they can only be used as proof against me when i inevitably get sent to and insane asylum

public class DatabaseSchema //TODO: copy indexers from bottom class to all classes :)
{
	public enum Column
	{
		Col1,
		Col2,
		Col3,
		Col4,
		Col5,
		Col6,
		Col7,
		Col8,
		Col9,
		Col10
	}

	public DatabaseColumn<dynamic>[] Columns { get; set; }
	public DatabaseSchema(params (string name, dynamic[] values)[] column)
	{
		int colCount;
		Columns = new DatabaseColumn<dynamic>[colCount = column.Length];
		for (int i = 0; i < colCount; i++)
		{
			var col = column[i];
			Columns[i] = new(col.name, col.values);
		}
	}

	public dynamic this[int col, int row]
	{
		get => Columns[col].Values[row];
		set 
		{
			Columns[col].Values[row] = value;
		}
	}

	public DatabaseColumn<dynamic> this[int col]
	{
		get => Columns[col];
		set
		{
			Columns[col] = value;
		}
	}
}

//public class DatabaseSchema<T>
//{
//	public DatabaseColumn<T> Columns { get; set; }
//	public DatabaseSchema(string name, params T[] values) => Columns = new(name, values);
//}

//public class DatabaseSchema<T1, T2>
//{
//	public DatabaseColumn<T1> Column1 { get; set; }
//	public DatabaseColumn<T2> Column2 { get; set; }
//	public DatabaseSchema(DatabaseColumn<T1> column1, DatabaseColumn<T2> column2)
//	{
//		Column1 = column1;
//		Column2 = column2;
//	}
//}

//public class DatabaseSchema<T1, T2, T3>
//{
//	public DatabaseColumn<T1> Column1 { get; set; }
//	public DatabaseColumn<T2> Column2 { get; set; }
//	public DatabaseColumn<T3> Column3 { get; set; }
//	public DatabaseSchema(DatabaseColumn<T1> column1, DatabaseColumn<T2> column2, DatabaseColumn<T3> column3)
//	{
//		Column1 = column1;
//		Column2 = column2;
//		Column3 = column3;
//	}
//}

//public class DatabaseSchema<T1, T2, T3, T4>
//{
//	public DatabaseColumn<T1> Column1 { get; set; }
//	public DatabaseColumn<T2> Column2 { get; set; }
//	public DatabaseColumn<T3> Column3 { get; set; }
//	public DatabaseColumn<T4> Column4 { get; set; }
//	public DatabaseSchema(DatabaseColumn<T1> column1, DatabaseColumn<T2> column2, DatabaseColumn<T3> column3, DatabaseColumn<T4> column4)
//	{
//		Column1 = column1;
//		Column2 = column2;
//		Column3 = column3;
//		Column4 = column4;
//	}
//}

//public class DatabaseSchema<T1, T2, T3, T4, T5>
//{
//	public DatabaseColumn<T1> Column1 { get; set; }
//	public DatabaseColumn<T2> Column2 { get; set; }
//	public DatabaseColumn<T3> Column3 { get; set; }
//	public DatabaseColumn<T4> Column4 { get; set; }
//	public DatabaseColumn<T5> Column5 { get; set; }

//	public DatabaseSchema(DatabaseColumn<T1> column1, DatabaseColumn<T2> column2, DatabaseColumn<T3> column3, DatabaseColumn<T4> column4, DatabaseColumn<T5> column5)
//	{
//		Column1 = column1;
//		Column2 = column2;
//		Column3 = column3;
//		Column4 = column4;
//		Column5 = column5;
//	}
//}

//public class DatabaseSchema<T1, T2, T3, T4, T5, T6>
//{
//	public DatabaseColumn<T1> Column1 { get; set; }
//	public DatabaseColumn<T2> Column2 { get; set; }
//	public DatabaseColumn<T3> Column3 { get; set; }
//	public DatabaseColumn<T4> Column4 { get; set; }
//	public DatabaseColumn<T5> Column5 { get; set; }
//	public DatabaseColumn<T6> Column6 { get; set; }
//	public DatabaseSchema(DatabaseColumn<T1> column1, DatabaseColumn<T2> column2, DatabaseColumn<T3> column3, DatabaseColumn<T4> column4, DatabaseColumn<T5> column5, DatabaseColumn<T6> column6)
//	{
//		Column1 = column1;
//		Column2 = column2;
//		Column3 = column3;
//		Column4 = column4;
//		Column5 = column5;
//		Column6 = column6;
//	}
//}

//public class DatabaseSchema<T1, T2, T3, T4, T5, T6, T7>
//{
//	public DatabaseColumn<T1> Column1 { get; set; }
//	public DatabaseColumn<T2> Column2 { get; set; }
//	public DatabaseColumn<T3> Column3 { get; set; }
//	public DatabaseColumn<T4> Column4 { get; set; }
//	public DatabaseColumn<T5> Column5 { get; set; }
//	public DatabaseColumn<T6> Column6 { get; set; }
//	public DatabaseColumn<T7> Column7 { get; set; }
//	public DatabaseSchema(DatabaseColumn<T1> column1, DatabaseColumn<T2> column2, DatabaseColumn<T3> column3, DatabaseColumn<T4> column4, DatabaseColumn<T5> column5, DatabaseColumn<T6> column6, DatabaseColumn<T7> column7)
//	{
//		Column1 = column1;
//		Column2 = column2;
//		Column3 = column3;
//		Column4 = column4;
//		Column5 = column5;
//		Column6 = column6;
//		Column7 = column7;
//	}
//}


//public class DatabaseSchema<T1, T2, T3, T4, T5, T6, T7, T8>
//{
//	public DatabaseColumn<T1> Column1 { get; set; }
//	public DatabaseColumn<T2> Column2 { get; set; }
//	public DatabaseColumn<T3> Column3 { get; set; }
//	public DatabaseColumn<T4> Column4 { get; set; }
//	public DatabaseColumn<T5> Column5 { get; set; }
//	public DatabaseColumn<T6> Column6 { get; set; }
//	public DatabaseColumn<T7> Column7 { get; set; }
//	public DatabaseColumn<T8> Column8 { get; set; }
//	public DatabaseSchema(DatabaseColumn<T1> column1, DatabaseColumn<T2> column2, DatabaseColumn<T3> column3, DatabaseColumn<T4> column4, DatabaseColumn<T5> column5, DatabaseColumn<T6> column6, DatabaseColumn<T7> column7, DatabaseColumn<T8> column8)
//	{
//		Column1 = column1;
//		Column2 = column2;
//		Column3 = column3;
//		Column4 = column4;
//		Column5 = column5;
//		Column6 = column6;
//		Column7 = column7;
//		Column8 = column8;
//	}
//}

//public class DatabaseSchema<T1, T2, T3, T4, T5, T6, T7, T8, T9>
//{
//	public DatabaseColumn<T1> Column1 { get; set; }
//	public DatabaseColumn<T2> Column2 { get; set; }
//	public DatabaseColumn<T3> Column3 { get; set; }
//	public DatabaseColumn<T4> Column4 { get; set; }
//	public DatabaseColumn<T5> Column5 { get; set; }
//	public DatabaseColumn<T6> Column6 { get; set; }
//	public DatabaseColumn<T7> Column7 { get; set; }
//	public DatabaseColumn<T8> Column8 { get; set; }
//	public DatabaseColumn<T9> Column9 { get; set; }
//	public DatabaseSchema(DatabaseColumn<T1> column1, DatabaseColumn<T2> column2, DatabaseColumn<T3> column3, DatabaseColumn<T4> column4, DatabaseColumn<T5> column5, DatabaseColumn<T6> column6, DatabaseColumn<T7> column7, DatabaseColumn<T8> column8, DatabaseColumn<T9> column9)
//	{
//		Column1 = column1;
//		Column2 = column2;
//		Column3 = column3;
//		Column4 = column4;
//		Column5 = column5;
//		Column6 = column6;
//		Column7 = column7;
//		Column8 = column8;
//		Column9 = column9;
//	}
//}

//public class DatabaseSchema<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
//{
//	public DatabaseColumn<T1> Column1 { get; set; }
//	public DatabaseColumn<T2> Column2 { get; set; }
//	public DatabaseColumn<T3> Column3 { get; set; }
//	public DatabaseColumn<T4> Column4 { get; set; }
//	public DatabaseColumn<T5> Column5 { get; set; }
//	public DatabaseColumn<T6> Column6 { get; set; }
//	public DatabaseColumn<T7> Column7 { get; set; }
//	public DatabaseColumn<T8> Column8 { get; set; }
//	public DatabaseColumn<T9> Column9 { get; set; }
//	public DatabaseColumn<T10> Column10 { get; set; }

//	public DatabaseSchema(DatabaseColumn<T1> column1, DatabaseColumn<T2> column2, DatabaseColumn<T3> column3, DatabaseColumn<T4> column4, DatabaseColumn<T5> column5, DatabaseColumn<T6> column6, DatabaseColumn<T7> column7, DatabaseColumn<T8> column8, DatabaseColumn<T9> column9, DatabaseColumn<T10> column10)
//	{
//		Column1 = column1;
//		Column2 = column2;
//		Column3 = column3;
//		Column4 = column4;
//		Column5 = column5;
//		Column6 = column6;
//		Column7 = column7;
//		Column8 = column8;
//		Column9 = column9;
//		Column10 = column10;
//	}

//	public dynamic this[int index]
//	{
//		get
//		{
//			switch (index)
//			{
//				case 0:
//					return Column1;
//				case 1:
//					return Column2;
//				case 2:
//					return Column3;
//				case 3:
//					return Column4;
//				case 4:
//					return Column5;
//				case 5:
//					return Column6;
//				case 6:
//					return Column7;
//				case 7:
//					return Column8;
//				case 8:
//					return Column9;
//				case 9:
//					return Column10;
//				default:
//					throw new IndexOutOfRangeException($"Column{index + 1} does not exist [param: index ({index})]");
//			}
//		}
//		set
//		{
//			switch (index)
//			{
//				case 0:
//					Column1 = value;
//					return;
//				case 1:
//					Column1 = value;
//					return;
//				case 2:
//					Column3 = value;
//					return;
//				case 3:
//					Column4 = value;
//					return;
//				case 4:
//					Column5 = value;
//					return;
//				case 5:
//					Column6 = value;
//					return;
//				case 6:
//					Column7 = value;
//					return;
//				case 7:
//					Column8 = value;
//					return;
//				case 8:
//					Column9 = value;
//					return;
//				case 9:
//					Column10 = value;
//					return;
//				default:
//					throw new IndexOutOfRangeException($"Column{index + 1} does not exist [param: {nameof(index)} ({index})]");
//			}
//		}
//	}

//	public dynamic this[DatabaseSchema.Column column]
//	{
//		get => this[(int)column];
//		set => this[(int)column] = value;
//	}

//	public DatabaseColumn<dynamic> this[string columnName]
//	{
//		get 
//		{
//			for (int i = 0; i < 10; i++)
//			{
//				DatabaseColumn<dynamic> col = this[i];
//				if (col.Name == columnName) return col;
//			}
//			return null;
//		}
//		set 
//		{
//			for (int i = 0; i < 10; i++)
//			{
//				DatabaseColumn<dynamic> col = this[i];
//				if (col.Name == columnName) this[i] = value;
//			}
//		}
//	}
//}

