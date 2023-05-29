using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.Database.Old;

public class DatabaseColumn<Tvalues>
{
	public string Name { get; set; }
	public Tvalues[] Values { get; set; }

    internal DatabaseColumn(string name, params Tvalues[] values)
    {
        Name = name;
        Values = values;
    }
}
