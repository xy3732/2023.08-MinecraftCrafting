using System;

public class DebugCommandBase 
{
    private string _commandID;
    private string _commandDescription;
    private string _commandFormat;

    public string commandID { get { return _commandID; } }
    public string commandDescription { get { return _commandDescription; } }
    public string commandFormat { get { return _commandFormat; } }

    public DebugCommandBase(string id, string descripion, string format)
    {
        _commandID = id;
        _commandDescription = descripion;
        _commandFormat = format;
    }
}
public class DebugCommand : DebugCommandBase
{
    private Action command;

    public DebugCommand(string id, string descripion, string format, Action command) : base(id, descripion, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> command;

    public DebugCommand(string id, string descripion, string format, Action<T1> command) : base (id, descripion, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}

public class DebugCommand<T1,T2> : DebugCommandBase
{
    private Action<T1, T2> command;

    public DebugCommand(string id, string descripion, string format, Action<T1, T2> command) : base(id, descripion, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value, T2 vector)
    {
        command.Invoke(value, vector);
    }
}



