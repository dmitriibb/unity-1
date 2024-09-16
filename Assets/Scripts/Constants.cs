
using System.Dynamic;

public static class Constants
{
    public const string TAG_PLAYER = "Player";
    public const string TAG_WALL = "Wall";
    public const string TAG_ENEMY = "Enemy";

    public const string TRIGGER_PLAYER_ATTACK = "attack";
    public const string TRIGGER_PLAYER_HURT = "hurt";
    public const string TRIGGER_PLAYER_DIED = "died";
    public const string TRIGGER_FIREBALL_EXPLODE = "explode";

    public const string TRIGGER_ENEMY_ATTACK_MELEE = "meleeAttack";
    public const string TRIGGER_ENEMY_ATTACK_RANGE = "rangeAttack";
    public const string TRIGGER_ENEMY_HURT = "hurt";
    public const string TRIGGER_ENEMY_DIED = "die";

    public const int LAYER_PLAYER = 8;
    public const int LAYER_ENEMY = 9;

    public const string ANIM_PARAM_ACTIVATED = "activated";
    public const string ANIM_PARAM_ENEMY_MOVING = "moving";
}