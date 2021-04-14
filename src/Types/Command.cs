namespace LSeeDee.Types
{
    public enum Command : byte
    {
        CursorHome = 1,
        ClearScreen = 2,
        HideCursor = 4,
        ShowCursor = 6,
        ClearDisplay = 12,
        SetCursorPosition = 17,
        SetHiddenCharacter = 21,
        ScrollText = 22,
        WrapOn = 23,
        WrapOff = 24
    }
}