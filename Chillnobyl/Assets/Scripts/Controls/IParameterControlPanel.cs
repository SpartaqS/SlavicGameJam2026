using System;

public interface IParameterControlPanel
{
    ReactorParameterType controlledParameterType { get;  }
    
    bool isMalfunctioning { get; }

    //Paramater parameter { get; }

    Func<float> deltaOnClick { get; }
    Func<float> deltaOnState { get; }
}
