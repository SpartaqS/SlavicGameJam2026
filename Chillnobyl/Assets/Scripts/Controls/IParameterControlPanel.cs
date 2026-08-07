using System;

public interface IParameterControlPanel
{
    bool isMalfunctioning { get; }

    //Paramater parameter { get; }

    Func<float> deltaOnClick { get; }
    Func<float> deltaOnState { get; }
}
