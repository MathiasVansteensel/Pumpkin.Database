using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumpkin.Database;

//This is a multidimentional recursive mess... help me

public class UserAutomations 
{
	public List<Automation> Automations { get; set; }
}

public class Automation 
{
	public string Name { get; set; }
	public Condition Condition { get; set; }
}

public class Condition : ICondition
{
	public string Variable { get; set; }
	public string Value { get; set; }
	public string Operator { get; set; }
	public string Type { get; set; }

	public bool Evaluate(/*Dataframe dataframe*/) //dataframe = structure packed with all meassured data :)
	{
		//eval condition based on val, var, type and operator using dataframe
	}
}

public class AndCondition : INestedCondition 
{
	public List<Condition> Conditions { get; set; }
	public List<AndCondition> AndConditions { get; set; }
	public List<OrCondition> OrConditions { get; set; }

	public bool Evaluate(/*Dataframe dataframe*/) //dataframe = structure packed with all meassured data :)
	{
		//if Condition is null: Loop over andconditions and orconditions and call evaluate function for all of them
		//else call evaluate on condition
	}
}

public class OrCondition : INestedCondition 
{
	public List<Condition> Conditions { get; set; }
	public List<AndCondition> AndConditions { get; set; }
	public List<OrCondition> OrConditions { get; set; }

	public bool Evaluate(/*Dataframe dataframe*/) //dataframe = structure packed with all meassured data :)
	{
		//if Condition is null: Loop over andconditions and orconditions and call evaluate function for all of them
		//else call evaluate on condition
	}
}

public class Action
{
	//
}

//Interfaces needed to be able to treat AND and OR conditions like normal conditions and evaluate them recursively

public interface IRootCondition
{
	public bool Evaluate();
}

public interface ICondition : IRootCondition
{
	public string Variable { get; set; }
	public string Value { get; set; }
	public string Operator { get; set; }
	public string Type { get; set; }
}

public interface INestedCondition : IRootCondition
{
	public List<Condition> Conditions { get; set; }
	public List<AndCondition> AndConditions { get; set; }
	public List<OrCondition> OrConditions { get; set; }
}