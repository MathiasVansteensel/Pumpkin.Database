using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumpkin.Database;

public enum Language
{

}


public class User
{
	
	public string Name { get; set; }
	public Country Country { get; set; }
	public 

	private string PasswordHash { get; set; }
	private string HashSalt { get; set; }

    public User()
    {
		
	}
}
