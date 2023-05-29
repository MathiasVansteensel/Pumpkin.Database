using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isopoh.Cryptography.Argon2;

namespace Pumpkin.Database;

public enum Language
{

}


public class User
{
	public long DbId { get; set; }
	public Guid UserId { get; set; }
	public string Name { get; set; }
	public Country Country { get; set; }
	public UserAutomations Automations { get; set; }

	//Replace with class to hold user vars (including their dates and ref to nested db file)
	public string ValuesPath { get; set; }

	public decimal EnergyPrice { get; set; }

	private string PasswordHash { get; set; }
	private string HashSalt { get; set; }

    public User()
    {
		
	}

	public bool VerifyPassword(string input) 
	{
#warning TODO abstract to other class/methods :)
		byte[] saltBuffer = Convert.FromBase64String(HashSalt);
		byte[] inputBuffer = Encoding.UTF8.GetBytes(input);
		string hash = Argon2.Hash(inputBuffer, saltBuffer);
		return PasswordHash.Equals(hash);
	}
}
