using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halo_2_Launcher.Objects
{
    public enum H2DisplayMode : int
    {
        None = -1,
        Windowed = 0,
        Fullscreen = 1,
        Borderless = 2
    }
    public enum H2ResponseCode : int
    {
        LoginSuccessful = 10,
        LoginInvalidPassword = 11,
        LoginAccountDoesntExsist = 12,
        LoginError = 13,
        LoginBanned = 14,
        LoginRememberTokenSuccessful = 20,
        LoginRememberTokenFailed = 21,
        LoginRememberTokenBanned = 22,
        LoginTokenSuccessful = 30,
        LoginTokenInvalid = 31,
        LoginTokenAccountDoesntExsist = 32,
        RegisterSuccessful = 40,
        RegisterUsernameInvalid = 41,
        RegisterUsernameFailed = 42,
    }
    public enum H2GameState : byte
    {
        lobby = 1,
        Starting = 2,
        ingame = 3,
        joining = 4,
        matchmaking = 5,
        unknown = 0
    }
}
