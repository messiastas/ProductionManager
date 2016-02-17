using System.Collections.Generic;
using System.Collections;

public class SharedVars  {

	public static SharedVars Inst = new SharedVars();

	public int worldSize = 32;

	public Dictionary<string, string> products =
		new Dictionary<string, string>()
	{
		{"mine", "mine"},
		{"factory", "factory"},
		{"worker", "worker"},
    };
}
