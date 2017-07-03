using System.Collections.Generic;

public static class AnimationKeyPath {
    public const string Opacity   = "Opacity";
    public const string Rotation  = "Rotation";
    public const string VelocityX = "VelocityX";
    public const string VelocityY = "VelocityY";
    public const string ScaleX    = "ScaleX";
    public const string ScaleY    = "ScaleY";
    public const string ColorR    = "ColorR";
    public const string ColorG    = "ColorG";
    public const string ColorB    = "ColorB";
    public const string RelPosX   = "RelPosX";
    public const string RelPosY   = "RelPosY";
    public const string RelScaleX = "RelScaleX";
    public const string RelScaleY = "RelScaleY";

    static Dictionary<string, TileMutexFlag> KEY_TO_TILE_MUTEX_FLAG = new Dictionary<string, TileMutexFlag> {
        {Opacity,     TileMutexFlag.Opacity},
        {Rotation,    TileMutexFlag.Rotation},
        {VelocityX,   TileMutexFlag.Velocity},
        {VelocityY,   TileMutexFlag.Velocity},
    };

    public static TileMutexFlag ToTileMutexFlag(string KeyPath) {
        if (KEY_TO_TILE_MUTEX_FLAG.ContainsKey(KeyPath)) {
            return KEY_TO_TILE_MUTEX_FLAG[KeyPath];
        }
        return TileMutexFlag.None;
    }
}
